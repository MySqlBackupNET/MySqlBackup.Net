using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Net.WebSockets;
using System.Text.Json.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.pages
{
    public partial class apiProgressReport2_2 : System.Web.UI.Page
    {
        //static ConcurrentDictionary<int, >

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsUserAuthenticated())
        //    {
        //        // HTTP Error 401 Unauthorized
        //        Response.StatusCode = 401;
        //        Response.Write("Unauthorized Access");
        //        return;
        //    }

        //    try
        //    {
        //        string action = (Request["action"] + "").Trim().ToLower();

        //        switch (action)
        //        {
        //            case "start_backup":
        //                Backup();
        //                break;
        //            case "start_restore":
        //                Restore();
        //                break;
        //            case "stop_task":
        //                Stop();
        //                break;
        //            case "get_status":
        //                GetStatus();
        //                break;
        //            case "":
        //                // HTTP Error 400 Bad Request
        //                Response.StatusCode = 400;
        //                Response.Write("Empty Request");
        //                break;
        //            default:
        //                // HTTP Error 406 Not Acceptable
        //                Response.StatusCode = 406;
        //                Response.Write($"Unsupported action: {action}");
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // HTTP Error 500 Internal Server Error
        //        Response.StatusCode = 500;
        //        Response.Write(ex.Message);
        //    }
        //}

        bool IsUserAuthenticated()
        {
            // User authentication logic here
            // Check if user is logged in and has backup permissions
            // TEMPORARY - for testing and debugging use

            //if (Session["user_login"] == null)
            //{
            //    return false;
            //}

            return true;
        }

        int GetNewTaskId()
        {
            int newTaskId = 0;

            string sqliteConstr = BackupFilesManager.sqliteConnectionString;

            using (var conn = new SQLiteConnection(sqliteConstr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                var helper = new SQLiteHelper(cmd);

                newTaskId = helper.ExecuteScalar<int>($"SELECT `Value` FROM Config WHERE `Key`='progress_report3_index';");

                newTaskId++;

                helper.Execute("REPLACE INTO Config (`Key`, `Value`) VALUES ('progress_report3_index', @newTaskId);",
                            new Dictionary<string, object> { ["@newTaskId"] = newTaskId });
            }

            return newTaskId;
        }

        public void Backup()
        {
            try
            {

            }
            catch (Exception ex)
            {
                // HTTP Error 500 Internal Server Error
                Response.StatusCode = 500;
                Response.Write(ex.Message);
            }
        }

        public void BeginBackup()
        {

        }


    }

    class TaskInfo4
    {
        [JsonIgnore]
        public WebSocket TaskWebSocket { get; set; } = null;

        public int TaskId { get; set; }
        public int TaskType { get; set; } // 1 = Backup, 2 = Restore

        [JsonIgnore]
        public DateTime TimeStart { get; set; } = DateTime.MinValue;

        [JsonIgnore]
        public DateTime TimeEnd { get; set; } = DateTime.MinValue;

        [JsonIgnore]
        public TimeSpan TimeUsed
        {
            get
            {
                if (TimeStart != DateTime.MinValue && TimeEnd != DateTime.MinValue)
                {
                    return TimeEnd - TimeStart;
                }
                return TimeSpan.Zero;
            }
        }

        public string FileName { get; set; }
        public string FileSha256 { get; set; }

        [JsonPropertyName("FileDownloadWebPath")]
        public string FileDownloadWebPath
        {
            get
            {
                if (string.IsNullOrEmpty(FileName))
                {
                    return "";
                }
                return $"/apiFiles?action=download&folder=backup&filename={FileName}";
            }
        }

        public bool IsCompleted { get; set; } = false;
        public bool IsCancelled { get; set; } = false;
        public bool RequestCancel { get; set; } = false;
        public bool HasError { get; set; } = false;
        public string ErrorMsg { get; set; } = "";

        public int TotalTables { get; set; } = 0;
        public int CurrentTableIndex { get; set; } = 0;
        public string CurrentTableName { get; set; } = "";
        public long TotalRows { get; set; } = 0L;
        public long CurrentRows { get; set; } = 0L;
        public long TotalRowsCurrentTable { get; set; } = 0L;
        public long CurrentRowsCurrentTable { get; set; } = 0L;

        public long TotalBytes { get; set; }
        public long CurrentBytes { get; set; }

        [JsonPropertyName("PercentCompleted")]
        public int PercentCompleted
        {
            get
            {
                if (TaskType == 1)
                {
                    if (TotalRows > 0L && CurrentRows > 0L)
                    {
                        return (int)(CurrentRows * 100L / TotalRows);
                    }
                    return 0;
                }
                else
                {
                    if (TotalBytes > 0L && CurrentBytes > 0L)
                    {
                        return (int)(CurrentBytes * 100L / TotalBytes);
                    }
                    return 0;
                }
            }
        }

        [JsonPropertyName("TaskTypeName")]
        public string TaskTypeName
        {
            get
            {
                if (TaskType == 1)
                    return "Backup";
                else if (TaskType == 2)
                    return "Restore";
                return "Unknown Task";
            }
        }

        [JsonPropertyName("TimeStartDisplay")]
        public string TimeStartDisplay
        {
            get
            {
                if (TimeStart != DateTime.MinValue)
                {
                    return TimeStart.ToString("yyyy-MM-dd HH:mm:ss");
                }

                return "---";
            }
        }

        [JsonPropertyName("TimeEndDisplay")]
        public string TimeEndDisplay
        {
            get
            {
                if (TimeEnd != DateTime.MinValue)
                {
                    return TimeEnd.ToString("yyyy-MM-dd HH:mm:ss");
                }

                return "---";
            }
        }

        [JsonPropertyName("TimeUsedDisplay")]
        public string TimeUsedDisplay
        {
            get
            {
                if (TimeUsed != TimeSpan.Zero)
                {
                    return TimeDisplayHelper.TimeSpanToString(TimeUsed);
                }

                return "---";
            }
        }
    }
}