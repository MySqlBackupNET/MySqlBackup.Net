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
using System.Threading;

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
            string reportFilePath = Path.Combine(folder, $"benchmark_report_{DateTime.Now:yyyy-MM-dd HHmmss}.txt");

            BenchmarkTest.TaskId++;
            int newTaskId = BenchmarkTest.TaskId;

            var bc = new BenchmarkConfiguration
            {
                TaskId = newTaskId,
                ConnectionString = config.ConnString,
                SourceDatabaseName = txtInitialSchema.Text,
                BaseFolder = folder,
                ReportFilePath = reportFilePath,
                MySqlDumpFilePath = txtFilePathMySqlDump.Text,
                MySqlFilePath = txtFilePathMySql.Text,
                MySqlInstanceDirect = cbMySqlInstanceExecuteDirect.Checked,
                SkipGetSystemInfo = cbSkipGetSystemInfo.Checked,
                CleanUpDatabase = cbCleanDatabaseAfterUse.Checked,
                RunStage1 = cbRunStage1.Checked,
                RunStage2 = cbRunStage2.Checked,
                RunStage3 = cbRunStage3.Checked,
                RunStage4 = cbRunStage4.Checked
            };

            // Fire and forget, run the task asynchronously in background
            _ = Task.Run(() => RunTest(bc));

            panelSetup.Visible = false;
            panelResult.Visible = true;
            literalTaskId.Text = $@"
<script>
var taskid = {newTaskId};
</script>
";
        }

        private async Task RunTest(BenchmarkConfiguration bc)
        {
            var benchmarkTest = new BenchmarkTest(bc);
            benchmarkTest.Run();
        }
    }

    public class BenchmarkTest
    {
        public static int TaskId = 0;
        public static ConcurrentDictionary<int, BenchmarkTestInfo> dicProgress = new ConcurrentDictionary<int, BenchmarkTestInfo>();

        private readonly BenchmarkConfiguration bc;
        private StringBuilder sb = null;
        private string password = "";
        private long totalRows = 0L;
        private DateTime timeStart;
        private DateTime timeEnd;

        public BenchmarkTest(BenchmarkConfiguration _bc)
        {
            bc = _bc ?? throw new ArgumentNullException(nameof(config));
            bc.Validate();
        }

        public void Run()
        {
            try
            {
                timeStart = DateTime.Now;

                dicProgress[bc.TaskId] = new BenchmarkTestInfo();
                dicProgress[bc.TaskId].TimeStart = timeStart;
                dicProgress[bc.TaskId].Started = true;

                sb = new StringBuilder();

                sb.AppendLine("Benchmark Report - MySqlBackup.NET, mysqldump.exe, mysql.exe");
                sb.AppendLine("============================================================");
                sb.AppendLine();
                sb.AppendLine($"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine();
                sb.AppendLine("Report file saved at:");
                sb.AppendLine(bc.ReportFilePath);
                sb.AppendLine();

                if (!bc.SkipGetSystemInfo)
                {
                    GetSystemVariables();
                }

                var dirReport = Path.GetDirectoryName(bc.ReportFilePath);
                Directory.CreateDirectory(dirReport);
                File.WriteAllText(bc.ReportFilePath, sb.ToString());

                string dumpFile1 = Path.Combine(bc.BaseFolder, $"benchmark-backup-mysqlbackupnet-1.txt");
                string dumpFile2 = Path.Combine(bc.BaseFolder, $"benchmark-backup-mysqlbackupnet-2.txt");
                string dumpFile3 = Path.Combine(bc.BaseFolder, $"benchmark-backup-mysqlbackupnet-3.txt");
                string dumpFile4 = Path.Combine(bc.BaseFolder, $"benchmark-backup-mysqldump-1.txt");
                string dumpFile5 = Path.Combine(bc.BaseFolder, $"benchmark-backup-mysqldump-2.txt");
                string dumpFile6 = Path.Combine(bc.BaseFolder, $"benchmark-backup-mysqldump-3.txt");

                string dbName1 = $"test_benchmark_restore_mysqlbackupnet_1";
                string dbName2 = $"test_benchmark_restore_mysqlbackupnet_2";
                string dbName3 = $"test_benchmark_restore_mysqlbackupnet_3";
                string dbName4 = $"test_benchmark_restore_mysql_1";
                string dbName5 = $"test_benchmark_restore_mysql_2";
                string dbName6 = $"test_benchmark_restore_mysql_3";

                sb.AppendLine();
                sb.AppendLine($"Initial Source Database: {bc.SourceDatabaseName}");
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
                dicProgress[bc.TaskId].dicTask[1] = new BenchmarkTask(1, 1, bc.SourceDatabaseName, dumpFile1);
                dicProgress[bc.TaskId].dicTask[2] = new BenchmarkTask(1, 2, bc.SourceDatabaseName, dumpFile2);
                dicProgress[bc.TaskId].dicTask[3] = new BenchmarkTask(1, 3, bc.SourceDatabaseName, dumpFile3);

                // backup - MySqlDump
                dicProgress[bc.TaskId].dicTask[4] = new BenchmarkTask(2, 1, bc.SourceDatabaseName, dumpFile4);
                dicProgress[bc.TaskId].dicTask[5] = new BenchmarkTask(2, 2, bc.SourceDatabaseName, dumpFile5);
                dicProgress[bc.TaskId].dicTask[6] = new BenchmarkTask(2, 3, bc.SourceDatabaseName, dumpFile6);

                // restore - MySqlBackup.NET
                dicProgress[bc.TaskId].dicTask[7] = new BenchmarkTask(3, 1, dbName1, dumpFile1);
                dicProgress[bc.TaskId].dicTask[8] = new BenchmarkTask(3, 2, dbName2, dumpFile1);
                dicProgress[bc.TaskId].dicTask[9] = new BenchmarkTask(3, 3, dbName3, dumpFile1);

                // restore - MySQL Instance
                dicProgress[bc.TaskId].dicTask[10] = new BenchmarkTask(4, 1, dbName4, dumpFile1);
                dicProgress[bc.TaskId].dicTask[11] = new BenchmarkTask(4, 2, dbName5, dumpFile1);
                dicProgress[bc.TaskId].dicTask[12] = new BenchmarkTask(4, 3, dbName6, dumpFile1);

                List<int> lstStage = new List<int>();

                for (int taskId = 1; taskId < 13; taskId++)
                {
                    var bt = dicProgress[bc.TaskId].dicTask[taskId];

                    if (!RequireRunStage(bt.Stage))
                    {
                        continue;
                    }

                    if (!lstStage.Contains(bt.Stage))
                    {
                        lstStage.Add(bt.Stage);

                        sb.AppendLine();
                        sb.AppendLine(bt.StageName);
                        sb.AppendLine("--------------------------------");
                    }

                    try
                    {
                        sb.AppendLine();
                        sb.AppendLine(bt.ActionInfo);
                        File.WriteAllText(bc.ReportFilePath, sb.ToString());
                        dicProgress[bc.TaskId].Remarks = sb.ToString();

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
                                if (!File.Exists(bt.DumpFile))
                                {
                                    throw new Exception("Dump File 1 is not created yet. Please run Stage 1.");
                                }
                                ImportMySqlBackupNet(bt.DatabaseName, bt.DumpFile);
                                break;
                            case 4:
                                if (!File.Exists(bt.DumpFile))
                                {
                                    throw new Exception("Dump File 1 is not created yet. Please run Stage 1.");
                                }
                                if (bc.MySqlInstanceDirect)
                                {
                                    ImportMySqlInstanceDirect(bt.DatabaseName, bt.DumpFile);
                                }
                                else
                                {
                                    ImportMySqlInstanceCmdShellRedirect(bt.DatabaseName, bt.DumpFile);
                                }
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

                var btProgress = dicProgress[bc.TaskId].dicTask;

                for (int round = 1; round < 7; round++)
                {
                    string file = btProgress[round].DumpFile;

                    if (!string.IsNullOrEmpty(file) && File.Exists(file))
                    {
                        btProgress[round].FileSize = new FileInfo(btProgress[round].DumpFile).Length;
                        btProgress[round].Sha256 = CalculateSHA256File(btProgress[round].DumpFile);
                    }
                }

                sb.AppendLine();
                sb.AppendLine("All processes ended");
                sb.AppendLine($"Time Start   : {timeStart:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"Time End     : {timeEnd:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"Time Elapsed : {totalTimeElapsed.Hours} h {totalTimeElapsed.Minutes} m {totalTimeElapsed.Seconds} s {totalTimeElapsed.Milliseconds} ms");

                sb.AppendLine();
                sb.AppendLine("SHA256 Checksums:");

                for (int round = 1; round < 7; round++)
                {
                    var bt = btProgress[round];

                    if (!string.IsNullOrEmpty(bt.DumpFile) && File.Exists(bt.DumpFile))
                    {
                        sb.AppendLine();
                        sb.AppendLine($"Dump File {round}: {bt.FileName}");
                        sb.AppendLine($"SHA256: {bt.Sha256}");
                    }
                }

                sb.AppendLine();
                sb.AppendLine("===================================");
                sb.AppendLine("Benchmark Results");
                sb.AppendLine("===================================");

                Dictionary<int, string> dicStageName = new Dictionary<int, string>();
                dicStageName[1] = "Backup/Export - MySqlBackup.NET";
                dicStageName[2] = "Backup/Export - mysqldump.exe";
                dicStageName[3] = "Restore/Import - MySqlBackup.NET";
                dicStageName[4] = "Restore/Import - mysql.exe";

                foreach (var kv in dicStageName)
                {
                    sb.AppendLine();
                    sb.AppendLine(kv.Value);
                    sb.AppendLine("---------------------------------");

                    foreach (var kv2 in btProgress)
                    {
                        var bt = kv2.Value;

                        if (bt.Stage != kv.Key)
                            continue;

                        if (bt.Stage < 3)
                            sb.AppendLine($"Round {bt.Round}    {bt.TimeUsedDisplay}    {bt.FileSizeDisplay}");
                        else
                            sb.AppendLine($"Round {bt.Round}    {bt.TimeUsedDisplay}");
                    }
                }

                if (bc.CleanUpDatabase)
                {
                    string[] dropSqls = { dbName1, dbName2, dbName3, dbName4, dbName5, dbName6 };
                    using (MySqlConnection conn = new MySqlConnection(bc.ConnectionString))
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            conn.Open();
                            foreach (var dbname in dropSqls)
                            {
                                cmd.CommandText = $"DROP DATABASE IF EXISTS `{dbname}`";
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    sb.AppendLine();
                    sb.AppendLine("Clean up / removing databases");
                    sb.AppendLine();
                    foreach (var dbname in dropSqls)
                    {
                        sb.AppendLine($"DROP DATABASE IF EXISTS `{dbname}`");
                    }
                }

                File.WriteAllText(bc.ReportFilePath, sb.ToString());

                dicProgress[bc.TaskId].Completed = true;
                dicProgress[bc.TaskId].Remarks = sb.ToString();
                dicProgress[bc.TaskId].TimeEnd = timeEnd;
                dicProgress[bc.TaskId].TimeUsed = totalTimeElapsed;
            }
            catch (Exception ex)
            {
                timeEnd = DateTime.Now;
                var totalTimeElapsed = timeEnd - timeStart;

                dicProgress[bc.TaskId].Remarks = sb?.ToString() ?? "";
                dicProgress[bc.TaskId].TimeEnd = timeEnd;
                dicProgress[bc.TaskId].TimeUsed = totalTimeElapsed;
                dicProgress[bc.TaskId].Completed = true;
                dicProgress[bc.TaskId].HasError = true;
                dicProgress[bc.TaskId].LastError = ex;
            }
        }

        string GenerateTempMySqlConfigFile()
        {
            var builder = new MySqlConnectionStringBuilder(bc.ConnectionString);

            string host = builder.Server ?? "localhost";
            string user = builder.UserID ?? "root";
            password = builder.Password ?? "";
            uint port = builder.Port != 0 ? builder.Port : 3306;
            string dbCharacterSet = "";

            using (MySqlConnection conn = new MySqlConnection(bc.ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    dbCharacterSet = QueryExpress.ExecuteScalarStr(cmd, "SHOW VARIABLES LIKE 'character_set_database'", 1);
                }
            }

            Random rd = new Random();

            string filePathCnf = Path.Combine(bc.BaseFolder, $"my_{rd.Next(100000, 999999)}.cnf");
            string cnfContent = $@"[client]
user={user}
password={password}
host={host}
port={port}
default-character-set={dbCharacterSet}";

            File.WriteAllText(filePathCnf, cnfContent);

            return filePathCnf;
        }

        private bool RequireRunStage(int stage)
        {
            switch (stage)
            {
                case 1: return bc.RunStage1;
                case 2: return bc.RunStage2;
                case 3: return bc.RunStage3;
                case 4: return bc.RunStage4;
            }
            return false;
        }

        void ExportMySqlBackupNET(string dbName, string dumpFilePath)
        {
            using (MySqlConnection conn = new MySqlConnection(bc.ConnectionString))
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

        void DropAndCreateDatabase(string dbName)
        {
            using (MySqlConnection conn = new MySqlConnection(bc.ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = $"DROP DATABASE IF EXISTS `{QueryExpress.EscapeIdentifier(dbName)}`";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{QueryExpress.EscapeIdentifier(dbName)}`";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        void ImportMySqlBackupNet(string dbName, string dumpFilePath)
        {
            DropAndCreateDatabase(dbName);

            using (MySqlConnection conn = new MySqlConnection(bc.ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

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
            string filePathCnf = GenerateTempMySqlConfigFile();
            string arguments = $"--defaults-file=\"{filePathCnf}\" --routines --events {database} --result-file=\"{dumpFilePath}\"";

            // Configure process to run hidden
            var processStartInfo = new ProcessStartInfo
            {
                FileName = bc.MySqlDumpFilePath,
                Arguments = arguments,
                UseShellExecute = false,        // Required for hiding window
                CreateNoWindow = true,          // Don't create a window
                WindowStyle = ProcessWindowStyle.Hidden,  // Hide if window is created
                RedirectStandardOutput = true,  // Capture output
                RedirectStandardError = true    // Capture errors
            };

            _ = Task.Run(() => DeleteMyCnf(filePathCnf));
            ExecuteProcess(processStartInfo);
        }

        void ImportMySqlInstanceDirect(string database, string dumpFilePath)
        {
            DropAndCreateDatabase(database);

            string filePathCnf = GenerateTempMySqlConfigFile();

            string arguments = $"\"--defaults-extra-file={filePathCnf}\" \"{database}\" \"--execute=SOURCE {dumpFilePath}\"";

            var processStartInfo = new ProcessStartInfo
            {
                FileName = bc.MySqlFilePath,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            _ = Task.Run(() => DeleteMyCnf(filePathCnf));
            ExecuteProcess(processStartInfo);
        }

        void ImportMySqlInstanceCmdShellRedirect(string database, string dumpFilePath)
        {
            DropAndCreateDatabase(database);

            string filePathCnf = GenerateTempMySqlConfigFile();

            string arguments = $"/C \"\"{bc.MySqlFilePath}\" --defaults-extra-file=\"{filePathCnf}\" --database={database} < \"{dumpFilePath}\"\"";

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            _ = Task.Run(() => DeleteMyCnf(filePathCnf));
            ExecuteProcess(processStartInfo);
        }

        async void ExecuteProcess(ProcessStartInfo processStartInfo)
        {
            sb.AppendLine();
            sb.AppendLine($"Executing Process: {processStartInfo.FileName}");
            sb.AppendLine($"Arguments: {processStartInfo.Arguments}");

            using (var process = Process.Start(processStartInfo))
            {
                // Read both streams asynchronously to prevent deadlock
                Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
                Task<string> errorTask = process.StandardError.ReadToEndAsync();

                process.WaitForExit();

                string output = await outputTask;
                string errors = await errorTask;

                if (!string.IsNullOrEmpty(output))
                {
                    sb.AppendLine();
                    sb.AppendLine("Process output:");
                    sb.AppendLine("-------------------------------");
                    sb.AppendLine(output);
                    sb.AppendLine("-------------------------------");
                }

                if (process.ExitCode != 0 || !string.IsNullOrEmpty(errors))
                {
                    sb.AppendLine();
                    sb.AppendLine("Process error:");
                    sb.AppendLine("-------------------------------");
                    sb.AppendLine($"Exit Code: {process.ExitCode}");
                    sb.AppendLine(errors);
                    sb.AppendLine("-------------------------------");
                    throw new Exception($"Process error: [Exit Code:{process.ExitCode}] {errors}");
                }
            }
        }

        void DeleteMyCnf(string filePathCnf)
        {
            const int maxAttempts = 3;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                Thread.Sleep(500);

                if (!File.Exists(filePathCnf))
                    return;

                try
                {
                    File.Delete(filePathCnf);
                    return;
                }
                catch { }
            }
        }

        void GetSystemVariables()
        {
            sb.AppendLine("System Information:");
            sb.AppendLine("============================================================");

            // OS Information
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject mo in searcher.Get())
                {
                    sb.AppendLine($"OS: {mo["Caption"]} {(Environment.Is64BitOperatingSystem ? "x64" : "x86")} (version {mo["Version"]}, build: {mo["BuildNumber"]})");
                    sb.AppendLine($"RAM: {(Convert.ToInt64(mo["TotalVisibleMemorySize"]) / 1000 / 1000):N0}GB");
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

            // Get MySQL data directory and dump file directory
            string mysqlDataDir = "";
            string dumpFileDir = "";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(bc.ConnectionString))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        mysqlDataDir = QueryExpress.ExecuteScalarStr(cmd, "SHOW VARIABLES LIKE 'datadir'", "Value");
                    }
                }

                // Get dump file directory from the first dump file path
                if (!string.IsNullOrEmpty(bc.BaseFolder))
                {
                    dumpFileDir = bc.BaseFolder;
                }
            }
            catch { }

            // Get relevant disk information
            try
            {
                // Get MySQL server disk
                string mysqlDrive = "";
                string dumpDrive = "";

                if (!string.IsNullOrEmpty(mysqlDataDir))
                {
                    mysqlDrive = Path.GetPathRoot(mysqlDataDir)?.TrimEnd('\\', '/');
                }

                if (!string.IsNullOrEmpty(dumpFileDir))
                {
                    dumpDrive = Path.GetPathRoot(dumpFileDir)?.TrimEnd('\\', '/');
                }

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive WHERE MediaType = 'Fixed hard disk media'");
                ManagementObjectSearcher logicalDiskSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk WHERE DriveType = 3");

                // Create mapping of drive letters to disk models and sizes
                Dictionary<string, string> driveInfo = new Dictionary<string, string>();

                foreach (ManagementObject logicalDisk in logicalDiskSearcher.Get())
                {
                    string deviceId = logicalDisk["DeviceID"]?.ToString();
                    if (!string.IsNullOrEmpty(deviceId))
                    {
                        // Get the physical disk for this logical disk
                        ManagementObjectSearcher partitionSearcher = new ManagementObjectSearcher($"ASSOCIATORS OF {{Win32_LogicalDisk.DeviceID='{deviceId}'}} WHERE AssocClass = Win32_LogicalDiskToPartition");
                        foreach (ManagementObject partition in partitionSearcher.Get())
                        {
                            ManagementObjectSearcher diskSearcher = new ManagementObjectSearcher($"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partition["DeviceID"]}'}} WHERE AssocClass = Win32_DiskDriveToDiskPartition");
                            foreach (ManagementObject disk in diskSearcher.Get())
                            {
                                string model = disk["Model"]?.ToString() ?? "Unknown";
                                long size = Convert.ToInt64(disk["Size"] ?? 0);
                                string sizeDisplay = $"{(size / 1024 / 1024 / 1024):N0}GB";
                                driveInfo[deviceId] = $"{model} (Size: {sizeDisplay})";
                                break;
                            }
                            break;
                        }
                    }
                }

                // Display MySQL server disk
                if (!string.IsNullOrEmpty(mysqlDrive) && driveInfo.ContainsKey(mysqlDrive))
                {
                    sb.AppendLine($"Hard Disk (MySql Server): {driveInfo[mysqlDrive]}");
                }
                else if (!string.IsNullOrEmpty(mysqlDrive))
                {
                    sb.AppendLine($"Hard Disk (MySql Server): {mysqlDrive} (Information not available)");
                }

                // Display dump file disk (only if different from MySQL server disk)
                if (!string.IsNullOrEmpty(dumpDrive) && driveInfo.ContainsKey(dumpDrive))
                {
                    if (mysqlDrive != dumpDrive)
                    {
                        sb.AppendLine($"Hard Disk (Dump File)   : {driveInfo[dumpDrive]}");
                    }
                    else
                    {
                        sb.AppendLine($"Hard Disk (Dump File)   : Same as MySql Server");
                    }
                }
                else if (!string.IsNullOrEmpty(dumpDrive))
                {
                    if (mysqlDrive != dumpDrive)
                    {
                        sb.AppendLine($"Hard Disk (Dump File)   : {dumpDrive} (Information not available)");
                    }
                    else
                    {
                        sb.AppendLine($"Hard Disk (Dump File)   : Same as MySql Server");
                    }
                }
            }
            catch
            {
                sb.AppendLine("Hard Disk: Information not available");
            }

            // MySQL Information
            string databaseFolderPath = "";
            DataTable dtTables = null;
            string serverVersion = "";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(bc.ConnectionString))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT @@version AS server_version, @@max_allowed_packet AS max_packet, (SELECT DEFAULT_CHARACTER_SET_NAME FROM information_schema.SCHEMATA WHERE SCHEMA_NAME = @dbName) AS db_charset";
                        cmd.Parameters.AddWithValue("@dbName", bc.SourceDatabaseName);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                serverVersion = reader.GetString("server_version");
                                sb.AppendLine($"MySQL Server: v{serverVersion}");
                                sb.AppendLine($"Database Character Set ({bc.SourceDatabaseName}): {(reader.IsDBNull(2) ? "Database not found" : reader.GetString("db_charset"))}");
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
            sb.AppendLine();
        }

        private long GetDatabaseSize()
        {
            long totalSize = 0;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(bc.ConnectionString))
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

        [JsonPropertyName("TimeStartDisplay")]
        public string TimeStartDisplay
        {
            get
            {
                if (TimeStart != DateTime.MinValue)
                    return TimeStart.ToString("yyyy-MM-dd, hh:mm:ss tt");
                return "---";
            }
        }

        [JsonPropertyName("TimeEndDisplay")]
        public string TimeEndDisplay
        {
            get
            {
                if (TimeStart != DateTime.MinValue)
                    return TimeStart.ToString("yyyy-MM-dd, hh:mm:ss tt");
                return "---";
            }
        }

        [JsonPropertyName("TimeUsedDisplay")]
        public string TimeUsedDisplay
        {
            get
            {
                return $"{TimeUsed.Hours}h {TimeUsed.Minutes}m {TimeUsed.Seconds}s {TimeUsed.Milliseconds}ms";
            }
        }

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

        [JsonPropertyName("TimeStartDisplay")]
        public string TimeStartDisplay
        {
            get
            {
                if (TimeStart != DateTime.MinValue)
                    return TimeStart.ToString("yyyy-MM-dd, hh:mm:ss tt");
                return "---";
            }
        }

        [JsonPropertyName("TimeEndDisplay")]
        public string TimeEndDisplay
        {
            get
            {
                if (TimeStart != DateTime.MinValue)
                    return TimeStart.ToString("yyyy-MM-dd, hh:mm:ss tt");
                return "---";
            }
        }

        [JsonPropertyName("TimeUsedDisplay")]
        public string TimeUsedDisplay
        {
            get
            {
                return $"{TimeUsed.Hours}h {TimeUsed.Minutes}m {TimeUsed.Seconds}s {TimeUsed.Milliseconds}ms";
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

    public class BenchmarkConfiguration
    {
        public int TaskId { get; set; }
        public string ConnectionString { get; set; }
        public string SourceDatabaseName { get; set; }
        public string BaseFolder { get; set; }
        public string ReportFilePath { get; set; }
        public string MySqlDumpFilePath { get; set; }
        public string MySqlFilePath { get; set; }
        public bool MySqlInstanceDirect { get; set; } = true;
        public bool SkipGetSystemInfo { get; set; } = true;
        public bool CleanUpDatabase { get; set; } = false;
        public bool RunStage1 { get; set; }
        public bool RunStage2 { get; set; }
        public bool RunStage3 { get; set; }
        public bool RunStage4 { get; set; }

        public BenchmarkConfiguration()
        {
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new ArgumentException("ConnectionString is required");
            if (string.IsNullOrEmpty(SourceDatabaseName))
                throw new ArgumentException("SourceDatabaseName is required");
            if (string.IsNullOrEmpty(BaseFolder))
                throw new ArgumentException("BaseFolder is required");
            if (string.IsNullOrEmpty(ReportFilePath))
                throw new ArgumentException("ReportFilePath is required");
        }
    }
}