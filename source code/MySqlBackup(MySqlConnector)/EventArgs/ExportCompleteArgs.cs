using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlConnector
{
    public class ExportCompleteArgs
    {
        MySqlBackup.ProcessEndType _completionType;
        DateTime _timeStart;
        DateTime _timeEnd;
        TimeSpan _timeUsed;
        Exception _exception;

        /// <summary>
        /// The Starting time of export process.
        /// </summary>
        public DateTime TimeStart { get { return _timeStart; } }

        /// <summary>
        /// The Ending time of export process.
        /// </summary>
        public DateTime TimeEnd { get { return _timeEnd; } }

        /// <summary>
        /// Total time used in current export process.
        /// </summary>
        public TimeSpan TimeUsed { get { return _timeUsed; } }

        public MySqlBackup.ProcessEndType CompletionType { get { return _completionType; } }

        public Exception LastError { get { return _exception; } }

        public bool HasError { get { if (_exception != null) return true; return false; } }

        public ExportCompleteArgs(DateTime timeStart, DateTime timeEnd, MySqlBackup.ProcessEndType endType, Exception exception)
        {
            _completionType = endType;
            _timeStart = timeStart;
            _timeEnd = timeEnd;
            _timeUsed = timeEnd - timeStart;
            _exception = exception;
        }
    }
}
