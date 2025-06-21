using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.pages
{
    public partial class apiProgressReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Register async task for Web Forms
            RegisterAsyncTask(new PageAsyncTask(Page_LoadAsync));
        }

        private async Task Page_LoadAsync()
        {
            string action = Request["action"] + "";
            action = action.ToLower();

            try
            {
                if (action == "restore")
                {
                    await RestoreAsync();
                }
                else if (action == "backup")
                {
                    await BackupAsync();
                }
                else if (action == "getalltasks")
                {
                    await GetAllTaskAsync();
                }
                else if (action == "getactivetasks")
                {
                    await GetActiveTasksAsync();
                }
                else if (action == "delete")
                {
                    await DeleteTask();
                }
                else if (action == "cancel")
                {
                    await CancelTask();
                }
            }
            catch (Exception ex)
            {
                Response.Write("0|" + ex.Message);
            }
        }

        private async Task RestoreAsync()
        {
            try
            {
                if (int.TryParse(Request["id"] + "", out int id))
                {
                    if (BackupFilesManager.FileExists(id))
                    {
                        ServiceRestore restoreService = new ServiceRestore();
                        await restoreService.StartAsync(id);
                        Response.Write("1");
                    }
                    else
                    {
                        Response.Write("0|File not exists");
                        return;
                    }
                }
                else if (Request.Files.Count > 0)
                {
                    ServiceRestore restoreService = new ServiceRestore();
                    await restoreService.StartAsync(Request.Files[0]);
                    Response.Write("1");
                }
                else
                {
                    Response.Write("0|Not file to restore");
                    return;
                }
            }
            catch (Exception ex)
            {
                Response.Write("0|" + ex.Message);
            }
        }

        private async Task CancelTask()
        {
            if (int.TryParse(Request["id"], out int id))
            {
                await Task.Run(() => ProgressReportManager.CancelTask(id));
            }
        }

        private async Task DeleteTask()
        {
            if (int.TryParse(Request["id"], out int id))
            {
                await Task.Run(() => ProgressReportManager.DeleteTask(id));
                Response.Write("1");
            }
        }

        private async Task BackupAsync()
        {
            try
            {
                ServiceBackup backupService = new ServiceBackup();
                await backupService.StartAsync();

                Response.Write("1");
            }
            catch (Exception ex)
            {
                Response.Write("0|" + ex.Message);
            }
        }

        private async Task GetAllTaskAsync()
        {
            var lst = ProgressReportManager.GetProgressReportList(0, true, false);
            await WriteJsonResponseAsync(lst);
        }

        private async Task GetActiveTasksAsync()
        {
            var lst = ProgressReportManager.GetProgressReportList(0, false, true);
            await WriteJsonResponseAsync(lst);
        }

        private async Task WriteJsonResponseAsync(object data)
        {
            string json = JsonSerializer.Serialize(data);

            Response.Clear();
            Response.ContentType = "application/json";

            // For ASP.NET Web Forms, Response.Write is synchronous
            // but we can still wrap it for consistency
            await Task.Run(() => Response.Write(json));
        }
    }
}