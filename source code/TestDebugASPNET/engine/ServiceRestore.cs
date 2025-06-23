using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace System
{
    public class ServiceRestore
    {
        int taskid = 0;

        public async Task StartAsync(int dbFileId)
        {
            await StartAsync(dbFileId, null);
        }

        public async Task StartAsync(int dbFileId, MySqlConnector.ImportInformations importInfo)
        {
            DatabaseFileRecord dbFile = BackupFilesManager.GetRecord(dbFileId);

            string dbFilePath = Path.Combine(BackupFilesManager.folder, dbFile.Filename);

            if (!File.Exists(dbFilePath))
            {
                throw new Exception("File not existed.");
            }

            ProgressReportInfo reportInfo = new ProgressReportInfo
            {
                operation = 2, // restore
                start_time = DateTime.Now,
                end_time = DateTime.MinValue,
                is_completed = false,
                has_error = false,
                is_cancelled = false,
                filename = dbFile.Filename,
                total_tables = 0,
                total_rows = 0,
                total_rows_current_table = 0,
                current_table = "",
                current_table_index = 0,
                current_row = 0,
                current_row_in_current_table = 0,
                total_bytes = 0,
                current_bytes = 0,
                percent_complete = 0,
                remarks = "",
                dbfile_id = dbFile.Id,
                last_update_time = DateTime.Now,
                client_request_cancel_task = false,
                has_file = true
            };

            taskid = ProgressReportManager.CreateNewTask(reportInfo);

            _ = Task.Run(async () => await RunAsync(dbFilePath, importInfo));
        }

        public async Task StartAsync(HttpPostedFile httpFile)
        {
            await StartAsync(httpFile, null);
        }

        public async Task StartAsync(HttpPostedFile httpFile, ImportInformations importInfo)
        {
            string filename = $"Progress-Restore-Zip-{DateTime.Now:yyyy-MM-dd HHmmss}";
            string filenamezip = $"{filename}.zip";
            string filenamesql = $"{filename}.sql";
            string filenamelog = $"{filename}.txt";

            string zipFileTempPath = Path.Combine(BackupFilesManager.tempFolder, filename);
            string zipFilePath = Path.Combine(BackupFilesManager.tempZipFolder, filenamezip);
            string dbFilePath = Path.Combine(BackupFilesManager.folder, filenamesql);

            string fileExtention = System.IO.Path.GetExtension(httpFile.FileName).ToLower();

            if (fileExtention == ".sql")
            {
                httpFile.SaveAs(dbFilePath);
            }
            else
            {
                httpFile.SaveAs(zipFilePath);
                Directory.CreateDirectory(zipFileTempPath);
                System.IO.Compression.ZipFile.ExtractToDirectory(zipFilePath, zipFileTempPath);

                string[] files = Directory.GetFiles(zipFileTempPath);
                if (files.Length == 0)
                {
                    throw new Exception("No sql file extracted.");
                }
                string extractedFilePath = files[0];
                File.Copy(extractedFilePath, dbFilePath);
            }

            FileInfo fileInfo = new FileInfo(dbFilePath);

            DatabaseFileRecord dbFile = new DatabaseFileRecord
            {
                Filename = filenamesql,
                Operation = "Progress Restore",
                OriginalFilename = httpFile.FileName,
                LogFilename = filenamelog,
                Sha256 = BackupFilesManager.ComputeSha256File(dbFilePath),
                Filesize = fileInfo.Length,
                DatabaseName = config.GetCurrentDatabaseName(),
                DateCreated = fileInfo.CreationTime,
                Remarks = ""
            };

            int dbfileid = BackupFilesManager.SaveRecord(dbFile);

            ProgressReportInfo reportInfo = new ProgressReportInfo
            {
                operation = 2, // restore
                start_time = DateTime.Now,
                end_time = DateTime.MinValue,
                is_completed = false,
                has_error = false,
                is_cancelled = false,
                filename = filenamesql,
                total_tables = 0,
                total_rows = 0,
                total_rows_current_table = 0,
                current_table = "",
                current_table_index = 0,
                current_row = 0,
                current_row_in_current_table = 0,
                total_bytes = 0,
                current_bytes = 0,
                percent_complete = 0,
                remarks = "",
                dbfile_id = dbfileid,
                last_update_time = DateTime.Now,
                client_request_cancel_task = false
            };

            taskid = ProgressReportManager.CreateNewTask(reportInfo);

            _ = Task.Run(async () => await RunAsync(dbFilePath, importInfo));
        }

        private async Task RunAsync(string filePath, MySqlConnector.ImportInformations importInfo)
        {
            try
            {
                using (MySqlConnection conn = config.GetNewConnection())
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();

                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            if (importInfo != null)
                            {
                                mb.ImportInfo = importInfo;
                            }

                            mb.ImportInfo.IntervalForProgressReport = 500;
                            mb.ImportProgressChanged += Mb_ImportProgressChanged;
                            mb.ImportCompleted += Mb_ImportCompleted;

                            mb.ImportFromFile(filePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var pr2 = new ProgressReportInfo()
                {
                    id = taskid,
                    end_time = DateTime.Now,
                    is_completed = true,
                    has_error = true,
                    remarks = $"Error: {ex.Message}"
                };

                ProgressReportManager.UpdateProgress(pr2);
            }
        }

        private void Mb_ImportProgressChanged(object sender, ImportProgressArgs e)
        {
            try
            {
                int percent = 0;

                if (e.CurrentBytes >= e.TotalBytes)
                {
                    percent = 100;
                }
                else if (e.CurrentBytes > 0L && e.TotalBytes > 0L)
                {
                    percent = (int)(e.CurrentBytes * 100L / e.TotalBytes);
                }

                var pr = new ProgressReportInfo()
                {
                    id = taskid,
                    has_error = false,
                    total_bytes = e.TotalBytes,
                    current_bytes = e.CurrentBytes,
                    percent_complete = percent,
                };

                ProgressReportManager.UpdateProgress(pr);

                bool requestCancelTask = ProgressReportManager.TaskIsBeingRequestedToCancel(taskid);

                if (requestCancelTask)
                {
                    // Explicitly calling the MySqlBackup.NET to stop
                    ((MySqlBackup)sender).StopAllProcess();
                }
            }
            catch
            {
                // TODO: Maybe log this some other day
                // too many log actions already...
            }
        }

        private void Mb_ImportCompleted(object sender, ImportCompleteArgs e)
        {
            var pr2 = new ProgressReportInfo()
            {
                id = taskid,
                end_time = DateTime.Now,
                is_completed = true,
                has_error = e.HasErrors,
                is_cancelled = e.CompleteType == MySqlBackup.ProcessEndType.Cancelled
            };

            if (e.HasErrors)
            {
                pr2.remarks = e.LastError.Message;
            }
            else if (e.CompleteType == MySqlBackup.ProcessEndType.Cancelled)
            {
                pr2.remarks = "Task cancelled by user request";
            }

            ProgressReportManager.CompleteProgress(pr2);
        }

    }
}