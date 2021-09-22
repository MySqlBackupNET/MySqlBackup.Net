using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Devart.Data.MySql;

namespace MySqlBackupTestApp
{
    public partial class FormTestImportProgressReport : Form
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        MySqlBackup mb;
        Timer timer1;
        BackgroundWorker bwImport;

        int curBytes;
        int totalBytes;

        bool cancel = false;

        DateTime timeStart = DateTime.MinValue;
        DateTime timeEnd = DateTime.MinValue;

        public FormTestImportProgressReport()
        {
            InitializeComponent();

            mb = new MySqlBackup();
            mb.ImportInfo.IntervalForProgressReport = (int)nmImInterval.Value;
            mb.ImportProgressChanged += mb_ImportProgressChanged;

            timer1 = new Timer();
            timer1.Interval = 50;
            timer1.Tick += timer1_Tick;

            bwImport = new BackgroundWorker();
            bwImport.DoWork += bwImport_DoWork;
            bwImport.RunWorkerCompleted += bwImport_RunWorkerCompleted;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            cancel = true;
        }

        private void btImport_Click(object sender, EventArgs e)
        {
            if (!Program.SourceFileExists())
                return;

            progressBar1.Value = 0;
            lbStatus.Text = "0 of 0 bytes";
            this.Refresh();

            cancel = false;
            curBytes = 0;
            totalBytes = 0;

            if (conn != null)
            {
                conn.Dispose();
                conn = null;
            }

            conn = new MySqlConnection(Program.ConnectionString);
            cmd = new MySqlCommand();
            cmd.Connection = conn;
            conn.Open();

            timer1.Start();

            mb.ImportInfo.IntervalForProgressReport = (int)nmImInterval.Value;
            mb.Command = cmd;

            timeStart = DateTime.Now;
            lbTotalTime.Text = "";

            bwImport.RunWorkerAsync();
        }

        void bwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                mb.ImportFromFile(Program.TargetFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            if (cancel)
            {
                timer1.Stop();
                return;
            }

            progressBar1.Maximum = totalBytes;

            if (curBytes < progressBar1.Maximum)
                progressBar1.Value = curBytes;

            lbStatus.Text = progressBar1.Value + " of " + progressBar1.Maximum;
        }

        void mb_ImportProgressChanged(object sender, ImportProgressArgs e)
        {
            if (cancel)
                mb.StopAllProcess();

            totalBytes = (int)e.TotalBytes;
            curBytes = (int)e.CurrentBytes;
        }

        void bwImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer1.Stop();

            CloseConnection();

            timeEnd = DateTime.Now;
            var ts = timeEnd - timeStart;
            lbTotalTime.Text = $"{ts.Hours} h {ts.Minutes} m {ts.Seconds} s {ts.Milliseconds} ms";

            if (cancel)
            {
                MessageBox.Show("Cancel by user.");
            }
            else
            {
                if (mb.LastError == null)
                {
                    progressBar1.Value = progressBar1.Maximum;
                    lbStatus.Text = progressBar1.Value + " of " + progressBar1.Maximum;
                    this.Refresh();

                    MessageBox.Show("Completed.");
                }
                else
                    MessageBox.Show("Completed with error(s)." + Environment.NewLine + Environment.NewLine + mb.LastError.ToString());
            }
        }

        void CloseConnection()
        {
            if (conn != null)
                conn.Close();

            if (conn != null)
                conn.Dispose();

            if (cmd != null)
                cmd.Dispose();
        }
    }
}
