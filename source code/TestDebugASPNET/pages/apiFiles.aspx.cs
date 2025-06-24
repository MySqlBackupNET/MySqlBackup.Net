using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.pages
{
    public partial class apiFiles : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            string filename = (Request["filename"] + "").Trim();
            string folderName = (Request["folder"] + "").Trim().ToLower();

            if (folderName == "temp" || folderName == "backup")
            { }
            else
            {
                return;
            }

            if (filename.Contains("\\") || filename.Contains("/"))
            {
                return;
            }

            string filePath = Server.MapPath($"~/App_Data/{folderName}/{filename}");

            if (File.Exists(filePath))
            {
                // Set appropriate headers
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", $"attachment; filename=\"{Path.GetFileName(filePath)}\"");

                // Asynchronous transmission
                await Task.Run(() => Response.TransmitFile(filePath));
            }
        }

    }
}