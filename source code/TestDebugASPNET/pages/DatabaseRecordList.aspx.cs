using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.pages
{
    public partial class DatabaseRecordList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRecords();
            }
            else
            {
                string action = Request["hiddenPostbackAction"] + "";

                if (action == "delete")
                {
                    DeleteRecords();
                }
            }
        }

        protected void btnLoadFiles_Click(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void LoadRecords()
        {
            var records = BackupFilesManager.GetRecordList(0, null, null);

            StringBuilder sb = new StringBuilder();

            sb.Append($@"
<div class='records-container'>
    <div class='summary-info'>Total Records: {records.Count}</div>
    <table class='records-table'>
        <thead>
            <tr>
                <th><input type='checkbox' id='chkSelectAll' onclick='toggleSelectAll()' /></th>
                <th>ID</th>
                <th>Operation</th>
                <th>Original Filename</th>
                <th>Size</th>
                <th>Database</th>
                <th>Date Created</th>
                <th>Remarks</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody>");

            foreach (var record in records)
            {
                string elapsedTime = GetElapsedTime(record.DateCreated);

                sb.Append($@"
            <tr>
                <td><input type='checkbox' class='record-checkbox' name='del_{record.Id}' /></td>
                <td>{record.Id}</td>
                <td>{HttpUtility.HtmlEncode(record.Operation)}</td>
                <td><span class='label-filename'>{HttpUtility.HtmlEncode(record.Filename)}</span><br />
                    <span class='label-sha'>SHA256:</span><span class='value-sha'>{record.Sha256}</span>
                </td>
                <td>{FormatFileSize(record.Filesize)}</td>
                <td>{HttpUtility.HtmlEncode(record.DatabaseName)}</td>
                <td>{record.DateCreated:yyyy-MM-dd HH:mm:ss}<br />{elapsedTime}</td>
                <td>{HttpUtility.HtmlEncode(record.Remarks)}</td>
                <td>
                    <a class='view-link' href='/DisplayFileContent?id={record.Id}' target='_blank'>View</a><br />
                    <a class='view-link' href='/DisplayFileContent?id={record.Id}&action=download' target='frame1' onclick='showBigLoading();'>Download</a><br />
                    <a class='view-link' href='#' onclick='event.preventDefault(); restoreFileId({record.Id});'>Restore</a>
                </td>
            </tr>");
            }

            sb.Append($@"
        </tbody>
    </table>
</div>");

            ltlRecords.Text = sb.ToString();
        }

        private string GetElapsedTime(DateTime dateCreated)
        {
            TimeSpan elapsed = DateTime.Now - dateCreated;

            if (elapsed.TotalDays >= 1)
            {
                int days = (int)elapsed.TotalDays;
                return $"{days} day{(days == 1 ? "" : "s")} ago";
            }
            else if (elapsed.TotalHours >= 1)
            {
                int hours = (int)elapsed.TotalHours;
                return $"{hours} hour{(hours == 1 ? "" : "s")} ago";
            }
            else if (elapsed.TotalMinutes >= 1)
            {
                int minutes = (int)elapsed.TotalMinutes;
                return $"{minutes} minute{(minutes == 1 ? "" : "s")} ago";
            }
            else
            {
                int seconds = (int)elapsed.TotalSeconds;
                return $"{seconds} second{(seconds == 1 ? "" : "s")} ago";
            }
        }

        void DeleteRecords()
        {
            List<int> lstDelId = new List<int>();

            // Collect all form keys that start with "del_"
            foreach (string key in Request.Form.AllKeys)
            {
                if (key != null && key.StartsWith("del_"))
                {
                    // Extract ID from the key name (del_123 -> 123)
                    string idStr = key.Substring(4); // Remove "del_" prefix
                    if (int.TryParse(idStr, out int id))
                    {
                        lstDelId.Add(id);
                    }
                }
            }

            if (lstDelId.Count > 0)
            {
                BackupFilesManager.DeleteRecords(lstDelId);
            }

            LoadRecords();
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return string.Format("{0:0.###} {1}", len, sizes[order]);
        }
    }
}