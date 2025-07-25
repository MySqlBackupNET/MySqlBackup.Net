using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.IO;
using MySqlConnector;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Runtime.Remoting.Contexts;
using System.Web.WebSockets;
using System.Net.Sockets;
using System.Data.SQLite;
using System.Data;

namespace System.pages
{
    public partial class apiProgressReport3 : System.Web.UI.Page
    {
        static ConcurrentDictionary<int, TaskInfo3> dicTaskInfo = new ConcurrentDictionary<int, TaskInfo3>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.IsWebSocketRequest)
            {
                Context.AcceptWebSocketRequest(HandleWebSocketProgressOnly);
            }
            else
            {
                // Serves as API Endpoint
                try
                {
                    string action = (Request["action"] + "").Trim().ToLower();

                    switch (action)
                    {
                        case "start_backup":
                            StartBackup();
                            break;
                        case "start_restore":
                            StartRestore();
                            break;
                        case "getstatus":
                            GetStatus();
                            break;
                        case "stop":
                            Stop();
                            break;
                        case "getalltask":
                            GetAllTasks();
                            break;
                        case "removetask":
                            RemoveTask();
                            break;
                        case "":
                            // HTTP Error 400 Bad Request
                            Response.StatusCode = 400;
                            Response.Write("Empty request");
                            break;
                        default:
                            // HTTP Error 405 Method Not Allowed
                            Response.StatusCode = 405;
                            Response.Write($"Action not supported: {action}");
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
        }

        bool IsUserAuthenticated()
        {
            // User authentication logic here
            // Check if user is logged in and has backup permissions
            // TEMPORARY - for testing and debugging use
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

        private async Task HandleWebSocketProgressOnly(AspNetWebSocketContext httpContext)
        {
            if (!IsUserAuthenticated())
            {
                return;
            }

            WebSocket webSocket = httpContext.WebSocket;
            byte[] buffer = new byte[1024];

            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                        // Parse message - expect format: "subscribe:taskId"
                        if (message.StartsWith("subscribe:"))
                        {
                            if (int.TryParse(message.Substring(10), out int taskId))
                            {
                                // Associate this WebSocket with the task
                                if (dicTaskInfo.TryGetValue(taskId, out var taskInfo))
                                {
                                    taskInfo.TaskWebSocket = webSocket;

                                    // Send current status immediately
                                    await SendWebSocketMessage(webSocket, taskInfo);
                                }
                                else
                                {
                                    await SendWebSocketMessage(webSocket, new { Error = "Task not found" });
                                }
                            }
                        }
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                        break;
                    }
                }
            }
            catch (WebSocketException)
            {
                // Client disconnected
            }
        }

        private async Task SendWebSocketMessage(WebSocket socket, object data)
        {
            try
            {
                if (socket.State == WebSocketState.Open)
                {
                    string json = JsonSerializer.Serialize(data);
                    byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

                    await socket.SendAsync(
                        new ArraySegment<byte>(jsonBytes),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
                }
            }
            catch (WebSocketException)
            {
                // Client disconnected
            }
        }

        void StartBackup()
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

                int newTaskId = GetNewTaskId();

                TaskInfo3 taskInfo = new TaskInfo3();
                taskInfo.TaskId = newTaskId;
                taskInfo.TimeStart = DateTime.Now;
                taskInfo.TaskType = 1; // backup

                dicTaskInfo[newTaskId] = taskInfo;

                // Start backup in background
                _ = Task.Run(() => BackupBegin(newTaskId));

                // Return task ID immediately
                var result = new { TaskId = newTaskId, Status = "Backup started" };
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

        void StartRestore()
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

                TaskInfo3 taskInfo = new TaskInfo3();
                taskInfo.TaskId = newTaskId;
                taskInfo.TimeStart = DateTime.Now;
                taskInfo.TaskType = 2; // restore
                taskInfo.FileName = file.FileName;

                dicTaskInfo[newTaskId] = taskInfo;

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

                // Start restore in background
                _ = Task.Run(() => RestoreBegin(newTaskId, sqlFilePath));

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

        void BackupBegin(int taskId)
        {
            if (dicTaskInfo.TryGetValue(taskId, out var taskInfo))
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

                try
                {
                    if (taskInfo.TaskWebSocket?.State == WebSocketState.Open)
                    {
                        _ = SendWebSocketMessage(taskInfo.TaskWebSocket, taskInfo);
                    }
                }
                catch (WebSocketException)
                {
                    // Client disconnected, ignore
                }

                SaveTaskSQLite(taskInfo);

                dicTaskInfo.TryRemove(taskId, out _);
            }
        }

        void RestoreBegin(int thisTaskId, string filePathSql)
        {
            if (dicTaskInfo.TryGetValue(thisTaskId, out TaskInfo3 taskInfo))
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

                try
                {
                    if (taskInfo.TaskWebSocket?.State == WebSocketState.Open)
                    {
                        _ = SendWebSocketMessage(taskInfo.TaskWebSocket, taskInfo);
                    }
                }
                catch { }

                SaveTaskSQLite(taskInfo);

                dicTaskInfo.TryRemove(thisTaskId, out _);
            }
        }

        private void Mb_ExportProgressChanged(object sender, ExportProgressArgs e, int thisTaskId)
        {
            if (dicTaskInfo.TryGetValue(thisTaskId, out var taskInfo))
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

                // Send progress only if WebSocket is connected
                if (taskInfo.TaskWebSocket?.State == WebSocketState.Open)
                {
                    _ = SendWebSocketMessage(taskInfo.TaskWebSocket, taskInfo);
                }
            }
        }

        private void Mb_ImportProgressChanged(object sender, ImportProgressArgs e, int thisTaskId)
        {
            if (dicTaskInfo.TryGetValue(thisTaskId, out var taskInfo))
            {
                taskInfo.TotalBytes = e.TotalBytes;
                taskInfo.CurrentBytes = e.CurrentBytes;

                if (taskInfo.RequestCancel)
                {
                    ((MySqlBackup)sender).StopAllProcess();
                }

                // Send progress only if WebSocket is connected
                if (taskInfo.TaskWebSocket?.State == WebSocketState.Open)
                {
                    _ = SendWebSocketMessage(taskInfo.TaskWebSocket, taskInfo);
                }
            }
        }

        void Stop()
        {
            try
            {
                if (int.TryParse(Request["taskid"] + "", out int taskid))
                {
                    if (dicTaskInfo.TryGetValue(taskid, out TaskInfo3 taskInfo))
                    {
                        taskInfo.RequestCancel = true;

                        // Return empty success response
                        Response.StatusCode = 200;
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

        TaskInfo3 GetTaskInfoObject(int taskid)
        {
            TaskInfo3 taskInfo = null;

            if (dicTaskInfo.TryGetValue(taskid, out taskInfo))
            {

            }
            else
            {
                taskInfo = GetTaskSQLite(taskid);
            }

            return taskInfo;
        }

        void GetStatus()
        {
            try
            {
                if (int.TryParse(Request["taskid"] + "", out int taskid))
                {
                    TaskInfo3 taskInfo = GetTaskInfoObject(taskid);

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
                    TaskInfo3 taskInfo = GetTaskInfoObject(taskid);

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
                        dicTaskInfo.TryRemove(taskid, out _);

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

        void GetAllTasks()
        {
            try
            {
                var allTasks = dicTaskInfo.Values
                    .Concat(GetAllTasksSQLite())
                    .OrderByDescending(t => t.TimeStart)
                    .ToList();

                string json = JsonSerializer.Serialize(allTasks);
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

        void SaveTaskSQLite(TaskInfo3 taskInfo)
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

        private TaskInfo3 GetTaskSQLite(int taskid)
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

                    return new TaskInfo3
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

        private List<TaskInfo3> GetAllTasksSQLite()
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
                    return new List<TaskInfo3>();

                try
                {
                    var dt = helper.Select("SELECT * FROM progress_report3 ORDER BY TimeStart DESC");
                    var list = new List<TaskInfo3>();

                    foreach (DataRow row in dt.Rows)
                    {
                        list.Add(new TaskInfo3
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
                        });
                    }

                    return list;
                }
                catch
                {
                    return new List<TaskInfo3>();
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

    class TaskInfo3
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