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
                try
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
                    txtOutputFolder.Text = Server.MapPath("~/App_Data/backup");

                    string filePathReport = Path.Combine(txtOutputFolder.Text, $"benchmark-report-{DateTime.Now:yyyy-MM-dd_HHmmss}.txt");

                    txtReportFilePath.Text = filePathReport;
                }
                catch (Exception ex)
                {
                    ((masterPage1)this.Master).ShowMessage("Error", ex.Message, false);
                    ((masterPage1)this.Master).WriteTopMessageBar("Error<br />" + ex.Message, false);
                }
            }
        }

        protected async void btRun_Click(object sender, EventArgs e)
        {
            try
            {
                string folder = txtOutputFolder.Text;
                Directory.CreateDirectory(folder);

                string reportFilePath = txtReportFilePath.Text;

                string folderReport = Path.GetDirectoryName(reportFilePath);
                Directory.CreateDirectory(folderReport);

                BenchmarkTest.TaskId++;
                int newTaskId = BenchmarkTest.TaskId;

                int.TryParse(txtTotalRound.Text, out int totalRounds);

                if (totalRounds < 1) totalRounds = 1;
                if (totalRounds > 3) totalRounds = 3;

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
                    GetSystemInfo = cbGetSystemInfo.Checked,
                    CleanUpDatabase = cbCleanDatabaseAfterUse.Checked,
                    RunStage1 = cbRunStage1.Checked,
                    RunStage2 = cbRunStage2.Checked,
                    RunStage3 = cbRunStage3.Checked,
                    RunStage4 = cbRunStage4.Checked,
                    RunStage5 = cbRunStage5.Checked,
                    RunStage6 = cbRunStage6.Checked,
                    DeleteDumpFileAfterProcess = cbDeleteDumpFile.Checked,
                    TotalRounds = totalRounds
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
            catch (Exception ex)
            {
                ((masterPage1)this.Master).WriteTopMessageBar("Error: " + ex.Message, false);
            }
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

                var bti = new BenchmarkTestInfo();
                bti.TimeStart = timeStart;
                bti.Started = true;
                bti.dicStageInfo = new Dictionary<int, StageInfo>();
                bti.dicStageInfo[1] = new StageInfo()
                {
                    RunStage = bc.RunStage1,
                    StageId = 1,
                    StageName = "Backup/Export - MySqlBackup.NET"
                };
                bti.dicStageInfo[2] = new StageInfo()
                {
                    RunStage = bc.RunStage2,
                    StageId = 2,
                    StageName = "Backup/Export - MySqlBackup.NET - Parallel Process"
                };
                bti.dicStageInfo[3] = new StageInfo()
                {
                    RunStage = bc.RunStage3,
                    StageId = 3,
                    StageName = "Backup/Export - mysqldump.exe"
                };
                bti.dicStageInfo[4] = new StageInfo()
                {
                    RunStage = bc.RunStage4,
                    StageId = 4,
                    StageName = "Restore/Import - MySqlBackup.NET"
                };
                bti.dicStageInfo[5] = new StageInfo()
                {
                    RunStage = bc.RunStage5,
                    StageId = 5,
                    StageName = "Import/Restore - MySqlBackup.NET - Parallel Processing"
                };
                bti.dicStageInfo[6] = new StageInfo()
                {
                    RunStage = bc.RunStage6,
                    StageId = 6,
                    StageName = "Restore/Import - mysql.exe"
                };

                dicProgress[bc.TaskId] = bti;

                sb = new StringBuilder();

                sb.AppendLine("Benchmark Report - MySqlBackup.NET, mysqldump.exe, mysql.exe");
                sb.AppendLine("============================================================");
                sb.AppendLine();
                sb.AppendLine($"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine();
                sb.AppendLine("Report file saved at:");
                sb.AppendLine(bc.ReportFilePath);
                sb.AppendLine();

                if (bc.GetSystemInfo)
                {
                    GetSystemVariables();
                }

                var dirReport = Path.GetDirectoryName(bc.ReportFilePath);
                Directory.CreateDirectory(dirReport);
                File.WriteAllText(bc.ReportFilePath, sb.ToString());

                // Generate dump file names based on total rounds
                List<string> dumpFiles1 = new List<string>();  // MySqlBackup.NET
                List<string> dumpFiles2 = new List<string>();  // MySqlBackup.NET Parallel
                List<string> dumpFiles3 = new List<string>();  // MySqlDump
                List<string> dbNames1 = new List<string>();    // MySqlBackup.NET restore
                List<string> dbNames2 = new List<string>();    // MySqlBackup.NET restore Parallel
                List<string> dbNames3 = new List<string>();    // MySQL Instance restore

                for (int i = 1; i <= bc.TotalRounds; i++)
                {
                    dumpFiles1.Add(Path.Combine(bc.BaseFolder, $"benchmark-backup-mysqlbackupnet-{i}.txt"));
                    dumpFiles2.Add(Path.Combine(bc.BaseFolder, $"benchmark-backup-mysqlbackupnet-parallel-{i}.txt"));
                    dumpFiles3.Add(Path.Combine(bc.BaseFolder, $"benchmark-backup-mysqldump-{i}.txt"));
                    dbNames1.Add($"test_benchmark_restore_mysqlbackupnet_{i}");
                    dbNames2.Add($"test_benchmark_restore_mysqlbackupnet_parallel_{i}");
                    dbNames3.Add($"test_benchmark_restore_mysql_{i}");
                }

                sb.AppendLine();
                sb.AppendLine($"Initial Source Database: {bc.SourceDatabaseName}");
                sb.AppendLine();

                int taskCounter = 1;

                if (bc.RunStage1)
                {
                    // backup - MySqlBackup.NET
                    for (int i = 0; i < bc.TotalRounds; i++)
                    {
                        dicProgress[bc.TaskId].dicTask[taskCounter] = new BenchmarkTask(1, i + 1, bc.SourceDatabaseName, dumpFiles1[i], bc.RunStage1);
                        sb.AppendLine($"Dump File {taskCounter}: {dumpFiles1[i]}");
                        taskCounter++;
                    }
                }

                if (bc.RunStage2)
                {
                    // backup - MySqlBackup.NET - Parallel Processing
                    for (int i = 0; i < bc.TotalRounds; i++)
                    {
                        dicProgress[bc.TaskId].dicTask[taskCounter] = new BenchmarkTask(2, i + 1, bc.SourceDatabaseName, dumpFiles2[i], bc.RunStage2, true);
                        sb.AppendLine($"Dump File {taskCounter}: {dumpFiles2[i]}");
                        taskCounter++;
                    }
                }

                if (bc.RunStage3)
                {
                    // backup - MySqlDump
                    for (int i = 0; i < bc.TotalRounds; i++)
                    {
                        dicProgress[bc.TaskId].dicTask[taskCounter] = new BenchmarkTask(3, i + 1, bc.SourceDatabaseName, dumpFiles3[i], bc.RunStage3);
                        sb.AppendLine($"Dump File {taskCounter}: {dumpFiles3[i]}");
                        taskCounter++;
                    }
                }

                if (bc.RunStage4 || bc.RunStage5 || bc.RunStage6)
                    sb.AppendLine();

                if (bc.RunStage4)
                {
                    // restore - MySqlBackup.NET
                    for (int i = 0; i < bc.TotalRounds; i++)
                    {
                        dicProgress[bc.TaskId].dicTask[taskCounter] = new BenchmarkTask(4, i + 1, dbNames1[i], dumpFiles1[0], bc.RunStage4); // Use first dump file for all restore tests
                        sb.AppendLine($"Database {taskCounter}: {dbNames1[i]}");
                        taskCounter++;
                    }
                }

                if (bc.RunStage5)
                {
                    // restore - MySqlBackup.NET - Parallel Processing
                    for (int i = 0; i < bc.TotalRounds; i++)
                    {
                        dicProgress[bc.TaskId].dicTask[taskCounter] = new BenchmarkTask(5, i + 1, dbNames1[i], dumpFiles1[0], bc.RunStage5, true); // Use first dump file for all restore tests
                        sb.AppendLine($"Database {taskCounter}: {dbNames2[i]}");
                        taskCounter++;
                    }
                }

                if (bc.RunStage6)
                {
                    // restore - MySQL Instance
                    for (int i = 0; i < bc.TotalRounds; i++)
                    {
                        dicProgress[bc.TaskId].dicTask[taskCounter] = new BenchmarkTask(6, i + 1, dbNames3[i], dumpFiles1[0], bc.RunStage6); // Use first dump file for all restore tests
                        sb.AppendLine($"Database {taskCounter}: {dbNames3[i]}");
                        taskCounter++;
                    }
                }

                foreach (var kvStage in bti.dicStageInfo)
                {
                    var stageInfo = kvStage.Value;

                    if (!stageInfo.RunStage)
                        continue;

                    sb.AppendLine();
                    sb.AppendLine(stageInfo.StageName);
                    sb.AppendLine("--------------------------------");

                    foreach (var kvTask in dicProgress[bc.TaskId].dicTask)
                    {
                        var bt = kvTask.Value;

                        if (bt.Stage != stageInfo.StageId)
                            continue;

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
                                case 2:
                                    ExportMySqlBackupNET(bt.DatabaseName, bt.DumpFile, bt.IsParallel);
                                    break;
                                case 3:
                                    ExportMySqlDump(bt.DatabaseName, bt.DumpFile);
                                    break;
                                case 4:
                                case 5:
                                    if (!File.Exists(bt.DumpFile))
                                    {
                                        throw new Exception("Dump File 1 is not created yet. Please run Stage 1 first.");
                                    }
                                    ImportMySqlBackupNet(bt.DatabaseName, bt.DumpFile, bt.IsParallel);
                                    break;
                                case 6:
                                    if (!File.Exists(bt.DumpFile))
                                    {
                                        throw new Exception("Dump File 1 is not created yet. Please run Stage 1 first.");
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

                            if ((bt.Stage == 4 || bt.Stage == 5 || bt.Stage == 6) && bc.CleanUpDatabase)
                            {
                                sb.AppendLine(" -- Deleting database / Clean up...");
                                DropAndCreateDatabase(bt.DatabaseName, true, false);
                            }

                            sb.AppendLine($" -- Completed ({FormatTimeSpan(bt.TimeUsed)})");

                            switch (stageInfo.StageId)
                            {
                                case 1:
                                case 2:
                                case 3:
                                    bt.FileSize = new FileInfo(bt.DumpFile).Length;
                                    bt.Sha256 = Sha256.Compute(bt.DumpFile);
                                    if (bc.DeleteDumpFileAfterProcess)
                                    {
                                        if ((bc.RunStage4 || bc.RunStage5 || bc.RunStage6) && bt.DumpFile == dumpFiles1[0])
                                        {
                                            // do not delete the first dump file, required for the import test    
                                        }
                                        else
                                        {
                                            try
                                            {
                                                File.Delete(bt.DumpFile);
                                            }
                                            catch { }
                                        }
                                    }
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            bt.Completed = true;
                            bt.TimeEnd = DateTime.Now;
                            bt.TimeUsed = bt.TimeEnd - bt.TimeStart;
                            bt.HasError = true;
                            bt.LastError = ex;
                        }
                    }
                }

                timeEnd = DateTime.Now;
                var totalTimeElapsed = timeEnd - timeStart;

                var btProgress = dicProgress[bc.TaskId].dicTask;

                sb.AppendLine();
                sb.AppendLine("All processes ended");
                sb.AppendLine($"Time Start   : {timeStart:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"Time End     : {timeEnd:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"Time Elapsed : {totalTimeElapsed.Hours} h {totalTimeElapsed.Minutes} m {totalTimeElapsed.Seconds} s {totalTimeElapsed.Milliseconds} ms");
                sb.AppendLine();
                sb.AppendLine("Calculating SHA256 Checksums...");

                foreach (var btInfo in btProgress)
                {
                    var bt = btInfo.Value;

                    if (!bt.IsExport)
                        continue;

                    if (!bt.ActiveTask)
                        continue;

                    sb.AppendLine();
                    sb.AppendLine($"Dump File {btInfo.Key}: {bt.FileName}");
                    sb.AppendLine($"SHA256: {bt.Sha256}");
                }

                sb.AppendLine();
                sb.AppendLine("===================================");
                sb.AppendLine("Benchmark Results");
                sb.AppendLine("===================================");

                foreach (var kvStage in bti.dicStageInfo)
                {
                    var stageInfo = kvStage.Value;

                    if (!stageInfo.RunStage)
                        continue;

                    sb.AppendLine();
                    sb.AppendLine(stageInfo.StageName);
                    sb.AppendLine("---------------------------------");

                    foreach (var kv2 in btProgress)
                    {
                        var bt = kv2.Value;
                        if (!bt.ActiveTask)
                            continue;

                        if (bt.Stage != stageInfo.StageId)
                            continue;

                        if (bt.Stage <= 3)
                            sb.AppendLine($"Round {bt.Round}    {bt.TimeUsedDisplay}    {bt.FileSizeDisplay}");
                        else
                            sb.AppendLine($"Round {bt.Round}    {bt.TimeUsedDisplay}");
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

        void DropAndCreateDatabase(string dbName, bool drop, bool create)
        {
            using (MySqlConnection conn = new MySqlConnection(bc.ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    if (drop)
                    {
                        cmd.CommandText = $"DROP DATABASE IF EXISTS `{QueryExpress.EscapeIdentifier(dbName)}`";
                        cmd.ExecuteNonQuery();
                    }

                    if (create)
                    {
                        cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{QueryExpress.EscapeIdentifier(dbName)}`";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        void ExportMySqlBackupNET(string dbName, string dumpFilePath, bool parallel)
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
                        mb.ExportInfo.EnableParallelProcessing = parallel;
                        mb.ExportInfo.RecordDumpTime = false;

                        mb.ExportToFile(dumpFilePath);
                    }
                }
            }
        }

        void ImportMySqlBackupNet(string dbName, string dumpFilePath, bool parallel)
        {
            DropAndCreateDatabase(dbName, true, true);

            using (MySqlConnection conn = new MySqlConnection(bc.ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = $"USE `{QueryExpress.EscapeIdentifier(dbName)}`";
                    cmd.ExecuteNonQuery();

                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        mb.ImportInfo.EnableParallelProcessing = parallel;
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
            DropAndCreateDatabase(database, true, true);

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
            DropAndCreateDatabase(database, true, true);

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

            // Enhanced RAM Information
            try
            {
                sb.AppendLine();
                sb.AppendLine("Memory Details:");
                sb.AppendLine("---------------");

                ManagementObjectSearcher memorySearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
                var memoryModules = memorySearcher.Get().Cast<ManagementObject>().ToList();

                if (memoryModules.Any())
                {
                    // Group by speed and type
                    var memoryGroups = memoryModules
                        .GroupBy(mo => new
                        {
                            Speed = mo["Speed"]?.ToString() ?? "Unknown",
                            MemoryType = GetMemoryTypeDescription(mo["MemoryType"]?.ToString()),
                            FormFactor = GetFormFactorDescription(mo["FormFactor"]?.ToString())
                        })
                        .ToList();

                    long totalCapacity = 0;
                    int moduleCount = 0;

                    foreach (var group in memoryGroups)
                    {
                        var modules = group.ToList();
                        long groupCapacity = 0;

                        foreach (var module in modules)
                        {
                            if (long.TryParse(module["Capacity"]?.ToString(), out long capacity))
                            {
                                groupCapacity += capacity;
                                totalCapacity += capacity;
                            }
                            moduleCount++;
                        }

                        double groupCapacityGB = (double)groupCapacity / (1024 * 1024 * 1024);

                        sb.AppendLine($"RAM Modules: {modules.Count}x {groupCapacityGB / modules.Count:0.#}GB " +
                                     $"({group.Key.MemoryType}, {group.Key.Speed} MHz, {group.Key.FormFactor})");
                    }

                    double totalCapacityGB = (double)totalCapacity / (1024 * 1024 * 1024);
                    sb.AppendLine($"Total RAM: {totalCapacityGB:0.#}GB ({moduleCount} modules)");

                    // Get additional memory information
                    try
                    {
                        ManagementObjectSearcher boardSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                        foreach (ManagementObject board in boardSearcher.Get())
                        {
                            string manufacturer = board["Manufacturer"]?.ToString();
                            string product = board["Product"]?.ToString();
                            if (!string.IsNullOrEmpty(manufacturer) && !string.IsNullOrEmpty(product))
                            {
                                sb.AppendLine($"Motherboard: {manufacturer} {product}");
                                break;
                            }
                        }
                    }
                    catch { }

                    // Memory configuration details
                    if (memoryModules.Count > 0)
                    {
                        var firstModule = memoryModules.First();
                        string manufacturer = firstModule["Manufacturer"]?.ToString()?.Trim();
                        string partNumber = firstModule["PartNumber"]?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(manufacturer))
                            sb.AppendLine($"RAM Manufacturer: {manufacturer}");
                        if (!string.IsNullOrEmpty(partNumber))
                            sb.AppendLine($"RAM Part Number: {partNumber}");
                    }
                }
                else
                {
                    sb.AppendLine("RAM Details: Information not available");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"RAM Details: Error retrieving information - {ex.Message}");
            }

            sb.AppendLine();

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

            // MySQL Information with Performance Variables
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
                        // Basic server info
                        cmd.CommandText = @"SELECT @@version AS server_version, 
                                   @@max_allowed_packet AS max_packet, 
                                   (SELECT DEFAULT_CHARACTER_SET_NAME FROM information_schema.SCHEMATA WHERE SCHEMA_NAME = @dbName) AS db_charset";
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

                        // Get MySQL performance variables organized by category
                        sb.AppendLine();
                        sb.AppendLine("MySQL Performance Configuration:");
                        sb.AppendLine("================================");

                        // Define performance variables by category with descriptions
                        var performanceCategories = new List<(string CategoryName, string Description, List<(string VarName, string DisplayName)> Variables)>
                {
                    ("Memory & Buffer Settings",
                     "Memory allocation for caching and buffering operations - larger values generally improve performance but consume more RAM",
                     new List<(string, string)>
                     {
                         ("innodb_buffer_pool_size", "InnoDB Buffer Pool Size"),
                         ("innodb_log_buffer_size", "InnoDB Log Buffer Size"),
                         ("query_cache_size", "Query Cache Size"),
                         ("tmp_table_size", "Temporary Table Size"),
                         ("max_heap_table_size", "Max Heap Table Size"),
                         ("key_buffer_size", "Key Buffer Size (MyISAM)"),
                         ("sort_buffer_size", "Sort Buffer Size"),
                         ("read_buffer_size", "Read Buffer Size"),
                         ("read_rnd_buffer_size", "Read Random Buffer Size"),
                         ("join_buffer_size", "Join Buffer Size"),
                         ("bulk_insert_buffer_size", "Bulk Insert Buffer Size"),
                         ("myisam_sort_buffer_size", "MyISAM Sort Buffer Size")
                     }),

                    ("Transaction & Logging Settings",
                     "Controls transaction durability vs performance trade-offs - lower values = faster but less durable",
                     new List<(string, string)>
                     {
                         ("innodb_flush_log_at_trx_commit", "InnoDB Flush Log at Commit"),
                         ("innodb_log_file_size", "InnoDB Log File Size"),
                         ("sync_binlog", "Sync Binary Log"),
                         ("binlog_format", "Binary Log Format"),
                         ("query_cache_type", "Query Cache Type")
                     }),

                    ("I/O Performance Settings",
                     "Disk I/O configuration - should be tuned based on storage type (SSD vs HDD) and workload",
                     new List<(string, string)>
                     {
                         ("innodb_io_capacity", "InnoDB I/O Capacity"),
                         ("innodb_io_capacity_max", "InnoDB I/O Capacity Max"),
                         ("innodb_flush_method", "InnoDB Flush Method"),
                         ("innodb_read_io_threads", "InnoDB Read I/O Threads"),
                         ("innodb_write_io_threads", "InnoDB Write I/O Threads")
                     })
                };

                        foreach (var category in performanceCategories)
                        {
                            sb.AppendLine();
                            sb.AppendLine($"{category.CategoryName}:");
                            sb.AppendLine($"# {category.Description}");
                            sb.AppendLine(new string('-', category.CategoryName.Length + 1));

                            foreach (var variable in category.Variables)
                            {
                                try
                                {
                                    var value = QueryExpress.ExecuteScalarStr(cmd, $"SHOW VARIABLES LIKE '{variable.VarName}'", "Value");

                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        string displayValue = FormatMySqlSize(variable.VarName, value);
                                        sb.AppendLine($"{variable.VarName} = {displayValue}");
                                    }
                                    else
                                    {
                                        sb.AppendLine($"{variable.VarName} = Not available");
                                    }
                                }
                                catch
                                {
                                    sb.AppendLine($"{variable.VarName} = Error retrieving value");
                                }
                            }
                        }

                        // Get storage engine information
                        sb.AppendLine();
                        sb.AppendLine("Storage Engine Information:");
                        sb.AppendLine("---------------------------");

                        try
                        {
                            cmd.CommandText = "SHOW ENGINES";
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string engine = reader.GetString("Engine");
                                    string support = reader.GetString("Support");
                                    if (support == "DEFAULT" || support == "YES")
                                    {
                                        sb.AppendLine($"{engine}: {support}");
                                    }
                                }
                            }
                        }
                        catch
                        {
                            sb.AppendLine("Storage engine info: Not available");
                        }

                        // Get table engine distribution for the benchmark database
                        sb.AppendLine();
                        sb.AppendLine($"Table Storage Engines ({bc.SourceDatabaseName}):");
                        sb.AppendLine("--------------------------------------");

                        try
                        {
                            cmd.CommandText = $@"SELECT ENGINE, COUNT(*) as table_count 
                                       FROM information_schema.TABLES 
                                       WHERE TABLE_SCHEMA = '{QueryExpress.EscapeIdentifier(bc.SourceDatabaseName)}' 
                                       AND TABLE_TYPE = 'BASE TABLE' 
                                       GROUP BY ENGINE";

                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    sb.AppendLine($"{reader.GetString("ENGINE")}: {reader.GetInt32("table_count")} tables");
                                }
                            }
                        }
                        catch
                        {
                            sb.AppendLine("Table engine distribution: Not available");
                        }

                        // Continue with existing code for datadir, tables, etc.
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
            sb.AppendLine("Execution Note:");
            sb.AppendLine("- MySqlDump and MySql.exe are executed through .NET System.Diagnostics.Process without external script");
            sb.AppendLine("- MySqlBackup is executed through ASP.NET Web Application (.NET Framework) in asynchronous task");
            sb.AppendLine();
        }

        // Helper methods for memory information
        private string GetMemoryTypeDescription(string memoryType)
        {
            if (string.IsNullOrEmpty(memoryType) || !int.TryParse(memoryType, out int type))
                return "Unknown";

            switch (type)
            {
                case 20:
                    return "DDR";
                case 21:
                    return "DDR2";
                case 22:
                    return "DDR2 FB-DIMM";
                case 24:
                    return "DDR3";
                case 26:
                    return "DDR4";
                case 30:
                    return "DDR5";
                default:
                    return $"Type {type}";
            }
        }

        private string GetFormFactorDescription(string formFactor)
        {
            if (string.IsNullOrEmpty(formFactor) || !int.TryParse(formFactor, out int factor))
                return "Unknown";

            switch (factor)
            {
                case 8:
                    return "DIMM";
                case 12:
                    return "SO-DIMM";
                case 13:
                    return "Micro-DIMM";
                default:
                    return $"Form {factor}";
            }
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

        private string FormatMySqlSize(string variableName, string value)
        {
            // Variables that represent byte sizes
            var sizeVariables = new HashSet<string>
    {
        "innodb_buffer_pool_size", "innodb_log_file_size", "innodb_log_buffer_size",
        "query_cache_size", "tmp_table_size", "max_heap_table_size",
        "bulk_insert_buffer_size", "myisam_sort_buffer_size", "key_buffer_size",
        "sort_buffer_size", "read_buffer_size", "read_rnd_buffer_size", "join_buffer_size"
    };

            if (sizeVariables.Contains(variableName) && long.TryParse(value, out long bytes))
            {
                const long GB = 1024 * 1024 * 1024;
                const long MB = 1024 * 1024;
                const long KB = 1024;

                if (bytes >= GB)
                    return $"{(double)bytes / GB:0.##} GB ({value} bytes)";
                else if (bytes >= MB)
                    return $"{(double)bytes / MB:0.##} MB ({value} bytes)";
                else if (bytes >= KB)
                    return $"{(double)bytes / KB:0.##} KB ({value} bytes)";
                else
                    return $"{value} bytes";
            }

            return value;
        }

        string FormatTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan.TotalHours >= 1)
            {
                return $"{timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s {timeSpan.Milliseconds}ms";
            }
            else if (timeSpan.TotalMinutes >= 1)
            {
                return $"{timeSpan.Minutes}m {timeSpan.Seconds}s {timeSpan.Milliseconds}ms";
            }
            else if (timeSpan.TotalSeconds >= 1)
            {
                return $"{timeSpan.Seconds}s {timeSpan.Milliseconds}ms";
            }
            else
            {
                return $"{timeSpan.Milliseconds}ms";
            }
        }
    }

    public class BenchmarkTestInfo
    {
        [JsonPropertyName("dicTask")]
        public Dictionary<int, BenchmarkTask> dicTask { get; set; } = new Dictionary<int, BenchmarkTask>();

        [JsonPropertyName("dicStageInfo")]
        public Dictionary<int, StageInfo> dicStageInfo { get; set; } = new Dictionary<int, StageInfo>();

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
                if (TimeEnd != DateTime.MinValue)
                    return TimeEnd.ToString("yyyy-MM-dd, hh:mm:ss tt");
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

    public class StageInfo
    {
        public int StageId { get; set; }
        public string StageName { get; set; }
        public bool RunStage { get; set; }
    }

    public class BenchmarkTask
    {
        public bool ActiveTask { get; set; }
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
        public int PercentComplete { get; set; } = 0;
        public bool IsParallel { get; set; } = false;
        [JsonPropertyName("IsExport")]
        public bool IsExport
        {
            get
            {
                switch (Stage)
                {
                    case 1:
                    case 2:
                    case 3:
                        return true;
                }
                return false;
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

        public BenchmarkTask(int _stage, int _round, string _dbname, string _dumpfile, bool _activeTask)
        {
            Stage = _stage;
            Round = _round;
            DatabaseName = _dbname;
            DumpFile = _dumpfile;
            ActiveTask = _activeTask;
        }

        public BenchmarkTask(int _stage, int _round, string _dbname, string _dumpfile, bool _activeTask, bool _isparallel)
        {
            Stage = _stage;
            Round = _round;
            DatabaseName = _dbname;
            DumpFile = _dumpfile;
            IsParallel = _isparallel;
            ActiveTask = _activeTask;
        }

        public string StageName
        {
            get
            {
                switch (Stage)
                {
                    case 1:
                        return "Export/Backup - MySqlBackup.NET - Single Thread";
                    case 2:
                        return "Export/Backup - MySqlBackup.NET - Parallel Processing";
                    case 3:
                        return "Export/Backup - MySqlDump";
                    case 4:
                        return "Import/Restore - MySqlBackup.NET";
                    case 5:
                        return "Import/Restore - MySqlBackup.NET - Parallel Processing";
                    case 6:
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
                if (TimeEnd != DateTime.MinValue)
                    return TimeEnd.ToString("yyyy-MM-dd, hh:mm:ss tt");
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
                    case 2: return $"Round {Round} - MySqlBackup.NET (Parallel) - {DatabaseName} > {FileName}";
                    case 3: return $"Round {Round} - MySqlDump.exe - {DatabaseName} > {FileName}";
                    case 4: return $"Round {Round} - MySqlBackup.NET - {DatabaseName} < {FileName}";
                    case 5: return $"Round {Round} - MySql.exe - {DatabaseName} < {FileName}";
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
        public bool GetSystemInfo { get; set; } = true;
        public bool CleanUpDatabase { get; set; } = false;
        public bool RunStage1 { get; set; }
        public bool RunStage2 { get; set; }
        public bool RunStage3 { get; set; }
        public bool RunStage4 { get; set; }
        public bool RunStage5 { get; set; }
        public bool RunStage6 { get; set; }
        public bool DeleteDumpFileAfterProcess { get; set; }
        public int TotalRounds { get; set; }

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

        public bool RunStage(int stageid)
        {
            switch (stageid)
            {
                case 1: return RunStage1;
                case 2: return RunStage2;
                case 3: return RunStage3;
                case 4: return RunStage4;
                case 5: return RunStage5;
                case 6: return RunStage6;
            }
            return false;
        }
    }
}