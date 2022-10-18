using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlConnector
{
    public class ImportCompleteArgs
    {
        MySqlBackup.ProcessEndType _completionType;
        DateTime _timeStart;
        DateTime _timeEnd;
        TimeSpan _timeUsed;
        Exception _exception;

        /// <summary>
        /// The starting time of import process.
        /// </summary>
        public DateTime TimeStart { get { return _timeStart; } }

        /// <summary>
        /// The ending time of import process.
        /// </summary>
        public DateTime TimeEnd { get { return _timeEnd; } }

        /// <summary>
        /// The completion type of current import processs.
        /// </summary>
        public MySqlBackup.ProcessEndType CompleteType { get { return _completionType; } }

        /// <summary>
        /// Indicates whether the import process has error(s).
        /// </summary>
        public bool HasErrors { get { if (LastError != null) return true; return false; } }

        /// <summary>
        /// The last error (exception) occur in import process.
        /// </summary>
        public Exception LastError { get { return _exception; } }

        /// <summary>
        /// Total time used in current import process.
        /// </summary>
        public TimeSpan TimeUsed { get { return _timeUsed; } }

        public ImportCompleteArgs(MySqlBackup.ProcessEndType completionType, DateTime timeStart, DateTime timeEnd, Exception exception)
        {
            _completionType = completionType;
            _timeStart = timeStart;
            _timeEnd = timeEnd;
            _timeUsed = timeEnd - timeStart;
            _exception = exception;
        }
    }
}
