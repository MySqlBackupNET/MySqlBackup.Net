using MySqlConnector;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.pages
{
    public partial class ParallelPipelineExport : System.Web.UI.Page
    {
        public static int globalRunningTaskId = 0;
        public static ConcurrentDictionary<int, ParallelExportInfo> dicParallelExportTask = new ConcurrentDictionary<int, ParallelExportInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btStart_Click(object sender, EventArgs e)
        {
            globalRunningTaskId++;

            // Fire and forget, run the task asynchronously in background
            _ = Task.Run(() => RunTest(globalRunningTaskId, cbParallel.Checked));

            panelSetup.Visible = false;
            panelResult.Visible = true;

            ltTaskId.Text = $"<script>var taskid = {globalRunningTaskId};</script>";
        }

        int thisTaskId = 0;
        ParallelExportInfo p = null;
        bool stopUpdateProgress = false;

        void RunTest(int _taskid, bool enableParallelProcessing)
        {
            thisTaskId = _taskid;
            stopUpdateProgress = false;

            if (enableParallelProcessing)
            {
                RunParallel();
            }
            else
            {
                RunSingleThread();
            }
        }

        void RunSingleThread()
        {
            string folder = Server.MapPath("~/App_Data/backup");
            Directory.CreateDirectory(folder);
            string filename = $"export-parallel-singlethread-{DateTime.Now:yyyy-MM-dd_HHmmss}.sql";
            string filepath = Path.Combine(folder, filename);

            p = new ParallelExportInfo()
            {
                id = thisTaskId,
                filename = filename,
                filepath = filepath,
                is_completed = false,
                percent_complete = 0,
                is_parallel = false,
                time_start = DateTime.Now
            };

            dicParallelExportTask[thisTaskId] = p;

            try
            {
                using (var conn = config.GetNewConnection())
                using (var cmd = conn.CreateCommand())
                using (var mb = new MySqlBackup(cmd))
                {
                    conn.Open();
                    mb.ExportInfo.RecordDumpTime = false;
                    mb.ExportInfo.EnableParallelProcessing = false;
                    mb.ExportInfo.IntervalForProgressReport = 500;
                    mb.ExportProgressChanged += Mb_ExportProgressChanged;
                    mb.ExportToFile(filepath);
                }

                stopUpdateProgress = true;
                p.time_end = DateTime.Now;
                p.has_error = false;
                p.is_completed = true;
                p.sha256 = Sha256.Compute(filepath);

                if (!p.request_cancel && !p.has_error)
                {
                    p.percent_complete = 100;
                    p.current_row = p.total_rows;
                }
            }
            catch (Exception ex)
            {
                p.time_end = DateTime.Now;
                p.has_error = true;
                p.is_completed = true;
                p.error_msg = ex.Message;
            }
        }

        void RunParallel()
        {
            string folder = Server.MapPath("~/App_Data/backup");
            Directory.CreateDirectory(folder);
            string filename = $"export-parallel-multithread-{DateTime.Now:yyyy-MM-dd_HHmmss}.sql";
            string filepath = Path.Combine(folder, filename);

            p = new ParallelExportInfo()
            {
                id = thisTaskId,
                filename = filename,
                filepath = filepath,
                is_completed = false,
                percent_complete = 0,
                is_parallel = true,
                time_start = DateTime.Now
            };

            dicParallelExportTask[thisTaskId] = p;

            try
            {
                using (var conn = config.GetNewConnection())
                using (var cmd = conn.CreateCommand())
                using (var mb = new MySqlBackup(cmd))
                {
                    conn.Open();
                    mb.ExportInfo.RecordDumpTime = false;
                    mb.ExportInfo.EnableParallelProcessing = true;
                    mb.ExportInfo.IntervalForProgressReport = 500;
                    mb.ExportProgressChanged += Mb_ExportProgressChanged;
                    mb.ExportToFile(filepath);
                }

                stopUpdateProgress = true;
                p.time_end = DateTime.Now;
                p.has_error = false;
                p.is_completed = true;
                p.sha256 = Sha256.Compute(filepath);

                if (!p.request_cancel && !p.has_error)
                {
                    p.percent_complete = 100;
                    p.current_row = p.total_rows;
                }
            }
            catch (Exception ex)
            {
                p.time_end = DateTime.Now;
                p.has_error = true;
                p.is_completed = true;
                p.error_msg = ex.Message;
            }
        }

        private void Mb_ExportProgressChanged(object sender, ExportProgressArgs e)
        {
            if (stopUpdateProgress)
                return;

            if (p.request_cancel)
            {
                if (p.is_parallel)
                    ((MySqlBackup)sender).StopAllProcess();
                else
                    ((MySqlBackup)sender).StopAllProcess();
                return;
            }

            if (!p.is_completed && !p.has_error)
            {
                p.current_row = e.CurrentRowIndexInAllTables;
                p.total_rows = e.TotalRowsInAllTables;

                if (!p.is_completed && e.CurrentRowIndexInAllTables > 0 && e.TotalRowsInAllTables > 0)
                {
                    p.percent_complete = Convert.ToInt32(e.CurrentRowIndexInAllTables * 100L / e.TotalRowsInAllTables);
                }
            }
        }
    }

    public class ParallelExportInfo
    {
        public int id { get; set; }
        public long total_rows { get; set; }
        public long current_row { get; set; }
        public int percent_complete { get; set; }
        public string filename { get; set; }
        public string filepath { get; set; }
        public bool is_completed { get; set; }
        public bool has_error { get; set; }
        public string error_msg { get; set; }
        public bool request_cancel { get; set; }
        public bool is_parallel { get; set; }
        public int api_call_id { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
        public string sha256 { get; set; }

        public string time_start_display
        {
            get
            {
                return time_start.ToString("yyyy-MM-dd HH:mm:ss tt");
            }
        }

        public string time_end_display
        {
            get
            {
                return time_end.ToString("yyyy-MM-dd HH:mm:ss tt");
            }
        }

        [JsonPropertyName("time_used")]
        public string time_used
        {
            get
            {
                var tu = time_end - time_start;
                return $"{tu.Hours}h {tu.Minutes}m {tu.Seconds}s {tu.Milliseconds}ms";
            }
        }
    }
}