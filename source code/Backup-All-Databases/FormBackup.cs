using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Backup_All_Databases
{
    public partial class FormBackup : Backup_All_Databases.baseform
    {
        public bool auto = false;
        private long lastLogPosition = 0;

        BackgroundWorker bw = new BackgroundWorker();
        Backup backup = null;
        string logFilePath = "";

        Timer timerAutoStart = null;

        public FormBackup()
        {
            InitializeComponent();
            bw.DoWork += Bw_DoWork;
            bw.ProgressChanged += Bw_ProgressChanged;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
        }

        private void FormBackup_Load(object sender, EventArgs e)
        {
            if (auto)
            {
                timerAutoStart = new Timer();
                timerAutoStart.Interval = 600;
                timerAutoStart.Tick += TimerAutoStart_Tick;
                timerAutoStart.Start();
            }
        }

        private void TimerAutoStart_Tick(object sender, EventArgs e)
        {
            timerAutoStart.Stop();
            btRunBackup_Click(null, null);
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (backup != null)
            {
                backup.ProgressChanged -= Backup_ProgressChanged;
                backup = null;
                textBox1.SuspendLayout();
                textBox1.AppendText($"Process ended\r\n");
                textBox1.Select(textBox1.TextLength, 0);
                textBox1.ScrollToCaret();
                textBox1.ResumeLayout(true);
                GC.Collect();
            }

            progressBar1.Visible = false;

            if (auto)
            {
                this.Close();
            }
        }

        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (logFilePath == "")
            {
                Backup.BackupChangedEventArgs arg = (Backup.BackupChangedEventArgs)e.UserState;
                logFilePath = arg.LogFilePath;
            }

            try
            {
                if (!string.IsNullOrEmpty(logFilePath) && File.Exists(logFilePath))
                {
                    using (FileStream fs = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        fs.Seek(lastLogPosition, SeekOrigin.Begin);
                        using (StreamReader reader = new StreamReader(fs))
                        {
                            string newContent = reader.ReadToEnd();
                            if (!string.IsNullOrEmpty(newContent))
                            {
                                textBox1.AppendText(newContent);
                                textBox1.Select(textBox1.TextLength, 0);
                                textBox1.ScrollToCaret();
                            }
                            lastLogPosition = fs.Position;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                textBox1.AppendText($"Error reading log file: {ex.Message}\r\n");
                textBox1.Select(textBox1.TextLength, 0);
                textBox1.ScrollToCaret();
            }

            if (bw.CancellationPending)
            {
                backup.Stop();
            }
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Config config = (Config)e.Argument;

            backup = new Backup();
            backup.ProgressChanged += Backup_ProgressChanged;
            backup.Run(config);
        }

        private void Backup_ProgressChanged(object sender, Backup.BackupChangedEventArgs e)
        {
            bw.ReportProgress(0, e);
        }

        private void btRunBackup_Click(object sender, EventArgs e)
        {
            if (bw.IsBusy)
            {
                toolTip1.Show("Backup is running... Please wait...", this, 55, 31, 2500);
                return;
            }

            logFilePath = "";
            lastLogPosition = 0;
            textBox1.Text = "";

            var config = Program.ReadConfigFile(false);

            if (config == null)
                return;

            progressBar1.Visible = true;

            bw.RunWorkerAsync(config);
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            bw.CancelAsync();
        }

        private void btOpenLogFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(logFilePath))
            {
                MessageBox.Show("Please run the backup first");
                return;
            }

            try
            {
                Process p = new Process();
                p.StartInfo.FileName = logFilePath;
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}