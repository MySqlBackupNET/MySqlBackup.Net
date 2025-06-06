using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;

namespace Backup_All_Databases
{
    public partial class FormTaskScheduler : Backup_All_Databases.baseform
    {
        public FormTaskScheduler()
        {
            InitializeComponent();

            try
            {
                string currentUser = GetUsername();
                lbUser.Text = currentUser;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load current user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lbUser.Text = "Error loading user";
            }
        }

        string GetUsername()
        {
            string uname = WindowsIdentity.GetCurrent().Name;
            return uname.Substring(uname.IndexOf("\\") + 1);
        }

        bool RequestAdminPrivilege()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                MessageBox.Show("This program will now restart to run as \"Administrator\".");

                try
                {
                    // Prepare to restart the application with elevation
                    ProcessStartInfo processInfo = new ProcessStartInfo
                    {
                        UseShellExecute = true,
                        FileName = Application.ExecutablePath, // Path to the current executable
                        Verb = "runas" // Requests elevation via UAC
                    };

                    // Start the new elevated process
                    Process.Start(processInfo);

                    // Close the current non-elevated instance
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    // Handle cases where the user cancels the UAC prompt or another error occurs
                    MessageBox.Show($"Failed to elevate permissions: {ex.Message}\nPlease run the application as an administrator.", "Elevation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit(); // Exit anyway to avoid running without permissions
                }
            }

            return true;
        }

        private void btCreateTaskScheduler_Click(object sender, EventArgs e)
        {
            if (!RequestAdminPrivilege())
            {
                return;
            }

            try
            {
                // Prompt for admin credentials to allow task to run whether user is logged on or not
                (string adminUser, string adminPassword) = PromptForUsername();

                if (string.IsNullOrEmpty(adminUser))
                {
                    return;
                }

                if (string.IsNullOrEmpty(adminPassword))
                {
                    MessageBox.Show("Admin username and password are required to run the task whether logged on or not.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Load the config to get the backup time
                Config config = Program.ReadConfigFile(false);
                if (config == null)
                {
                    MessageBox.Show("No configuration found. Please save settings first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Create or update the task
                using (TaskService ts = new TaskService())
                {
                    // Define the task
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = "Daily backup of all databases using Backup_All_Databases application";
                    td.Principal.UserId = adminUser;
                    td.Principal.LogonType = TaskLogonType.Password; // Run whether user is logged on or not
                    td.Principal.RunLevel = TaskRunLevel.Highest; // Run with highest privileges
                    td.Settings.DisallowStartIfOnBatteries = false; // Allow running on battery power
                    td.Settings.ExecutionTimeLimit = TimeSpan.FromDays(1); // Allow task to run for up to 1 day
                    td.Settings.Enabled = true; // Ensure task is enabled
                    td.Settings.Hidden = false; // Optional: make task visible in Task Scheduler

                    // Set daily trigger at the specified time
                    DailyTrigger dt = new DailyTrigger
                    {
                        StartBoundary = DateTime.Today.AddHours(config.StartHour).AddMinutes(config.StartMinute),
                        DaysInterval = 1 // Run every day
                    };
                    td.Triggers.Add(dt);

                    // Set the action to run the application with /SILENT argument
                    string exePath = Application.ExecutablePath;
                    ExecAction action = new ExecAction(exePath, "/SILENT", null); // Corrected argument syntax
                    td.Actions.Add(action);

                    // Register the task (create or update) with credentials
                    ts.RootFolder.RegisterTaskDefinition("Backup All Databases MySQL", td, TaskCreation.CreateOrUpdate, adminUser, adminPassword, TaskLogonType.Password);

                    MessageBox.Show("Task scheduler created/updated successfully to run whether user is logged on or not.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create/update task scheduler: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Placeholder method to prompt for admin username
        private (string, string) PromptForUsername()
        {
            string password = "";
            string username = "";
            using (baseform prompt = new baseform())
            {
                prompt.TopMost = true;
                prompt.ShowInTaskbar = false;
                prompt.MaximizeBox = false;
                prompt.MinimizeBox = false;
                prompt.Width = 350;
                prompt.Height = 240;
                prompt.Text = "Enter Admin Username";
                Label label = new Label() { Left = 20, Top = 20, Text = "Windows Current Username:", AutoSize = true };
                TextBox textBoxUsername = new TextBox() { Left = 20, Top = 40, Width = 240 };
                textBoxUsername.Text = GetUsername();
                Label labelpwd = new Label() { Left = 20, Top = 80, Text = "Windows User Login Password:", AutoSize = true };
                TextBox textBoxPwd = new TextBox() { Left = 20, Top = 100, Width = 240, UseSystemPasswordChar = true };
                Button ok = new Button() { Text = "OK", Left = 20, Top = 140, Width = 100, Height = 30, DialogResult = DialogResult.OK };
                Button cancel = new Button() { Text = "Cancel", Left = 130, Top = 140, Width = 100, Height = 30, DialogResult = DialogResult.Cancel };
                prompt.Controls.Add(label);
                prompt.Controls.Add(textBoxUsername);
                prompt.Controls.Add(labelpwd);
                prompt.Controls.Add(textBoxPwd);
                prompt.Controls.Add(ok);
                prompt.Controls.Add(cancel);
                prompt.AcceptButton = ok;
                prompt.CancelButton = cancel;

                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    username = textBoxUsername.Text.Trim();
                    password = textBoxPwd.Text.Trim();
                }
            }
            if (username == "")
                return (null, null);
            return (username, password);
        }

        private void btDeleteTaskScheduler_Click(object sender, EventArgs e)
        {
            if (!RequestAdminPrivilege())
            {
                return;
            }

            try
            {
                using (TaskService ts = new TaskService())
                {
                    // Check if the task exists
                    Task task = ts.GetTask("BackupAllDatabasesTask");
                    if (task == null)
                    {
                        MessageBox.Show("No task scheduler found to delete.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Delete the task
                    ts.RootFolder.DeleteTask("BackupAllDatabasesTask");
                    MessageBox.Show("Task scheduler deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete task scheduler: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}