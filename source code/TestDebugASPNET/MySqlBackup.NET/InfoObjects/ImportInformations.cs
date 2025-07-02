using System.Text;

namespace MySqlConnector
{
    public class ImportInformations
    {
        int _interval = 100;
        /// <summary>
        /// Gets or Sets a value indicates the interval of time (in miliseconds) to raise the event of ExportProgressChanged.
        /// </summary>
        public int IntervalForProgressReport { get { if (_interval == 0) return 250; return _interval; } set { _interval = value; } }

        /// <summary>
        /// Gets or Sets a value indicates whether SQL errors occurs in import process should be ignored.
        /// </summary>
        public bool IgnoreSqlError { get; set; } = false;

        /// <summary>
        /// Gets or Sets the file path used to log error messages.
        /// </summary>
        public string ErrorLogFile { get; set; } = string.Empty;

        /// <summary>
        /// Gets or Sets a value indicates the encoding to be used for importing the dump. Default = UTF8Coding(false)
        /// </summary>
        public Encoding TextEncoding { get; set; } = new UTF8Encoding(false);

        public ImportInformations()
        { }
    }
}
