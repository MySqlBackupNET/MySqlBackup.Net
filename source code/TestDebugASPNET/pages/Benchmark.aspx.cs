using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Management;
using System.Data;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Data.Entity;
using System.Text.Json.Serialization;

namespace System.pages
{
    public partial class Benchmark : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string currentDB = "";
                string instancePathMySqlDump = "";
                string instancePathMySql = "";
                string basedir = "";

                using (MySqlConnection conn = config.GetNewConnection())
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        currentDB = QueryExpress.ExecuteScalarStr(cmd, "SELECT database();");
                        basedir = QueryExpress.ExecuteScalarStr(cmd, "SHOW VARIABLES LIKE 'basedir';", "Value");

                        string separator = System.IO.Path.DirectorySeparatorChar.ToString();
                        string binPath = System.IO.Path.Combine(basedir, "bin");
                        instancePathMySqlDump = System.IO.Path.Combine(binPath, separator == "/" ? "mysqldump" : "mysqldump.exe");
                        instancePathMySql = System.IO.Path.Combine(binPath, separator == "/" ? "mysql" : "mysql.exe");
                    }
                }

                txtInitialSchema.Text = currentDB;
                txtFilePathMySqlDump.Text = instancePathMySqlDump;
                txtFilePathMySql.Text = instancePathMySql;
            }
        }

        protected async void btRun_Click(object sender, EventArgs e)
        {
            string folder = Server.MapPath("~/App_Data/backup");
            string reportFilePath = Path.Combine(folder, $"benchmark_report_{DateTime.Now:yyyy-MM-dd TT hhmmss}.txt");

            BenchmarkTest.TaskId++;
            int newTaskId = BenchmarkTest.TaskId;

            _ = Task.Run(() => RunTest(newTaskId, config.ConnString, folder, reportFilePath, txtFilePathMySqlDump.Text, txtFilePathMySql.Text, cbCleanDatabaseAfterUse.Checked));

            panelSetup.Visible = false;
            panelResult.Visible = true;
            literalTaskId.Text = $@"
<script>
var taskid = {newTaskId};
</script>
";
        }

        async Task RunTest(int newTaskId, string _constr, string _baseDumpFolder, string _filePathReport, string _filePathMySqlDump, string _filePathMySql, bool _cleanUpDatabase)
        {
            BenchmarkTest t = new BenchmarkTest(newTaskId, _constr, _baseDumpFolder, _filePathReport, _filePathMySqlDump, _filePathMySql, _cleanUpDatabase);
            t.Run();
        }
    }

    public class BenchmarkTest
    {
        public static int TaskId = 0;
        public static ConcurrentDictionary<int, BenchmarkTestInfo> dicProgress = new ConcurrentDictionary<int, BenchmarkTestInfo>();

        int CurrentTaskId = 0;
        string connstr = "";
        string baseFolder = "";
        string filePathMySqlDump = "";
        string filePathMySql = "";
        string filePathReport = "";
        string filePathCnf = "";
        bool cleanUpDatabase = false;
        StringBuilder sb = null;
        string password = "";

        string sourceDbName = "";
        long totalRows = 0L;
        DateTime timeStart;
        DateTime timeEnd;

        public BenchmarkTest(int _newtaskid, string _constr, string _baseDumpFolder, string _fileReport, string _filePathMySqlDump, string _filePathMySql, bool _cleanUpDatabase)
        {
            CurrentTaskId = _newtaskid;
            connstr = _constr;
            baseFolder = _baseDumpFolder;
            filePathMySqlDump = _filePathMySqlDump;
            filePathMySql = _filePathMySql;
            filePathReport = _fileReport;
            cleanUpDatabase = _cleanUpDatabase;
            filePathCnf = Path.Combine(baseFolder, "cnf.txt");
        }

        public void Run()
        {
            try
            {
                timeStart = DateTime.Now;

                dicProgress[CurrentTaskId] = new BenchmarkTestInfo();
                dicProgress[CurrentTaskId].TimeStart = timeStart;
                dicProgress[CurrentTaskId].Started = true;

                sb = new StringBuilder();
                GetSystemVariables();

                var builder = new MySqlConnectionStringBuilder(connstr);

                string host = builder.Server ?? "localhost";
                string user = builder.UserID ?? "root";
                password = builder.Password ?? "";
                uint port = builder.Port != 0 ? builder.Port : 3306;
                string dbCharacterSet = "";

                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        dbCharacterSet = QueryExpress.ExecuteScalarStr(cmd, "SHOW VARIABLES LIKE 'character_set_database'", 1);
                    }
                }

                filePathCnf = Path.Combine(baseFolder, "my.cnf");
                string cnfContent = $@"[client]
user={user}
password={password}
host={host}
port={port}
default-character-set={dbCharacterSet}";

                File.WriteAllText(filePathCnf, cnfContent);

                var dirReport = Path.GetDirectoryName(filePathReport);
                Directory.CreateDirectory(dirReport);
                File.WriteAllText(filePathReport, sb.ToString());

                string dumpFile1 = Path.Combine(baseFolder, $"benchmark-backup-mysqlbackupnet-1.txt");
                string dumpFile2 = Path.Combine(baseFolder, $"benchmark-backup-mysqlbackupnet-2.txt");
                string dumpFile3 = Path.Combine(baseFolder, $"benchmark-backup-mysqlbackupnet-3.txt");
                string dumpFile4 = Path.Combine(baseFolder, $"benchmark-backup-mysqldump-1.txt");
                string dumpFile5 = Path.Combine(baseFolder, $"benchmark-backup-mysqldump-2.txt");
                string dumpFile6 = Path.Combine(baseFolder, $"benchmark-backup-mysqldump-3.txt");

                string dbName1 = $"test_benchmark_restore_mysqlbackupnet_1";
                string dbName2 = $"test_benchmark_restore_mysqlbackupnet_2";
                string dbName3 = $"test_benchmark_restore_mysqlbackupnet_3";
                string dbName4 = $"test_benchmark_restore_mysql_1";
                string dbName5 = $"test_benchmark_restore_mysql_2";
                string dbName6 = $"test_benchmark_restore_mysql_3";

                sb.AppendLine();
                sb.AppendLine($"Initial Source Database: {sourceDbName}");
                sb.AppendLine();
                sb.AppendLine($"Dump File 1: {dumpFile1}");
                sb.AppendLine($"Dump File 2: {dumpFile2}");
                sb.AppendLine($"Dump File 3: {dumpFile3}");
                sb.AppendLine($"Dump File 4: {dumpFile4}");
                sb.AppendLine($"Dump File 5: {dumpFile5}");
                sb.AppendLine($"Dump File 6: {dumpFile6}");
                sb.AppendLine();
                sb.AppendLine($"Database 1: {dbName1}");
                sb.AppendLine($"Database 2: {dbName2}");
                sb.AppendLine($"Database 3: {dbName3}");
                sb.AppendLine($"Database 4: {dbName4}");
                sb.AppendLine($"Database 5: {dbName5}");
                sb.AppendLine($"Database 6: {dbName6}");

                // backup - MySqlBackup.NET
                dicProgress[CurrentTaskId].dicTask[1] = new BenchmarkTask(1, 1, sourceDbName, dumpFile1);
                dicProgress[CurrentTaskId].dicTask[2] = new BenchmarkTask(1, 2, sourceDbName, dumpFile2);
                dicProgress[CurrentTaskId].dicTask[3] = new BenchmarkTask(1, 3, sourceDbName, dumpFile3);

                // backup - MySqlDump
                dicProgress[CurrentTaskId].dicTask[4] = new BenchmarkTask(2, 1, sourceDbName, dumpFile4);
                dicProgress[CurrentTaskId].dicTask[5] = new BenchmarkTask(2, 2, sourceDbName, dumpFile5);
                dicProgress[CurrentTaskId].dicTask[6] = new BenchmarkTask(2, 3, sourceDbName, dumpFile6);

                // restore - MySqlBackup.NET
                dicProgress[CurrentTaskId].dicTask[7] = new BenchmarkTask(3, 1, dbName1, dumpFile1);
                dicProgress[CurrentTaskId].dicTask[8] = new BenchmarkTask(3, 2, dbName2, dumpFile1);
                dicProgress[CurrentTaskId].dicTask[9] = new BenchmarkTask(3, 3, dbName3, dumpFile1);

                // restore - MySQL Instance
                dicProgress[CurrentTaskId].dicTask[10] = new BenchmarkTask(4, 1, dbName4, dumpFile1);
                dicProgress[CurrentTaskId].dicTask[11] = new BenchmarkTask(4, 2, dbName5, dumpFile1);
                dicProgress[CurrentTaskId].dicTask[12] = new BenchmarkTask(4, 3, dbName6, dumpFile1);

                List<int> lstStage = new List<int>();

                for (int _taskid = 1; _taskid < 13; _taskid++)
                {
                    var bt = dicProgress[CurrentTaskId].dicTask[_taskid];

                    if (!lstStage.Contains(bt.Stage))
                    {
                        lstStage.Add(bt.Stage);
                        sb.AppendLine();
                        sb.AppendLine(bt.StageName);
                        sb.AppendLine();
                    }

                    try
                    {
                        sb.AppendLine(bt.ActionInfo);
                        File.WriteAllText(filePathReport, sb.ToString());
                        dicProgress[CurrentTaskId].Remarks = sb.ToString();

                        bt.Started = true;
                        bt.TimeStart = DateTime.Now;

                        switch (bt.Stage)
                        {
                            case 1:
                                ExportMySqlBackupNET(bt.DatabaseName, bt.DumpFile);
                                break;
                            case 2:
                                ExportMySqlDump(bt.DatabaseName, bt.DumpFile);
                                break;
                            case 3:
                                ImportMySqlBackupNet(bt.DatabaseName, bt.DumpFile);
                                break;
                            case 4:
                                ImportMySqlInstance(bt.DatabaseName, bt.DumpFile);
                                break;
                        }

                        bt.Completed = true;
                        bt.TimeEnd = DateTime.Now;
                        bt.TimeUsed = bt.TimeEnd - bt.TimeStart;
                    }
                    catch (Exception ex)
                    {
                        bt.Completed = true;
                        bt.TimeEnd = DateTime.Now;
                        bt.TimeUsed = bt.TimeEnd - bt.TimeStart;
                        bt.HasError = true;
                        bt.LastError = ex;

                        throw;
                    }
                }

                timeEnd = DateTime.Now;
                var totalTimeElapsed = timeEnd - timeStart;

                var btProgress = dicProgress[CurrentTaskId].dicTask;

                for (int round = 1; round < 13; round++)
                {
                    string _file = btProgress[round].DumpFile;

                    if (!string.IsNullOrEmpty(_file) && File.Exists(_file))
                    {
                        btProgress[round].Sha256 = CalculateSHA256File(btProgress[round].DumpFile);
                    }
                }

                sb.AppendLine();
                sb.AppendLine("All processes ended");
                sb.AppendLine($"Time Start   : {timeStart:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"Time End     : {timeEnd:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"Time Elapsed : {totalTimeElapsed.Hours} h {totalTimeElapsed.Minutes} m {totalTimeElapsed.Seconds} s");

                sb.AppendLine();
                sb.AppendLine("SHA256 Checksums:");

                for (int round = 1; round < 13; round++)
                {
                    var bt = btProgress[round];

                    if (!string.IsNullOrEmpty(bt.DumpFile) && File.Exists(bt.DumpFile))
                    {
                        sb.AppendLine();
                        sb.AppendLine(bt.FileName);
                        sb.AppendLine($"SHA256: {bt.Sha256}");
                    }
                }

                sb.AppendLine();
                sb.AppendLine("==================================");
                sb.AppendLine("Benchmark Results");
                sb.AppendLine("==================================");

                Dictionary<int, string> dicStageName = new Dictionary<int, string>();
                dicStageName[1] = "Backup/Export - MySqlBackup.NET";
                dicStageName[2] = "Backup/Export - mysqldump.exe";
                dicStageName[3] = "Restore/Import - MySqlBackup.NET";
                dicStageName[4] = "Restore/Import - mysql.exe";

                foreach(var kv in dicStageName)
                {
                    sb.AppendLine();
                    sb.AppendLine(kv.Value);
                    sb.AppendLine("-------------------------------");
                    
                    foreach(var kv2 in btProgress)
                    {
                        var bt = kv2.Value;

                        if (bt.Stage != kv.Key)
                            continue;

                        sb.AppendLine($"Round {bt.Round}  {bt.TimeUsedDisplay.PadRight(10, ' ')}{bt.FileSizeDisplay}");
                    }
                }

                if (cleanUpDatabase)
                {
                    string[] dropSqls = { dbName1, dbName2, dbName3, dbName4, dbName5, dbName6 };
                    using (MySqlConnection conn = new MySqlConnection(connstr))
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            foreach (var _dbname in dropSqls)
                            {
                                cmd.CommandText = $"DROP DATABASE IF EXISTS `{_dbname}`";
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    sb.AppendLine();
                    sb.AppendLine("Clean up / removing databases");
                    sb.AppendLine();
                    foreach (var _dbname in dropSqls)
                    {
                        sb.AppendLine($"DROP DATABASE IF EXISTS `{_dbname}`");
                    }
                }

                File.WriteAllText(filePathReport, sb.ToString());

                dicProgress[CurrentTaskId].Completed = true;
                dicProgress[CurrentTaskId].Remarks = sb.ToString();
                dicProgress[CurrentTaskId].TimeEnd = timeEnd;
                dicProgress[CurrentTaskId].TimeUsed = totalTimeElapsed;
            }
            catch (Exception ex)
            {
                timeEnd = DateTime.Now;
                var totalTimeElapsed = timeEnd - timeStart;

                dicProgress[CurrentTaskId].Remarks = sb.ToString();
                dicProgress[CurrentTaskId].TimeEnd = timeEnd;
                dicProgress[CurrentTaskId].TimeUsed = totalTimeElapsed;
                dicProgress[CurrentTaskId].Completed = true;
                dicProgress[CurrentTaskId].HasError = true;
                dicProgress[CurrentTaskId].LastError = ex;
            }
        }

        void ExportMySqlBackupNET(string dbName, string dumpFilePath)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = $"USE `{QueryExpress.EscapeIdentifier(dbName)}`";
                    cmd.ExecuteNonQuery();

                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        mb.ExportInfo.RecordDumpTime = false;

                        mb.ExportToFile(dumpFilePath);
                    }
                }
            }
        }

        void ImportMySqlBackupNet(string dbName, string dumpFilePath)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = $"DROP DATABASE IF EXISTS `{QueryExpress.EscapeIdentifier(dbName)}`";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{QueryExpress.EscapeIdentifier(dbName)}`";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"USE `{QueryExpress.EscapeIdentifier(dbName)}`";
                    cmd.ExecuteNonQuery();

                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        mb.ImportFromFile(dumpFilePath);
                    }
                }
            }
        }

        void ExportMySqlDump(string database, string dumpFilePath)
        {
            string arguments = $"--defaults-file=\"{filePathCnf}\" --routines --events {database} --result-file=\"{dumpFilePath}\"";

            // Configure process to run hidden
            var processStartInfo = new ProcessStartInfo
            {
                FileName = filePathMySqlDump,
                Arguments = arguments,
                UseShellExecute = false,        // Required for hiding window
                CreateNoWindow = true,          // Don't create a window
                WindowStyle = ProcessWindowStyle.Hidden,  // Hide if window is created
                RedirectStandardOutput = true,  // Capture output
                RedirectStandardError = true    // Capture errors
            };

            using (var process = Process.Start(processStartInfo))
            {
                string errors = process.StandardError.ReadToEnd();

                process.WaitForExit();

                // Check if dump was successful
                if (process.ExitCode != 0)
                {
                    throw new Exception($"mysqldump failed with exit code {process.ExitCode}. Error: {errors}");
                }
            }
        }

        void ImportMySqlInstance(string database, string dumpFilePath)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = $"DROP DATABASE IF EXISTS `{QueryExpress.EscapeIdentifier(database)}`";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{QueryExpress.EscapeIdentifier(database)}`";
                    cmd.ExecuteNonQuery();
                }
            }

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "",
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var process = Process.Start(processStartInfo))
            {
                process.StandardInput.Write($"{filePathMySql} --defaults-file=\"{filePathCnf}\" {database} < \"{dumpFilePath}\"");
                process.StandardInput.Close();

                // Read both streams asynchronously to prevent deadlock
                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception($"mysql failed with exit code {process.ExitCode}. Error: {errors}");
                }
            }
        }

        void GetSystemVariables()
        {
            sb.AppendLine("System Information:");
            sb.AppendLine("================================");

            // OS Information
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject mo in searcher.Get())
                {
                    sb.AppendLine($"OS: {mo["Caption"]} {(Environment.Is64BitOperatingSystem ? "x64" : "x86")} (version {mo["Version"]}, build: {mo["BuildNumber"]})");
                    sb.AppendLine($"RAM: {(Convert.ToInt64(mo["TotalVisibleMemorySize"]) / 1024 / 1024):N0}GB");
                }
            }
            catch { sb.AppendLine("OS: Information not available"); }

            // CPU Information
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                foreach (ManagementObject mo in searcher.Get())
                {
                    sb.AppendLine($"CPU: {mo["Name"]}");
                }
            }
            catch { sb.AppendLine("CPU: Information not available"); }

            // Disk Information
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive WHERE MediaType = 'Fixed hard disk media'");
                foreach (ManagementObject mo in searcher.Get())
                {
                    sb.AppendLine($"Hard Disk: {mo["Model"]} (Size: {(Convert.ToInt64(mo["Size"]) / 1024 / 1024 / 1024):N0}GB)");
                }
            }
            catch { sb.AppendLine("Hard Disk: Information not available"); }

            // MySQL Information
            string databaseFolderPath = "";
            DataTable dtTables = null;
            string serverVersion = "";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        sourceDbName = QueryExpress.ExecuteScalarStr(cmd, "SELECT DATABASE();");

                        cmd.CommandText = @"SELECT @@version AS server_version, @@max_allowed_packet AS max_packet, (SELECT DEFAULT_CHARACTER_SET_NAME FROM information_schema.SCHEMATA WHERE SCHEMA_NAME = @dbName) AS db_charset";
                        cmd.Parameters.AddWithValue("@dbName", sourceDbName);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                serverVersion = reader.GetString("server_version");
                                sb.AppendLine($"MySQL Server: v{serverVersion}");
                                sb.AppendLine($"Database Character Set ({sourceDbName}): {(reader.IsDBNull(2) ? "Database not found" : reader.GetString("db_charset"))}");
                                sb.AppendLine($"max_allowed_packet: {(reader.GetInt64("max_packet") / 1024 / 1024)}M");
                            }
                        }

                        databaseFolderPath = QueryExpress.ExecuteScalarStr(cmd, "SHOW VARIABLES LIKE 'datadir'", "Value");

                        dtTables = QueryExpress.GetTable(cmd, "SHOW FULL TABLES WHERE Table_type='BASE TABLE';");

                        foreach (DataRow dr in dtTables.Rows)
                        {
                            long thisRows = QueryExpress.ExecuteScalarLong(cmd, $"SELECT COUNT(*) FROM `{QueryExpress.EscapeIdentifier(dr[0] + "")}`");
                            totalRows += thisRows;
                        }
                    }
                }
            }
            catch { sb.AppendLine("MySQL Information: Not available"); }

            long dbSize = GetDatabaseSize();
            double dbSize_mb = dbSize / (1024.0 * 1024.0);
            double dbSize_gb = dbSize / (1024.0 * 1024.0 * 1024.0);
            if (dbSize_gb >= 1.0)
            {
                sb.AppendLine($"Database Size: {dbSize_gb:0.###} GB");
            }
            else
            {
                sb.AppendLine($"Database Size: {dbSize_mb:0.###} MB");
            }

            sb.AppendLine($"Total Rows: {totalRows}");
            sb.AppendLine($"MySqlDump: {serverVersion} (Export)");
            sb.AppendLine($"MySql: {serverVersion} (Import)");
            sb.AppendLine($"MySqlBackup: {typeof(MySqlBackup).Assembly.GetName().Version}, with MySqlConnector.dll (MIT) v{typeof(MySqlConnection).Assembly.GetName().Version}");
            sb.AppendLine("Execution Note: MySqlDump and MySql.exe are executed through system commands");
            sb.AppendLine("Execution Note: MySqlBackup is executed through ASP.NET Web Application (.NET Framework)");
            sb.AppendLine("Note: MySql server and backup/restore dump files are on the same hard disk");
            sb.AppendLine();
        }

        private long GetDatabaseSize()
        {
            long totalSize = 0;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        // Get the data directory
                        string dataDir = QueryExpress.ExecuteScalarStr(cmd, "SHOW VARIABLES LIKE 'datadir'", "Value");
                        if (string.IsNullOrEmpty(dataDir))
                        {
                            return 0;
                        }

                        // Remove trailing slash/backslash
                        dataDir = dataDir.TrimEnd('/', '\\');

                        // Get the current database name
                        string databaseName = QueryExpress.ExecuteScalarStr(cmd, "SELECT DATABASE();");
                        if (string.IsNullOrEmpty(databaseName))
                        {
                            return 0;
                        }

                        // Construct database directory path
                        string databasePath = Path.Combine(dataDir, databaseName);

                        if (!Directory.Exists(databasePath))
                        {
                            return 0;
                        }

                        DirectoryInfo dbDirectory = new DirectoryInfo(databasePath);
                        FileInfo[] files = dbDirectory.GetFiles();

                        foreach (FileInfo file in files)
                        {
                            totalSize += file.Length;
                        }
                    }
                }
            }
            catch
            {
                return 0;
            }

            return totalSize;
        }

        private string CalculateSHA256File(string filePath)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    byte[] hash = sha256.ComputeHash(fileStream);
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hash)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
        }

    }

    public class BenchmarkTestInfo
    {
        [JsonPropertyName("dicTask")]
        public Dictionary<int, BenchmarkTask> dicTask { get; set; } = new Dictionary<int, BenchmarkTask>();

        public bool Started { get; set; } = false;
        public bool Completed { get; set; } = false;
        public string Remarks { get; set; } = "";
        public DateTime TimeStart { get; set; } = DateTime.MinValue;
        public DateTime TimeEnd { get; set; } = DateTime.MinValue;
        public TimeSpan TimeUsed { get; set; } = TimeSpan.Zero;
        public bool HasError { get; set; } = false;

        [JsonIgnore]
        public Exception LastError { get; set; } = null;
    }

    public class BenchmarkTask
    {
        public int Round { get; set; } = 0;
        public int Stage { get; set; } = 0;
        public string DatabaseName { get; set; } = "";
        public string DumpFile { get; set; } = "";
        public bool Started { get; set; } = false;
        public bool Completed { get; set; } = false;
        public DateTime TimeStart { get; set; } = DateTime.MinValue;
        public DateTime TimeEnd { get; set; } = DateTime.MinValue;
        public TimeSpan TimeUsed { get; set; } = TimeSpan.Zero;
        public string Remarks { get; set; } = "";
        public long FileSize { get; set; } = 0L;
        public bool HasError { get; set; } = false;
        public string Sha256 = "";
        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(DumpFile))
                    return "";
                return Path.GetFileName(DumpFile);
            }
        }

        [JsonIgnore]
        public Exception LastError = null;

        public string ErrorMsg
        {
            get
            {
                if (HasError && LastError != null)
                {
                    return $@"{LastError.Message}";
                }
                return "";
            }
        }

        public BenchmarkTask(int _stage, int _round, string _dbname, string _dumpfile)
        {
            Stage = _stage;
            Round = _round;
            DatabaseName = _dbname;
            DumpFile = _dumpfile;
        }

        public string StageName
        {
            get
            {
                switch (Stage)
                {
                    case 1:
                        return "Export/Backup - MySqlBackup.NET";
                    case 2:
                        return "Export/Backup - MySqlDump";
                    case 3:
                        return "Import/Restore - MySqlBackup.NET";
                    case 4:
                        return "Import/Restore - MySql Instance";
                }
                return "";
            }
        }

        public string TimeUsedDisplay
        {
            get
            {
                if (TimeUsed.TotalHours >= 1)
                {
                    return $"{TimeUsed.Hours}h {TimeUsed.Minutes}m";
                }
                else
                {
                    return $"{TimeUsed.Minutes}m {TimeUsed.Seconds}s";
                }
            }
        }

        public string FileSizeDisplay
        {
            get
            {
                const long GB = 1024 * 1024 * 1024;
                const long MB = 1024 * 1024;
                const long KB = 1024;

                if (FileSize >= GB)
                {
                    double sizeInGB = (double)FileSize / GB;
                    return $"{sizeInGB:0.###} GB";
                }
                else if (FileSize >= MB)
                {
                    double sizeInMB = (double)FileSize / MB;
                    return $"{sizeInMB:0.###} MB";
                }
                else
                {
                    double sizeInKB = (double)FileSize / KB;
                    return $"{sizeInKB:0.###} KB";
                }
            }
        }

        public string ActionInfo
        {
            get
            {

                switch (Stage)
                {
                    case 1: return $"Round {Round} - MySqlBackup.NET - {DatabaseName} > {FileName}";
                    case 2: return $"Round {Round} - MySqlDump.exe - {DatabaseName} > {FileName}";
                    case 3: return $"Round {Round} - MySqlBackup.NET - {DatabaseName} < {FileName}";
                    case 4: return $"Round {Round} - MySql.exe - {DatabaseName} < {FileName}";
                }
                return "";
            }
        }
    }
}