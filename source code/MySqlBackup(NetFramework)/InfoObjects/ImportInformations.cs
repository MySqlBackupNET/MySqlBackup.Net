using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.Data.MySqlClient
{
    public class ImportInformations
    {
        int _interval = 100;
        //string _targetDatabase = "";
        //string _databaseDefaultCharSet = "";

        /// <summary>
        /// Gets or Sets a value indicates whether the Imported Dump File is encrypted.
        /// </summary>
        //[System.Obsolete("This implementation will slow down the whole process which is not recommended. Encrypt the content externally after the export process completed. For more information, please read documentation.")]
        //public bool EnableEncryption = false;

        /// <summary>
        /// Sets the password used to decrypt the exported dump file.
        /// </summary>
        //[System.Obsolete("This implementation will slow down the whole process which is not recommended. Encrypt the content externally after the export process completed. For more information, please read documentation.")]
        //public string EncryptionPassword = "";

        /// <summary>
        /// Gets or Sets a value indicates the interval of time (in miliseconds) to raise the event of ExportProgressChanged.
        /// </summary>
        public int IntervalForProgressReport { get { if (_interval == 0) return 100; return _interval; } set { _interval = value; } }

        /// <summary>
        /// Gets or Sets the name of target database.
        /// </summary>
        //public string TargetDatabase { get { return (_targetDatabase + "").Trim(); } set { _targetDatabase = value; } }

        /// <summary>
        /// Gets or Sets the default character set of the target database. This will only take effect when targetting new non-existed database.
        /// </summary>
        //public string DatabaseDefaultCharSet { get { return (_databaseDefaultCharSet + "").Trim(); } set { _databaseDefaultCharSet = value; } }

        /// <summary>
        /// Gets or Sets a value indicates whether SQL errors occurs in import process should be ignored.
        /// </summary>
        public bool IgnoreSqlError = false;

        /// <summary>
        /// Gets or Sets the file path used to log error messages.
        /// </summary>
        public string ErrorLogFile = "";

        public ImportInformations()
        { }
    }
}
