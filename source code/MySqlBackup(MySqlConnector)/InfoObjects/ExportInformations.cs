using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlConnector
{
    /// <summary>
    /// Informations and Settings of MySQL Database Export Process
    /// </summary>
    public class ExportInformations
    {
        private int _maxSqlLength = 16 * 1024 * 1024;
        private int _interval = 100;
        string _delimiter = "|";

        List<string> _documentHeaders = null;
        List<string> _documentFooters = null;

        Dictionary<string, string> _customTable = new Dictionary<string, string>();

        List<string> _lstExcludeTables = null;
        List<string> _lstExcludeRowsForTables = null;

        private Dictionary<string, Dictionary<string, Func<object, object>>> _columnAdjustments;

        /// <summary>
        /// Gets or Sets the tables (black list) that will be excluded for export. The rows of the these tables will not be exported too.
        /// </summary>
        public List<string> ExcludeTables
        {
            get
            {
                if (_lstExcludeTables == null)
                    _lstExcludeTables = new List<string>();
                return _lstExcludeTables;
            }
            set
            {
                _lstExcludeTables = value;
            }
        }

        /// <summary>
        /// Gets or Sets the tables (black list) that will be excluded for row export.
        /// </summary>
        public List<string> ExcludeRowsForTables
        {
            get
            {
                if (_lstExcludeRowsForTables == null)
                    _lstExcludeRowsForTables = new List<string>();
                return _lstExcludeRowsForTables;
            }
            set
            {
                _lstExcludeRowsForTables = value;
            }
        }

        /// <summary>
        /// Gets the list of document headers.
        /// </summary>
        /// <param name="cmd">The MySqlCommand that will be used to retrieve the database default character set.</param>
        /// <returns>List of document headers.</returns>
        public List<string> GetDocumentHeaders(MySqlCommand cmd)
        {
            if (_documentHeaders == null)
            {
                string databaseCharSet = QueryExpress.ExecuteScalarStr(cmd, "SHOW VARIABLES LIKE 'character_set_database';", 1);
                if (string.IsNullOrEmpty(databaseCharSet))
                {
                    databaseCharSet = "utf8mb4"; // Default to modern Unicode character set
                }

                _documentHeaders = new List<string>();
                _documentHeaders.Add("/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;");
                _documentHeaders.Add("/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;");
                _documentHeaders.Add("/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;");
                _documentHeaders.Add($"/*!40101 SET NAMES {databaseCharSet} */;");
                _documentHeaders.Add("/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;");
                _documentHeaders.Add("/*!40103 SET TIME_ZONE='+00:00' */;");
                _documentHeaders.Add("/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;");
                _documentHeaders.Add("/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;");
                _documentHeaders.Add("/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;");
                _documentHeaders.Add("/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;");
            }

            return _documentHeaders;
        }

        /// <summary>
        /// Sets the document headers.
        /// </summary>
        /// <param name="lstHeaders">List of document headers</param>
        public void SetDocumentHeaders(List<string> lstHeaders)
        {
            _documentHeaders = lstHeaders;
        }

        /// <summary>
        /// Gets the document footers.
        /// </summary>
        /// <returns>List of document footers.</returns>
        public List<string> GetDocumentFooters()
        {
            if (_documentFooters == null)
            {
                _documentFooters = new List<string>();
                _documentFooters.Add("/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;");
                _documentFooters.Add("/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;");
                _documentFooters.Add("/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;");
                _documentFooters.Add("/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;");
                _documentFooters.Add("/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;");
                _documentFooters.Add("/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;");
                _documentFooters.Add("/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;");
                _documentFooters.Add("/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;");
            }

            return _documentFooters;
        }

        /// <summary>
        /// Sets the document footers.
        /// </summary>
        /// <param name="lstFooters">List of document footers.</param>
        public void SetDocumentFooters(List<string> lstFooters)
        {
            _documentFooters = lstFooters;
        }

        /// <summary>
        /// Gets or Sets the list of tables that will be exported. If none, all tables will be exported.
        /// </summary>
        public List<string> TablesToBeExportedList
        {
            get
            {
                List<string> lst = new List<string>();
                foreach (KeyValuePair<string, string> kv in _customTable)
                {
                    lst.Add(kv.Key);
                }
                return lst;
            }
            set
            {
                _customTable.Clear();
                foreach (string s in value)
                {
                    _customTable[s] = string.Format("SELECT * FROM `{0}`;", s);
                }
            }
        }

        /// <summary>
        /// Gets or Sets the tables that will be exported with custom SELECT defined. If none or empty, all tables and rows will be exported. Key = Table's Name. Value = Custom SELECT Statement. Example 1: SELECT * FROM `product` WHERE `category` = 1; Example 2: SELECT `name`,`description` FROM `product`;
        /// </summary>
        public Dictionary<string, string> TablesToBeExportedDic
        {
            get
            {
                return _customTable;
            }
            set
            {
                _customTable = value;
            }
        }

        /// <summary>
        /// Gets or Sets a value indicates whether the Dump Time should recorded in dump file.
        /// </summary>
        public bool RecordDumpTime { get; set; } = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the SQL statement of "CREATE DATABASE" should be added into dump file.
        /// </summary>
        public bool AddCreateDatabase { get; set; } = false;

        /// <summary>
        /// Gets or Sets a value indicates whether the SQL statement of "DROP DATABASE" should be added into dump file.
        /// </summary>
        public bool AddDropDatabase { get; set; } = false;

        /// <summary>
        /// Gets or Sets a value indicates whether the Table Structure (CREATE TABLE) should be exported.
        /// </summary>
        public bool ExportTableStructure { get; set; } = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the SQL statement of "DROP TABLE" should be added into the dump file.
        /// </summary>
        public bool AddDropTable { get; set; } = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the value of auto-increment of each table should be reset to 1.
        /// </summary>
        public bool ResetAutoIncrement { get; set; } = false;

        /// <summary>
        /// Gets or Sets a value indicates whether the Rows should be exported.
        /// </summary>
        public bool ExportRows { get; set; } = true;

        /// <summary>
        /// Gets or Sets the maximum length for combining multiple INSERTs into single sql. Default value is 5MB. Only applies if RowsExportMode = "INSERT" or "INSERTIGNORE" or "REPLACE". This value will be ignored if RowsExportMode = ONDUPLICATEKEYUPDATE or UPDATE.
        /// </summary>
        public int MaxSqlLength
        {
            get => _maxSqlLength;
            set
            {
                if (value < 1024)
                    throw new ArgumentOutOfRangeException(nameof(value), value, "SQL length must be at least 1KB (1024 bytes).");

                if (value > 1073741824) // 1GB
                    throw new ArgumentOutOfRangeException(nameof(value), value, "SQL length cannot exceed 1GB (1073741824 bytes).");
                
                _maxSqlLength = value;
            }
        }

        /// <summary>
        /// Gets or Sets a value indicates whether the Stored Procedures should be exported.
        /// </summary>
        public bool ExportProcedures { get; set; } = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the Stored Functions should be exported.
        /// </summary>
        public bool ExportFunctions { get; set; } = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the Stored Triggers should be exported.
        /// </summary>
        public bool ExportTriggers { get; set; } = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the Stored Views should be exported.
        /// </summary>
        public bool ExportViews { get; set; } = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the Stored Events should be exported.
        /// </summary>
        public bool ExportEvents { get; set; } = true;

        /// <summary>
        /// Gets or Sets a value indicates the interval of time (in miliseconds) to raise the event of ExportProgressChanged.
        /// </summary>
        public int IntervalForProgressReport { get { if (_interval == 0) return 100; return _interval; } set { _interval = value; } }

        /// <summary>
        /// Gets or Sets the delimiter used for exporting Procedures, Functions, Events and Triggers. Default delimiter is "|".
        /// </summary>
        public string ScriptsDelimiter { get { if (string.IsNullOrEmpty(_delimiter)) return "|"; return _delimiter; } set { _delimiter = value; } }

        /// <summary>
        /// Gets or Sets a value indicates whether the exported Scripts (Procedure, Functions, Events, Triggers, Events) should exclude DEFINER.
        /// </summary>
        public bool ExportRoutinesWithoutDefiner { get; set; } = true;

        /// <summary>
        /// Gets or Sets a enum value indicates how the rows of each table should be exported. INSERT = The default option. Recommended if exporting to a new database. If the primary key existed, the process will halt; INSERT IGNORE = If the primary key existed, skip it; REPLACE = If the primary key existed, delete the row and insert new data; OnDuplicateKeyUpdate = If the primary key existed, update the row. If all fields are primary keys, it will change to INSERT IGNORE; UPDATE = If the primary key is not existed, skip it and if all the fields are primary key, no rows will be exported.
        /// </summary>
        public RowsDataExportMode RowsExportMode { get; set; } = RowsDataExportMode.Insert;

        /// <summary>
        /// Gets or Sets a value indicates whether the rows dump should be wrapped with transaction. Recommended to set this value to FALSE if using RowsExportMode = "INSERT" or "INSERTIGNORE" or "REPLACE", else TRUE.
        /// </summary>
        public bool WrapWithinTransaction { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to use LOCK TABLES WRITE during export operations.
        /// When enabled, tables are locked for writing to ensure data consistency but may block other operations.
        /// </summary>
        public bool EnableLockTablesWrite { get; set; } = false;

        /// <summary>
        /// Gets or Sets a value indicates the encoding to be used for exporting the dump. Default = UTF8Coding(false)
        /// </summary>
        public Encoding TextEncoding { get; set; } = new UTF8Encoding(false);

        /// <summary>
        /// Gets or Sets a value indicates the method of how the total rows value is being obtained. InformationSchema = Fast, but approximate value; SelectCount = Slow but accurate; Skip = Skip obtaining total rows.
        /// </summary>
        public GetTotalRowsMethod GetTotalRowsMode { get; set; } = GetTotalRowsMethod.InformationSchema;

        /// <summary>
        /// Gets or Sets a value indicates whether comments should be included in the dump content.
        /// </summary>
        public bool EnableComment { get; set; } = true;

        /// <summary>
        /// Gets or Sets a value indicates whether line breaks should be added in between multiple INSERTs.
        /// </summary>
        public bool InsertLineBreakBetweenInserts { get; set; } = false;

        /// <summary>
        /// Gets or sets table and column-specific value adjustment functions.
        /// Key structure: [TableName][ColumnName] = AdjustmentFunction
        /// Use SetTableColumnValueAdjustment() to add individual adjustments.
        /// Read documentation for more details at wiki page [Table Column Value Adjustments] at github
        /// </summary>
        public Dictionary<string, Dictionary<string, Func<object, object>>> TableColumnValueAdjustments
        {
            get => _columnAdjustments;
            set => _columnAdjustments = value;
        }

        /// <summary>
        /// Helper method to add column adjustment
        /// </summary>
        public void AddTableColumnValueAdjustment(string tableName, string columnName, Func<object, object> adjustFunc)
        {
            if (_columnAdjustments == null)
                _columnAdjustments = new Dictionary<string, Dictionary<string, Func<object, object>>>(StringComparer.OrdinalIgnoreCase);

            if (!_columnAdjustments.ContainsKey(tableName))
                _columnAdjustments[tableName] = new Dictionary<string, Func<object, object>>(StringComparer.OrdinalIgnoreCase);

            _columnAdjustments[tableName][columnName] = adjustFunc;
        }

        public ExportInformations()
        {

        }
    }
}
