using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup_All_Databases
{
    internal class Config
    {
        public string ConnectionString { get; set; } = "";
        public string BackupPath { get; set; } = "";
        public int MaxBackupCopies { get; set; } = 30;
        public bool BackupAllDatabases { get; set; } = false;
        public List<string> IncludeList { get; set; } = new List<string>();
        public List<string> ExcludeList { get; set; } = new List<string>();
        public int StartHour { get; set; } = 0;
        public int StartMinute { get; set; } = 0;
        public byte[] Key { get; set; } = null;
    }
}
