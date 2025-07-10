using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySqlConnector;
using System.Data.SQLite;
using static System.Net.WebRequestMethods;
using System.Security.Cryptography;

namespace System
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                config.GetMySqlInstancePath(out string mysqld_path, out string mysqladmin_path);

                if (string.IsNullOrEmpty(mysqld_path))
                {
                    txtMysqldExePath.Text = @"C:\mysql\bin\mysqld.exe";
                }
                else
                {
                    txtMysqldExePath.Text = mysqld_path;
                }

                if (string.IsNullOrEmpty(mysqladmin_path))
                {
                    txtMySqlAdminExePath.Text = @"C:\mysql\bin\mysqladmin.exe";
                }
                else
                {
                    txtMySqlAdminExePath.Text = mysqladmin_path;
                }

                if (string.IsNullOrEmpty(config.ConnString))
                {
                    txtConnStr.Text = "server=localhost;user=root;pwd=;convertzerodatetime=true;treattinyasboolean=true;";
                }
                else
                {
                    txtConnStr.Text = config.ConnString;
                }
            }
        }

        protected void btBackup_Click(object sender, EventArgs e)
        {
            try
            {
                string folder = Server.MapPath("~/App_Data/temp");
                Directory.CreateDirectory(folder);

                string databaseName = "";
                string filename = "";
                string filePath = "";

                using (var conn = config.GetNewConnection())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "select database()";
                    databaseName = cmd.ExecuteScalar() + "";
                }

                if (string.IsNullOrEmpty(databaseName))
                {
                    throw new Exception("No database selected");
                }

                filename = $"{databaseName}_{DateTime.Now:yyyy-MM-dd_hhmmss}.sql";
                filePath = Path.Combine(folder, filename);

                using (var conn = config.GetNewConnection())
                using (var cmd = conn.CreateCommand())
                using (var mb = new MySqlBackup(cmd))
                {
                    conn.Open();
                    mb.ExportToFile(filePath);
                }

                string result = $@"
<span style='color: darkgreen;'>
Backup Success!<br />
<br />
Download File: <a href='/apiFiles?folder=temp&filename={filename}'>{filename}</a><br />
<br />
File saved at: {filePath}</span>";

                phBackup.Controls.Add(new LiteralControl(result));
                ((masterPage1)this.Master).ShowMessage("Success", "Backup Success", true);
            }
            catch (Exception ex)
            {
                phBackup.Controls.Add(new LiteralControl($"<pre style='color: maroon;'>Task Failed: {ex.Message}</pre>"));
                ((masterPage1)this.Master).ShowMessage("Error", "Backup Failed", false);
            }
        }

        protected void btRestore_Click(object sender, EventArgs e)
        {
            try
            {
                if (!fileUploadSql.HasFile)
                {
                    throw new Exception("No file uploaded.");
                }

                if (fileUploadSql.FileName.ToLower().EndsWith(".zip"))
                {
                    throw new Exception("Zip file is not supported in this page. Please go to other pages for zip file.");
                }

                string folder = Server.MapPath("~/App_Data/temp");
                Directory.CreateDirectory(folder);
                string databaseName = "";
                string filename = "";
                string filePath = "";

                using (var conn = config.GetNewConnection())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "select database()";
                    databaseName = cmd.ExecuteScalar() + "";
                }

                if (string.IsNullOrEmpty(databaseName))
                {
                    throw new Exception("No database selected");
                }

                filename = $"{databaseName}_(restore)_{DateTime.Now:yyyy-MM-dd_hhmmss}.txt";
                filePath = Path.Combine(folder, filename);
                fileUploadSql.SaveAs(filePath);

                using (var conn = config.GetNewConnection())
                using (var cmd = conn.CreateCommand())
                using (var mb = new MySqlBackup(cmd))
                {
                    conn.Open();
                    mb.ImportFromFile(filePath);
                }

                string result = $@"
<span style='color: darkgreen;'>
Restore Success!<br />
<br />
Download File: <a href='/apiFiles?folder=temp&filename={filename}'>{filename}</a><br />
<br />
File saved at: {filePath}</span>";

                phRestore.Controls.Add(new LiteralControl(result));
                ((masterPage1)this.Master).ShowMessage("Restore", "Restore Success", true);
            }
            catch (Exception ex)
            {
                phRestore.Controls.Add(new LiteralControl($"<pre style='color: maroon;'>Task Failed: {ex.Message}</pre>"));
                ((masterPage1)this.Master).ShowMessage("Error", "Restore Failed", false);
            }
        }
    }
}