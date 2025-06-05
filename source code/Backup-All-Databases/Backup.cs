using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Backup_All_Databases
{
    internal class Backup
    {
        public event EventHandler<BackupChangedEventArgs> ProgressChanged;

        public class BackupChangedEventArgs : EventArgs
        {
            public string CurrentDatabase { get; set; }
            public int Total { get; set; }
            public int Current { get; set; }
            public string LogFilePath { get; set; }
        }

        long TotalAllFilesSize { get; set; }
        public bool Terminate { get; set; } = false;

        public Backup()
        {

        }

        public void Run(Config config)
        {
            if (config == null)
            {
                return;
            }

            try
            {
                string backupFolder = Path.Combine(config.BackupPath, DateTime.Now.ToString("yyyy-MM-dd tt hhmmss"));
                Directory.CreateDirectory(backupFolder);

                Stopwatch sw1 = new Stopwatch();
                sw1.Start();

                string logFile = Path.Combine(backupFolder, "log.txt");

                using (FileStream fs = new FileStream(logFile, FileMode.Create, FileAccess.Write, FileShare.Read))
                using (StreamWriter logWriter = new StreamWriter(fs))
                {
                    logWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Process started");
                    logWriter.Flush();

                    try
                    {
                        if (config.MaxBackupCopies > 0)
                        {
                            // Get all backup folders in the backup path
                            string[] backupFolders = Directory.GetDirectories(config.BackupPath)
                                .Where(dir => Directory.GetCreationTime(dir) != DateTime.MinValue) // Ensure valid creation time
                                .OrderBy(dir => Directory.GetCreationTime(dir)) // Sort by creation time (oldest first)
                                .ToArray();

                            // Delete oldest folders if count exceeds MaxBackupCopies
                            int foldersToDelete = backupFolders.Length - (config.MaxBackupCopies - 1); // -1 to make room for the new folder
                            if (foldersToDelete > 0)
                            {
                                for (int i = 0; i < foldersToDelete && i < backupFolders.Length; i++)
                                {
                                    try
                                    {
                                        Directory.Delete(backupFolders[i], true); // Recursive delete
                                        logWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Deleted old backup folder: {backupFolders[i]}");
                                    }
                                    catch (Exception ex)
                                    {
                                        logWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Failed to delete old backup folder {backupFolders[i]}: {ex.Message}");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Error managing backup folders: {ex.Message}");
                    }

                    using (MySqlConnection conn = new MySqlConnection(config.ConnectionString))
                    {
                        conn.Open();

                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.Connection = conn;

                            List<string> lstDatabases = new List<string>();

                            if (config.BackupAllDatabases || config.IncludeList == null || config.IncludeList.Count == 0)
                            {
                                DataTable dtDatabases = new DataTable();
                                cmd.CommandText = "SELECT SCHEMA_NAME FROM information_schema.SCHEMATA WHERE SCHEMA_NAME NOT IN ('information_schema', 'mysql', 'performance_schema', 'sys', 'rdsadmin');";
                                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                                adapter.Fill(dtDatabases);
                                foreach (DataRow dr in dtDatabases.Rows)
                                {
                                    lstDatabases.Add(dr[0] + "");
                                }
                            }
                            else
                            {
                                lstDatabases.AddRange(config.IncludeList);
                            }

                            if (config.ExcludeList != null && config.ExcludeList.Count > 0)
                                lstDatabases.RemoveAll(db => config.ExcludeList.Contains(db));

                            int count = 0;

                            foreach (var db in lstDatabases)
                            {
                                if (Terminate)
                                {
                                    logWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Process terminated by user");
                                    if (ProgressChanged != null)
                                    {
                                        BackupChangedEventArgs arg = new BackupChangedEventArgs()
                                        {
                                            CurrentDatabase = db,
                                            Current = count,
                                            Total = lstDatabases.Count,
                                            LogFilePath = logFile
                                        };
                                        ProgressChanged.Invoke(this, arg);
                                    }
                                    break;
                                }

                                count++;

                                if (ProgressChanged != null)
                                {
                                    BackupChangedEventArgs arg = new BackupChangedEventArgs()
                                    {
                                        CurrentDatabase = db,
                                        Current = count,
                                        Total = lstDatabases.Count,
                                        LogFilePath = logFile
                                    };
                                    ProgressChanged.Invoke(this, arg);
                                }

                                Stopwatch sw2 = Stopwatch.StartNew();

                                try
                                {
                                    if (string.IsNullOrEmpty(db))
                                        continue;

                                    string dbBackupFilePath = Path.Combine(backupFolder, $"{db}.sql");

                                    logWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {count} of {lstDatabases.Count}");
                                    logWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{db}] Backup started....");
                                    logWriter.Flush();

                                    if (ProgressChanged != null)
                                    {
                                        BackupChangedEventArgs arg = new BackupChangedEventArgs()
                                        {
                                            CurrentDatabase = db,
                                            Current = count,
                                            Total = lstDatabases.Count,
                                            LogFilePath = logFile
                                        };
                                        ProgressChanged.Invoke(this, arg);
                                    }

                                    cmd.CommandText = $"USE `{db}`;";
                                    cmd.ExecuteNonQuery();

                                    using (FileStream fsSQL = new FileStream(dbBackupFilePath, FileMode.Create))
                                    {
                                        using (MySqlBackup mb = new MySqlBackup(cmd))
                                        {
                                            mb.ExportToStream(fsSQL);
                                        }
                                    }

                                    sw2.Stop();

                                    var dbTime = sw2.Elapsed;

                                    string timeString = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{db}] Completed";
                                    if (dbTime.Days > 0)
                                        timeString += $" {dbTime.Days} d";
                                    if (dbTime.Hours > 0)
                                        timeString += $" {dbTime.Hours} h";
                                    if (dbTime.Minutes > 0)
                                        timeString += $" {dbTime.Minutes} m";
                                    timeString += $" {dbTime.Seconds} s {dbTime.Milliseconds} ms";

                                    // Calculate file size
                                    long fileSizeBytes = new FileInfo(dbBackupFilePath).Length;
                                    long fileSizeMB = fileSizeBytes / (1024 * 1024);
                                    long fileSizeGB = fileSizeMB / 1024;
                                    fileSizeMB = fileSizeMB % 1024;
                                    long remainingBytes = fileSizeBytes % (1024 * 1024);

                                    TotalAllFilesSize += fileSizeBytes;

                                    logWriter.WriteLine(timeString);
                                    logWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{db}] File size: {fileSizeGB} GB {fileSizeMB} MB {remainingBytes} Bytes");
                                    logWriter.WriteLine();
                                }
                                catch (Exception ex)
                                {
                                    sw2.Stop();
                                    logWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{db}] Error: {ex.Message}");
                                }

                                sw2 = null;

                                GC.Collect();
                                logWriter.Flush();
                            }
                        }
                        conn.Close();
                    }

                    sw1.Stop();
                    var totalTime = sw1.Elapsed;

                    string timeString2 = $"Total time spent:";
                    if (totalTime.Days > 0)
                        timeString2 += $" {totalTime.Days} d";
                    if (totalTime.Hours > 0)
                        timeString2 += $" {totalTime.Hours} h";
                    if (totalTime.Minutes > 0)
                        timeString2 += $" {totalTime.Minutes} m";
                    timeString2 += $" {totalTime.Seconds} s {totalTime.Milliseconds} ms";

                    long tfileSizeMB = TotalAllFilesSize / (1024 * 1024);
                    long tfileSizeGB = tfileSizeMB / 1024;
                    tfileSizeMB = tfileSizeMB % 1024;
                    long tremainingBytes = TotalAllFilesSize % (1024 * 1024);

                    logWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Process ended");
                    logWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {timeString2}");
                    logWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Total File size: {tfileSizeGB} GB {tfileSizeMB} MB {tremainingBytes} Bytes");
                    logWriter.Flush();
                }

                if (ProgressChanged != null)
                {
                    BackupChangedEventArgs arg = new BackupChangedEventArgs();
                    ProgressChanged.Invoke(this, arg);
                }
            }
            catch (Exception ex)
            {
                string errorLog = Path.Combine(config.BackupPath, "error_log.txt");
                using (StreamWriter streamWriter = new StreamWriter(errorLog, true))
                {
                    streamWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Backup failed: {ex.Message}");
                }
            }
        }

        public void Stop()
        {
            Terminate = true;
        }
    }
}
