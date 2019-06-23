using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.Data.MySqlClient
{
    public class ImportCompleteArgs
    {
        /// <summary>
        /// The starting time of import process.
        /// </summary>
        public DateTime TimeStart;

        /// <summary>
        /// The ending time of import process.
        /// </summary>
        public DateTime TimeEnd;

        /// <summary>
        /// Enum of completion type
        /// </summary>
        public enum CompleteType
        {
            Completed,
            Cancelled,
            Error
        }

        /// <summary>
        /// Indicates whether the import process has error(s).
        /// </summary>
        public bool HasErrors { get { if (LastError != null) return true; return false; } }

        /// <summary>
        /// The last error (exception) occur in import process.
        /// </summary>
        public Exception LastError = null;

        // <summary>
        /// The completion type of current import processs.
        /// </summary>
        public CompleteType CompletedType = CompleteType.Completed;

        /// <summary>
        /// Total time used in current import process.
        /// </summary>
        public TimeSpan TimeUsed
        {
            get
            {
                TimeSpan ts = new TimeSpan();
                ts = TimeEnd - TimeStart;
                return ts;
            }
        }

    }
}
