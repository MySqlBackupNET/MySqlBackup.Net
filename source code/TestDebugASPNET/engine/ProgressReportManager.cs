using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.EnterpriseServices.Internal;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MySqlConnector;

namespace System
{
    public static class ProgressReportManager
    {
        public static int CreateNewTask(ProgressReportInfo pr)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["operation"] = pr.operation;
            dic["start_time"] = pr.start_time;
            dic["end_time"] = pr.end_time;
            dic["is_completed"] = pr.is_completed ? 1 : 0;
            dic["has_error"] = pr.has_error ? 1 : 0;
            dic["is_cancelled"] = pr.is_cancelled ? 1 : 0;
            dic["filename"] = pr.filename ?? "";
            dic["total_tables"] = pr.total_tables;
            dic["total_rows"] = pr.total_rows;
            dic["total_rows_current_table"] = pr.total_rows_current_table;
            dic["current_table"] = pr.current_table ?? "";
            dic["current_table_index"] = pr.current_table_index;
            dic["current_row"] = pr.current_row;
            dic["current_row_in_current_table"] = pr.current_row_in_current_table;
            dic["total_bytes"] = pr.total_bytes;
            dic["current_bytes"] = pr.current_bytes;
            dic["percent_complete"] = pr.percent_complete;
            dic["remarks"] = pr.remarks;
            dic["last_update_time"] = DateTime.Now;

            using (var connection = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand(connection))
                {
                    SQLiteHelper h = new SQLiteHelper(cmd);

                    h.Insert("progress_report", dic);

                    return Convert.ToInt32(connection.LastInsertRowId);
                }
            }
        }

        public static void CancelTask(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = $"update progress_report set client_request_cancel_task=1 where id={id};";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static bool TaskIsBeingRequestedToCancel(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = $"select client_request_cancel_task from progress_report where id={id};";
                    return cmd.ExecuteScalar() + "" == "1";
                }
            }
        }

        public static void DeleteTask(int id)
        {
            using (var conn = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"delete from progress_report where id={id};";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static ProgressReportInfo GetProgressReport(int id)
        {
            var lst = GetProgressReportList(id, false, false);
            if (lst.Count == 0)
                return null;
            return lst[0];
        }

        public static List<ProgressReportInfo> GetProgressReportList(int id, bool getAll, bool inProgress)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("select * from progress_report");

            if (id != 0)
            {
                sb.Append($" where id={id}");
            }
            else
            {
                if (!getAll)
                {
                    sb.Append(" where is_completed=");
                    if (inProgress)
                    {
                        sb.Append("0");
                    }
                    else
                    {
                        sb.Append("1");
                    }
                }
                sb.Append(" order by start_time desc, id desc;");
            }

            string sql = sb.ToString();

            List<ProgressReportInfo> lst = new List<ProgressReportInfo>();

            using (var connection = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand(connection))
                {
                    SQLiteHelper h = new SQLiteHelper(cmd);

                    lst = h.GetObjectList<ProgressReportInfo>(sql);

                    foreach (var pr in lst)
                    {
                        if ((DateTime.Now - pr.last_update_time).TotalSeconds > 10 && !pr.is_completed)
                        {
                            // some error has occur, the update is scheduled to be executed every 500ms
                            // if after 10000ms (10 seconds), there isn't any update and the task is not completed
                            // this indicates that the process (ASP.NET) has been terminated by IIS App Pool
                            // the task will never complete

                            h.Execute($"update progress_report set is_completed=1, is_cancelled=1, has_error=1, remarks='Terminated by system. Unknown error.' where id={pr.id};");

                            pr.is_completed = true;
                            pr.has_error = true;
                            pr.remarks = "Terminated by system. Unknown error.";
                        }
                    }
                }
            }
            return lst;
        }

        public static void UpdateProgress(ProgressReportInfo pr)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            if (pr.has_error)
            {
                dic["has_error"] = 1;
                dic["is_completed"] = 1;
                dic["is_cancelled"] = 1;
                dic["remarks"] = pr.remarks;
            }
            else
            {
                dic["total_tables"] = pr.total_tables;
                dic["total_rows"] = pr.total_rows;
                dic["total_rows_current_table"] = pr.total_rows_current_table;
                dic["current_table"] = pr.current_table ?? "";
                dic["current_table_index"] = pr.current_table_index;
                dic["current_row"] = pr.current_row;
                dic["current_row_in_current_table"] = pr.current_row_in_current_table;
                dic["percent_complete"] = pr.percent_complete;
                dic["last_update_time"] = DateTime.Now;
                dic["total_bytes"] = pr.total_bytes;
                dic["current_bytes"] = pr.current_bytes;
            }

            using (var connection = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand(connection))
                {
                    SQLiteHelper h = new SQLiteHelper(cmd);

                    h.Update("progress_report", dic, "id", pr.id);
                }
            }
        }

        public static void CompleteProgress(ProgressReportInfo pr)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["end_time"] = pr.end_time;
            dic["is_completed"] = 1;
            dic["has_error"] = pr.has_error ? 1 : 0;
            dic["is_cancelled"] = pr.is_cancelled ? 1 : 0;
            dic["last_update_time"] = DateTime.Now;

            if (pr.has_error)
                dic["remarks"] = pr.remarks;

            string filename = "";

            using (var connection = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(connection))
                {
                    SQLiteHelper h = new SQLiteHelper(cmd);

                    // First, get the filename BEFORE updating
                    filename = h.ExecuteScalar<string>($"select filename from progress_report where id={pr.id}");

                    h.Update("progress_report", dic, "id", pr.id);

                    cmd.ExecuteNonQuery();
                }
            }

            string filePath = Path.Combine(BackupFilesManager.folder, filename);

            // the process is cancelled
            if (pr.is_cancelled)
            {
                // delete the incomplete file
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return;
                }
            }

            // File doesn't exist, possibly because backup was cancelled before file was created
            if (!File.Exists(filePath))
            {
                return;
            }

            var fileInfo = new FileInfo(filePath);

            DatabaseFileRecord dbFile = new DatabaseFileRecord();
            dbFile.Operation = "Progress Backup";
            dbFile.Filename = filename;
            dbFile.OriginalFilename = filename;
            dbFile.LogFilename = "";
            dbFile.Filesize = fileInfo.Length;
            dbFile.DatabaseName = config.GetCurrentDatabaseName();
            dbFile.DateCreated = fileInfo.CreationTime;
            dbFile.Remarks = "";
            dbFile.Sha256 = BackupFilesManager.ComputeSha256File(filePath);
            dbFile.TaskId = pr.id;

            int dbfileid = BackupFilesManager.SaveRecord(dbFile);

            using (var conn = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"update progress_report set dbfile_id={dbfileid} where id={pr.id};";
                    cmd.ExecuteNonQuery();
                }
            }

            // Pre-generate the zip file in background if backup was successful
            if (!pr.has_error && !pr.is_cancelled && File.Exists(filePath))
            {
                Task.Run(() =>
                {
                    try
                    {
                        BackupFilesManager.PreGenerateZipFile(filePath);
                    }
                    catch
                    {
                        // Ignore errors - pre-generation is optional
                    }
                });
            }
        }
    }
}