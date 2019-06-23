using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.Data.MySqlClient
{
    public class ExportCompleteArgs
    {
        DateTime _timeStart, _timeEnd;
        TimeSpan _timeUsed = new TimeSpan();
        Exception _exception;

        MySqlBackup.ProcessEndType _completionType = MySqlBackup.ProcessEndType.UnknownStatus;

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
        public TimeSpan TimeUsed { get { return _timeUsed;}}

        public MySqlBackup.ProcessEndType CompletionType { get { return _completionType; } }

        public Exception LastError { get { return _exception; } }

        public bool HasError { get { if (LastError != null) return true; return false; } }
        
        public ExportCompleteArgs(DateTime timeStart, DateTime timeEnd, MySqlBackup.ProcessEndType endType, Exception exception)
        {
            _completionType = endType;
            _timeStart = timeStart;
            _timeEnd = timeEnd;
            _timeUsed = timeStart - timeEnd;
            _exception = exception;
        }
    }
}
