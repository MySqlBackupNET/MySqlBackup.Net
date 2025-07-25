﻿//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Timers;

//namespace MySqlConnector
//{
//    public partial class MySqlBackup : IDisposable
//    {
//        public static string Version =>
//            typeof(MySqlBackup).Assembly.GetName().Version.ToString();

//        MySqlDatabase _database = new MySqlDatabase();
//        MySqlServer _server = new MySqlServer();

//        StringBuilder _sb = null;

//        Encoding textEncoding = new UTF8Encoding(false);

//        TextWriter textWriter;

//        DateTime timeStart;
//        DateTime timeEnd;
//        ProcessType currentProcess;

//        ProcessEndType processCompletionType;
//        volatile bool stopProcess = false;
//        Exception _lastError = null;
//        string _lastErrorSql = string.Empty;

//        string _currentTableName = string.Empty;
//        long _totalRowsInCurrentTable = 0;
//        long _totalRowsInAllTables = 0;
//        long _currentRowIndexInCurrentTable = 0;
//        long _currentRowIndexInAllTable = 0;
//        int _totalTables = 0;
//        int _currentTableIndex = 0;
//        Timer timerReport = null;
//        string _originalTimeZone = "";

//        // for used of AdjustedColumnValue
//        bool _hasAdjustedValueRule = false;
//        bool _currentTableHasAdjustedValueRule = false;
//        Dictionary<string, Func<object, object>> _currentTableColumnValueAdjustment = null;

//        // for used of import
//        TextReader textReader;
//        StreamReader streamReader;
//        bool canSeekStreamPosition;
//        bool useStreamReader;
//        bool _calculateBytes = false;
//        long _currentBytes = 0L;
//        long _totalBytes = 0L;

//        string _delimiter = string.Empty;

//        int _initialMaxStringBuilderCapacity = 16 * 1024 * 1024;

//        enum NextImportAction
//        {
//            Ignore,
//            SetNames,
//            CreateNewDatabase,
//            AppendLine,
//            ChangeDelimiter,
//            AppendLineAndExecute
//        }

//        public Exception LastError { get { return _lastError; } }
//        public string LastErrorSQL { get { return _lastErrorSql; } }

//        /// <summary>
//        /// Gets the information about the connected database.
//        /// </summary>
//        public MySqlDatabase Database { get { return _database; } }
//        /// <summary>
//        /// Gets the information about the connected MySQL server.
//        /// </summary>
//        public MySqlServer Server { get { return _server; } }
//        /// <summary>
//        /// Gets or Sets the instance of MySqlCommand.
//        /// </summary>
//        public MySqlCommand Command { get; set; }

//        public ExportInformations ExportInfo = new ExportInformations();
//        public ImportInformations ImportInfo = new ImportInformations();

//        public delegate void exportProgressChange(object sender, ExportProgressArgs e);
//        public event exportProgressChange ExportProgressChanged;

//        public delegate void exportComplete(object sender, ExportCompleteArgs e);
//        public event exportComplete ExportCompleted;

//        public delegate void importProgressChange(object sender, ImportProgressArgs e);
//        public event importProgressChange ImportProgressChanged;

//        public delegate void importComplete(object sender, ImportCompleteArgs e);
//        public event importComplete ImportCompleted;

//        public delegate void getTotalRowsProgressChange(object sender, GetTotalRowsArgs e);
//        public event getTotalRowsProgressChange GetTotalRowsProgressChanged;

//        /// <summary>
//        /// Provides backup and restore functionality for MySQL databases.
//        /// </summary>
//        public MySqlBackup()
//        {
//            InitializeComponents();
//        }

//        /// <summary>
//        /// Provides backup and restore functionality for MySQL databases.
//        /// </summary>
//        public MySqlBackup(MySqlCommand cmd)
//        {
//            InitializeComponents();
//            Command = cmd;
//        }

//        void InitializeComponents()
//        {
//            _database.GetTotalRowsProgressChanged += _database_GetTotalRowsProgressChanged;

//            timerReport = new Timer();
//            timerReport.Elapsed += timerReport_Elapsed;
//        }

//        void _database_GetTotalRowsProgressChanged(object sender, GetTotalRowsArgs e)
//        {
//            if (GetTotalRowsProgressChanged != null)
//            {
//                GetTotalRowsProgressChanged(this, e);
//            }
//        }

//        #region Export

//        public string ExportToString()
//        {
//            try
//            {
//                string result;
//                using (MemoryStream ms = new MemoryStream())
//                {
//                    // Use StreamWriter directly instead of calling ExportToMemoryStream
//                    using (StreamWriter writer = new StreamWriter(ms, textEncoding, GetOptimalStreamBufferSize(), true)) // leaveOpen: true
//                    {
//                        textWriter = writer;
//                        ExportStart();
//                        textWriter.Flush();
//                        textWriter = null;
//                    }

//                    // Reset position and read the result
//                    ms.Position = 0L;
//                    using (var thisReader = new StreamReader(ms, textEncoding, false, GetOptimalStreamBufferSize()))
//                    {
//                        result = thisReader.ReadToEnd();
//                    }
//                }

//                Export_RaiseCompletionEvent();

//                return result;
//            }
//            catch (Exception ex)
//            {
//                _lastError = ex;
//                Export_RaiseCompletionEvent();
//                throw;
//            }
//            finally
//            {
//                Export_RestoreOriginalVariables();
//            }
//        }

//        public void ExportToFile(string filePath)
//        {
//            try
//            {
//                if (string.IsNullOrEmpty(filePath))
//                    throw new ArgumentNullException(nameof(filePath), "File path cannot be null or empty.");

//                string dir = Path.GetDirectoryName(filePath);
//                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
//                {
//                    Directory.CreateDirectory(dir);
//                }
//                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, GetOptimalStreamBufferSize()))
//                using (StreamWriter writer = new StreamWriter(fileStream, textEncoding, GetOptimalStreamBufferSize()))
//                {
//                    textWriter = writer;
//                    ExportStart();
//                    textWriter = null;
//                }

//                Export_RaiseCompletionEvent();
//            }
//            catch (Exception ex)
//            {
//                _lastError = ex;
//                Export_RaiseCompletionEvent();
//                throw;
//            }
//            finally
//            {
//                Export_RestoreOriginalVariables();
//            }
//        }

//        public void ExportToTextWriter(TextWriter tw)
//        {
//            try
//            {
//                if (tw == null)
//                    throw new ArgumentNullException(nameof(tw), "TextWriter cannot be null.");

//                textWriter = tw;
//                ExportStart();

//                Export_RaiseCompletionEvent();
//            }
//            catch (Exception ex)
//            {
//                _lastError = ex;
//                Export_RaiseCompletionEvent();
//                throw;
//            }
//            finally
//            {
//                Export_RestoreOriginalVariables();
//            }
//        }

//        public void ExportToStream(Stream sm)
//        {
//            try
//            {
//                if (sm == null)
//                    throw new ArgumentNullException(nameof(sm), "Stream cannot be null.");

//                if (sm.CanSeek)
//                    sm.Seek(0, SeekOrigin.Begin);

//                using (var writer = new StreamWriter(sm, textEncoding, GetOptimalStreamBufferSize(), true))
//                {
//                    textWriter = writer;
//                    ExportStart();
//                }

//                textWriter = null;

//                Export_RaiseCompletionEvent();
//            }
//            catch (Exception ex)
//            {
//                _lastError = ex;
//                Export_RaiseCompletionEvent();
//                throw;
//            }
//            finally
//            {
//                Export_RestoreOriginalVariables();
//            }
//        }

//        public void ExportToMemoryStream(MemoryStream ms)
//        {
//            ExportToMemoryStream(ms, true);
//        }

//        public void ExportToMemoryStream(MemoryStream ms, bool resetMemoryStreamPosition)
//        {
//            try
//            {
//                if (ms == null)
//                {
//                    throw new ArgumentNullException(nameof(ms), "MemoryStream cannot be null.");
//                }

//                if (resetMemoryStreamPosition)
//                {
//                    ms.SetLength(0);
//                    ms.Position = 0L;
//                }
//                using (var writer = new StreamWriter(ms, textEncoding, GetOptimalStreamBufferSize(), true)) // leaveOpen: true
//                {
//                    textWriter = writer;
//                    ExportStart();
//                    textWriter.Flush();
//                }

//                textWriter = null;

//                Export_RaiseCompletionEvent();
//            }
//            catch (Exception ex)
//            {
//                _lastError = ex;
//                Export_RaiseCompletionEvent();
//                throw;
//            }
//            finally
//            {
//                Export_RestoreOriginalVariables();
//            }
//        }

//        private int GetOptimalStreamBufferSize()
//        {
//            // Use 1% of MaxSqlLength, between 4 KB and 256 KB
//            int size = ExportInfo.MaxSqlLength / 100;
//            return Math.Max(4096, Math.Min(262144, size)); // Min 4KB, Max 256KB
//        }

//        void ExportStart()
//        {
//            try
//            {
//                Export_InitializeVariables();

//                int stage = 1;

//                while (stage < 11)
//                {
//                    if (stopProcess) break;

//                    switch (stage)
//                    {
//                        case 1: Export_BasicInfo(); break;
//                        case 2: Export_CreateDatabase(); break;
//                        case 3: Export_DocumentHeader(); break;
//                        case 4: Export_TableRows(); break;
//                        case 5: Export_Functions(); break;
//                        case 6: Export_Procedures(); break;
//                        case 7: Export_Events(); break;
//                        case 8: Export_Views(); break;
//                        case 9: Export_Triggers(); break;
//                        case 10: Export_DocumentFooter(); break;
//                        default: break;
//                    }

//                    textWriter.Flush();

//                    stage++;
//                }

//                if (ExportInfo.SetTimeZoneUTC)
//                {
//                    string safeTimeZone = _originalTimeZone.Replace("'", "''");
//                    Command.CommandText = $"/*!40103 SET TIME_ZONE='{safeTimeZone}' */;";
//                    Command.ExecuteNonQuery();
//                }

//                if (stopProcess) processCompletionType = ProcessEndType.Cancelled;
//                else processCompletionType = ProcessEndType.Complete;
//            }
//            catch (Exception ex)
//            {
//                _lastError = ex;
//                StopAllProcess();
//                throw;
//            }
//            finally
//            {
//                ReportEndProcess();
//            }
//        }

//        void Export_InitializeVariables()
//        {
//            if (Command == null)
//            {
//                throw new InvalidOperationException("MySqlCommand is not initialized. Please provide a valid MySqlCommand instance before calling export methods.");
//            }

//            if (Command.Connection == null)
//            {
//                throw new InvalidOperationException("MySqlCommand.Connection is not initialized. Please ensure the MySqlCommand has a valid connection.");
//            }

//            if (Command.Connection.State != System.Data.ConnectionState.Open)
//            {
//                throw new InvalidOperationException("MySqlCommand.Connection is not open. Please open the connection before calling export methods.");
//            }

//            timeStart = DateTime.Now;

//            if (string.IsNullOrEmpty(ExportInfo.ScriptsDelimiter))
//            {
//                throw new Exception("Script delimeter is empty");
//            }

//            if (ExportInfo.ScriptsDelimiter.Trim() == ";")
//            {
//                throw new Exception("Script delimeter cannot be ';'");
//            }

//            if (ExportInfo.SetTimeZoneUTC)
//            {
//                // Cache the timezone
//                Command.CommandText = "SELECT @@session.time_zone;";
//                _originalTimeZone = Command.ExecuteScalar() + "";
//                if (string.IsNullOrEmpty(_originalTimeZone))
//                {
//                    _originalTimeZone = "SYSTEM";
//                }
//                // Important step, set the timezone to UTC 00:00 to ensure consistent timestamp values export
//                // Without this, the exported timestamp value will be shifted by timezone, resulting inaccurancy during import of data
//                Command.CommandText = "/*!40103 SET TIME_ZONE='+00:00' */;";
//                Command.ExecuteNonQuery();
//            }

//            ExportInfo.ScriptsDelimiter = ExportInfo.ScriptsDelimiter.Trim();

//            stopProcess = false;
//            processCompletionType = ProcessEndType.UnknownStatus;
//            currentProcess = ProcessType.Export;
//            _lastError = null;
//            timerReport.Interval = ExportInfo.IntervalForProgressReport;

//            textEncoding = ExportInfo.TextEncoding;

//            _database.GetDatabaseInfo(Command, ExportInfo.GetTotalRowsMode);
//            _server.GetServerInfo(Command);
//            _hasAdjustedValueRule = ExportInfo.TableColumnValueAdjustments != null;
//            _currentTableName = string.Empty;
//            _totalRowsInCurrentTable = 0L;
//            _totalRowsInAllTables = Export_GetTablesToBeExported()
//                .Sum(pair => _database.Tables[pair.Key].TotalRows);
//            _currentRowIndexInCurrentTable = 0;
//            _currentRowIndexInAllTable = 0;
//            _totalTables = 0;
//            _currentTableIndex = 0;

//            _sb = new StringBuilder(Math.Min(ExportInfo.MaxSqlLength, _initialMaxStringBuilderCapacity));
//        }

//        void Export_BasicInfo()
//        {
//            Export_WriteComment(string.Format("MySqlBackup.NET {0}", Version));

//            if (ExportInfo.RecordDumpTime)
//                Export_WriteComment(string.Format("Dump Time: {0}", timeStart.ToString("yyyy-MM-dd HH:mm:ss")));
//            else
//                Export_WriteComment(string.Empty);

//            Export_WriteComment("--------------------------------------");
//            Export_WriteComment(string.Format("Server version {0}", _server.Version));
//            textWriter.WriteLine();
//        }

//        void Export_CreateDatabase()
//        {
//            if (!ExportInfo.AddCreateDatabase && !ExportInfo.AddDropDatabase)
//                return;

//            textWriter.WriteLine();
//            textWriter.WriteLine();
//            if (ExportInfo.AddDropDatabase)
//            {
//                //Export_WriteLine(String.Format("DROP DATABASE `{0}`;", _database.Name));
//                Export_WriteLine(String.Format("DROP DATABASE `{0}`;", QueryExpress.EscapeIdentifier(_database.Name)));
//            }
//            if (ExportInfo.AddCreateDatabase)
//            {
//                Export_WriteLine(_database.CreateDatabaseSQL);
//                //Export_WriteLine(string.Format("USE `{0}`;", _database.Name));
//                Export_WriteLine(string.Format("USE `{0}`;", QueryExpress.EscapeIdentifier(_database.Name)));
//            }
//            textWriter.WriteLine();
//            textWriter.WriteLine();
//        }

//        void Export_DocumentHeader()
//        {
//            textWriter.WriteLine();

//            List<string> lstHeaders = ExportInfo.GetDocumentHeaders(Command);
//            if (lstHeaders.Count > 0)
//            {
//                foreach (string s in lstHeaders)
//                {
//                    Export_WriteLine(s);
//                }

//                textWriter.WriteLine();
//                textWriter.WriteLine();
//            }
//        }

//        void Export_TableRows()
//        {
//            Dictionary<string, string> dicTables = Export_GetTablesToBeExportedReArranged();

//            _totalTables = dicTables.Count;

//            if (ExportInfo.ExportTableStructure || ExportInfo.ExportRows)
//            {
//                if (ExportProgressChanged != null)
//                    timerReport.Start();

//                foreach (KeyValuePair<string, string> kvTable in dicTables)
//                {
//                    if (stopProcess)
//                        return;

//                    string tableName = kvTable.Key;
//                    string selectSQL = kvTable.Value;

//                    bool exclude = Export_ThisTableIsExcluded(tableName);
//                    if (exclude)
//                    {
//                        continue;
//                    }

//                    _currentTableName = tableName;
//                    _currentTableIndex = _currentTableIndex + 1;
//                    _totalRowsInCurrentTable = _database.Tables[tableName].TotalRows;

//                    if (ExportInfo.ExportTableStructure)
//                        Export_TableStructure(tableName);

//                    var excludeRows = Export_ThisRowForTableIsExcluded(tableName);
//                    if (ExportInfo.ExportRows && !excludeRows)
//                        Export_Rows(tableName, selectSQL);
//                }
//            }
//        }

//        bool Export_ThisTableIsExcluded(string tableName)
//        {
//            string tableNameLower = tableName.ToLower();

//            foreach (string blacklistedTable in ExportInfo.ExcludeTables)
//            {
//                if (blacklistedTable.ToLower() == tableNameLower)
//                    return true;
//            }

//            return false;
//        }

//        bool Export_ThisRowForTableIsExcluded(string tableName)
//        {
//            var tableNameLower = tableName.ToLower();

//            foreach (var blacklistedTable in ExportInfo.ExcludeRowsForTables)
//            {
//                if (blacklistedTable.ToLower() == tableNameLower)
//                    return true;
//            }

//            return false;
//        }

//        void Export_TableStructure(string tableName)
//        {
//            if (stopProcess)
//                return;

//            Export_WriteComment(string.Empty);
//            Export_WriteComment(string.Format("Definition of {0}", tableName));
//            Export_WriteComment(string.Empty);

//            textWriter.WriteLine();

//            if (ExportInfo.AddDropTable)
//            {
//                //Export_WriteLine(string.Format("DROP TABLE IF EXISTS `{0}`;", tableName));
//                Export_WriteLine(string.Format("DROP TABLE IF EXISTS `{0}`;", QueryExpress.EscapeIdentifier(tableName)));
//            }

//            if (ExportInfo.ResetAutoIncrement)
//                Export_WriteLine(_database.Tables[tableName].CreateTableSqlWithoutAutoIncrement);
//            else
//                Export_WriteLine(_database.Tables[tableName].CreateTableSql);

//            textWriter.WriteLine();

//            textWriter.Flush();
//        }

//        Dictionary<string, string> Export_GetTablesToBeExportedReArranged()
//        {
//            var dic = Export_GetTablesToBeExported();

//            Dictionary<string, string> dic2 = new Dictionary<string, string>();
//            foreach (var kv in dic)
//            {
//                dic2[kv.Key] = _database.Tables[kv.Key].CreateTableSql;
//            }

//            var lst = Export_ReArrangeDependencies(dic2, "foreign key", "`");
//            dic2 = lst.ToDictionary(k => k, k => dic[k]);
//            return dic2;
//        }

//        private Dictionary<string, string> Export_GetTablesToBeExported()
//        {
//            if (ExportInfo.TablesToBeExportedDic is null ||
//                ExportInfo.TablesToBeExportedDic.Count == 0)
//            {
//                return _database.Tables
//                    .ToDictionary(
//                        table => table.Name,
//                        //table => string.Format("SELECT * FROM `{0}`;", table.Name));
//                        table => string.Format("SELECT * FROM `{0}`;", QueryExpress.EscapeIdentifier(table.Name)));

//            }

//            return ExportInfo.TablesToBeExportedDic
//                .Where(table => _database.Tables.Contains(table.Key))
//                .ToDictionary(pair => pair.Key, pair => pair.Value);
//        }

//        List<string> Export_ReArrangeDependencies(Dictionary<string, string> dic1, string splitKeyword, string keyNameWrapper)
//        {
//            List<string> lst = new List<string>();
//            HashSet<string> index = new HashSet<string>();

//            bool requireLoop = true;

//            while (requireLoop)
//            {
//                requireLoop = false;

//                foreach (var kv in dic1)
//                {
//                    if (index.Contains(kv.Key))
//                        continue;

//                    bool allReferencedAdded = true;

//                    string createSql = kv.Value.ToLower();
//                    string referenceInfo = string.Empty;

//                    bool referenceTaken = false;
//                    if (!string.IsNullOrEmpty(splitKeyword))
//                    {
//                        if (createSql.Contains(string.Format(" {0} ", splitKeyword)))
//                        {
//                            string[] sa = createSql.Split(new string[] { string.Format(" {0} ", splitKeyword) }, StringSplitOptions.RemoveEmptyEntries);
//                            referenceInfo = sa[sa.Length - 1];
//                            referenceTaken = true;
//                        }
//                    }

//                    if (!referenceTaken)
//                        referenceInfo = createSql;

//                    foreach (var kv2 in dic1)
//                    {
//                        if (kv.Key == kv2.Key)
//                            continue;

//                        if (index.Contains(kv2.Key))
//                            continue;

//                        //string _thisTBname = string.Format("{0}{1}{0}", keyNameWrapper, kv2.Key.ToLower());
//                        string _thisTBname = string.Format("{0}{1}{0}", keyNameWrapper, QueryExpress.EscapeIdentifier(kv2.Key.ToLower()));

//                        if (referenceInfo.Contains(_thisTBname))
//                        {
//                            allReferencedAdded = false;
//                            break;
//                        }
//                    }

//                    if (allReferencedAdded)
//                    {
//                        if (!index.Contains(kv.Key))
//                        {
//                            lst.Add(kv.Key);
//                            index.Add(kv.Key);
//                            requireLoop = true;
//                            break;
//                        }
//                    }
//                }
//            }

//            foreach (var kv in dic1)
//            {
//                if (!index.Contains(kv.Key))
//                {
//                    lst.Add(kv.Key);
//                    index.Add(kv.Key);
//                }
//            }

//            return lst;
//        }

//        void Export_Rows(string tableName, string selectSQL)
//        {
//            Export_WriteComment(string.Empty);
//            Export_WriteComment(string.Format("Dumping data for table {0}", tableName));
//            Export_WriteComment(string.Empty);
//            textWriter.WriteLine();
//            Export_WriteLine(string.Format("/*!40000 ALTER TABLE `{0}` DISABLE KEYS */;", QueryExpress.EscapeIdentifier(tableName)));

//            if (ExportInfo.WrapWithinTransaction)
//                Export_WriteLine("START TRANSACTION;");

//            if (ExportInfo.EnableLockTablesWrite)
//                Export_WriteLine($"LOCK TABLES `{QueryExpress.EscapeIdentifier(tableName)}` WRITE;");

//            Export_RowsData(tableName, selectSQL);

//            if (ExportInfo.EnableLockTablesWrite)
//                Export_WriteLine($"UNLOCK TABLES;");

//            if (ExportInfo.WrapWithinTransaction)
//                Export_WriteLine("COMMIT;");

//            Export_WriteLine(string.Format("/*!40000 ALTER TABLE `{0}` ENABLE KEYS */;", QueryExpress.EscapeIdentifier(tableName)));

//            textWriter.WriteLine();
//            textWriter.Flush();
//        }

//        void Export_RowsData(string tableName, string selectSQL)
//        {
//            _currentRowIndexInCurrentTable = 0L;

//            if (_hasAdjustedValueRule)
//            {
//                if (ExportInfo.TableColumnValueAdjustments.ContainsKey(tableName))
//                {
//                    _currentTableHasAdjustedValueRule = true;
//                    _currentTableColumnValueAdjustment = ExportInfo.TableColumnValueAdjustments[tableName];
//                }
//                else
//                {
//                    _currentTableHasAdjustedValueRule = false;
//                    _currentTableColumnValueAdjustment = null;
//                }
//            }

//            switch (ExportInfo.RowsExportMode)
//            {
//                case RowsDataExportMode.Insert:
//                case RowsDataExportMode.InsertIgnore:
//                case RowsDataExportMode.Replace:
//                    Export_RowsData_Insert_Ignore_Replace(tableName, selectSQL, ExportInfo.RowsExportMode);
//                    break;
//                case RowsDataExportMode.Update:
//                    Export_RowsData_Update(tableName, selectSQL);
//                    break;
//                case RowsDataExportMode.OnDuplicateKeyUpdate:
//                    Export_RowsData_OnDuplicateKeyUpdate(tableName, selectSQL);
//                    break;
//            }
//        }

//        void Export_RowsData_Insert_Ignore_Replace(string tableName, string selectSQL, RowsDataExportMode _mode)
//        {
//            MySqlTable table = _database.Tables[tableName];

//            string insertStatementHeader = null;
//            long insertStatementHeaderByteLength = 0L;
//            long currentSqlByteLength = 0L;  // Track bytes, not characters
//            bool isNewSQLStatement = true;
//            bool isFirstSQLValueBlock = true;
//            bool isFirstRowRead = true;

//            // Pre-calculate byte length of line break
//            int lineBreakByteLength = textEncoding.GetByteCount(Environment.NewLine);

//            Command.CommandText = selectSQL;
//            _sb.Clear();
//            StringBuilder sbValue = new StringBuilder();

//            using (MySqlDataReader rdr = Command.ExecuteReader())
//            {
//                while (rdr.Read())
//                {
//                    if (stopProcess)
//                        return;

//                    _currentRowIndexInAllTable++;
//                    _currentRowIndexInCurrentTable++;

//                    if (insertStatementHeader == null)
//                    {
//                        insertStatementHeader = Export_GetInsertStatementHeader(_mode, tableName, rdr);
//                        insertStatementHeaderByteLength = QueryExpress.EstimateByteCount(insertStatementHeader, textEncoding);
//                    }

//                    // 3rd level temporary data cache
//                    // we can't append this directly to the StringBuilder yet
//                    // we still need to use it to calculate the length of bytes
//                    // preventing the SQL statement from being too long to hit MySQL server's max_allowed_packet
//                    // or limit set by ExportInfo.MaxSqlLength
//                    sbValue.Clear();
//                    Export_GetValueString(rdr, table, sbValue);

//                    // Calculate actual byte length of the value string
//                    string valueString = sbValue.ToString();
//                    long sqlValueByteLength = QueryExpress.EstimateByteCount(valueString, textEncoding);

//                    // Calculate forecast byte length
//                    long forecastSqlStatementByteLength = currentSqlByteLength + sqlValueByteLength + 1L; // +1 for comma or semicolon

//                    if (ExportInfo.InsertLineBreakBetweenInserts)
//                        forecastSqlStatementByteLength += lineBreakByteLength;

//                    if (forecastSqlStatementByteLength > ExportInfo.MaxSqlLength)
//                    {
//                        isNewSQLStatement = true;
//                    }

//                    if (isNewSQLStatement)
//                    {
//                        isNewSQLStatement = false;
//                        isFirstSQLValueBlock = true;

//                        if (isFirstRowRead)
//                        {
//                            isFirstRowRead = false;
//                        }
//                        else
//                        {
//                            textWriter.Write(_sb.ToString());
//                            textWriter.WriteLine(";");
//                            currentSqlByteLength = 0L;
//                            _sb.Clear();
//                        }

//                        _sb.Append(insertStatementHeader);
//                        currentSqlByteLength = insertStatementHeaderByteLength;

//                        if (ExportInfo.InsertLineBreakBetweenInserts)
//                        {
//                            _sb.AppendLine();
//                            currentSqlByteLength += lineBreakByteLength;
//                        }
//                    }

//                    if (isFirstSQLValueBlock)
//                    {
//                        isFirstSQLValueBlock = false;
//                    }
//                    else
//                    {
//                        _sb.Append(",");
//                        currentSqlByteLength += 1;  // comma is always 1 byte in UTF-8

//                        if (ExportInfo.InsertLineBreakBetweenInserts)
//                        {
//                            _sb.AppendLine();
//                            currentSqlByteLength += lineBreakByteLength;
//                        }
//                    }

//                    _sb.Append(valueString);
//                    currentSqlByteLength += sqlValueByteLength;
//                }
//            }

//            if (!isFirstRowRead)
//            {
//                textWriter.Write(_sb.ToString());
//                textWriter.WriteLine(";");
//                _sb.Clear();
//            }

//            textWriter.Flush();
//        }

//        void Export_RowsData_OnDuplicateKeyUpdate(string tableName, string selectSQL)
//        {
//            MySqlTable table = _database.Tables[tableName];

//            bool allPrimaryField = true;

//            foreach (var col in table.Columns)
//            {
//                if (!col.IsPrimaryKey)
//                {
//                    allPrimaryField = false;
//                    break;
//                }
//            }

//            if (allPrimaryField)
//            {
//                Export_RowsData_Insert_Ignore_Replace(tableName, selectSQL, RowsDataExportMode.Insert);
//                return;
//            }

//            string insertStatementHeader = null;

//            Command.CommandText = selectSQL;

//            _sb.Clear();

//            using (MySqlDataReader rdr = Command.ExecuteReader())
//            {
//                while (rdr.Read())
//                {
//                    if (stopProcess)
//                        return;

//                    _sb.Clear();

//                    _currentRowIndexInAllTable = _currentRowIndexInAllTable + 1;
//                    _currentRowIndexInCurrentTable = _currentRowIndexInCurrentTable + 1;

//                    if (insertStatementHeader == null)
//                        insertStatementHeader = Export_GetInsertStatementHeader(RowsDataExportMode.Insert, tableName, rdr);

//                    _sb.Append(insertStatementHeader);
//                    Export_GetValueString(rdr, table, _sb);
//                    _sb.Append(" ON DUPLICATE KEY UPDATE ");
//                    Export_GetUpdateString(rdr, table, _sb);
//                    _sb.Append(";");
//                    textWriter.WriteLine(_sb.ToString());
//                    _sb.Clear();
//                }
//            }

//            textWriter.Flush();
//        }

//        void Export_RowsData_Update(string tableName, string selectSQL)
//        {
//            MySqlTable table = _database.Tables[tableName];

//            bool allPrimaryField = true;
//            foreach (var col in table.Columns)
//            {
//                if (!col.IsPrimaryKey)
//                {
//                    allPrimaryField = false;
//                    break;
//                }
//            }

//            if (allPrimaryField)
//            {
//                Export_RowsData_Insert_Ignore_Replace(tableName, selectSQL, RowsDataExportMode.Insert);
//                return;
//            }

//            bool allNonPrimaryField = true;
//            foreach (var col in table.Columns)
//            {
//                if (col.IsPrimaryKey)
//                {
//                    allNonPrimaryField = false;
//                    break;
//                }
//            }

//            if (allNonPrimaryField)
//            {
//                Export_RowsData_Insert_Ignore_Replace(tableName, selectSQL, RowsDataExportMode.Insert);
//                return;
//            }

//            Command.CommandText = selectSQL;

//            _sb.Clear();

//            using (MySqlDataReader rdr = Command.ExecuteReader())
//            {
//                while (rdr.Read())
//                {
//                    if (stopProcess)
//                        return;

//                    _currentRowIndexInAllTable = _currentRowIndexInAllTable + 1;
//                    _currentRowIndexInCurrentTable = _currentRowIndexInCurrentTable + 1;

//                    _sb.Clear();

//                    _sb.Append("UPDATE `");
//                    //_sb.Append(tableName);
//                    _sb.Append(QueryExpress.EscapeIdentifier(tableName));
//                    _sb.Append("` SET ");

//                    Export_GetUpdateString(rdr, table, _sb);

//                    _sb.Append(" WHERE ");

//                    Export_GetConditionString(rdr, table, _sb);

//                    _sb.Append(";");

//                    textWriter.WriteLine(_sb.ToString());
//                }
//            }

//            textWriter.Flush();
//        }

//        string Export_GetInsertStatementHeader(RowsDataExportMode rowsExportMode, string tableName, MySqlDataReader rdr)
//        {
//            StringBuilder sb = new StringBuilder();

//            if (rowsExportMode == RowsDataExportMode.Insert)
//                sb.Append("INSERT INTO `");
//            else if (rowsExportMode == RowsDataExportMode.InsertIgnore)
//                sb.Append("INSERT IGNORE INTO `");
//            else if (rowsExportMode == RowsDataExportMode.Replace)
//                sb.Append("REPLACE INTO `");

//            //sb.Append(tableName);
//            sb.Append(QueryExpress.EscapeIdentifier(tableName));

//            sb.Append("`(");

//            for (int i = 0; i < rdr.FieldCount; i++)
//            {
//                string _colname = rdr.GetName(i);

//                if (_database.Tables[tableName].Columns[_colname].IsGeneratedColumn)
//                    continue;

//                if (i > 0)
//                    sb.Append(",");
//                sb.Append("`");

//                //sb.Append(rdr.GetName(i));
//                sb.Append(QueryExpress.EscapeIdentifier(rdr.GetName(i)));

//                sb.Append("`");
//            }

//            sb.Append(") VALUES");
//            return sb.ToString();
//        }

//        void Export_GetValueString(MySqlDataReader rdr, MySqlTable table, StringBuilder sb)
//        {
//            bool isfirst = true;

//            for (int i = 0; i < rdr.FieldCount; i++)
//            {
//                string columnName = rdr.GetName(i);

//                if (table.Columns[columnName].IsGeneratedColumn)
//                    continue;

//                if (isfirst)
//                {
//                    isfirst = false;
//                    sb.AppendFormat("(");
//                }
//                else
//                {
//                    sb.AppendFormat(",");
//                }

//                object ob = rdr[i];
//                var col = table.Columns[columnName];

//                // perform value adjustment
//                if (_currentTableHasAdjustedValueRule && _currentTableColumnValueAdjustment.TryGetValue(columnName, out var adjustFunc))
//                {
//                    ob = adjustFunc(ob);
//                }

//                QueryExpress.ConvertToSqlFormat(sb, ob, col, true, true);
//            }

//            sb.AppendFormat(")");
//        }

//        void Export_GetUpdateString(MySqlDataReader rdr, MySqlTable table, StringBuilder sb)
//        {
//            bool isFirst = true;

//            for (int i = 0; i < rdr.FieldCount; i++)
//            {
//                string columnName = rdr.GetName(i);

//                var col = table.Columns[columnName];

//                if (col.IsGeneratedColumn)
//                    continue;

//                if (!col.IsPrimaryKey)
//                {
//                    if (isFirst)
//                        isFirst = false;
//                    else
//                        sb.Append(",");

//                    sb.Append("`");

//                    //sb.Append(columnName);
//                    sb.Append(QueryExpress.EscapeIdentifier(columnName));

//                    sb.Append("`=");

//                    var ob = rdr[i];

//                    if (_currentTableHasAdjustedValueRule && _currentTableColumnValueAdjustment.TryGetValue(columnName, out var adjustFunc))
//                    {
//                        ob = adjustFunc(ob);
//                    }

//                    QueryExpress.ConvertToSqlFormat(sb, ob, col, true, true);
//                }
//            }
//        }

//        void Export_GetConditionString(MySqlDataReader rdr, MySqlTable table, StringBuilder sb)
//        {
//            bool isFirst = true;

//            for (int i = 0; i < rdr.FieldCount; i++)
//            {
//                string columnName = rdr.GetName(i);

//                var col = table.Columns[columnName];

//                if (col.IsPrimaryKey)
//                {
//                    if (isFirst)
//                        isFirst = false;
//                    else
//                        sb.Append(" and ");

//                    sb.Append("`");

//                    //sb.Append(columnName);
//                    sb.Append(QueryExpress.EscapeIdentifier(columnName));

//                    sb.Append("`=");

//                    var ob = rdr[i];

//                    if (_currentTableHasAdjustedValueRule && _currentTableColumnValueAdjustment.TryGetValue(columnName, out var adjustFunc))
//                    {
//                        ob = adjustFunc(ob);
//                    }

//                    QueryExpress.ConvertToSqlFormat(sb, ob, col, true, true);
//                }
//            }
//        }

//        void Export_Procedures()
//        {
//            if (!ExportInfo.ExportProcedures || _database.Procedures.Count == 0)
//                return;

//            Export_WriteComment(string.Empty);
//            Export_WriteComment("Dumping procedures");
//            Export_WriteComment(string.Empty);
//            textWriter.WriteLine();

//            foreach (MySqlProcedure procedure in _database.Procedures)
//            {
//                if (stopProcess)
//                    return;

//                if (procedure.CreateProcedureSQLWithoutDefiner.Trim().Length == 0 ||
//                    procedure.CreateProcedureSQL.Trim().Length == 0)
//                    continue;

//                //Export_WriteLine(string.Format("DROP PROCEDURE IF EXISTS `{0}`;", procedure.Name));
//                Export_WriteLine(string.Format("DROP PROCEDURE IF EXISTS `{0}`;", QueryExpress.EscapeIdentifier(procedure.Name)));

//                Export_WriteLine("DELIMITER " + ExportInfo.ScriptsDelimiter);

//                if (ExportInfo.ExportRoutinesWithoutDefiner)
//                    Export_WriteLine(procedure.CreateProcedureSQLWithoutDefiner + " " + ExportInfo.ScriptsDelimiter);
//                else
//                    Export_WriteLine(procedure.CreateProcedureSQL + " " + ExportInfo.ScriptsDelimiter);

//                Export_WriteLine("DELIMITER ;");
//                textWriter.WriteLine();
//            }
//            textWriter.Flush();
//        }

//        void Export_Functions()
//        {
//            if (!ExportInfo.ExportFunctions || _database.Functions.Count == 0)
//                return;

//            Export_WriteComment(string.Empty);
//            Export_WriteComment("Dumping functions");
//            Export_WriteComment(string.Empty);
//            textWriter.WriteLine();

//            foreach (MySqlFunction function in _database.Functions)
//            {
//                if (stopProcess)
//                    return;

//                if (function.CreateFunctionSQL.Trim().Length == 0 ||
//                    function.CreateFunctionSQLWithoutDefiner.Trim().Length == 0)
//                    continue;

//                //Export_WriteLine(string.Format("DROP FUNCTION IF EXISTS `{0}`;", function.Name));
//                Export_WriteLine(string.Format("DROP FUNCTION IF EXISTS `{0}`;", QueryExpress.EscapeIdentifier(function.Name)));

//                Export_WriteLine("DELIMITER " + ExportInfo.ScriptsDelimiter);

//                if (ExportInfo.ExportRoutinesWithoutDefiner)
//                    Export_WriteLine(function.CreateFunctionSQLWithoutDefiner + " " + ExportInfo.ScriptsDelimiter);
//                else
//                    Export_WriteLine(function.CreateFunctionSQL + " " + ExportInfo.ScriptsDelimiter);

//                Export_WriteLine("DELIMITER ;");
//                textWriter.WriteLine();
//            }

//            textWriter.Flush();
//        }

//        void Export_Views()
//        {
//            if (!ExportInfo.ExportViews || _database.Views.Count == 0)
//                return;

//            // ReArrange Views
//            Dictionary<string, string> dicView_Create = new Dictionary<string, string>();
//            foreach (var view in _database.Views)
//            {
//                dicView_Create[view.Name] = view.CreateViewSQL;
//            }

//            var lst = Export_ReArrangeDependencies(dicView_Create, null, "`");

//            Export_WriteComment(string.Empty);
//            Export_WriteComment("Dumping views");
//            Export_WriteComment(string.Empty);
//            textWriter.WriteLine();

//            foreach (var viewname in lst)
//            {
//                if (stopProcess)
//                    return;

//                var view = _database.Views[viewname];

//                if (view.CreateViewSQL.Trim().Length == 0 ||
//                    view.CreateViewSQLWithoutDefiner.Trim().Length == 0)
//                    continue;

//                //Export_WriteLine(string.Format("DROP TABLE IF EXISTS `{0}`;", view.Name));
//                //Export_WriteLine(string.Format("DROP VIEW IF EXISTS `{0}`;", view.Name));

//                Export_WriteLine(string.Format("DROP TABLE IF EXISTS `{0}`;", QueryExpress.EscapeIdentifier(view.Name)));
//                Export_WriteLine(string.Format("DROP VIEW IF EXISTS `{0}`;", QueryExpress.EscapeIdentifier(view.Name)));

//                if (ExportInfo.ExportRoutinesWithoutDefiner)
//                    Export_WriteLine(view.CreateViewSQLWithoutDefiner);
//                else
//                    Export_WriteLine(view.CreateViewSQL);

//                textWriter.WriteLine();
//            }

//            textWriter.WriteLine();
//            textWriter.Flush();
//        }

//        void Export_Events()
//        {
//            if (!ExportInfo.ExportEvents || _database.Events.Count == 0)
//                return;

//            Export_WriteComment(string.Empty);
//            Export_WriteComment("Dumping events");
//            Export_WriteComment(string.Empty);
//            textWriter.WriteLine();

//            foreach (MySqlEvent e in _database.Events)
//            {
//                if (stopProcess)
//                    return;

//                if (e.CreateEventSql.Trim().Length == 0 ||
//                    e.CreateEventSqlWithoutDefiner.Trim().Length == 0)
//                    continue;

//                //Export_WriteLine(string.Format("DROP EVENT IF EXISTS `{0}`;", e.Name));
//                Export_WriteLine(string.Format("DROP EVENT IF EXISTS `{0}`;", QueryExpress.EscapeIdentifier(e.Name)));

//                Export_WriteLine("DELIMITER " + ExportInfo.ScriptsDelimiter);

//                if (ExportInfo.ExportRoutinesWithoutDefiner)
//                    Export_WriteLine(e.CreateEventSqlWithoutDefiner + " " + ExportInfo.ScriptsDelimiter);
//                else
//                    Export_WriteLine(e.CreateEventSql + " " + ExportInfo.ScriptsDelimiter);

//                Export_WriteLine("DELIMITER ;");
//                textWriter.WriteLine();
//            }

//            textWriter.Flush();
//        }

//        void Export_Triggers()
//        {
//            if (!ExportInfo.ExportTriggers ||
//                _database.Triggers.Count == 0)
//                return;

//            Export_WriteComment(string.Empty);
//            Export_WriteComment("Dumping triggers");
//            Export_WriteComment(string.Empty);
//            textWriter.WriteLine();

//            foreach (MySqlTrigger trigger in _database.Triggers)
//            {
//                if (stopProcess)
//                    return;

//                var createTriggerSQL = trigger.CreateTriggerSQL.Trim();
//                var createTriggerSQLWithoutDefiner = trigger.CreateTriggerSQLWithoutDefiner.Trim();
//                if (createTriggerSQL.Length == 0 ||
//                    createTriggerSQLWithoutDefiner.Length == 0)
//                    continue;

//                //Export_WriteLine(string.Format("DROP TRIGGER /*!50030 IF EXISTS */ `{0}`;", trigger.Name));
//                Export_WriteLine(string.Format("DROP TRIGGER /*!50030 IF EXISTS */ `{0}`;", QueryExpress.EscapeIdentifier(trigger.Name)));

//                Export_WriteLine("DELIMITER " + ExportInfo.ScriptsDelimiter);

//                if (ExportInfo.ExportRoutinesWithoutDefiner)
//                    Export_WriteLine(createTriggerSQLWithoutDefiner + " " + ExportInfo.ScriptsDelimiter);
//                else
//                    Export_WriteLine(createTriggerSQL + " " + ExportInfo.ScriptsDelimiter);

//                Export_WriteLine("DELIMITER ;");
//                textWriter.WriteLine();
//            }

//            textWriter.Flush();
//        }

//        void Export_DocumentFooter()
//        {
//            textWriter.WriteLine();

//            List<string> lstFooters = ExportInfo.GetDocumentFooters();
//            if (lstFooters.Count > 0)
//            {
//                foreach (string s in lstFooters)
//                {
//                    Export_WriteLine(s);
//                }
//            }

//            timeEnd = DateTime.Now;

//            if (ExportInfo.RecordDumpTime)
//            {
//                TimeSpan ts = timeEnd - timeStart;

//                textWriter.WriteLine();
//                textWriter.WriteLine();
//                Export_WriteComment(string.Format("Dump completed on {0}", timeEnd.ToString("yyyy-MM-dd HH:mm:ss")));
//                Export_WriteComment(string.Format("Total time: {0}:{1}:{2}:{3}:{4} (d:h:m:s:ms)", ts.Days, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds));
//            }

//            textWriter.Flush();
//        }

//        void Export_WriteComment(string text)
//        {
//            if (ExportInfo.EnableComment)
//                Export_WriteLine(string.Format("-- {0}", text));
//        }

//        void Export_WriteLine(string text)
//        {
//            textWriter.WriteLine(text);
//        }

//        void Export_RaiseCompletionEvent()
//        {
//            if (ExportCompleted != null)
//            {
//                ExportCompleteArgs arg = new ExportCompleteArgs(timeStart, timeEnd, processCompletionType, _lastError);
//                ExportCompleted(this, arg);
//            }
//        }

//        void Export_RestoreOriginalVariables()
//        {
//            try
//            {
//                Command.CommandText = $"SET @@session.time_zone= '{_originalTimeZone}';";
//                Command.ExecuteNonQuery();
//            }
//            catch { }
//        }

//        #endregion

//        #region Import

//        public void ImportFromString(string sqldumptext)
//        {
//            try
//            {
//                textReader = null;
//                useStreamReader = true;

//                using (MemoryStream ms = new MemoryStream())
//                {
//                    using (StreamWriter thisWriter = new StreamWriter(ms))
//                    {
//                        thisWriter.Write(sqldumptext);
//                        thisWriter.Flush();

//                        ms.Position = 0L;

//                        if (_calculateBytes)
//                            _totalBytes = ms.Length;

//                        using (var sr = new StreamReader(ms))
//                        {
//                            streamReader = sr;
//                            canSeekStreamPosition = streamReader.BaseStream.CanSeek;
//                            Import_Start();
//                        }
//                    }
//                }

//                Import_RaiseCompletionEvent();
//            }
//            catch (Exception ex)
//            {
//                _lastError = ex;
//                Import_RaiseCompletionEvent();
//                throw;
//            }
//        }

//        public void ImportFromFile(string filePath)
//        {
//            try
//            {
//                textReader = null;
//                useStreamReader = true;

//                if (_calculateBytes)
//                {
//                    System.IO.FileInfo fi = new FileInfo(filePath);
//                    _totalBytes = fi.Length;
//                }

//                using (var sr = new StreamReader(filePath))
//                {
//                    streamReader = sr;
//                    canSeekStreamPosition = streamReader.BaseStream.CanSeek;
//                    Import_Start();
//                }

//                Import_RaiseCompletionEvent();
//            }
//            catch (Exception ex)
//            {
//                _lastError = ex;
//                Import_RaiseCompletionEvent();
//                throw;
//            }
//        }

//        public void ImportFromMemoryStream(MemoryStream ms)
//        {
//            ImportFromMemoryStream(ms, 0L);
//        }

//        public void ImportFromMemoryStream(MemoryStream ms, long bytesTotal)
//        {
//            try
//            {
//                if (ms == null)
//                    throw new ArgumentNullException(nameof(ms), "MemoryStream cannot be null.");

//                if (_calculateBytes)
//                {
//                    if (bytesTotal < 0L)
//                        throw new ArgumentException("Total bytes cannot be negative.", nameof(bytesTotal));
//                }

//                if (ms.Length == 0)
//                    throw new ArgumentException("MemoryStream is empty.", nameof(ms));

//                textReader = null;
//                useStreamReader = true;

//                ms.Position = 0;

//                if (_calculateBytes)
//                {
//                    if (bytesTotal > 0L)
//                    {
//                        _totalBytes = bytesTotal;
//                    }
//                    else
//                    {
//                        _totalBytes = ms.Length;
//                    }
//                }

//                using (var sr = new StreamReader(ms))
//                {
//                    streamReader = sr;
//                    canSeekStreamPosition = streamReader.BaseStream.CanSeek;
//                    Import_Start();
//                }

//                Import_RaiseCompletionEvent();
//            }
//            catch (Exception ex)
//            {
//                _lastError = ex;
//                Import_RaiseCompletionEvent();
//                throw;
//            }
//        }

//        public void ImportFromStream(Stream sm)
//        {
//            ImportFromStream(sm, 0L);
//        }

//        public void ImportFromStream(Stream sm, long bytesTotal)
//        {
//            try
//            {
//                if (sm == null)
//                    throw new ArgumentNullException(nameof(sm), "Stream cannot be null.");

//                if (bytesTotal < 0L)
//                    throw new ArgumentException("Total bytes cannot be negative.", nameof(bytesTotal));

//                if (sm.CanSeek)
//                {
//                    if (sm.Length == 0)
//                        throw new ArgumentException("Stream is empty.", nameof(sm));

//                    sm.Seek(0, SeekOrigin.Begin);

//                    if (_calculateBytes && bytesTotal == 0L)
//                    {
//                        _totalBytes = sm.Length;
//                    }
//                }

//                if (bytesTotal > 0L)
//                {
//                    _totalBytes = bytesTotal;
//                }

//                textReader = null;
//                useStreamReader = true;

//                using (var sr = new StreamReader(sm))
//                {
//                    streamReader = sr;
//                    canSeekStreamPosition = streamReader.BaseStream.CanSeek;
//                    Import_Start();
//                }

//                Import_RaiseCompletionEvent();
//            }
//            catch (Exception ex)
//            {
//                _lastError = ex;
//                Import_RaiseCompletionEvent();
//                throw;
//            }
//        }

//        public void ImportFromTextReader(TextReader tr)
//        {
//            ImportFromTextReader(tr, 0L);
//        }

//        public void ImportFromTextReader(TextReader tr, long bytesTotal)
//        {
//            try
//            {
//                if (tr == null)
//                    throw new ArgumentNullException(nameof(tr), "TextReader cannot be null.");

//                if (_calculateBytes)
//                {
//                    if (bytesTotal < 0L)
//                        throw new ArgumentException("Total bytes cannot be negative.", nameof(bytesTotal));

//                    if (bytesTotal > 0L)
//                        _totalBytes = bytesTotal;
//                }

//                useStreamReader = false;
//                streamReader = null;
//                canSeekStreamPosition = false;

//                textReader = tr;

//                Import_Start();

//                Import_RaiseCompletionEvent();
//            }
//            catch (Exception ex)
//            {
//                _lastError = ex;
//                Import_RaiseCompletionEvent();
//                throw;
//            }
//        }

//        void Import_Start()
//        {
//            Import_InitializeVariables();

//            try
//            {
//                string line = string.Empty;

//                while (line != null)
//                {
//                    if (stopProcess)
//                    {
//                        processCompletionType = ProcessEndType.Cancelled;
//                        break;
//                    }

//                    try
//                    {
//                        if (useStreamReader)
//                            line = Import_GetLineStreamReader();
//                        else
//                            line = Import_GetLineTextReader();

//                        if (line == null)
//                            break;

//                        if (line.Length == 0)
//                            continue;

//                        Import_ProcessLine(line);
//                    }
//                    catch (Exception ex)
//                    {
//                        line = string.Empty;
//                        _lastError = ex;
//                        _lastErrorSql = _sb.ToString();

//                        if (!string.IsNullOrEmpty(ImportInfo.ErrorLogFile))
//                        {
//                            File.AppendAllText(ImportInfo.ErrorLogFile, ex.Message + Environment.NewLine + Environment.NewLine + _lastErrorSql + Environment.NewLine + Environment.NewLine);
//                        }

//                        _sb.Clear();

//                        //GC.Collect();

//                        if (!ImportInfo.IgnoreSqlError)
//                        {
//                            StopAllProcess();
//                            throw;
//                        }
//                    }
//                }
//            }
//            finally
//            {
//                ReportEndProcess();
//            }
//        }

//        void Import_InitializeVariables()
//        {
//            if (Command == null)
//            {
//                throw new Exception("MySqlCommand is not initialized. Object not set to an instance of an object.");
//            }

//            if (Command.Connection == null)
//            {
//                throw new Exception("MySqlCommand.Connection is not initialized. Object not set to an instance of an object.");
//            }

//            if (Command.Connection.State != System.Data.ConnectionState.Open)
//            {
//                throw new Exception("MySqlCommand.Connection is not opened.");
//            }

//            //_createViewDetected = false;
//            //_dicImportRoutines = new Dictionary<string, bool>();
//            stopProcess = false;
//            //GetSHA512HashFromPassword(ImportInfo.EncryptionPassword);
//            _lastError = null;
//            timeStart = DateTime.Now;
//            _currentBytes = 0L;
//            textEncoding = ImportInfo.TextEncoding;
//            _sb = new StringBuilder(_initialMaxStringBuilderCapacity);

//            currentProcess = ProcessType.Import;
//            processCompletionType = ProcessEndType.Complete;
//            _delimiter = ";";
//            _lastErrorSql = string.Empty;

//            _calculateBytes = ImportProgressChanged != null;

//            if (ImportProgressChanged != null)
//                timerReport.Start();

//        }

//        string Import_GetLineStreamReader()
//        {
//            long startPosition = 0L;

//            // Capture starting position for seekable streams
//            if (canSeekStreamPosition)
//            {
//                startPosition = streamReader.BaseStream.Position;
//            }

//            string line = streamReader.ReadLine();
//            if (line == null)
//                return null;

//            // Calculate bytes read when progress reporting is enabled
//            if (ImportProgressChanged != null)
//            {
//                if (canSeekStreamPosition)
//                {
//                    // Use stream position for seekable streams
//                    long endPosition = streamReader.BaseStream.Position;
//                    _currentBytes += (endPosition - startPosition);
//                }
//                else
//                {
//                    // Hybrid approach for non-seekable streams
//                    string lineWithTerminator = line + Environment.NewLine;
//                    _currentBytes += QueryExpress.EstimateByteCount(lineWithTerminator, textEncoding);
//                }
//            }

//            line = line.Trim();
//            if (Import_IsEmptyLine(line))
//                return string.Empty;

//            return line;
//        }

//        string Import_GetLineTextReader()
//        {
//            string line = textReader.ReadLine();

//            if (line == null)
//                return null;

//            if (ImportProgressChanged != null)
//            {
//                string lineWithTerminator = line + Environment.NewLine;
//                _currentBytes += QueryExpress.EstimateByteCount(lineWithTerminator, textEncoding);
//            }

//            line = line.Trim();

//            if (Import_IsEmptyLine(line))
//            {
//                return string.Empty;
//            }

//            return line;
//        }

//        void Import_ProcessLine(string line)
//        {
//            NextImportAction nextAction = Import_AnalyseNextAction(line);

//            switch (nextAction)
//            {
//                case NextImportAction.Ignore:
//                    break;
//                case NextImportAction.AppendLine:
//                    Import_AppendLine(line);
//                    break;
//                case NextImportAction.ChangeDelimiter:
//                    Import_ChangeDelimiter(line);
//                    break;
//                case NextImportAction.AppendLineAndExecute:
//                    Import_AppendLineAndExecute(line);
//                    break;
//                default:
//                    break;
//            }
//        }

//        NextImportAction Import_AnalyseNextAction(string line)
//        {
//            if (line == null)
//                return NextImportAction.Ignore;

//            if (line == string.Empty)
//                return NextImportAction.Ignore;

//            if (line.StartsWith("DELIMITER ", StringComparison.OrdinalIgnoreCase))
//                return NextImportAction.ChangeDelimiter;

//            if (line.EndsWith(_delimiter))
//                return NextImportAction.AppendLineAndExecute;

//            return NextImportAction.AppendLine;
//        }

//        void Import_AppendLine(string line)
//        {
//            _sb.AppendLine(line);
//        }

//        void Import_ChangeDelimiter(string line)
//        {
//            string nextDelimiter = line.Substring(9);
//            _delimiter = nextDelimiter.Replace(" ", string.Empty);
//        }

//        void Import_AppendLineAndExecute(string line)
//        {
//            _sb.Append(line);

//            string _query = _sb.ToString();

//            if (_delimiter != ";")
//            {
//                string trimmed = _query.TrimEnd();
//                int lastIndex = trimmed.LastIndexOf(_delimiter);
//                _query = lastIndex != -1 ? trimmed.Remove(lastIndex, _delimiter.Length) : trimmed;
//            }

//            Command.CommandText = _query;
//            Command.ExecuteNonQuery();

//            //if (_delimiter != ";")
//            //{
//            //    _mySqlScript.Query = $"DELIMITER {_delimiter}{Environment.NewLine}{_query}";
//            //    _mySqlScript.Delimiter = _delimiter;
//            //    _mySqlScript.Execute();
//            //}
//            //else
//            //{
//            //    Command.CommandText = _query;
//            //    Command.ExecuteNonQuery();
//            //}

//            _sb.Clear();

//            //GC.Collect();
//        }

//        bool Import_IsEmptyLine(string line)
//        {
//            if (line == null)
//                return true;
//            if (line == string.Empty)
//                return true;
//            if (line.Length == 0)
//                return true;
//            if (line.StartsWith("--"))
//                return true;
//            if (line == Environment.NewLine)
//                return true;
//            if (line == "\r")
//                return true;
//            if (line == "\n")
//                return true;
//            if (line == "\r\n")
//                return true;

//            return false;
//        }

//        void Import_RaiseCompletionEvent()
//        {
//            if (ImportCompleted != null)
//            {
//                MySqlBackup.ProcessEndType completedType = ProcessEndType.UnknownStatus;

//                switch (processCompletionType)
//                {
//                    case ProcessEndType.Complete:
//                        completedType = MySqlBackup.ProcessEndType.Complete;
//                        break;
//                    case ProcessEndType.Error:
//                        completedType = MySqlBackup.ProcessEndType.Error;
//                        break;
//                    case ProcessEndType.Cancelled:
//                        completedType = MySqlBackup.ProcessEndType.Cancelled;
//                        break;
//                }

//                ImportCompleteArgs arg = new ImportCompleteArgs(completedType, timeStart, timeEnd, _lastError);
//                ImportCompleted(this, arg);
//            }
//        }

//        #endregion

//        void ReportEndProcess()
//        {
//            timeEnd = DateTime.Now;

//            StopAllProcess();

//            if (currentProcess == ProcessType.Export)
//            {
//                ReportProgress();
//            }
//            else if (currentProcess == ProcessType.Import)
//            {
//                _currentBytes = _totalBytes;

//                ReportProgress();
//            }
//        }

//        void timerReport_Elapsed(object sender, ElapsedEventArgs e)
//        {
//            ReportProgress();
//        }

//        void ReportProgress()
//        {
//            if (currentProcess == ProcessType.Export)
//            {
//                if (ExportProgressChanged != null)
//                {
//                    ExportProgressArgs arg = new ExportProgressArgs(_currentTableName, _totalRowsInCurrentTable, _totalRowsInAllTables, _currentRowIndexInCurrentTable, _currentRowIndexInAllTable, _totalTables, _currentTableIndex);
//                    ExportProgressChanged(this, arg);
//                }
//            }
//            else if (currentProcess == ProcessType.Import)
//            {
//                if (ImportProgressChanged != null)
//                {
//                    ImportProgressArgs arg = new ImportProgressArgs(_currentBytes, _totalBytes);
//                    ImportProgressChanged(this, arg);
//                }
//            }
//        }

//        public void StopAllProcess()
//        {
//            stopProcess = true;
//            Command?.Cancel();
//            timerReport.Stop();
//        }

//        public void Dispose()
//        {
//            StopAllProcess();

//            if (timerReport != null)
//            {
//                timerReport.Stop();
//                timerReport.Dispose();
//                timerReport = null;
//            }

//            if (textWriter != null)
//            {
//                textWriter.Dispose();
//                textWriter = null;
//            }

//            if (textReader != null)
//            {
//                textReader.Dispose();
//                textReader = null;
//            }
//        }
//    }
//}
