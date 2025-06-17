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
            using (var connection = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = @"INSERT INTO progress_report 
                        (operation, start_time, end_time, is_completed, has_error, is_cancelled, filename, 
                         total_tables, total_rows, total_rows_current_table, current_table, 
                         current_table_index, current_row, current_row_in_current_table, 
                         total_bytes, current_bytes, percent_complete, remarks, last_update_time) 
                        VALUES 
                        (@operation, @start_time, @end_time, @is_completed, @has_error, @is_cancelled, @filename, 
                         @total_tables, @total_rows, @total_rows_current_table, @current_table, 
                         @current_table_index, @current_row, @current_row_in_current_table, 
                         @total_bytes, @current_bytes, @percent_complete, @remarks, @last_update_time);
                        SELECT last_insert_rowid();";

                    cmd.Parameters.AddWithValue("@operation", pr.operation);
                    cmd.Parameters.AddWithValue("@start_time", pr.start_time);
                    cmd.Parameters.AddWithValue("@end_time", pr.end_time);
                    cmd.Parameters.AddWithValue("@is_completed", pr.is_completed ? 1 : 0);
                    cmd.Parameters.AddWithValue("@has_error", pr.has_error ? 1 : 0);
                    cmd.Parameters.AddWithValue("@is_cancelled", pr.is_cancelled ? 1 : 0);
                    cmd.Parameters.AddWithValue("@filename", pr.filename ?? "");
                    cmd.Parameters.AddWithValue("@total_tables", pr.total_tables);
                    cmd.Parameters.AddWithValue("@total_rows", pr.total_rows);
                    cmd.Parameters.AddWithValue("@total_rows_current_table", pr.total_rows_current_table);
                    cmd.Parameters.AddWithValue("@current_table", pr.current_table ?? "");
                    cmd.Parameters.AddWithValue("@current_table_index", pr.current_table_index);
                    cmd.Parameters.AddWithValue("@current_row", pr.current_row);
                    cmd.Parameters.AddWithValue("@current_row_in_current_table", pr.current_row_in_current_table);
                    cmd.Parameters.AddWithValue("@total_bytes", pr.total_bytes);
                    cmd.Parameters.AddWithValue("@current_bytes", pr.current_bytes);
                    cmd.Parameters.AddWithValue("@percent_complete", pr.percent_complete);
                    cmd.Parameters.AddWithValue("@remarks", pr.remarks);
                    cmd.Parameters.AddWithValue("@last_update_time", DateTime.Now);

                    cmd.ExecuteNonQuery();

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
                    cmd.CommandText = sql;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProgressReportInfo pr = new ProgressReportInfo();

                            pr.id = SafeConvertToInt(reader["id"]);
                            pr.operation = SafeConvertToInt(reader["operation"]);
                            pr.start_time = SafeConvertToDateTime(reader["start_time"]);
                            pr.end_time = SafeConvertToDateTime(reader["end_time"]);
                            pr.is_completed = SafeConvertToInt(reader["is_completed"]) == 1;
                            pr.has_error = SafeConvertToInt(reader["has_error"]) == 1;
                            pr.is_cancelled = SafeConvertToInt(reader["is_cancelled"]) == 1;
                            pr.filename = reader["filename"]?.ToString() ?? "";
                            pr.total_tables = SafeConvertToLong(reader["total_tables"]);
                            pr.total_rows = SafeConvertToLong(reader["total_rows"]);
                            pr.total_rows_current_table = SafeConvertToLong(reader["total_rows_current_table"]);
                            pr.current_table = reader["current_table"]?.ToString() ?? "";
                            pr.current_table_index = SafeConvertToLong(reader["current_table_index"]);
                            pr.current_row = SafeConvertToLong(reader["current_row"]);
                            pr.current_row_in_current_table = SafeConvertToLong(reader["current_row_in_current_table"]);
                            pr.total_bytes = SafeConvertToLong(reader["total_bytes"]);
                            pr.current_bytes = SafeConvertToLong(reader["current_bytes"]);
                            pr.percent_complete = SafeConvertToLong(reader["percent_complete"]);
                            pr.dbfile_id = SafeConvertToInt(reader["dbfile_id"]);
                            pr.last_update_time = SafeConvertToDateTime(reader["last_update_time"]);
                            pr.remarks = reader["remarks"] + "";
                            pr.client_request_cancel_task = reader["client_request_cancel_task"] + "" == "1";

                            lst.Add(pr);
                        }
                    }

                    foreach (var pr in lst)
                    {
                        if ((DateTime.Now - pr.last_update_time).TotalSeconds > 30 && !pr.is_completed)
                        {
                            // some error has occur, the update is scheduled to be executed every 500ms
                            // if after 10000ms (30 seconds), there isn't any update and the task is not completed
                            // this indicates that the process (ASP.NET) has been terminated by IIS App Pool
                            // the task will never complete

                            cmd.CommandText = $"update progress_report set is_completed=1, is_cancelled=1, has_error=1, remarks='Terminated by system. Unknown error.' where id={pr.id};";
                            cmd.ExecuteNonQuery();

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
            using (var connection = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = @"UPDATE progress_report SET 
                has_error = @has_error,
                total_tables = @total_tables,
                total_rows = @total_rows,
                total_rows_current_table = @total_rows_current_table,
                current_table = @current_table,
                current_table_index = @current_table_index,
                current_row = @current_row,
                current_row_in_current_table = @current_row_in_current_table,
                percent_complete = @percent_complete,
                last_update_time = @last_update_time
            WHERE id = @id";

                    cmd.Parameters.AddWithValue("@id", pr.id);
                    cmd.Parameters.AddWithValue("@has_error", pr.has_error ? 1 : 0);
                    cmd.Parameters.AddWithValue("@total_tables", pr.total_tables);
                    cmd.Parameters.AddWithValue("@total_rows", pr.total_rows);
                    cmd.Parameters.AddWithValue("@total_rows_current_table", pr.total_rows_current_table);
                    cmd.Parameters.AddWithValue("@current_table", pr.current_table ?? "");
                    cmd.Parameters.AddWithValue("@current_table_index", pr.current_table_index);
                    cmd.Parameters.AddWithValue("@current_row", pr.current_row);
                    cmd.Parameters.AddWithValue("@current_row_in_current_table", pr.current_row_in_current_table);
                    cmd.Parameters.AddWithValue("@percent_complete", pr.percent_complete);
                    cmd.Parameters.AddWithValue("@last_update_time", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void CompleteProgress(ProgressReportInfo pr)
        {
            string remarks = "";
            if (pr.has_error)
                remarks = ", remarks = @remarks";

            string filename = "";

            using (var connection = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(connection))
                {
                    // First, get the filename BEFORE updating
                    cmd.CommandText = $"select filename from progress_report where id={pr.id}";
                    filename = cmd.ExecuteScalar() + "";

                    // Now update the record
                    cmd.CommandText = $@"UPDATE progress_report SET 
                end_time = @end_time,
                is_completed = @is_completed,
                has_error = @has_error,
                is_cancelled = @is_cancelled,
                last_update_time = @last_update_time
                {remarks}
            WHERE id = @id";

                    cmd.Parameters.AddWithValue("@id", pr.id);
                    cmd.Parameters.AddWithValue("@end_time", pr.end_time);
                    cmd.Parameters.AddWithValue("@is_completed", pr.is_completed ? 1 : 0);
                    cmd.Parameters.AddWithValue("@has_error", pr.has_error ? 1 : 0);
                    cmd.Parameters.AddWithValue("@is_cancelled", pr.is_cancelled ? 1 : 0);
                    cmd.Parameters.AddWithValue("@last_update_time", DateTime.Now);

                    if (pr.has_error)
                        cmd.Parameters.AddWithValue("@remarks", pr.remarks ?? "");

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

        private static int SafeConvertToInt(object value)
        {
            if (value == null || value == DBNull.Value)
                return 0;
            return Convert.ToInt32(value);
        }

        private static long SafeConvertToLong(object value)
        {
            if (value == null || value == DBNull.Value)
                return 0L;
            return Convert.ToInt64(value);
        }

        private static DateTime SafeConvertToDateTime(object value)
        {
            if (value is DateTime)
            {
                return (DateTime)value;
            }

            if (value == null || value == DBNull.Value)
                return DateTime.MinValue;

            DateTime.TryParseExact(value + "", "yyyy-MM-dd HH:mm:ss", QueryExpress.MySqlDateTimeFormat, Globalization.DateTimeStyles.AssumeUniversal, out DateTime date);

            return date;
        }
    }
}