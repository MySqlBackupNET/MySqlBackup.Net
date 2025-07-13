using MySqlConnector;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        volatile static int LatestTaskId = 0;
        static ConcurrentDictionary<int, ProgressReport2Task> dicTask = new ConcurrentDictionary<int, ProgressReport2Task>();

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = (Request["action"] + "").ToLower();

            switch (action)
            {
                case "backup":
                    Backup();
                    break;
                case "restore":
                    Restore();
                    break;
                case "stoptask":
                    StopTask();
                    break;
                case "getallstatus":
                    GetAllTaskStatus();
                    break;
                case "getspecificandactivestatus":
                    GetSpecificAndActiveTaskStatus();
                    break;
                case "deletetaskfile":
                    DeleteTaskAndFile();
                    break;
                case "gettaskstatus":
                    GetTaskStatus();
                    break;
            }
        }

        void Backup()
        {
            LatestTaskId++;
            _ = Task.Run(() => { BeginExport(LatestTaskId); });
            Response.Write(LatestTaskId.ToString());
        }

        int thisTaskId = 0;

        void BeginExport(int newtaskid)
        {
            thisTaskId = newtaskid;

            ProgressReport2Task t = new ProgressReport2Task()
            {
                TaskId = newtaskid,
                TaskType = 1, // backup
                TimeStart = DateTime.Now,
                IsStarted = true
            };

            dicTask[newtaskid] = t;

            try
            {
                string folder = Server.MapPath("~/App_Data/backup");
                Directory.CreateDirectory(folder);

                string fileNameSql = $"progress-report2-backup-{DateTime.Now:yyyy-MM-dd_HHmmss}.sql";
                string filePathSql = Path.Combine(folder, fileNameSql);

                using (var conn = config.GetNewConnection())
                using (var cmd = conn.CreateCommand())
                using (var mb = new MySqlBackup(cmd))
                {
                    conn.Open();
                    mb.ExportInfo.IntervalForProgressReport = 250;
                    mb.ExportProgressChanged += Mb_ExportProgressChanged;
                    mb.ExportToFile(filePathSql);
                }

                if (t.RequestCancel)
                {
                    t.IsCancelled = true;
                    try
                    {
                        if (File.Exists(filePathSql))
                            File.Delete(filePathSql);
                    }
                    catch { }
                }
                else
                {
                    t.FileName = fileNameSql;
                    t.SHA256 = Sha256.Compute(filePathSql);

                    // do not alter the accurancy of progress report,
                    // let the value be updated in Mb_ExportProgressChanged to test the accurancy of progress report by MySqlBackup.NET
                    //t.PercentCompleted = 100;
                    //t.CurrentRowIndex = t.TotalRows;
                }
                
                t.TimeEnd = DateTime.Now;
                t.TimeUsed = DateTime.Now - t.TimeStart;
                t.IsCompleted = true;
            }
            catch (Exception ex)
            {
                t.HasError = true;
                t.ErrorMsg = ex.Message;
                t.TimeEnd = DateTime.Now;
                t.IsCompleted = true;
            }
        }

        private void Mb_ExportProgressChanged(object sender, ExportProgressArgs e)
        {
            if (dicTask.TryGetValue(thisTaskId, out var t))
            {
                t.CurrentTableName = e.CurrentTableName;

                t.TotalTables = e.TotalTables;
                t.CurrentTableIndex = e.CurrentTableIndex;

                t.TotalRows = e.TotalRowsInAllTables;
                t.CurrentRowIndex = e.CurrentRowIndexInAllTables;

                t.TotalRowsCurrentTable = e.TotalRowsInCurrentTable;
                t.CurrentRowCurrentTable = e.CurrentRowIndexInCurrentTable;

                if (e.CurrentRowIndexInAllTables > 0L && e.TotalRowsInAllTables > 0L)
                {
                    if (e.CurrentRowIndexInAllTables >= e.TotalRowsInAllTables)
                    {
                        t.PercentCompleted = 100;
                    }
                    else
                    {
                        t.PercentCompleted = (int)(e.CurrentRowIndexInAllTables * 100L / e.TotalRowsInAllTables);
                    }
                }
                else
                {
                    t.PercentCompleted = 0;
                }

                if (t.RequestCancel)
                {
                    ((MySqlBackup)sender).StopAllProcess();
                }
            }
        }

        void Restore()
        {
            LatestTaskId++;
            ProgressReport2Task t = new ProgressReport2Task()
            {
                TaskId = LatestTaskId,
                TaskType = 2, // restore
                TimeStart = DateTime.Now,
                IsStarted = true
            };
            dicTask[LatestTaskId] = t;

            // Handle file validation in main thread (where Request is available)
            if (Request.Files.Count == 0)
            {
                t.IsCompleted = true;
                t.HasError = true;
                t.ErrorMsg = "No file uploaded";
                t.TimeEnd = DateTime.Now;
                return;
            }

            string fileExtension = Request.Files[0].FileName.ToLower().Trim();
            if (!fileExtension.EndsWith(".zip") && !fileExtension.EndsWith(".sql"))
            {
                t.IsCompleted = true;
                t.HasError = true;
                t.ErrorMsg = "Incorrect file type. zip or sql file only.";
                t.TimeEnd = DateTime.Now;
                return;
            }

            // Save file in main thread
            string folder = Server.MapPath("~/App_Data/backup");
            Directory.CreateDirectory(folder);
            string fileNameSql = $"progress-report2-restore-{DateTime.Now:yyyy-MM-dd_HHmmss}.sql";
            string fileNameZip = $"progress-report2-restore-{DateTime.Now:yyyy-MM-dd_HHmmss}.zip";
            string filePathSql = Path.Combine(folder, fileNameSql);
            string filePathZip = Path.Combine(folder, fileNameZip);

            if (fileExtension.EndsWith(".zip"))
            {
                Request.Files[0].SaveAs(filePathZip);
                ZipHelper.ExtractFile(filePathZip, filePathSql);
                t.FileName = fileNameZip;
            }
            else if (fileExtension.EndsWith(".sql"))
            {
                Request.Files[0].SaveAs(filePathSql);
                t.FileName = fileNameSql;
            }

            // Now start background task with file path (no Request access needed)
            _ = Task.Run(() => { BeginRestore(LatestTaskId, filePathSql); });
            Response.Write(LatestTaskId.ToString());
        }

        void BeginRestore(int newtaskid, string filePathSql)
        {
            thisTaskId = newtaskid;

            if (dicTask.TryGetValue(thisTaskId, out ProgressReport2Task t))
            {
                try
                {
                    t.FileName = Path.GetFileName(filePathSql);
                    t.SHA256 = Sha256.Compute(filePathSql);

                    using (var conn = config.GetNewConnection())
                    using (var cmd = conn.CreateCommand())
                    using (var mb = new MySqlBackup(cmd))
                    {
                        conn.Open();
                        mb.ImportInfo.IntervalForProgressReport = 250;
                        mb.ImportProgressChanged += Mb_ImportProgressChanged;
                        mb.ImportFromFile(filePathSql);
                    }

                    
                    if (t.RequestCancel)
                    {
                        t.IsCancelled = true;
                    }
                    else
                    {
                        // do not alter the accurancy of progress report,
                        // let the value be updated in Mb_ExportProgressChanged to test the accurancy of progress report by MySqlBackup.NET
                        //t.PercentCompleted = 100;
                        //t.CurrentBytes = t.TotalBytes;
                    }
                    
                    t.TimeEnd = DateTime.Now;
                    t.TimeUsed = DateTime.Now - t.TimeStart;
                    t.IsCompleted = true;
                }
                catch (Exception ex)
                {
                    t.HasError = true;
                    t.ErrorMsg = ex.Message;
                    t.TimeEnd = DateTime.Now;
                    t.TimeUsed = DateTime.Now - t.TimeStart;
                    t.IsCompleted = true;
                }
            }
        }

        private void Mb_ImportProgressChanged(object sender, ImportProgressArgs e)
        {
            if (dicTask.TryGetValue(thisTaskId, out var t))
            {
                t.TotalBytes = e.TotalBytes;
                t.CurrentBytes = e.CurrentBytes;

                if (e.CurrentBytes > 0L && e.TotalBytes > 0L)
                {
                    if (e.CurrentBytes >= e.TotalBytes)
                    {
                        t.PercentCompleted = 100;
                    }
                    else
                    {
                        t.PercentCompleted = (int)(e.CurrentBytes * 100L / e.TotalBytes);
                    }
                }
                else
                {
                    t.PercentCompleted = 0;
                }

                if (t.RequestCancel)
                {
                    ((MySqlBackup)sender).StopAllProcess();
                }
            }
        }

        void StopTask()
        {
            if (int.TryParse(Request["taskid"] + "", out int taskid))
            {
                if (dicTask.TryGetValue(taskid, out ProgressReport2Task task))
                {
                    task.RequestCancel = true;
                }
            }
        }

        void GetAllTaskStatus()
        {
            if (int.TryParse(Request["apicallid"] + "", out int apicallid))
            {
                List<ProgressReport2Task> lst = dicTask.Values.ToList();

                var tasks = new
                {
                    apicallid = apicallid,
                    lstTask = lst
                };

                lst = lst.OrderByDescending(task => task.TaskId).ToList();

                string json = JsonSerializer.Serialize(tasks);
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(json);
            }
        }

        void GetSpecificAndActiveTaskStatus()
        {
            if (int.TryParse(Request["apicallid"] + "", out int apicallid))
            {
                string taskidstr = Request["taskidstr"] + "";
                string[] taskidarray = taskidstr.Split(',');

                List<int> requestedTaskIds = new List<int>();
                foreach (string taskIdStr in taskidarray)
                {
                    if (int.TryParse(taskIdStr.Trim(), out int taskId))
                    {
                        requestedTaskIds.Add(taskId);
                    }
                }

                List<ProgressReport2Task> allTasks = dicTask.Values.ToList();

                List<ProgressReport2Task> filteredTasks = allTasks
                    .Where(task => requestedTaskIds.Contains(task.TaskId) || !task.IsCompleted)
                    .ToList();

                filteredTasks = filteredTasks.OrderByDescending(task => task.TaskId).ToList();

                var tasks = new
                {
                    apicallid = apicallid,
                    lstTask = filteredTasks
                };

                string json = JsonSerializer.Serialize(tasks);
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(json);
            }
        }

        void GetTaskStatus()
        {
            if (int.TryParse(Request["apicallid"] + "", out int apicallid))
            {
                if (int.TryParse(Request["taskid"] + "", out int taskid))
                {
                    if (dicTask.TryGetValue(taskid, out ProgressReport2Task t))
                    {
                        var tasks = new
                        {
                            apicallid = apicallid,
                            task = t
                        };

                        string json = JsonSerializer.Serialize(t);
                        Response.Clear();
                        Response.ContentType = "application/json";
                        Response.Write(json);
                    }
                }
            }
        }

        void DeleteTaskAndFile()
        {
            if (int.TryParse(Request["taskid"] + "", out int tid))
            {
                if (dicTask.TryGetValue(tid, out ProgressReport2Task t))
                {
                    string folder = Server.MapPath("~/App_Data/backup");
                    string fileName = Path.GetFileNameWithoutExtension(t.FileName);
                    string fileExtension = Path.GetExtension(t.FileName);

                    string fileNameSql = fileName + ".sql";
                    string fileNameZip = fileName + ".zip";
                    string filePathSql = Path.Combine(folder, fileNameSql);
                    string filePathZip = Path.Combine(folder, fileNameZip);


                    try
                    {
                        if (File.Exists(filePathSql))
                        {
                            File.Delete(filePathSql);
                        }
                    }
                    catch { }

                    try
                    {
                        if (File.Exists(filePathZip))
                        {
                            File.Delete(filePathZip);
                        }
                    }
                    catch { }

                    dicTask.TryRemove(tid, out ProgressReport2Task t2);
                }
            }
        }
    }
}

class ProgressReport2Task
{
    // for api call index
    public int ApiCallIndex { get; set; }

    public int TaskId { get; set; }
    public int TaskType { get; set; }  // 1 = backup, 2 = restore
    public string FileName { get; set; }
    public string SHA256 { get; set; }

    public bool IsStarted { get; set; }
    public bool IsCompleted { get; set; } = false;
    public bool IsCancelled { get; set; } = false;
    public bool RequestCancel { get; set; } = false;
    public bool HasError { get; set; } = false;
    public string ErrorMsg { get; set; } = "";
    public DateTime TimeStart { get; set; } = DateTime.MinValue;
    public DateTime TimeEnd { get; set; } = DateTime.MinValue;
    public TimeSpan TimeUsed { get; set; } = TimeSpan.Zero;

    // for export
    public int TotalTables { get; set; } = 0;
    public int CurrentTableIndex { get; set; } = 0;
    public string CurrentTableName { get; set; } = "";
    public long TotalRowsCurrentTable { get; set; } = 0;
    public long CurrentRowCurrentTable { get; set; } = 0;
    public long TotalRows { get; set; } = 0;
    public long CurrentRowIndex { get; set; } = 0;

    // for import
    public long TotalBytes { get; set; } = 0;
    public long CurrentBytes { get; set; } = 0;

    public int PercentCompleted { get; set; } = 0;

    [JsonPropertyName("TaskTypeName")]
    public string TaskTypeName
    {
        get
        {
            if (TaskType == 1)
                return "Backup";
            else if (TaskType == 2)
                return "Restore";
            return "Unknown";
        }
    }

    [JsonPropertyName("TimeStartDisplay")]
    public string TimeStartDisplay
    {
        get
        {
            return TimeStart.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

    [JsonPropertyName("TimeEndDisplay")]
    public string TimeEndDisplay
    {
        get
        {
            return TimeEnd.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

    [JsonPropertyName("TimeUsedDisplay")]
    public string TimeUsedDisplay
    {
        get
        {
            return TimeDisplayHelper.TimeSpanToString(TimeUsed);
        }
    }

    [JsonPropertyName("FileDownloadUrl")]
    public string FileDownloadUrl
    {
        get
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                return $"/apiFiles?folder=backup&filename={FileName}";
            }
            return "";
        }
    }
}