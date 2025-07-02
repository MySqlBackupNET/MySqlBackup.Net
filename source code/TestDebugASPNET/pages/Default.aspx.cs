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
                LoadConstr();
            }
            else
            {
                string action = Request["postbackAction"] + "";
                if (action == "restore-memory")
                {
                    RestoreInMemory();
                }
            }
        }

        void LoadConstr()
        {
            txtConnStr.Text = config.ConnString;
        }

        protected void btSaveConnStr_Click(object sender, EventArgs e)
        {
            try
            {
                config.SaveConnStr(txtConnStr.Text);

                MySqlConnectionStringBuilder consb = new MySqlConnectionStringBuilder(txtConnStr.Text);
                string database = consb.Database;

                if (database != "")
                {
                    consb.Database = "";

                    using (var conn = new MySqlConnection(consb.ConnectionString))
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        string dbName = QueryExpress.ExecuteScalarStr(cmd, $"show databases like '{QueryExpress.EscapeIdentifier(database)}';");
                        if (dbName != database)
                        {
                            cmd.CommandText = $"create database if not exists `{QueryExpress.EscapeIdentifier(database)}`";
                            cmd.ExecuteNonQuery();
                            ((masterPage1)this.Master).WriteTopMessageBar($"New database created: {database}", true);
                        }
                    }
                }

                string timenow = "";

                using (var conn = config.GetNewConnection())
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "select now();";
                        timenow = cmd.ExecuteScalar() + "";
                    }
                }

                ((masterPage1)this.Master).ShowMessage("Ok", "Connection Success", true);
                ((masterPage1)this.Master).WriteTopMessageBar($"Connection string saved and the connection test is success. {timenow}", true);
            }
            catch (Exception ex)
            {
                ((masterPage1)this.Master).ShowMessage("Error", $"Connection Failed. Error: {ex.Message}", false);
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

        protected void btBackupMemory_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = $"backup-{DateTime.Now:yyyy-MM-dd_HHmmss}.sql";

                using (var conn = config.GetNewConnection())
                using (var cmd = conn.CreateCommand())
                using (var mb = new MySqlBackup(cmd))
                {
                    conn.Open();

                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{filename}\"");
                    Response.BufferOutput = false; // Important for streaming

                    mb.ExportToStream(Response.OutputStream);

                    conn.Close();

                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                phBackup.Controls.Add(new LiteralControl($"<pre style='color: maroon;'>Task Failed: {ex.Message}</pre>"));
                ((masterPage1)this.Master).ShowMessage("Error", "Backup Failed", false);
            }
        }

        void RestoreInMemory()
        {
            try
            {
                using (var ms = new MemoryStream(fileUploadSql.FileBytes))
                using (var conn = config.GetNewConnection())
                using (var cmd = conn.CreateCommand())
                using (var mb = new MySqlBackup(cmd))
                {
                    conn.Open();
                    mb.ImportFromMemoryStream(ms);
                }

                string result = $@"<span style='color: darkgreen;'>Restore Success!<br /><br />File processed in memory</span>";
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