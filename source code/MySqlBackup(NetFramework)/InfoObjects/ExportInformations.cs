using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// Informations and Settings of MySQL Database Export Process
    /// </summary>
    public class ExportInformations
    {
        int _interval = 50;
        string _delimiter = "|";

        List<string> _documentHeaders = null;
        List<string> _documentFooters = null;

        Dictionary<string, string> _customTable = new Dictionary<string, string>();

        List<string> _lstExcludeTables = null;
        
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
        /// Gets the list of document headers.
        /// </summary>
        /// <param name="cmd">The MySqlCommand that will be used to retrieve the database default character set.</param>
        /// <returns>List of document headers.</returns>
        public List<string> GetDocumentHeaders(MySqlCommand cmd)
        {
            if (_documentHeaders == null)
            {
                _documentHeaders = new List<string>();
                string databaseCharSet = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'character_set_database';", 1);

                _documentHeaders.Add("/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;");
                _documentHeaders.Add("/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;");
                _documentHeaders.Add("/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;");
                _documentHeaders.Add(string.Format("/*!40101 SET NAMES {0} */;", databaseCharSet));
                //_documentHeaders.Add("/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;");
                //_documentHeaders.Add("/*!40103 SET TIME_ZONE='+00:00' */;");
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
                //_documentFooters.Add("/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;");
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
        public bool RecordDumpTime = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the Exported Dump File should be encrypted. Enabling encryption will slow down the whole process.
        /// </summary>
        //[System.Obsolete("This implementation will slow down the whole process which is not recommended. Encrypt the content externally after the export process completed. For more information, please read documentation.")]
        //public bool EnableEncryption = false;

        /// <summary>
        /// Sets the password used to encrypt the exported dump file.
        /// </summary>
        //[System.Obsolete("This implementation will slow down the whole process which is not recommended. Encrypt the content externally after the export process completed. For more information, please read documentation.")]
        //public string EncryptionPassword = "";

        /// <summary>
        /// Gets or Sets a value indicates whether the SQL statement of "CREATE DATABASE" should be added into dump file.
        /// </summary>
        public bool AddCreateDatabase = false;

        /// <summary>
        /// Gets or Sets a value indicates whether the SQL statement of "DROP DATABASE" should be added into dump file.
        /// </summary>
        public bool AddDropDatabase = false;

        /// <summary>
        /// Gets or Sets a value indicates whether the Table Structure (CREATE TABLE) should be exported.
        /// </summary>
        public bool ExportTableStructure = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the SQL statement of "DROP TABLE" should be added into the dump file.
        /// </summary>
        public bool AddDropTable = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the value of auto-increment of each table should be reset to 1.
        /// </summary>
        public bool ResetAutoIncrement = false;

        /// <summary>
        /// Gets or Sets a value indicates whether the Rows should be exported.
        /// </summary>
        public bool ExportRows = true;

        /// <summary>
        /// Gets or Sets the maximum length for combining multiple INSERTs into single sql. Default value is 5MB. Only applies if RowsExportMode = "INSERT" or "INSERTIGNORE" or "REPLACE". This value will be ignored if RowsExportMode = ONDUPLICATEKEYUPDATE or UPDATE.
        /// </summary>
        public int MaxSqlLength = 5 * 1024 * 1024;

        /// <summary>
        /// Gets or Sets a value indicates whether the Stored Procedures should be exported.
        /// </summary>
        public bool ExportProcedures = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the Stored Functions should be exported.
        /// </summary>
        public bool ExportFunctions = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the Stored Triggers should be exported.
        /// </summary>
        public bool ExportTriggers = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the Stored Views should be exported.
        /// </summary>
        public bool ExportViews = true;

        /// <summary>
        /// Gets or Sets a value indicates whether the Stored Events should be exported.
        /// </summary>
        public bool ExportEvents = true;

        /// <summary>
        /// Gets or Sets a value indicates the interval of time (in miliseconds) to raise the event of ExportProgressChanged.
        /// </summary>
        public int IntervalForProgressReport { get { if (_interval == 0) return 100; return _interval; } set { _interval = value; } }

        /// <summary>
        /// Gets or Sets a value indicates whether the totals of rows should be counted before export process commence. The value of total rows is used for progress reporting. Extra time is needed to get the total rows. Sets this value to FALSE if not applying progress reporting.
        /// </summary>
        //public bool GetTotalRowsBeforeExport = true;

        /// <summary>
        /// Gets or Sets the delimiter used for exporting Procedures, Functions, Events and Triggers. Default delimiter is "|".
        /// </summary>
        public string ScriptsDelimiter { get { if (string.IsNullOrEmpty(_delimiter)) return "|"; return _delimiter; } set { _delimiter = value; } }

        /// <summary>
        /// Gets or Sets a value indicates whether the exported Scripts (Procedure, Functions, Events, Triggers, Events) should exclude DEFINER.
        /// </summary>
        public bool ExportRoutinesWithoutDefiner = true;

        /// <summary>
        /// Gets or Sets a enum value indicates how the rows of each table should be exported. INSERT = The default option. Recommended if exporting to a new database. If the primary key existed, the process will halt; INSERT IGNORE = If the primary key existed, skip it; REPLACE = If the primary key existed, delete the row and insert new data; OnDuplicateKeyUpdate = If the primary key existed, update the row. If all fields are primary keys, it will change to INSERT IGNORE; UPDATE = If the primary key is not existed, skip it and if all the fields are primary key, no rows will be exported.
        /// </summary>
        public RowsDataExportMode RowsExportMode = RowsDataExportMode.Insert;

        /// <summary>
        /// Gets or Sets a value indicates whether the rows dump should be wrapped with transaction. Recommended to set this value to FALSE if using RowsExportMode = "INSERT" or "INSERTIGNORE" or "REPLACE", else TRUE.
        /// </summary>
        public bool WrapWithinTransaction = false;

        /// <summary>
        /// Gets or Sets a value indicates the encoding to be used for exporting the dump. Default = UTF8Coding(false)
        /// </summary>
        public Encoding TextEncoding = new UTF8Encoding(false);

        /// <summary>
        /// Gets or Sets a enum value indicates how the BLOB should be exported. HexString = Hexa Decimal String (default); BinaryChar = char format.
        /// </summary>
        public BlobDataExportMode BlobExportMode = BlobDataExportMode.HexString;

        /// <summary>
        /// BlobExportMode = BlobDataExportMode.BinaryChar is disabled by default as this feature is under development. Set this value to true if you wish continue to export BLOB into binary string/char format. This is temporary available for debugging and development purposes.
        /// </summary>
        public bool BlobExportModeForBinaryStringAllow = false;

        /// <summary>
        /// Gets or Sets a value indicates the method of how the total rows value is being obtained. InformationSchema = Fast, but approximate value; SelectCount = Slow but accurate; Skip = Skip obtaining total rows.
        /// </summary>
        public GetTotalRowsMethod GetTotalRowsMode = GetTotalRowsMethod.InformationSchema;

        public ExportInformations()
        {

        }
    }
}
