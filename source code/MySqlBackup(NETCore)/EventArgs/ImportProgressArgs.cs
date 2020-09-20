using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.Data.MySqlClient
{
    public class ImportProgressArgs : EventArgs
    {
        long _curBytes = 0L;
        long _totalBytes = 0L;

        /// <summary>
        /// Number of processed bytes in current import process.
        /// </summary>
        public long CurrentBytes { get { return _curBytes; } }

        /// <summary>
        /// Total bytes to be processed.
        /// </summary>
        public long TotalBytes { get { return _totalBytes; } }

        /// <summary>
        /// Percentage of completeness.
        /// </summary>
        public int PercentageCompleted { get { return (int)(CurrentBytes *100L / TotalBytes); } }

        public ImportProgressArgs(long currentBytes, long totalBytes)
        {
            _curBytes = currentBytes;
            _totalBytes = totalBytes;
        }
    }
}
