using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection.Emit;
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
                string currentUser = WindowsIdentity.GetCurrent().Name;
                lbUser.Text = currentUser;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load current user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lbUser.Text = "Error loading user";
            }
        }

        private void btCreateTaskScheduler_Click(object sender, EventArgs e)
        {
            try
            {
                // Load the config to get the backup time
                Config config = Program.ReadConfigFile(false);
                if (config == null)
                {
                    MessageBox.Show("No configuration found. Please save settings first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Get the current user
                string currentUser = WindowsIdentity.GetCurrent().Name;

                // Create or update the task
                using (TaskService ts = new TaskService())
                {
                    // Define the task
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = "Daily backup of all databases using Backup_All_Databases application";
                    td.Principal.UserId = currentUser;
                    td.Principal.LogonType = TaskLogonType.InteractiveToken; // Run with current user credentials
                    td.Principal.RunLevel = TaskRunLevel.Highest; // Run with highest privileges

                    // Set daily trigger at the specified time
                    DailyTrigger dt = new DailyTrigger
                    {
                        StartBoundary = DateTime.Today.AddHours(config.StartHour).AddMinutes(config.StartMinute),
                        DaysInterval = 1 // Run every day
                    };
                    td.Triggers.Add(dt);

                    // Set the action to run the application with /SILENT argument
                    string exePath = Application.ExecutablePath;
                    ExecAction action = new ExecAction(exePath, "\\SILENT", null);
                    td.Actions.Add(action);

                    // Register the task (create or update)
                    ts.RootFolder.RegisterTaskDefinition("BackupAllDatabasesTask", td);

                    MessageBox.Show("Task scheduler created/updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create/update task scheduler: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btDeleteTaskScheduler_Click(object sender, EventArgs e)
        {
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