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
        }

        protected void btnLoadFiles_Click(object sender, EventArgs e)
        {
            LoadRecords();
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            DeleteSelectedRecords();
            LoadRecords();
        }

        private void LoadRecords()
        {
            try
            {
                // Get all records (empty string for operation to get all)
                List<DatabaseFileRecord> records = EngineSQLite.GetRecordList("");

                StringBuilder sb = new StringBuilder();

                // Start container div and add summary info
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
                <th>SHA256</th>
                <th>Remarks</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody>");

                foreach (var record in records)
                {
                    sb.Append($@"
            <tr>
                <td><input type='checkbox' class='record-checkbox' name='del_{record.Id}' /></td>
                <td>{record.Id}</td>
                <td>{HttpUtility.HtmlEncode(record.Operation)}</td>
                <td>{HttpUtility.HtmlEncode(record.OriginalFilename)}</td>
                <td>{FormatFileSize(record.Filesize)}</td>
                <td>{HttpUtility.HtmlEncode(record.DatabaseName)}</td>
                <td>{record.DateCreated:yyyy-MM-dd HH:mm:ss}</td>
                <td>{record.Sha256}</td>
                <td>{HttpUtility.HtmlEncode(record.Remarks)}</td>
                <td><a href='/DisplayFileContent?id={record.Id}' target='_blank' class='view-link'>View</a></td>
            </tr>");
                }

                sb.Append($@"
        </tbody>
    </table>
</div>");

                // Write to literal control
                ltlRecords.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                ltlRecords.Text = $@"<div class='error-message'>Error loading records: {HttpUtility.HtmlEncode(ex.Message)}</div>";
            }
        }

        private void DeleteSelectedRecords()
        {
            try
            {
                // Collect all form keys that start with "del_"
                foreach (string key in Request.Form.AllKeys)
                {
                    if (key != null && key.StartsWith("del_"))
                    {
                        // Extract ID from the key name (del_123 -> 123)
                        string idStr = key.Substring(4); // Remove "del_" prefix
                        if (int.TryParse(idStr, out int id))
                        {
                            EngineSQLite.DeleteRecord(id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error or show message
                ltlMessage.Text = $@"<div class='error-message'>Error deleting records: {HttpUtility.HtmlEncode(ex.Message)}</div>";
            }
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
            return string.Format("{0:0.##} {1}", len, sizes[order]);
        }
    }
}