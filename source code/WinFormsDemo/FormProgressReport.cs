using MySqlConnector;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsDemo
{
    public partial class FormProgressReport : baseForm
    {
        Timer timerSaveConstr = new Timer();
        string fileConstr = Path.Combine(Application.StartupPath, "constr_progress_report.txt");

        volatile TaskResultProgressReport taskInfo = new TaskResultProgressReport();

        Timer timerUpdateProgress = new Timer();

        public FormProgressReport()
        {
            InitializeComponent();

            toolStripStatusLabel1.Text = "Idle. Stopped";

            timerSaveConstr.Interval = 3000;
            timerSaveConstr.Tick += TimerSaveConstr_Tick;

            toolStripProgressBar1.Style = ProgressBarStyle.Blocks;

            EnableButtons(true, true, false);

            try
            {
                if (File.Exists(fileConstr))
                {
                    txtConstr.Text = File.ReadAllText(fileConstr);
                    WriteStatus("Constr auto loaded");
                }
                else
                {
                    WriteStatus("No constr file saved");
                }
            }
            catch (Exception ex)
            {
                WriteStatus($"Failed to auto load constr file: {ex.Message}");
            }

            txtConstr.TextChanged += txtConstr_TextChanged;

            timerUpdateProgress.Interval = 200;
            timerUpdateProgress.Tick += TimerUpdateProgress_Tick;

            ResetUI();
        }

        #region Basic Other UI controls

        private void btOpenBackupFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (Process p = new Process())
                {
                    p.StartInfo.FileName = txtBackupFile.Text;
                    p.Start();
                }
            }
            catch (Exception ex)
            {
                WriteStatus($"Error opening backup file: {ex.Message}");
            }
        }

        private void btTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new MySqlConnection(txtConstr.Text))
                {
                    conn.Open();
                }

                WriteStatus("Connection success");
            }
            catch (Exception ex)
            {
                WriteStatus($"Connection test failed: {ex.Message}");
            }
        }

        private void TimerSaveConstr_Tick(object sender, EventArgs e)
        {
            timerSaveConstr.Stop();
            try
            {
                File.WriteAllText(fileConstr, txtConstr.Text);
                WriteStatus($"Constr auto saved at: {fileConstr}");
            }
            catch (Exception ex)
            {
                WriteStatus($"Auto Save Constr Failed: {ex.Message}");
            }
        }

        void WriteStatus(string msg)
        {
            toolStripStatusLabel1.Text = msg;
            string log = $"{DateTime.Now:yyyy-MM-dd hh:mm:ss tt}    {msg}";

            txtLog.SelectionStart = 0;
            txtLog.SelectionLength = 0;
            txtLog.SelectedText = log + Environment.NewLine;
        }

        void EnableButtons(bool backup, bool restore, bool stop)
        {
            btBackup.Enabled = backup;
            btRestore.Enabled = restore;
            btStop.Enabled = stop;
        }

        void ResetUI()
        {
            lbTimeStart.Text = "---";
            lbTimeEnd.Text = "---";
            lbTimeUsed.Text = "---";
            lbIsCompleted.Text = "---";
            lbIsCancelled.Text = "---";
            lbHasError.Text = "---";
            lbErrorMsg.Text = "---";
            lbTotalTables.Text = "--- / ---";
            lbTotalBytes.Text = "--- / ---";
            lbTotalRows.Text = "--- / ---";
            lbRowsCurrentTable.Text = "--- / ---";

            progressBar_TotalRows.Value = 0;
            progressBar_TotalTables.Value = 0;
            progressBar_RowsCurrentTable.Value = 0;
            progressBar_TotalBytes.Value = 0;
        }

        private void txtConstr_TextChanged(object sender, EventArgs e)
        {
            timerSaveConstr.Stop();
            timerSaveConstr.Start();
        }

        private void btClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Text = "";
        }

        #endregion

        private void btBackup_Click(object sender, EventArgs e)
        {
            EnableButtons(false, false, false);

            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "*.sql|*.sql";
            sf.FileName = $"backup-{DateTime.Now:yyyy-MM-dd_HHmmss}.sql";

            if (sf.ShowDialog() == DialogResult.OK)
            {
                string sqlFilePath = sf.FileName;

                if (MessageBox.Show($"Are you ready to continue?\r\n\r\nThe file will be saved at:\r\n\r\n{sqlFilePath}", "Backup", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ResetUI();
                    txtBackupFile.Text = sqlFilePath;
                    WriteStatus("Backup task is running...");
                    toolStripProgressBar1.Style = ProgressBarStyle.Marquee;

                    int taskType = 1; // backup

                    _ = Task.Run(() => { BeginTask(taskType, txtConstr.Text, sqlFilePath); });

                    timerUpdateProgress.Start();

                    EnableButtons(false, false, true);
                }
                else
                {
                    EnableButtons(true, true, false);
                }
            }
            else
            {
                EnableButtons(true, true, false);
            }
        }

        private void btRestore_Click(object sender, EventArgs e)
        {
            EnableButtons(false, false, false);

            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = false;
            of.Filter = "*.sql|*.sql|All|*.*";

            if (of.ShowDialog() == DialogResult.OK)
            {
                string sqlFilePath = of.FileName;

                if (MessageBox.Show($"Are you ready to continue?\r\n\r\nRestoring database from file:\r\n\r\n{sqlFilePath}", "Restore", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ResetUI();
                    txtBackupFile.Text = sqlFilePath;
                    WriteStatus("Restore task is running...");
                    toolStripProgressBar1.Style = ProgressBarStyle.Marquee;

                    int taskType = 2; // restore

                    _ = Task.Run(() => { BeginTask(taskType, txtConstr.Text, sqlFilePath); });

                    timerUpdateProgress.Start();

                    EnableButtons(false, false, true);
                }
                else
                {
                    EnableButtons(true, true, false);
                }
            }
            else
            {
                EnableButtons(true, true, false);
            }
        }

        void BeginTask(int taskType, string constr, string sqlFile)
        {
            taskInfo.Reset();

            taskInfo.TaskType = taskType;
            taskInfo.IsRunning = true;

            taskInfo.TimeStart = DateTime.Now;

            try
            {
                using (var conn = new MySqlConnection(constr))
                using (var cmd = conn.CreateCommand())
                using (var mb = new MySqlBackup(cmd))
                {
                    conn.Open();

                    if (taskType == 1)
                    {
                        mb.ExportInfo.IntervalForProgressReport = 200;
                        mb.ExportProgressChanged += Mb_ExportProgressChanged;
                        mb.ExportToFile(sqlFile);
                    }
                    else if (taskType == 2)
                    {
                        // for better real-time accuracy
                        mb.ImportInfo.EnableParallelProcessing = false;
                        mb.ImportInfo.IntervalForProgressReport = 200;
                        mb.ImportProgressChanged += Mb_ImportProgressChanged;
                        mb.ImportFromFile(sqlFile);
                    }
                }
            }
            catch (Exception ex)
            {
                taskInfo.HasError = true;
                taskInfo.ErrorMsg = ex.Message;
            }

            if (taskInfo.RequestCancel)
            {
                taskInfo.IsCancelled = true;
            }

            taskInfo.TimeEnd = DateTime.Now;
            taskInfo.IsCompleted = true;

            taskInfo.IsRunning = false;
        }

        private void Mb_ExportProgressChanged(object sender, ExportProgressArgs e)
        {
            taskInfo.TotalRows = e.TotalRowsInAllTables;
            taskInfo.CurrentRows = e.CurrentRowIndexInAllTables;

            taskInfo.TotalTables = e.TotalTables;
            taskInfo.CurrentTableIndex = e.CurrentTableIndex;

            taskInfo.TotalRowsCurrentTable = e.TotalRowsInCurrentTable;
            taskInfo.CurrentRowsCurrentTable = e.CurrentRowIndexInCurrentTable;

            taskInfo.CurrentTableName = e.CurrentTableName;

            if (taskInfo.RequestCancel)
            {
                ((MySqlBackup)sender).StopAllProcess();
            }
        }

        private void Mb_ImportProgressChanged(object sender, ImportProgressArgs e)
        {
            taskInfo.TotalBytes = e.TotalBytes;
            taskInfo.CurrentBytes = e.CurrentBytes;

            if (taskInfo.RequestCancel)
            {
                ((MySqlBackup)sender).StopAllProcess();
            }
        }

        private void TimerUpdateProgress_Tick(object sender, EventArgs e)
        {
            lbTaskType.Text = taskInfo.TaskName;
            lbTimeStart.Text = taskInfo.TimeStartDisplay;
            lbTimeEnd.Text = taskInfo.TimeEndDisplay;
            lbTimeUsed.Text = taskInfo.TimeUsedDisplay;

            lbIsCompleted.Text = taskInfo.IsCompleted.ToString();
            lbIsCancelled.Text = taskInfo.IsCancelled.ToString();
            lbHasError.Text = taskInfo.HasError.ToString();
            lbErrorMsg.Text = taskInfo.ErrorMsg;

            if (taskInfo.TaskType == 1)
            {
                lbTotalRows.Text = $"{taskInfo.CurrentRows} / {taskInfo.TotalRows} ({taskInfo.Percent_TotalRows}%)";
                lbTotalTables.Text = $"{taskInfo.CurrentTableIndex} / {taskInfo.TotalTables} ({taskInfo.Percent_TotalTables}%)";
                lbRowsCurrentTable.Text = $"{taskInfo.CurrentRowsCurrentTable} / {taskInfo.TotalRowsCurrentTable} ({taskInfo.Percent_TotalRowsTable}%)";

                progressBar_TotalRows.Value = taskInfo.Percent_TotalRows;
                progressBar_TotalTables.Value = taskInfo.Percent_TotalTables;
                progressBar_RowsCurrentTable.Value = taskInfo.Percent_TotalRowsTable;
            }
            else
            {
                lbTotalBytes.Text = $"{taskInfo.CurrentBytes} / {taskInfo.TotalBytes} ({taskInfo.Percent_TotalBytes}%)";
                progressBar_TotalBytes.Value = taskInfo.Percent_TotalBytes;
            }

            if (taskInfo.IsCompleted)
            {
                timerUpdateProgress.Stop();
                EnableButtons(true, true, false);
                toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
                WriteStatus($"{taskInfo.TaskName} task is stopped/completed");
            }

            if (taskInfo.HasError)
            {
                WriteStatus($"{taskInfo.TaskName} task has error");
                WriteStatus($"{taskInfo.ErrorMsg}");
            }

            if (taskInfo.IsCancelled)
            {
                WriteStatus($"{taskInfo.TaskName} is cancelled by user");
            }
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            if (taskInfo.IsRunning)
            {
                taskInfo.RequestCancel = true;
                WriteStatus("Calling task to cancel...");
            }
            else
            {
                WriteStatus("No task is running");
            }
        }
    }

    class TaskResultProgressReport
    {
        public int TaskType { get; set; }
        public DateTime TimeStart { get; set; } = DateTime.MinValue;
        public DateTime TimeEnd { get; set; } = DateTime.MinValue;
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

        public bool IsRunning { get; set; } = false;
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

        public string TaskName
        {
            get
            {
                if (TaskType == 1)
                    return "Backup";
                else if (TaskType == 2)
                    return "Restore";
                return "---";
            }
        }

        public int Percent_TotalRows
        {
            get
            {
                if (CurrentRows == 0L || TotalRows == 0L)
                    return 0;

                if (CurrentRows >= TotalRows)
                    return 100;

                if (CurrentRows > 0L && TotalRows > 0L)
                    return (int)((double)CurrentRows * 100.0 / (double)TotalRows);

                return 0;
            }
        }

        public int Percent_TotalRowsTable
        {
            get
            {
                if (CurrentRowsCurrentTable == 0L || TotalRowsCurrentTable == 0L)
                    return 0;

                if (CurrentRowsCurrentTable >= TotalRowsCurrentTable)
                    return 100;

                if (CurrentRowsCurrentTable > 0L && TotalRowsCurrentTable > 0L)
                    return (int)((double)CurrentRowsCurrentTable * 100.0 / (double)TotalRowsCurrentTable);

                return 0;
            }
        }

        public int Percent_TotalTables
        {
            get
            {
                if (CurrentTableIndex == 0 || TotalTables == 0)
                    return 0;

                if (CurrentTableIndex >= TotalTables)
                    return 100;

                if (CurrentTableIndex > 0 && TotalTables > 0)
                    return (int)((double)CurrentTableIndex * 100.0 / (double)TotalTables);

                return 0;
            }
        }

        public int Percent_TotalBytes
        {
            get
            {
                if (CurrentBytes == 0L || TotalBytes == 0L)
                    return 0;

                if (CurrentBytes >= TotalBytes)
                    return 100;

                if (CurrentBytes >= 0L && TotalBytes >= 0L)
                    return (int)((double)CurrentBytes * 100.0 / (double)TotalBytes);

                return 0;
            }
        }

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

        public string TimeUsedDisplay
        {
            get
            {
                if (TimeUsed != TimeSpan.Zero)
                {
                    return $"{TimeUsed.Hours}h {TimeUsed.Minutes}m {TimeUsed.Seconds}s {TimeUsed.Milliseconds}ms";
                }

                return "---";
            }
        }

        public void Reset()
        {
            TaskType = 0;
            TimeStart = DateTime.MinValue;
            TimeEnd = DateTime.MinValue;
            IsRunning = false;
            IsCompleted = false;
            IsCancelled = false;
            RequestCancel = false;
            HasError = false;
            ErrorMsg = string.Empty;
            TotalTables = 0;
            CurrentTableIndex = 0;
            TotalRows = 0;
            CurrentRows = 0;
            TotalRowsCurrentTable = 0;
            CurrentRowsCurrentTable = 0;
            TotalBytes = 0;
            CurrentBytes = 0;
        }
    }
}
