using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace MySqlBackupTestApp
{
    public partial class FormTestExportProgresBar : Form
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        MySqlBackup mb;
        Timer timer1;
        BackgroundWorker bwExport;

        string _currentTableName = "";
        int _totalRowsInCurrentTable = 0;
        int _totalRowsInAllTables = 0;
        int _currentRowIndexInCurrentTable = 0;
        int _currentRowIndexInAllTable = 0;
        int _totalTables = 0;
        int _currentTableIndex = 0;

        bool cancel = false;

        public FormTestExportProgresBar()
        {
            InitializeComponent();

            mb = new MySqlBackup();
            mb.ExportProgressChanged += mb_ExportProgressChanged;

            timer1 = new Timer();
            timer1.Interval = 50;
            timer1.Tick += timer1_Tick;

            bwExport = new BackgroundWorker();
            bwExport.DoWork += bwExport_DoWork;
            bwExport.RunWorkerCompleted += bwExport_RunWorkerCompleted;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            cancel = true;
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            if (!Program.TargetDirectoryIsValid())
                return;

            _currentTableName = "";
            _totalRowsInCurrentTable = 0;
            _totalRowsInAllTables = 0;
            _currentRowIndexInCurrentTable = 0;
            _currentRowIndexInAllTable = 0;
            _totalTables = 0;
            _currentTableIndex = 0;

            conn = new MySqlConnection(Program.ConnectionString);
            cmd = new MySqlCommand();
            cmd.Connection = conn;
            conn.Open();

            timer1.Start();

            mb.ExportInfo.IntervalForProgressReport = (int)nmExInterval.Value;
            mb.ExportInfo.GetTotalRowsBeforeExport = true;
            mb.Command = cmd;

            bwExport.RunWorkerAsync();
        }

        void bwExport_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                mb.ExportToFile(Program.TargetFile);
            }
            catch (Exception ex)
            {
                CloseConnection();
                MessageBox.Show(ex.ToString());
            }
        }

        void bwExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CloseConnection();

            if (cancel)
            {
                MessageBox.Show("Cancel by user.");
            }
            else
            {
                if (mb.LastError == null)
                {
                    pbRowInAllTable.Value = pbRowInAllTable.Maximum;
                    pbRowInCurTable.Value = pbRowInCurTable.Maximum;
                    pbTable.Value = pbTable.Maximum;

                    lbRowInCurTable.Text = pbRowInCurTable.Value + " of " + pbRowInCurTable.Maximum;
                    lbRowInAllTable.Text = pbRowInAllTable.Value + " of " + pbRowInAllTable.Maximum;
                    lbTableCount.Text = _currentTableIndex + " of " + _totalTables;

                    this.Refresh();
                    MessageBox.Show("Completed.");
                }
                else
                    MessageBox.Show("Completed with error(s)." + Environment.NewLine + Environment.NewLine + mb.LastError.ToString());
            }

            timer1.Stop();
        }

        void mb_ExportProgressChanged(object sender, ExportProgressArgs e)
        {
            if (cancel)
            {
                mb.StopAllProcess();
                return;
            }

            _currentRowIndexInAllTable = (int)e.CurrentRowIndexInAllTables;
            _currentRowIndexInCurrentTable = (int)e.CurrentRowIndexInCurrentTable;
            _currentTableIndex = e.CurrentTableIndex;
            _currentTableName = e.CurrentTableName;
            _totalRowsInAllTables = (int)e.TotalRowsInAllTables;
            _totalRowsInCurrentTable = (int)e.TotalRowsInCurrentTable;
            _totalTables = e.TotalTables;
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            if (cancel)
            {
                timer1.Stop();
                return;
            }

            pbTable.Maximum = _totalTables;
            if (_currentTableIndex <= pbTable.Maximum)
                pbTable.Value = _currentTableIndex;

            pbRowInCurTable.Maximum = _totalRowsInCurrentTable;
            if (_currentRowIndexInCurrentTable <= pbRowInCurTable.Maximum)
                pbRowInCurTable.Value = _currentRowIndexInCurrentTable;

            pbRowInAllTable.Maximum = _totalRowsInAllTables;
            if (_currentRowIndexInAllTable <= pbRowInAllTable.Maximum)
                pbRowInAllTable.Value = _currentRowIndexInAllTable;

            lbCurrentTableName.Text = "Current Processing Table = " + _currentTableName;
            lbRowInCurTable.Text = pbRowInCurTable.Value + " of " + pbRowInCurTable.Maximum;
            lbRowInAllTable.Text = pbRowInAllTable.Value + " of " + pbRowInAllTable.Maximum;
            lbTableCount.Text = _currentTableIndex + " of " + _totalTables;

            lbTotalRows_Tables.Text = _totalTables + "\r\n" + _totalRowsInAllTables;
        }

        void CloseConnection()
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }

            if (cmd != null)
                cmd.Dispose();
        }
    }
}