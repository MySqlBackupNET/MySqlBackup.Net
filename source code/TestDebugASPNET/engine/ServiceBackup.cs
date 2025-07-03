using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;

namespace System
{
    public class ServiceBackup
    {
        int taskid = 0;

        public async Task StartAsync()
        {
            await StartAsync(null);
        }

        public async Task StartAsync(MySqlConnector.ExportInformations exportInfo)
        {
            string filename = $"Progress-Export-{DateTime.Now:yyyy-MM-dd HHmmss}.sql";
            string filePath = Path.Combine(BackupFilesManager.folder, filename);

            ProgressReportInfo pr = new ProgressReportInfo()
            {
                operation = 1, // backup
                start_time = DateTime.Now,
                end_time = DateTime.MinValue,
                is_completed = false,
                has_error = false,
                is_cancelled = false,
                filename = filename,
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
                dbfile_id = 0,
                has_file = true
            };

            taskid = ProgressReportManager.CreateNewTask(pr);

            // Fire and forget - start backup in background
            _ = Task.Run(async () => await RunAsync(filePath, exportInfo));
        }

        private async Task RunAsync(string filePath, MySqlConnector.ExportInformations exportInfo)
        {
            try
            {
                using (var conn = config.GetNewConnection())
                {
                    using (var cmd = new MySqlCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;

                        using (var mb = new MySqlBackup(cmd))
                        {
                            if (exportInfo != null)
                            {
                                mb.ExportInfo = exportInfo;
                            }
                            mb.ExportInfo.IntervalForProgressReport = 500;
                            mb.ExportProgressChanged += (sender, e) => Mb_ExportProgressChanged(sender, e);
                            mb.ExportCompleted += (sender, e) => Mb_ExportCompleted(sender, e);

                            // MySqlBackup.ExportToFile is synchronous, so wrap in Task.Run
                            await Task.Run(() => mb.ExportToFile(filePath));
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

        private void Mb_ExportProgressChanged(object sender, ExportProgressArgs e)
        {
            try
            {
                int percent = 0;

                if (e.CurrentRowIndexInAllTables >= e.TotalRowsInAllTables)
                {
                    percent = 100;
                }
                else if (e.CurrentRowIndexInAllTables > 0L && e.TotalRowsInAllTables > 0L)
                {
                    percent = (int)(e.CurrentRowIndexInAllTables * 100L / e.TotalRowsInAllTables);
                }

                // sometimes, the rows values will be inaccurate, because of mysql information schema caching out of sync
                if (percent > 100)
                {
                    percent = 100;
                }

                var pr = new ProgressReportInfo()
                {
                    id = taskid,
                    has_error = false,
                    total_tables = e.TotalTables,
                    total_rows = e.TotalRowsInAllTables,
                    total_rows_current_table = e.TotalRowsInCurrentTable,
                    current_table = e.CurrentTableName,
                    current_table_index = e.CurrentTableIndex,
                    current_row = e.CurrentRowIndexInAllTables,
                    current_row_in_current_table = e.CurrentRowIndexInCurrentTable,
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

        private void Mb_ExportCompleted(object sender, ExportCompleteArgs e)
        {
            var pr2 = new ProgressReportInfo()
            {
                id = taskid,
                end_time = DateTime.Now,
                is_completed = true,
                has_error = e.HasError,
                is_cancelled = e.CompletionType == MySqlBackup.ProcessEndType.Cancelled
            };

            if (e.HasError)
            {
                pr2.remarks = e.LastError.Message;
            }
            else if (e.CompletionType == MySqlBackup.ProcessEndType.Cancelled)
            {
                pr2.remarks = "Task cancelled by user request";
            }

            ProgressReportManager.CompleteProgress(pr2);
        }

    }
}