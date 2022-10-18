using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlConnector
{
    public class ImportProgressArgs : EventArgs
    {
        long _curBytes = 0L;
        long _totalBytes = 0L;
        double _percentComplete = 0d;

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
        public double PercentageCompleted { get { return _percentComplete; } }

        public ImportProgressArgs(long currentBytes, long totalBytes)
        {
            _curBytes = currentBytes;
            _totalBytes = totalBytes;

            if (currentBytes == 0L || totalBytes == 0L)
            {
                _percentComplete = 0d;
            }
            else
            {
                _percentComplete = (double)currentBytes / (double)totalBytes * 100d;
            }
        }
    }
}
