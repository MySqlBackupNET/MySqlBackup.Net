using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace MySql.Data.MySqlClient
{
    public class MySqlBackup : IDisposable
    {
        enum ProcessType
        {
            Export,
            Import
        }

        public enum ProcessEndType
        {
            UnknownStatus,
            Complete,
            Cancelled,
            Error
        }

        public static string Version =>
            typeof(MySqlBackup).Assembly.GetName().Version.ToString();

        MySqlDatabase _database = new MySqlDatabase();
        MySqlServer _server = new MySqlServer();

        Encoding textEncoding
        {
            get
            {

                try
                {
                    return ExportInfo.TextEncoding;
                }
                catch { }
                return new UTF8Encoding(false);
            }
        }

        TextWriter textWriter;
        TextReader textReader;
        DateTime timeStart;
        DateTime timeEnd;
        ProcessType currentProcess;

        ProcessEndType processCompletionType;
        bool stopProcess = false;
        Exception _lastError = null;
        string _lastErrorSql = string.Empty;

        string _currentTableName = string.Empty;
        long _totalRowsInCurrentTable = 0;
        long _totalRowsInAllTables = 0;
        long _currentRowIndexInCurrentTable = 0;
        long _currentRowIndexInAllTable = 0;
        int _totalTables = 0;
        int _currentTableIndex = 0;
        Timer timerReport = null;

        long _currentBytes = 0L;
        long _totalBytes = 0L;
        StringBuilder _sbImport = null;
        MySqlScript _mySqlScript = null;
        string _delimiter = string.Empty;

        // for used of AdjustedColumnValue
        bool _hasAdjustedValueRule = false;
        bool _currentTableHasAdjustedValueRule = false;
        Dictionary<string, Func<object, object>> _currentTableColumnValueAdjustment = null;

        enum NextImportAction
        {
            Ignore,
            SetNames,
            CreateNewDatabase,
            AppendLine,
            ChangeDelimiter,
            AppendLineAndExecute
        }

        public Exception LastError { get { return _lastError; } }
        public string LastErrorSQL { get { return _lastErrorSql; } }

        /// <summary>
        /// Gets the information about the connected database.
        /// </summary>
        public MySqlDatabase Database { get { return _database; } }
        /// <summary>
        /// Gets the information about the connected MySQL server.
        /// </summary>
        public MySqlServer Server { get { return _server; } }
        /// <summary>
        /// Gets or Sets the instance of MySqlCommand.
        /// </summary>
        public MySqlCommand Command { get; set; }

        public ExportInformations ExportInfo = new ExportInformations();
        public ImportInformations ImportInfo = new ImportInformations();

        public delegate void exportProgressChange(object sender, ExportProgressArgs e);
        public event exportProgressChange ExportProgressChanged;

        public delegate void exportComplete(object sender, ExportCompleteArgs e);
        public event exportComplete ExportCompleted;

        public delegate void importProgressChange(object sender, ImportProgressArgs e);
        public event importProgressChange ImportProgressChanged;

        public delegate void importComplete(object sender, ImportCompleteArgs e);
        public event importComplete ImportCompleted;

        public delegate void getTotalRowsProgressChange(object sender, GetTotalRowsArgs e);
        public event getTotalRowsProgressChange GetTotalRowsProgressChanged;

        public MySqlBackup()
        {
            InitializeComponents();
        }

        public MySqlBackup(MySqlCommand cmd)
        {
            InitializeComponents();
            Command = cmd;
        }

        void InitializeComponents()
        {
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            _database.GetTotalRowsProgressChanged += _database_GetTotalRowsProgressChanged;

            timerReport = new Timer();
            timerReport.Elapsed += timerReport_Elapsed;

            //textEncoding = new UTF8Encoding(false);
        }

        void _database_GetTotalRowsProgressChanged(object sender, GetTotalRowsArgs e)
        {
            if (GetTotalRowsProgressChanged != null)
            {
                GetTotalRowsProgressChanged(this, e);
            }
        }

        #region Export

        public string ExportToString()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ExportToMemoryStream(ms);
                ms.Position = 0L;
                using (var thisReader = new StreamReader(ms))
                {
                    return thisReader.ReadToEnd();
                }
            }
        }

        public void ExportToFile(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (textWriter = new StreamWriter(filePath, false, textEncoding))
            {
                ExportStart();
                textWriter.Close();
            }
        }

        public void ExportToTextWriter(TextWriter tw)
        {
            textWriter = tw;
            ExportStart();
        }

        public void ExportToMemoryStream(MemoryStream ms)
        {
            ExportToMemoryStream(ms, true);
        }

        public void ExportToMemoryStream(MemoryStream ms, bool resetMemoryStreamPosition)
        {
            if (resetMemoryStreamPosition)
            {
                if (ms == null)
                    ms = new MemoryStream();
                if (ms.Length > 0)
                    ms = new MemoryStream();
                ms.Position = 0L;
            }

            textWriter = new StreamWriter(ms, textEncoding);
            ExportStart();
        }

        public void ExportToStream(Stream sm)
        {
            if (sm.CanSeek)
                sm.Seek(0, SeekOrigin.Begin);

            textWriter = new StreamWriter(sm, textEncoding);
            ExportStart();
        }

        void ExportStart()
        {
            try
            {
                Export_InitializeVariables();

                int stage = 1;

                while (stage < 11)
                {
                    if (stopProcess) break;

                    switch (stage)
                    {
                        case 1: Export_BasicInfo(); break;
                        case 2: Export_CreateDatabase(); break;
                        case 3: Export_DocumentHeader(); break;
                        case 4: Export_TableRows(); break;
                        case 5: Export_Functions(); break;
                        case 6: Export_Procedures(); break;
                        case 7: Export_Events(); break;
                        case 8: Export_Views(); break;
                        case 9: Export_Triggers(); break;
                        case 10: Export_DocumentFooter(); break;
                        default: break;
                    }

                    textWriter.Flush();

                    stage = stage + 1;
                }

                if (stopProcess) processCompletionType = ProcessEndType.Cancelled;
                else processCompletionType = ProcessEndType.Complete;
            }
            catch (Exception ex)
            {
                _lastError = ex;
                StopAllProcess();
                throw;
            }
            finally
            {
                ReportEndProcess();
            }
        }

        void Export_InitializeVariables()
        {
            if (Command == null)
            {
                throw new Exception("MySqlCommand is not initialized. Object not set to an instance of an object.");
            }

            if (Command.Connection == null)
            {
                throw new Exception("MySqlCommand.Connection is not initialized. Object not set to an instance of an object.");
            }

            if (Command.Connection.State != System.Data.ConnectionState.Open)
            {
                throw new Exception("MySqlCommand.Connection is not opened.");
            }

            if (ExportInfo.BlobExportMode == BlobDataExportMode.BinaryChar &&
                !ExportInfo.BlobExportModeForBinaryStringAllow)
            {
                throw new Exception("[ExportInfo.BlobExportMode = BlobDataExportMode.BinaryString] is still under development. Please join the discussion at https://github.com/MySqlBackupNET/MySqlBackup.Net/issues (Title: Help requires. Unable to export BLOB in Char Format)");
            }

            timeStart = DateTime.Now;

            stopProcess = false;
            processCompletionType = ProcessEndType.UnknownStatus;
            currentProcess = ProcessType.Export;
            _lastError = null;
            timerReport.Interval = ExportInfo.IntervalForProgressReport;
            //GetSHA512HashFromPassword(ExportInfo.EncryptionPassword);

            _database.GetDatabaseInfo(Command, ExportInfo.GetTotalRowsMode);
            _server.GetServerInfo(Command);
            _hasAdjustedValueRule = ExportInfo.TableColumnValueAdjustments != null;
            _currentTableName = string.Empty;
            _totalRowsInCurrentTable = 0L;
            _totalRowsInAllTables = Export_GetTablesToBeExported()
                .Sum(pair => _database.Tables[pair.Key].TotalRows);
            _currentRowIndexInCurrentTable = 0;
            _currentRowIndexInAllTable = 0;
            _totalTables = 0;
            _currentTableIndex = 0;
        }

        void Export_BasicInfo()
        {
            Export_WriteComment(string.Format("MySqlBackup.NET {0}", Version));

            if (ExportInfo.RecordDumpTime)
                Export_WriteComment(string.Format("Dump Time: {0}", timeStart.ToString("yyyy-MM-dd HH:mm:ss")));
            else
                Export_WriteComment(string.Empty);

            Export_WriteComment("--------------------------------------");
            Export_WriteComment(string.Format("Server version {0}", _server.Version));
            textWriter.WriteLine();
        }

        void Export_CreateDatabase()
        {
            if (!ExportInfo.AddCreateDatabase && !ExportInfo.AddDropDatabase)
                return;

            textWriter.WriteLine();
            textWriter.WriteLine();
            if (ExportInfo.AddDropDatabase)
                Export_WriteLine(String.Format("DROP DATABASE `{0}`;", _database.Name));
            if (ExportInfo.AddCreateDatabase)
            {
                Export_WriteLine(_database.CreateDatabaseSQL);
                Export_WriteLine(string.Format("USE `{0}`;", _database.Name));
            }
            textWriter.WriteLine();
            textWriter.WriteLine();
        }

        void Export_DocumentHeader()
        {
            textWriter.WriteLine();

            List<string> lstHeaders = ExportInfo.GetDocumentHeaders(Command);
            if (lstHeaders.Count > 0)
            {
                foreach (string s in lstHeaders)
                {
                    Export_WriteLine(s);
                }

                textWriter.WriteLine();
                textWriter.WriteLine();
            }
        }

        void Export_TableRows()
        {
            Dictionary<string, string> dicTables = Export_GetTablesToBeExportedReArranged();

            _totalTables = dicTables.Count;

            if (ExportInfo.ExportTableStructure || ExportInfo.ExportRows)
            {
                if (ExportProgressChanged != null)
                    timerReport.Start();

                foreach (KeyValuePair<string, string> kvTable in dicTables)
                {
                    if (stopProcess)
                        return;

                    string tableName = kvTable.Key;
                    string selectSQL = kvTable.Value;

                    bool exclude = Export_ThisTableIsExcluded(tableName);
                    if (exclude)
                    {
                        continue;
                    }

                    _currentTableName = tableName;
                    _currentTableIndex = _currentTableIndex + 1;
                    _totalRowsInCurrentTable = _database.Tables[tableName].TotalRows;

                    if (ExportInfo.ExportTableStructure)
                        Export_TableStructure(tableName);

                    var excludeRows = Export_ThisRowForTableIsExcluded(tableName);
                    if (ExportInfo.ExportRows && !excludeRows)
                        Export_Rows(tableName, selectSQL);
                }
            }
        }

        bool Export_ThisTableIsExcluded(string tableName)
        {
            string tableNameLower = tableName.ToLower();

            foreach (string blacklistedTable in ExportInfo.ExcludeTables)
            {
                if (blacklistedTable.ToLower() == tableNameLower)
                    return true;
            }

            return false;
        }

        bool Export_ThisRowForTableIsExcluded(string tableName)
        {
            var tableNameLower = tableName.ToLower();

            foreach (var blacklistedTable in ExportInfo.ExcludeRowsForTables)
            {
                if (blacklistedTable.ToLower() == tableNameLower)
                    return true;
            }

            return false;
        }

        void Export_TableStructure(string tableName)
        {
            if (stopProcess)
                return;

            Export_WriteComment(string.Empty);
            Export_WriteComment(string.Format("Definition of {0}", tableName));
            Export_WriteComment(string.Empty);

            textWriter.WriteLine();

            if (ExportInfo.AddDropTable)
                Export_WriteLine(string.Format("DROP TABLE IF EXISTS `{0}`;", tableName));

            if (ExportInfo.ResetAutoIncrement)
                Export_WriteLine(_database.Tables[tableName].CreateTableSqlWithoutAutoIncrement);
            else
                Export_WriteLine(_database.Tables[tableName].CreateTableSql);

            textWriter.WriteLine();

            textWriter.Flush();
        }

        Dictionary<string, string> Export_GetTablesToBeExportedReArranged()
        {
            var dic = Export_GetTablesToBeExported();

            Dictionary<string, string> dic2 = new Dictionary<string, string>();
            foreach (var kv in dic)
            {
                dic2[kv.Key] = _database.Tables[kv.Key].CreateTableSql;
            }

            var lst = Export_ReArrangeDependencies(dic2, "foreign key", "`");
            dic2 = lst.ToDictionary(k => k, k => dic[k]);
            return dic2;
        }

        private Dictionary<string, string> Export_GetTablesToBeExported()
        {
            if (ExportInfo.TablesToBeExportedDic is null ||
                ExportInfo.TablesToBeExportedDic.Count == 0)
            {
                return _database.Tables
                    .ToDictionary(
                        table => table.Name,
                        table => string.Format("SELECT * FROM `{0}`;", table.Name));
            }

            return ExportInfo.TablesToBeExportedDic
                .Where(table => _database.Tables.Contains(table.Key))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        List<string> Export_ReArrangeDependencies(Dictionary<string, string> dic1, string splitKeyword, string keyNameWrapper)
        {
            List<string> lst = new List<string>();
            HashSet<string> index = new HashSet<string>();

            bool requireLoop = true;

            while (requireLoop)
            {
                requireLoop = false;

                foreach (var kv in dic1)
                {
                    if (index.Contains(kv.Key))
                        continue;

                    bool allReferencedAdded = true;

                    string createSql = kv.Value.ToLower();
                    string referenceInfo = string.Empty;

                    bool referenceTaken = false;
                    if (!string.IsNullOrEmpty(splitKeyword))
                    {
                        if (createSql.Contains(string.Format(" {0} ", splitKeyword)))
                        {
                            string[] sa = createSql.Split(new string[] { string.Format(" {0} ", splitKeyword) }, StringSplitOptions.RemoveEmptyEntries);
                            referenceInfo = sa[sa.Length - 1];
                            referenceTaken = true;
                        }
                    }

                    if (!referenceTaken)
                        referenceInfo = createSql;

                    foreach (var kv2 in dic1)
                    {
                        if (kv.Key == kv2.Key)
                            continue;

                        if (index.Contains(kv2.Key))
                            continue;

                        string _thisTBname = string.Format("{0}{1}{0}", keyNameWrapper, kv2.Key.ToLower());

                        if (referenceInfo.Contains(_thisTBname))
                        {
                            allReferencedAdded = false;
                            break;
                        }
                    }

                    if (allReferencedAdded)
                    {
                        if (!index.Contains(kv.Key))
                        {
                            lst.Add(kv.Key);
                            index.Add(kv.Key);
                            requireLoop = true;
                            break;
                        }
                    }
                }
            }

            foreach (var kv in dic1)
            {
                if (!index.Contains(kv.Key))
                {
                    lst.Add(kv.Key);
                    index.Add(kv.Key);
                }
            }

            return lst;
        }

        void Export_Rows(string tableName, string selectSQL)
        {
            Export_WriteComment(string.Empty);
            Export_WriteComment(string.Format("Dumping data for table {0}", tableName));
            Export_WriteComment(string.Empty);
            textWriter.WriteLine();
            Export_WriteLine(string.Format("/*!40000 ALTER TABLE `{0}` DISABLE KEYS */;", tableName));

            if (ExportInfo.WrapWithinTransaction)
                Export_WriteLine("START TRANSACTION;");

            Export_RowsData(tableName, selectSQL);

            if (ExportInfo.WrapWithinTransaction)
                Export_WriteLine("COMMIT;");

            Export_WriteLine(string.Format("/*!40000 ALTER TABLE `{0}` ENABLE KEYS */;", tableName));
            textWriter.WriteLine();
            textWriter.Flush();
        }

        void Export_RowsData(string tableName, string selectSQL)
        {
            _currentRowIndexInCurrentTable = 0L;

            if (_hasAdjustedValueRule)
            {
                if (ExportInfo.TableColumnValueAdjustments.ContainsKey(tableName))
                {
                    _currentTableHasAdjustedValueRule = true;
                    _currentTableColumnValueAdjustment = ExportInfo.TableColumnValueAdjustments[tableName];
                }
                else
                {
                    _currentTableHasAdjustedValueRule = false;
                    _currentTableColumnValueAdjustment = null;
                }
            }

            if (ExportInfo.RowsExportMode == RowsDataExportMode.Insert ||
                ExportInfo.RowsExportMode == RowsDataExportMode.InsertIgnore ||
                ExportInfo.RowsExportMode == RowsDataExportMode.Replace)
            {
                Export_RowsData_Insert_Ignore_Replace(tableName, selectSQL);
            }
            else if (ExportInfo.RowsExportMode == RowsDataExportMode.OnDuplicateKeyUpdate)
            {
                Export_RowsData_OnDuplicateKeyUpdate(tableName, selectSQL);
            }
            else if (ExportInfo.RowsExportMode == RowsDataExportMode.Update)
            {
                Export_RowsData_Update(tableName, selectSQL);
            }
        }

        void Export_RowsData_Insert_Ignore_Replace(string tableName, string selectSQL)
        {
            MySqlTable table = _database.Tables[tableName];

            Command.CommandText = selectSQL;
            MySqlDataReader rdr = Command.ExecuteReader();

            string insertStatementHeader = null;

            var sb = new StringBuilder((int)ExportInfo.MaxSqlLength);

            while (rdr.Read())
            {
                if (stopProcess)
                    return;

                _currentRowIndexInAllTable = _currentRowIndexInAllTable + 1;
                _currentRowIndexInCurrentTable = _currentRowIndexInCurrentTable + 1;

                if (insertStatementHeader == null)
                {
                    insertStatementHeader = Export_GetInsertStatementHeader(ExportInfo.RowsExportMode, tableName, rdr);
                }

                string sqlDataRow = Export_GetValueString(rdr, table);

                if (sb.Length == 0)
                {
                    if (ExportInfo.InsertLineBreakBetweenInserts)
                        sb.AppendLine(insertStatementHeader);
                    else
                        sb.Append(insertStatementHeader);
                    sb.Append(sqlDataRow);
                }
                else if ((long)sb.Length + (long)sqlDataRow.Length < ExportInfo.MaxSqlLength)
                {
                    if (ExportInfo.InsertLineBreakBetweenInserts)
                        sb.AppendLine(",");
                    else
                        sb.Append(",");
                    sb.Append(sqlDataRow);
                }
                else
                {
                    sb.AppendFormat(";");

                    Export_WriteLine(sb.ToString());
                    textWriter.Flush();

                    sb = new StringBuilder((int)ExportInfo.MaxSqlLength);
                    sb.AppendLine(insertStatementHeader);
                    sb.Append(sqlDataRow);
                }
            }

            rdr.Close();

            if (sb.Length > 0)
            {
                sb.Append(";");
            }

            Export_WriteLine(sb.ToString());
            textWriter.Flush();

            sb = null;
        }

        void Export_RowsData_OnDuplicateKeyUpdate(string tableName, string selectSQL)
        {
            MySqlTable table = _database.Tables[tableName];

            bool allPrimaryField = true;
            foreach (var col in table.Columns)
            {
                if (!col.IsPrimaryKey)
                {
                    allPrimaryField = false;
                    break;
                }
            }

            Command.CommandText = selectSQL;
            MySqlDataReader rdr = Command.ExecuteReader();

            while (rdr.Read())
            {
                if (stopProcess)
                    return;

                _currentRowIndexInAllTable = _currentRowIndexInAllTable + 1;
                _currentRowIndexInCurrentTable = _currentRowIndexInCurrentTable + 1;

                StringBuilder sb = new StringBuilder();

                if (allPrimaryField)
                {
                    sb.Append(Export_GetInsertStatementHeader(RowsDataExportMode.InsertIgnore, tableName, rdr));
                    sb.Append(Export_GetValueString(rdr, table));
                }
                else
                {
                    sb.Append(Export_GetInsertStatementHeader(RowsDataExportMode.Insert, tableName, rdr));
                    sb.Append(Export_GetValueString(rdr, table));
                    sb.Append(" ON DUPLICATE KEY UPDATE ");
                    Export_GetUpdateString(rdr, table, sb);
                }

                sb.Append(";");

                Export_WriteLine(sb.ToString());
                textWriter.Flush();
            }

            rdr.Close();
        }

        void Export_RowsData_Update(string tableName, string selectSQL)
        {
            MySqlTable table = _database.Tables[tableName];

            bool allPrimaryField = true;
            foreach (var col in table.Columns)
            {
                if (!col.IsPrimaryKey)
                {
                    allPrimaryField = false;
                    break;
                }
            }

            if (allPrimaryField)
                return;

            bool allNonPrimaryField = true;
            foreach (var col in table.Columns)
            {
                if (col.IsPrimaryKey)
                {
                    allNonPrimaryField = false;
                    break;
                }
            }

            if (allNonPrimaryField)
                return;

            Command.CommandText = selectSQL;
            MySqlDataReader rdr = Command.ExecuteReader();

            while (rdr.Read())
            {
                if (stopProcess)
                    return;

                _currentRowIndexInAllTable = _currentRowIndexInAllTable + 1;
                _currentRowIndexInCurrentTable = _currentRowIndexInCurrentTable + 1;

                StringBuilder sb = new StringBuilder();
                sb.Append("UPDATE `");
                sb.Append(tableName);
                sb.Append("` SET ");

                Export_GetUpdateString(rdr, table, sb);

                sb.Append(" WHERE ");

                Export_GetConditionString(rdr, table, sb);

                sb.Append(";");

                Export_WriteLine(sb.ToString());

                textWriter.Flush();
            }

            rdr.Close();
        }

        private string Export_GetInsertStatementHeader(RowsDataExportMode rowsExportMode, string tableName, MySqlDataReader rdr)
        {
            StringBuilder sb = new StringBuilder();

            if (rowsExportMode == RowsDataExportMode.Insert)
                sb.Append("INSERT INTO `");
            else if (rowsExportMode == RowsDataExportMode.InsertIgnore)
                sb.Append("INSERT IGNORE INTO `");
            else if (rowsExportMode == RowsDataExportMode.Replace)
                sb.Append("REPLACE INTO `");

            sb.Append(tableName);
            sb.Append("`(");

            for (int i = 0; i < rdr.FieldCount; i++)
            {
                string _colname = rdr.GetName(i);

                if (_database.Tables[tableName].Columns[_colname].IsGeneratedColumn)
                    continue;

                if (i > 0)
                    sb.Append(",");
                sb.Append("`");
                sb.Append(rdr.GetName(i));
                sb.Append("`");
            }

            sb.Append(") VALUES");
            return sb.ToString();
        }

        private string Export_GetValueString(MySqlDataReader rdr, MySqlTable table)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < rdr.FieldCount; i++)
            {
                string columnName = rdr.GetName(i);

                if (table.Columns[columnName].IsGeneratedColumn)
                    continue;

                if (sb.Length == 0)
                    sb.AppendFormat("(");
                else
                    sb.AppendFormat(",");


                object ob = rdr[i];
                var col = table.Columns[columnName];

                if (_currentTableHasAdjustedValueRule && _currentTableColumnValueAdjustment.TryGetValue(columnName, out var adjustFunc))
                {
                    ob = adjustFunc(ob);
                }

                sb.Append(QueryExpress.ConvertToSqlFormat(ob, true, true, col, ExportInfo.BlobExportMode));
            }

            sb.AppendFormat(")");
            return sb.ToString();
        }

        private void Export_GetUpdateString(MySqlDataReader rdr, MySqlTable table, StringBuilder sb)
        {
            bool isFirst = true;

            for (int i = 0; i < rdr.FieldCount; i++)
            {
                string columnName = rdr.GetName(i);

                var col = table.Columns[columnName];

                if (!col.IsPrimaryKey)
                {
                    if (isFirst)
                        isFirst = false;
                    else
                        sb.Append(",");

                    sb.Append("`");
                    sb.Append(columnName);
                    sb.Append("`=");

                    var ob = rdr[i];

                    if (_currentTableHasAdjustedValueRule && _currentTableColumnValueAdjustment.TryGetValue(columnName, out var adjustFunc))
                    {
                        ob = adjustFunc(ob);
                    }

                    //sb.Append(QueryExpress.ConvertToSqlFormat(rdr, i, true, true, col));
                    sb.Append(QueryExpress.ConvertToSqlFormat(ob, true, true, col, ExportInfo.BlobExportMode));
                }
            }
        }

        private void Export_GetConditionString(MySqlDataReader rdr, MySqlTable table, StringBuilder sb)
        {
            bool isFirst = true;

            for (int i = 0; i < rdr.FieldCount; i++)
            {
                string columnName = rdr.GetName(i);

                var col = table.Columns[columnName];

                if (col.IsPrimaryKey)
                {
                    if (isFirst)
                        isFirst = false;
                    else
                        sb.Append(" and ");

                    sb.Append("`");
                    sb.Append(columnName);
                    sb.Append("`=");

                    var ob = rdr[i];

                    if (_currentTableHasAdjustedValueRule && _currentTableColumnValueAdjustment.TryGetValue(columnName, out var adjustFunc))
                    {
                        ob = adjustFunc(ob);
                    }

                    //sb.Append(QueryExpress.ConvertToSqlFormat(rdr, i, true, true, col));
                    sb.Append(QueryExpress.ConvertToSqlFormat(ob, true, true, col, ExportInfo.BlobExportMode));
                }
            }
        }

        void Export_Procedures()
        {
            if (!ExportInfo.ExportProcedures || _database.Procedures.Count == 0)
                return;

            Export_WriteComment(string.Empty);
            Export_WriteComment("Dumping procedures");
            Export_WriteComment(string.Empty);
            textWriter.WriteLine();

            foreach (MySqlProcedure procedure in _database.Procedures)
            {
                if (stopProcess)
                    return;

                if (procedure.CreateProcedureSQLWithoutDefiner.Trim().Length == 0 ||
                    procedure.CreateProcedureSQL.Trim().Length == 0)
                    continue;

                Export_WriteLine(string.Format("DROP PROCEDURE IF EXISTS `{0}`;", procedure.Name));
                Export_WriteLine("DELIMITER " + ExportInfo.ScriptsDelimiter);

                if (ExportInfo.ExportRoutinesWithoutDefiner)
                    Export_WriteLine(procedure.CreateProcedureSQLWithoutDefiner + " " + ExportInfo.ScriptsDelimiter);
                else
                    Export_WriteLine(procedure.CreateProcedureSQL + " " + ExportInfo.ScriptsDelimiter);

                Export_WriteLine("DELIMITER ;");
                textWriter.WriteLine();
            }
            textWriter.Flush();
        }

        void Export_Functions()
        {
            if (!ExportInfo.ExportFunctions || _database.Functions.Count == 0)
                return;

            Export_WriteComment(string.Empty);
            Export_WriteComment("Dumping functions");
            Export_WriteComment(string.Empty);
            textWriter.WriteLine();

            foreach (MySqlFunction function in _database.Functions)
            {
                if (stopProcess)
                    return;

                if (function.CreateFunctionSQL.Trim().Length == 0 ||
                    function.CreateFunctionSQLWithoutDefiner.Trim().Length == 0)
                    continue;

                Export_WriteLine(string.Format("DROP FUNCTION IF EXISTS `{0}`;", function.Name));
                Export_WriteLine("DELIMITER " + ExportInfo.ScriptsDelimiter);

                if (ExportInfo.ExportRoutinesWithoutDefiner)
                    Export_WriteLine(function.CreateFunctionSQLWithoutDefiner + " " + ExportInfo.ScriptsDelimiter);
                else
                    Export_WriteLine(function.CreateFunctionSQL + " " + ExportInfo.ScriptsDelimiter);

                Export_WriteLine("DELIMITER ;");
                textWriter.WriteLine();
            }

            textWriter.Flush();
        }

        void Export_Views()
        {
            if (!ExportInfo.ExportViews || _database.Views.Count == 0)
                return;

            // ReArrange Views
            Dictionary<string, string> dicView_Create = new Dictionary<string, string>();
            foreach (var view in _database.Views)
            {
                dicView_Create[view.Name] = view.CreateViewSQL;
            }

            var lst = Export_ReArrangeDependencies(dicView_Create, null, "`");

            Export_WriteComment(string.Empty);
            Export_WriteComment("Dumping views");
            Export_WriteComment(string.Empty);
            textWriter.WriteLine();

            foreach (var viewname in lst)
            {
                if (stopProcess)
                    return;

                var view = _database.Views[viewname];

                if (view.CreateViewSQL.Trim().Length == 0 ||
                    view.CreateViewSQLWithoutDefiner.Trim().Length == 0)
                    continue;

                Export_WriteLine(string.Format("DROP TABLE IF EXISTS `{0}`;", view.Name));
                Export_WriteLine(string.Format("DROP VIEW IF EXISTS `{0}`;", view.Name));

                if (ExportInfo.ExportRoutinesWithoutDefiner)
                    Export_WriteLine(view.CreateViewSQLWithoutDefiner);
                else
                    Export_WriteLine(view.CreateViewSQL);

                textWriter.WriteLine();
            }

            textWriter.WriteLine();
            textWriter.Flush();
        }

        void Export_Events()
        {
            if (!ExportInfo.ExportEvents || _database.Events.Count == 0)
                return;

            Export_WriteComment(string.Empty);
            Export_WriteComment("Dumping events");
            Export_WriteComment(string.Empty);
            textWriter.WriteLine();

            foreach (MySqlEvent e in _database.Events)
            {
                if (stopProcess)
                    return;

                if (e.CreateEventSql.Trim().Length == 0 ||
                    e.CreateEventSqlWithoutDefiner.Trim().Length == 0)
                    continue;

                Export_WriteLine(string.Format("DROP EVENT IF EXISTS `{0}`;", e.Name));
                Export_WriteLine("DELIMITER " + ExportInfo.ScriptsDelimiter);

                if (ExportInfo.ExportRoutinesWithoutDefiner)
                    Export_WriteLine(e.CreateEventSqlWithoutDefiner + " " + ExportInfo.ScriptsDelimiter);
                else
                    Export_WriteLine(e.CreateEventSql + " " + ExportInfo.ScriptsDelimiter);

                Export_WriteLine("DELIMITER ;");
                textWriter.WriteLine();
            }

            textWriter.Flush();
        }

        void Export_Triggers()
        {
            if (!ExportInfo.ExportTriggers ||
                _database.Triggers.Count == 0)
                return;

            Export_WriteComment(string.Empty);
            Export_WriteComment("Dumping triggers");
            Export_WriteComment(string.Empty);
            textWriter.WriteLine();

            foreach (MySqlTrigger trigger in _database.Triggers)
            {
                if (stopProcess)
                    return;

                var createTriggerSQL = trigger.CreateTriggerSQL.Trim();
                var createTriggerSQLWithoutDefiner = trigger.CreateTriggerSQLWithoutDefiner.Trim();
                if (createTriggerSQL.Length == 0 ||
                    createTriggerSQLWithoutDefiner.Length == 0)
                    continue;

                Export_WriteLine(string.Format("DROP TRIGGER /*!50030 IF EXISTS */ `{0}`;", trigger.Name));
                Export_WriteLine("DELIMITER " + ExportInfo.ScriptsDelimiter);

                if (ExportInfo.ExportRoutinesWithoutDefiner)
                    Export_WriteLine(createTriggerSQLWithoutDefiner + " " + ExportInfo.ScriptsDelimiter);
                else
                    Export_WriteLine(createTriggerSQL + " " + ExportInfo.ScriptsDelimiter);

                Export_WriteLine("DELIMITER ;");
                textWriter.WriteLine();
            }

            textWriter.Flush();
        }

        void Export_DocumentFooter()
        {
            textWriter.WriteLine();

            List<string> lstFooters = ExportInfo.GetDocumentFooters();
            if (lstFooters.Count > 0)
            {
                foreach (string s in lstFooters)
                {
                    Export_WriteLine(s);
                }
            }

            timeEnd = DateTime.Now;

            if (ExportInfo.RecordDumpTime)
            {
                TimeSpan ts = timeEnd - timeStart;

                textWriter.WriteLine();
                textWriter.WriteLine();
                Export_WriteComment(string.Format("Dump completed on {0}", timeEnd.ToString("yyyy-MM-dd HH:mm:ss")));
                Export_WriteComment(string.Format("Total time: {0}:{1}:{2}:{3}:{4} (d:h:m:s:ms)", ts.Days, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds));
            }

            textWriter.Flush();
        }

        void Export_WriteComment(string text)
        {
            if (ExportInfo.EnableComment)
                Export_WriteLine(string.Format("-- {0}", text));
        }

        void Export_WriteLine(string text)
        {
            textWriter.WriteLine(text);
        }

        #endregion

        #region Import

        public void ImportFromString(string sqldumptext)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter thisWriter = new StreamWriter(ms))
                {
                    thisWriter.Write(sqldumptext);
                    thisWriter.Flush();

                    ms.Position = 0L;

                    ImportFromMemoryStream(ms);
                }
            }
        }

        public void ImportFromFile(string filePath)
        {
            System.IO.FileInfo fi = new FileInfo(filePath);

            using (TextReader tr = new StreamReader(filePath))
            {
                ImportFromTextReaderStream(tr, fi);
            }
        }

        public void ImportFromTextReader(TextReader tr)
        {
            ImportFromTextReaderStream(tr, null);
        }

        public void ImportFromMemoryStream(MemoryStream ms)
        {
            ms.Position = 0;
            _totalBytes = ms.Length;
            textReader = new StreamReader(ms);
            Import_Start();
        }

        public void ImportFromStream(Stream sm)
        {
            if (sm.CanSeek)
                sm.Seek(0, SeekOrigin.Begin);

            textReader = new StreamReader(sm);
            Import_Start();
        }

        void ImportFromTextReaderStream(TextReader tr, FileInfo fileInfo)
        {
            if (fileInfo != null)
                _totalBytes = fileInfo.Length;
            else
                _totalBytes = 0L;

            textReader = tr;

            Import_Start();
        }

        void Import_Start()
        {
            Import_InitializeVariables();

            try
            {
                string line = string.Empty;

                while (line != null)
                {
                    if (stopProcess)
                    {
                        processCompletionType = ProcessEndType.Cancelled;
                        break;
                    }

                    try
                    {
                        line = Import_GetLine();

                        if (line == null)
                            break;

                        if (line.Length == 0)
                            continue;

                        Import_ProcessLine(line);
                    }
                    catch (Exception ex)
                    {
                        line = string.Empty;
                        _lastError = ex;
                        _lastErrorSql = _sbImport.ToString();

                        if (!string.IsNullOrEmpty(ImportInfo.ErrorLogFile))
                        {
                            File.AppendAllText(ImportInfo.ErrorLogFile, ex.Message + Environment.NewLine + Environment.NewLine + _lastErrorSql + Environment.NewLine + Environment.NewLine);
                        }

                        _sbImport = new StringBuilder();

                        GC.Collect();

                        if (!ImportInfo.IgnoreSqlError)
                        {
                            StopAllProcess();
                            throw;
                        }
                    }
                }
            }
            finally
            {
                ReportEndProcess();
            }
        }

        void Import_InitializeVariables()
        {
            if (Command == null)
            {
                throw new Exception("MySqlCommand is not initialized. Object not set to an instance of an object.");
            }

            if (Command.Connection == null)
            {
                throw new Exception("MySqlCommand.Connection is not initialized. Object not set to an instance of an object.");
            }

            if (Command.Connection.State != System.Data.ConnectionState.Open)
            {
                throw new Exception("MySqlCommand.Connection is not opened.");
            }

            //_createViewDetected = false;
            //_dicImportRoutines = new Dictionary<string, bool>();
            stopProcess = false;
            //GetSHA512HashFromPassword(ImportInfo.EncryptionPassword);
            _lastError = null;
            timeStart = DateTime.Now;
            _currentBytes = 0L;
            _sbImport = new StringBuilder();
            _mySqlScript = new MySqlScript(Command.Connection);
            currentProcess = ProcessType.Import;
            processCompletionType = ProcessEndType.Complete;
            _delimiter = ";";
            _lastErrorSql = string.Empty;

            if (ImportProgressChanged != null)
                timerReport.Start();

        }

        string Import_GetLine()
        {
            string line = textReader.ReadLine();

            if (line == null)
                return null;

            if (ImportProgressChanged != null)
            {
                _currentBytes = _currentBytes + (long)line.Length;
            }

            line = line.Trim();

            if (Import_IsEmptyLine(line))
            {
                return string.Empty;
            }

            return line;
        }

        void Import_ProcessLine(string line)
        {
            NextImportAction nextAction = Import_AnalyseNextAction(line);

            switch (nextAction)
            {
                case NextImportAction.Ignore:
                    break;
                case NextImportAction.AppendLine:
                    Import_AppendLine(line);
                    break;
                case NextImportAction.ChangeDelimiter:
                    Import_ChangeDelimiter(line);
                    Import_AppendLine(line);
                    break;
                case NextImportAction.AppendLineAndExecute:
                    Import_AppendLineAndExecute(line);
                    break;
                default:
                    break;
            }
        }

        NextImportAction Import_AnalyseNextAction(string line)
        {
            if (line == null)
                return NextImportAction.Ignore;

            if (line == string.Empty)
                return NextImportAction.Ignore;

            if (line.StartsWith("DELIMITER ", StringComparison.OrdinalIgnoreCase))
                return NextImportAction.ChangeDelimiter;

            if (line.EndsWith(_delimiter))
                return NextImportAction.AppendLineAndExecute;

            return NextImportAction.AppendLine;
        }

        void Import_AppendLine(string line)
        {
            _sbImport.AppendLine(line);
        }

        void Import_ChangeDelimiter(string line)
        {
            string nextDelimiter = line.Substring(9);
            _delimiter = nextDelimiter.Replace(" ", string.Empty);
        }

        void Import_AppendLineAndExecute(string line)
        {
            _sbImport.Append(line);

            string _query = _sbImport.ToString();

            if (_query.StartsWith("DELIMITER ", StringComparison.OrdinalIgnoreCase))
            {
                _mySqlScript.Query = _sbImport.ToString();
                _mySqlScript.Delimiter = _delimiter;
                _mySqlScript.Execute();
            }
            else
            {
                Command.CommandText = _query;
                Command.ExecuteNonQuery();
            }

            _sbImport = new StringBuilder();

            GC.Collect();
        }

        bool Import_IsEmptyLine(string line)
        {
            if (line == null)
                return true;
            if (line == string.Empty)
                return true;
            if (line.Length == 0)
                return true;
            if (line.StartsWith("--"))
                return true;
            if (line == Environment.NewLine)
                return true;
            if (line == "\r")
                return true;
            if (line == "\n")
                return true;
            if (line == "\r\n")
                return true;

            return false;
        }

        #endregion

        void ReportEndProcess()
        {
            timeEnd = DateTime.Now;

            StopAllProcess();

            if (currentProcess == ProcessType.Export)
            {
                ReportProgress();
                if (ExportCompleted != null)
                {
                    ExportCompleteArgs arg = new ExportCompleteArgs(timeStart, timeEnd, processCompletionType, _lastError);
                    ExportCompleted(this, arg);
                }
            }
            else if (currentProcess == ProcessType.Import)
            {
                _currentBytes = _totalBytes;

                ReportProgress();
                if (ImportCompleted != null)
                {
                    MySqlBackup.ProcessEndType completedType = ProcessEndType.UnknownStatus;
                    switch (processCompletionType)
                    {
                        case ProcessEndType.Complete:
                            completedType = MySqlBackup.ProcessEndType.Complete;
                            break;
                        case ProcessEndType.Error:
                            completedType = MySqlBackup.ProcessEndType.Error;
                            break;
                        case ProcessEndType.Cancelled:
                            completedType = MySqlBackup.ProcessEndType.Cancelled;
                            break;
                    }

                    ImportCompleteArgs arg = new ImportCompleteArgs(completedType, timeStart, timeEnd, _lastError);
                    ImportCompleted(this, arg);
                }
            }
        }

        void timerReport_Elapsed(object sender, ElapsedEventArgs e)
        {
            ReportProgress();
        }

        void ReportProgress()
        {
            if (currentProcess == ProcessType.Export)
            {
                if (ExportProgressChanged != null)
                {
                    ExportProgressArgs arg = new ExportProgressArgs(_currentTableName, _totalRowsInCurrentTable, _totalRowsInAllTables, _currentRowIndexInCurrentTable, _currentRowIndexInAllTable, _totalTables, _currentTableIndex);
                    ExportProgressChanged(this, arg);
                }
            }
            else if (currentProcess == ProcessType.Import)
            {
                if (ImportProgressChanged != null)
                {
                    ImportProgressArgs arg = new ImportProgressArgs(_currentBytes, _totalBytes);
                    ImportProgressChanged(this, arg);
                }
            }
        }

        public void StopAllProcess()
        {
            stopProcess = true;
            timerReport.Stop();
        }

        public void Dispose()
        {
            try
            {
                _database.Dispose();
            }
            catch { }

            try
            {
                _server = null;
            }
            catch { }

            try
            {
                _mySqlScript = null;
            }
            catch { }
        }
    }
}
