namespace MySqlConnector
{
    public partial class MySqlBackup
    {
        public enum ProcessType
        {
            Export,
            Import
        }

        public enum ProcessEndType
        {
            UnknownStatus,
            Complete,
            Cancelled,
            Error
        }
    }
}