using MySqlConnector;
using System;
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
    public partial class FormSimpleDemo : baseForm
    {
        Timer timerSaveConstr = new Timer();
        string fileConstr = Path.Combine(Application.StartupPath, "constr_simple_demo.txt");

        BackgroundWorker bw = new BackgroundWorker();

        // Keep reference to MySqlBackup instance at global class level variable,
        // required for stop process
        MySqlBackup mb = null;

        public FormSimpleDemo()
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

            bw.WorkerSupportsCancellation = true;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.DoWork += Bw_DoWork;
        }

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

        #region Basic Other UI controls

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
                    txtBackupFile.Text = sqlFilePath;
                    WriteStatus("Backup task is running...");
                    toolStripProgressBar1.Style = ProgressBarStyle.Marquee;

                    int taskType = 1; // backup

                    object[] objectArray = new object[] { taskType, sqlFilePath };

                    bw.RunWorkerAsync(objectArray);

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
                    txtBackupFile.Text = sqlFilePath;
                    WriteStatus("Restore task is running...");
                    toolStripProgressBar1.Style = ProgressBarStyle.Marquee;

                    int taskType = 2; // restore

                    object[] objectArray = new object[] { taskType, sqlFilePath };

                    bw.RunWorkerAsync(objectArray);

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

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            TaskResult tr = new TaskResult();

            try
            {
                object[] objectArray = (object[])e.Argument;

                int taskType = Convert.ToInt32(objectArray[0]);
                string sqlFilePath = objectArray[1] + "";

                using (var conn = new MySqlConnection(txtConstr.Text))
                using (var cmd = conn.CreateCommand())
                using (mb = new MySqlBackup(cmd))
                {
                    conn.Open();

                    if (taskType == 1)
                    {
                        mb.ExportToFile(sqlFilePath);
                    }
                    else
                    {
                        mb.ImportFromFile(sqlFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                tr.HasError = true;
                tr.ErrMsg = ex.Message;
            }

            e.Result = tr;
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = (TaskResult)e.Result;

            if (result.HasError)
            {
                WriteStatus($"Error: {result.ErrMsg}");
            }
            else
            {
                if (e.Cancelled)
                {
                    WriteStatus("Backup was cancelled by user");
                }
                else if (e.Error != null)
                {
                    WriteStatus($"Backup failed: {e.Error.Message}");
                }
                else
                {
                    WriteStatus("Backup completed successfully");
                }
            }

            // Clean up MySqlBackup reference
            mb?.Dispose();
            mb = null;

            toolStripProgressBar1.Style = ProgressBarStyle.Blocks;

            EnableButtons(true, true, false);
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            if (bw.IsBusy)
            {
                WriteStatus("Cancelling task...");

                bw.CancelAsync();

                if (mb != null)
                {
                    mb.StopAllProcess();
                }

                btStop.Enabled = false;
            }
            else
            {
                WriteStatus("No backup operation is currently running");
            }

        }

    }

    class TaskResult
    {
        public bool HasError { get; set; } = false;
        public string ErrMsg { get; set; } = "";
    }
}
