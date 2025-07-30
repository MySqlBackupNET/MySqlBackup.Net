using MySqlConnector;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.pages
{
    public partial class apiProgressReport2 : System.Web.UI.Page
    {
        static ConcurrentDictionary<int, TaskInfo4> dicTask = new ConcurrentDictionary<int, TaskInfo4>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsUserAuthenticated())
            {
                // HTTP Error 401 Unauthorized
                Response.StatusCode = 401;
                Response.Write("Unauthorized Access");
                return;
            }

            try
            {
                string action = (Request["action"] + "").Trim().ToLower();

                switch (action)
                {
                    case "start_backup":
                        Backup();
                        break;
                    case "start_restore":
                        Restore();
                        break;
                    case "stop_task":
                        Stop();
                        break;
                    case "get_status":
                        GetStatus();
                        break;
                    case "remove_task":
                        RemoveTask();
                        break;
                    case "":
                        // HTTP Error 400 Bad Request
                        Response.StatusCode = 400;
                        Response.Write("Empty Request");
                        break;
                    default:
                        // HTTP Error 406 Not Acceptable
                        Response.StatusCode = 406;
                        Response.Write($"Unsupported action: {action}");
                        break;
                }
            }
            catch (Exception ex)
            {
                // HTTP Error 500 Internal Server Error
                Response.StatusCode = 500;
                Response.Write(ex.Message);
            }
        }

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
                int taskId = GetNewTaskId();

                TaskInfo4 taskInfo = new TaskInfo4();
                taskInfo.TaskId = taskId;
                taskInfo.TaskType = 1; // backup
                taskInfo.TimeStart = DateTime.Now;

                dicTask[taskId] = taskInfo;

                _ = Task.Run(() => { BeginBackup(taskId); });

                var result = new
                {
                    TaskId = taskId
                };

                var json = JsonSerializer.Serialize(result);
                Response.ContentType = "application/json";
                Response.Write(json);
            }
            catch (Exception ex)
            {
                // HTTP Error 500 Internal Server Error
                Response.StatusCode = 500;
                Response.Write(ex.Message);
            }
        }

        public void BeginBackup(int taskId)
        {
            if (dicTask.TryGetValue(taskId, out var taskInfo))
            {
                try
                {
                    string fileName = $"{DateTime.Now:yyyy-MM-dd_HHmmssd}.sql";
                    string folder = Server.MapPath("~/App_Data/backup");
                    string sqlFile = Server.MapPath($"/App_Data/backup/{fileName}");

                    Directory.CreateDirectory(folder);

                    using (var conn = config.GetNewConnection())
                    using (var cmd = conn.CreateCommand())
                    using (var mb = new MySqlBackup(cmd))
                    {
                        conn.Open();
                        mb.ExportInfo.IntervalForProgressReport = 200;
                        mb.ExportProgressChanged += (sender, e) => Mb_ExportProgressChanged(sender, e, taskId);
                        mb.ExportToFile(sqlFile);
                    }

                    if (taskInfo.RequestCancel)
                    {
                        taskInfo.IsCancelled = true;
                        try
                        {
                            File.Delete(sqlFile);
                        }
                        catch { }
                    }
                    else
                    {
                        taskInfo.FileSha256 = Sha256.Compute(sqlFile);
                        taskInfo.FileName = fileName;
                    }
                }
                catch (Exception ex)
                {
                    taskInfo.HasError = true;
                    taskInfo.ErrorMsg = ex.Message;
                }

                taskInfo.TimeEnd = DateTime.Now;
                taskInfo.IsCompleted = true;

                SaveTaskSQLite(taskInfo);

                dicTask.TryRemove(taskId, out _);
            }
        }

        void Mb_ExportProgressChanged(object sender, ExportProgressArgs e, int taskId)
        {
            if (dicTask.TryGetValue(taskId, out var taskInfo))
            {
                taskInfo.TotalTables = e.TotalTables;
                taskInfo.CurrentTableIndex = e.CurrentTableIndex;
                taskInfo.CurrentTableName = e.CurrentTableName;
                taskInfo.TotalRows = e.TotalRowsInAllTables;
                taskInfo.CurrentRows = e.CurrentRowIndexInAllTables;
                taskInfo.TotalRowsCurrentTable = e.TotalRowsInCurrentTable;
                taskInfo.CurrentRowsCurrentTable = e.CurrentRowIndexInCurrentTable;

                if (taskInfo.RequestCancel)
                {
                    ((MySqlBackup)sender).StopAllProcess();
                }
            }
        }

        void Restore()
        {
            try
            {
                if (!IsUserAuthenticated())
                {
                    // HTTP Error 401 Unauthorized
                    Response.StatusCode = 401;
                    Response.Write("Unauthorized");
                    return;
                }

                if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
                {
                    Response.StatusCode = 400;
                    Response.Write("No file uploaded");
                    return;
                }

                var file = Request.Files[0];
                int newTaskId = GetNewTaskId();

                TaskInfo4 taskInfo = new TaskInfo4();
                taskInfo.TaskId = newTaskId;
                taskInfo.TimeStart = DateTime.Now;
                taskInfo.TaskType = 2; // restore
                taskInfo.FileName = file.FileName;

                dicTask[newTaskId] = taskInfo;

                // Save and process uploaded file
                string folder = Server.MapPath("~/App_Data/backup");
                Directory.CreateDirectory(folder);
                string fileName = $"restore_{newTaskId}_{DateTime.Now:yyyyMMdd_HHmmss}{Path.GetExtension(file.FileName)}";
                string filePath = Path.Combine(folder, fileName);

                file.SaveAs(filePath);

                // Handle ZIP extraction if needed
                string sqlFilePath = filePath;
                if (file.FileName.ToLower().EndsWith(".zip"))
                {
                    string sqlFileName = Path.ChangeExtension(fileName, ".sql");
                    sqlFilePath = Path.Combine(folder, sqlFileName);
                    ZipHelper.ExtractFile(filePath, sqlFilePath);
                }

                // Start process in another separated thread
                _ = Task.Run(() => BeginRestore(newTaskId, sqlFilePath));

                // Return task ID immediately
                var result = new { TaskId = newTaskId, Status = "File uploaded, restore started" };
                Response.ContentType = "application/json";
                Response.Write(JsonSerializer.Serialize(result));
            }
            catch (Exception ex)
            {
                // HTTP Error 500 Internal Server Error
                Response.StatusCode = 500;
                Response.Write(ex.Message);
            }
        }

        void BeginRestore(int thisTaskId, string filePathSql)
        {
            if (dicTask.TryGetValue(thisTaskId, out TaskInfo4 taskInfo))
            {
                try
                {
                    taskInfo.FileSha256 = Sha256.Compute(filePathSql);
                    taskInfo.FileName = Path.GetFileName(filePathSql);

                    using (var conn = config.GetNewConnection())
                    using (var cmd = conn.CreateCommand())
                    using (var mb = new MySqlBackup(cmd))
                    {
                        conn.Open();
                        mb.ImportInfo.EnableParallelProcessing = false;
                        mb.ImportInfo.IntervalForProgressReport = 200;
                        mb.ImportProgressChanged += (sender, e) => Mb_ImportProgressChanged(sender, e, thisTaskId);
                        mb.ImportFromFile(filePathSql);
                    }

                    if (taskInfo.RequestCancel)
                    {
                        taskInfo.IsCancelled = true;
                    }
                }
                catch (Exception ex)
                {
                    taskInfo.HasError = true;
                    taskInfo.ErrorMsg = ex.Message;
                }

                taskInfo.TimeEnd = DateTime.Now;
                taskInfo.IsCompleted = true;

                SaveTaskSQLite(taskInfo);

                dicTask.TryRemove(thisTaskId, out _);
            }
        }

        void Mb_ImportProgressChanged(object sender, ImportProgressArgs e, int thisTaskId)
        {
            if (dicTask.TryGetValue(thisTaskId, out var taskInfo))
            {
                taskInfo.TotalBytes = e.TotalBytes;
                taskInfo.CurrentBytes = e.CurrentBytes;

                if (taskInfo.RequestCancel)
                {
                    ((MySqlBackup)sender).StopAllProcess();
                }
            }
        }

        void Stop()
        {
            try
            {
                if (int.TryParse(Request["taskid"] + "", out int taskid))
                {
                    if (dicTask.TryGetValue(taskid, out TaskInfo4 taskInfo))
                    {
                        taskInfo.RequestCancel = true;

                        // do not modify response code
                        // default response status = 200 = ok/success
                    }
                    else
                    {
                        // HTTP Error 404 Not Found
                        Response.StatusCode = 404;
                        Response.Write($"Task has already completed and stopped or it is an invalid Task ID.");
                    }
                }
                else
                {
                    // HTTP Error 404 Not Found
                    Response.StatusCode = 400;
                    Response.Write($"Empty or Invalid Task ID.");
                }
            }
            catch (Exception ex)
            {
                // HTTP Error 500 Internal Server Error
                Response.StatusCode = 500;
                Response.Write(ex.Message);
            }
        }

        void GetStatus()
        {
            try
            {
                if (int.TryParse(Request["taskid"] + "", out int taskid))
                {
                    int.TryParse(Request["api_call_index"] + "", out int api_call_index);

                    TaskInfo4 taskInfo = GetTaskInfoObject(taskid);

                    taskInfo.ApiCallIndex = api_call_index;

                    if (taskInfo != null)
                    {
                        string json = JsonSerializer.Serialize(taskInfo);
                        Response.ContentType = "application/json";
                        Response.Write(json);
                    }
                    else
                    {
                        // HTTP Error 404 Not Found
                        Response.StatusCode = 404;
                        Response.Write($"Task ID not found: {taskid}");
                    }
                }
                else
                {
                    // HTTP Error 400 Bad Request
                    Response.StatusCode = 400;
                    Response.Write($"Invalid Task ID");
                }
            }
            catch (Exception ex)
            {
                // HTTP Error 500 Internal Server Error
                Response.StatusCode = 500;
                Response.Write(ex.Message);
            }
        }

        void RemoveTask()
        {
            try
            {
                if (int.TryParse(Request["taskid"] + "", out int taskid))
                {
                    TaskInfo4 taskInfo = GetTaskInfoObject(taskid);

                    if (taskInfo != null)
                    {
                        if (!string.IsNullOrEmpty(taskInfo.FileName))
                        {
                            try
                            {
                                string file = Server.MapPath($"~/App_Data/backup/{taskInfo.FileName}");
                                if (File.Exists(file))
                                {
                                    File.Delete(file);
                                }
                            }
                            catch { }
                        }
                        dicTask.TryRemove(taskid, out _);

                        RemoveTaskSQLite(taskid);
                    }
                    else
                    {
                        Response.StatusCode = 404;
                        Response.Write($"Task ID not found: {taskid}, or it has already removed.");
                    }
                }
                else
                {
                    // HTTP Error 400 Bad Request
                    Response.StatusCode = 400;
                    Response.Write($"Invalid Task ID");
                }
            }
            catch (Exception ex)
            {
                // HTTP Error 500 Internal Server Error
                Response.StatusCode = 500;
                Response.Write(ex.Message);
            }
        }

        TaskInfo4 GetTaskInfoObject(int taskid)
        {
            TaskInfo4 taskInfo = null;

            if (dicTask.TryGetValue(taskid, out taskInfo))
            {

            }
            else
            {
                taskInfo = GetTaskSQLite(taskid);
            }

            return taskInfo;
        }

        void SaveTaskSQLite(TaskInfo4 taskInfo)
        {
            string sqliteConstr = BackupFilesManager.sqliteConnectionString;

            using (var conn = new SQLiteConnection(sqliteConstr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                var helper = new SQLiteHelper(cmd);

                // Check if table exists and create if not
                var tables = helper.GetTableList();
                bool tableExists = false;

                foreach (DataRow row in tables.Rows)
                {
                    if (row["Tables"].ToString().ToLower() == "progress_report3")
                    {
                        tableExists = true;
                        break;
                    }
                }

                if (!tableExists)
                {
                    // Create table based on TaskInfo3 properties
                    var table = new SQLiteTable("progress_report3");
                    table.Columns.Add(new SQLiteColumn("TaskId", ColType.Integer, true, true, true, "")); // Primary key
                    table.Columns.Add(new SQLiteColumn("TaskType", ColType.Integer));
                    table.Columns.Add(new SQLiteColumn("TimeStart", ColType.DateTime));
                    table.Columns.Add(new SQLiteColumn("TimeEnd", ColType.DateTime));
                    table.Columns.Add(new SQLiteColumn("FileName", ColType.Text));
                    table.Columns.Add(new SQLiteColumn("FileSha256", ColType.Text));
                    table.Columns.Add(new SQLiteColumn("IsCompleted", ColType.Integer)); // SQLite boolean as integer
                    table.Columns.Add(new SQLiteColumn("IsCancelled", ColType.Integer));
                    table.Columns.Add(new SQLiteColumn("RequestCancel", ColType.Integer));
                    table.Columns.Add(new SQLiteColumn("HasError", ColType.Integer));
                    table.Columns.Add(new SQLiteColumn("ErrorMsg", ColType.Text));
                    table.Columns.Add(new SQLiteColumn("TotalTables", ColType.Integer));
                    table.Columns.Add(new SQLiteColumn("CurrentTableIndex", ColType.Integer));
                    table.Columns.Add(new SQLiteColumn("CurrentTableName", ColType.Text));
                    table.Columns.Add(new SQLiteColumn("TotalRows", ColType.Integer));
                    table.Columns.Add(new SQLiteColumn("CurrentRows", ColType.Integer));
                    table.Columns.Add(new SQLiteColumn("TotalRowsCurrentTable", ColType.Integer));
                    table.Columns.Add(new SQLiteColumn("CurrentRowsCurrentTable", ColType.Integer));
                    table.Columns.Add(new SQLiteColumn("TotalBytes", ColType.Integer));
                    table.Columns.Add(new SQLiteColumn("CurrentBytes", ColType.Integer));

                    helper.CreateTable(table);
                }

                var div = new Dictionary<string, object>
                {
                    ["TaskId"] = taskInfo.TaskId,
                    ["TaskType"] = taskInfo.TaskType,
                    ["TimeStart"] = taskInfo.TimeStart,
                    ["TimeEnd"] = taskInfo.TimeEnd,
                    ["FileName"] = taskInfo.FileName ?? "",
                    ["FileSha256"] = taskInfo.FileSha256 ?? "",
                    ["IsCompleted"] = taskInfo.IsCompleted ? 1 : 0,
                    ["IsCancelled"] = taskInfo.IsCancelled ? 1 : 0,
                    ["RequestCancel"] = taskInfo.RequestCancel ? 1 : 0,
                    ["HasError"] = taskInfo.HasError ? 1 : 0,
                    ["ErrorMsg"] = taskInfo.ErrorMsg ?? "",
                    ["TotalTables"] = taskInfo.TotalTables,
                    ["CurrentTableIndex"] = taskInfo.CurrentTableIndex,
                    ["CurrentTableName"] = taskInfo.CurrentTableName ?? "",
                    ["TotalRows"] = taskInfo.TotalRows,
                    ["CurrentRows"] = taskInfo.CurrentRows,
                    ["TotalRowsCurrentTable"] = taskInfo.TotalRowsCurrentTable,
                    ["CurrentRowsCurrentTable"] = taskInfo.CurrentRowsCurrentTable,
                    ["TotalBytes"] = taskInfo.TotalBytes,
                    ["CurrentBytes"] = taskInfo.CurrentBytes
                };

                // Insert or update (upsert)
                var existingTask = helper.ExecuteScalar<int>("SELECT COUNT(*) FROM progress_report3 WHERE TaskId = @TaskId",
                    new Dictionary<string, object> { ["@TaskId"] = taskInfo.TaskId });

                if (existingTask > 0)
                {
                    // Update existing record
                    helper.Update("progress_report3", div, "TaskId", taskInfo.TaskId);
                }
                else
                {
                    // Insert new record
                    helper.Insert("progress_report3", div);
                }
            }
        }

        private TaskInfo4 GetTaskSQLite(int taskid)
        {
            string sqliteConstr = BackupFilesManager.sqliteConnectionString;

            using (var conn = new SQLiteConnection(sqliteConstr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                var helper = new SQLiteHelper(cmd);

                // Check if table exists
                var tables = helper.GetTableList();
                bool tableExists = false;
                foreach (DataRow row in tables.Rows)
                {
                    if (row["Tables"].ToString().ToLower() == "progress_report3")
                    {
                        tableExists = true;
                        break;
                    }
                }

                if (!tableExists)
                    return null;

                try
                {
                    var dt = helper.Select("SELECT * FROM progress_report3 WHERE TaskId = @TaskId",
                        new Dictionary<string, object> { ["@TaskId"] = taskid });

                    if (dt.Rows.Count == 0)
                        return null;

                    var row = dt.Rows[0];

                    return new TaskInfo4
                    {
                        TaskId = Convert.ToInt32(row["TaskId"]),
                        TaskType = Convert.ToInt32(row["TaskType"]),
                        TimeStart = Convert.ToDateTime(row["TimeStart"]),
                        TimeEnd = Convert.ToDateTime(row["TimeEnd"]),
                        FileName = row["FileName"].ToString(),
                        FileSha256 = row["FileSha256"].ToString(),
                        IsCompleted = Convert.ToInt32(row["IsCompleted"]) == 1,
                        IsCancelled = Convert.ToInt32(row["IsCancelled"]) == 1,
                        RequestCancel = Convert.ToInt32(row["RequestCancel"]) == 1,
                        HasError = Convert.ToInt32(row["HasError"]) == 1,
                        ErrorMsg = row["ErrorMsg"].ToString(),
                        TotalTables = Convert.ToInt32(row["TotalTables"]),
                        CurrentTableIndex = Convert.ToInt32(row["CurrentTableIndex"]),
                        CurrentTableName = row["CurrentTableName"].ToString(),
                        TotalRows = Convert.ToInt64(row["TotalRows"]),
                        CurrentRows = Convert.ToInt64(row["CurrentRows"]),
                        TotalRowsCurrentTable = Convert.ToInt64(row["TotalRowsCurrentTable"]),
                        CurrentRowsCurrentTable = Convert.ToInt64(row["CurrentRowsCurrentTable"]),
                        TotalBytes = Convert.ToInt64(row["TotalBytes"]),
                        CurrentBytes = Convert.ToInt64(row["CurrentBytes"])
                    };
                }
                catch
                {
                    return null;
                }
            }
        }

        void RemoveTaskSQLite(int taskid)
        {
            string sqliteConstr = BackupFilesManager.sqliteConnectionString;

            using (var conn = new SQLiteConnection(sqliteConstr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                var helper = new SQLiteHelper(cmd);

                // Check if table exists
                var tables = helper.GetTableList();
                bool tableExists = false;
                foreach (DataRow row in tables.Rows)
                {
                    if (row["Tables"].ToString().ToLower() == "progress_report3")
                    {
                        tableExists = true;
                        break;
                    }
                }

                if (!tableExists)
                    return;

                helper.Execute($"DELETE FROM progress_report3 WHERE TaskId = @TaskId",
                        new Dictionary<string, object> { ["@TaskId"] = taskid });
            }
        }
    }

    class TaskInfo4
    {
        public int ApiCallIndex { get; set; }

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

        [JsonPropertyName("Status")]
        public string Status
        {
            get
            {
                if (HasError)
                    return "Error";
                if (IsCancelled)
                    return "Cancelled";
                if (IsCompleted)
                    return "Completed";
                return "Running";
            }
        }

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