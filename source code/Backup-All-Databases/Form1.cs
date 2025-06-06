using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;

namespace Backup_All_Databases
{
    public partial class Form1 : baseform
    {
        bool timerStarted = false;
        bool selectIsChanging = false;

        Timer timer1 = null;
        Timer timer2 = null;

        public Form1()
        {
            InitializeComponent();

            lbNextBackup.Text = "";

            timer2 = new Timer();
            timer2.Tick += Timer2_Tick;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (MessageBox.Show("A config is saved previously. Do you want to load the saved config settings?",
                    "Load Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LoadSettings();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(Program.ConfigFilePath))
            {
                timer1 = new Timer();
                timer1.Interval = 500;
                timer1.Tick += Timer1_Tick;
                timer1.Start();
            }
        }

        void LoadDatabaseList()
        {
            DataTable dtDatabases = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(txtConnString.Text))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;

                    cmd.CommandText = "SELECT SCHEMA_NAME FROM information_schema.SCHEMATA WHERE SCHEMA_NAME NOT IN ('information_schema', 'mysql', 'performance_schema', 'sys', 'rdsadmin');";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dtDatabases);

                    conn.Close();
                }
            }

            checkedListExcludeDatabases.Items.Clear();
            checkedListIncludeDatabases.Items.Clear();

            foreach (DataRow dr in dtDatabases.Rows)
            {
                checkedListExcludeDatabases.Items.Add(dr[0] + "");
                checkedListIncludeDatabases.Items.Add(dr[0] + "");
            }
        }

        void LoadSettings()
        {
            try
            {
                // Load configuration
                Config config = Program.ReadConfigFile(true);
                if (config == null)
                {
                    MessageBox.Show("No configuration found or unable to load settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Populate form controls
                txtConnString.Text = config.ConnectionString;
                txtDestinationFolder.Text = config.BackupPath;
                numericUpDownMaxBackupCount.Value = config.MaxBackupCopies;
                checkBoxBackupAllDatabases.Checked = config.BackupAllDatabases;
                nmBackupHour.Value = config.StartHour;
                nmBackupMinute.Value = config.StartMinute;

                // Fetch available databases if connection string is provided
                LoadDatabaseList();

                for (int i = 0; i < checkedListIncludeDatabases.Items.Count; i++)
                {
                    var item = checkedListIncludeDatabases.Items[i];
                    string dbName = item.ToString();

                    checkedListIncludeDatabases.SetItemChecked(i, config.IncludeList.Contains(dbName));
                }

                for (int i = 0; i < checkedListExcludeDatabases.Items.Count; i++)
                {
                    var item = checkedListExcludeDatabases.Items[i];
                    string dbName = item.ToString();

                    checkedListExcludeDatabases.SetItemChecked(i, config.ExcludeList.Contains(dbName));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(txtConnString.Text))
                {
                    MessageBox.Show("Connection string cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDestinationFolder.Text))
                {
                    MessageBox.Show("Destination backup folder must be selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Create Config object
                Config config = new Config
                {
                    ConnectionString = txtConnString.Text.Trim(),
                    BackupPath = txtDestinationFolder.Text.Trim(),
                    MaxBackupCopies = (int)numericUpDownMaxBackupCount.Value,
                    BackupAllDatabases = checkBoxBackupAllDatabases.Checked,
                    IncludeList = checkedListIncludeDatabases.CheckedItems.Cast<string>().ToList(),
                    ExcludeList = checkedListExcludeDatabases.CheckedItems.Cast<string>().ToList(),
                    StartHour = Convert.ToInt32(nmBackupHour.Value),
                    StartMinute = Convert.ToInt32(nmBackupMinute.Value)
                };

                // Save configuration
                Program.WriteConfigFile(config);
                MessageBox.Show("Settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btLoadSettings_Click(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void btSetBackupFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select the destination folder for backups";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtDestinationFolder.Text = fbd.SelectedPath;
                }
            }
        }

        private void btLoadDatabaseList_Click(object sender, EventArgs e)
        {
            try
            {
                LoadDatabaseList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBoxBackupAllDatabases_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = !checkBoxBackupAllDatabases.Checked;
            groupBox2.Enabled = !checkBoxBackupAllDatabases.Checked;
        }

        private void btTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                string timenow = "";
                using (MySqlConnection conn = new MySqlConnection(txtConnString.Text))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select now();";
                    timenow = cmd.ExecuteScalar() + "";
                    conn.Close();
                }

                MessageBox.Show("Connection success!\r\n\r\n" + timenow, "Good", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btRunBackup_Click(object sender, EventArgs e)
        {
            FormBackup fb = new FormBackup();
            fb.Show();
        }

        private void btIncludeSelectAll_Click(object sender, EventArgs e)
        {
            selectIsChanging = true;
            checkedListIncludeDatabases.SuspendLayout();
            for (int i = 0; i < checkedListIncludeDatabases.Items.Count; i++)
            {
                checkedListIncludeDatabases.SetItemChecked(i, true);
            }
            checkedListIncludeDatabases.ResumeLayout(true);
            selectIsChanging = false;
        }

        private void btIncludeClear_Click(object sender, EventArgs e)
        {
            selectIsChanging = true;
            checkedListIncludeDatabases.SuspendLayout();
            for (int i = 0; i < checkedListIncludeDatabases.Items.Count; i++)
            {
                checkedListIncludeDatabases.SetItemChecked(i, false);
            }
            checkedListIncludeDatabases.ResumeLayout(true);
            selectIsChanging = false;
        }

        private void btExcludeSelectAll_Click(object sender, EventArgs e)
        {
            selectIsChanging = true;
            checkedListExcludeDatabases.SuspendLayout();
            for (int i = 0; i < checkedListExcludeDatabases.Items.Count; i++)
            {
                checkedListExcludeDatabases.SetItemChecked(i, true);
            }
            checkedListExcludeDatabases.ResumeLayout(true);
            selectIsChanging = false;
        }

        private void btExcludeClear_Click(object sender, EventArgs e)
        {
            selectIsChanging = true;
            checkedListExcludeDatabases.SuspendLayout();
            for (int i = 0; i < checkedListExcludeDatabases.Items.Count; i++)
            {
                checkedListExcludeDatabases.SetItemChecked(i, false);
            }
            checkedListExcludeDatabases.ResumeLayout(true);
            selectIsChanging = false;
        }

        private void checkedListIncludeDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectIsChanging)
                return;

            if (checkedListIncludeDatabases.GetItemCheckState(checkedListIncludeDatabases.SelectedIndex) == CheckState.Checked)
                checkedListIncludeDatabases.SetItemCheckState(checkedListIncludeDatabases.SelectedIndex, CheckState.Unchecked);
            else
                checkedListIncludeDatabases.SetItemCheckState(checkedListIncludeDatabases.SelectedIndex, CheckState.Checked);
        }

        private void checkedListExcludeDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectIsChanging)
                return;

            if (checkedListExcludeDatabases.GetItemCheckState(checkedListExcludeDatabases.SelectedIndex) == CheckState.Checked)
                checkedListExcludeDatabases.SetItemCheckState(checkedListExcludeDatabases.SelectedIndex, CheckState.Unchecked);
            else
                checkedListExcludeDatabases.SetItemCheckState(checkedListExcludeDatabases.SelectedIndex, CheckState.Checked);
        }

        private void btTaskScheduler_Click(object sender, EventArgs e)
        {
            FormTaskScheduler f = new FormTaskScheduler();
            f.ShowDialog();
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();

            FormBackup f = new FormBackup();
            f.auto = true;
            f.ShowDialog();

            f = null;

            GC.Collect();

            StartTimer();
        }

        void StartTimer()
        {
            timer2.Stop();

            int hour = Convert.ToInt32(nmBackupHour.Value);
            int minute = Convert.ToInt32(nmBackupMinute.Value);

            DateTime timeNextBackup = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, 0);

            var ts = timeNextBackup - DateTime.Now;
            if (ts.TotalSeconds < 0)
            {
                timeNextBackup = timeNextBackup.AddDays(1);
            }

            ts = timeNextBackup - DateTime.Now;

            lbNextBackup.Text = $"Timer Started\r\nNext Backup:\r\n{timeNextBackup:yyyy-MM-dd HH:mm}";

            timer2.Interval = Convert.ToInt32(ts.TotalSeconds * 1000);
            timer2.Start();

            timerStarted = true;
        }

        private void btTimer_Click(object sender, EventArgs e)
        {
            if (timerStarted)
            {
                timer2.Stop();
                timerStarted = false;
                lbNextBackup.Text = "Timer Stoped";
                return;
            }

            StartTimer();
        }

        private void btDeleteConfig_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure to delete the config files?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    File.Delete(Program.ConfigFilePath);
                    File.Delete(Program.KeyFilePath);
                    File.Delete(Program.saltFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot delete config files: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btInfo_Click(object sender, EventArgs e)
        {
            FormInfo f = new FormInfo();
            f.ShowDialog();
        }
    }
}