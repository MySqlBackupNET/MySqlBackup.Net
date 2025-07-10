using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.pages
{
    public partial class apiMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = (Request["action"] + "").ToLower();

            switch (action)
            {
                case "checkserverstatus":
                    CheckServerStatus();
                    break;
                case "startserver":
                    StartMySqlServer();
                    break;
                case "stopserver":
                    StopMySqlServer();
                    break;
                case "saveinstancepath":
                    SaveMySqlInstancePath();
                    break;
                case "saveconnectionstring":
                    SaveConnectionString();
                    break;
                case "backupsimple":
                    BackupSimple();
                    break;
                case "backupmemory":
                    BackupMemory();
                    break;
                case "restoresimple":
                    RestoreSimple();
                    break;
                case "restorememory":
                    RestoreMemory();
                    break;

            }
        }

        void CheckServerStatus()
        {
            bool isRunning = config.CheckMySqlServerRunningState();

            Response.Clear();

            if (isRunning)
            {
                Response.Write("1");
            }
            else
            {
                Response.Write("0");
            }
        }

        void StartMySqlServer()
        {
            Response.Clear();

            try
            {
                config.GetMySqlInstancePath(out string mysqld_path, out string mysqladmin_path);

                if (string.IsNullOrEmpty(mysqld_path))
                {
                    Response.Write("0|mysqld.exe file path is not defined");
                    return;
                }

                if (!File.Exists(mysqld_path))
                {
                    Response.Write("0|Unable to locate mysqld.exe");
                    return;
                }

                if (config.CheckMySqlServerRunningState())
                {
                    Response.Write("2"); // already on
                    return;
                }

                config.StartMySqlServer();

                for (int checkAttemp = 0; checkAttemp < 3; checkAttemp++)
                {
                    System.Threading.Thread.Sleep(500);

                    if (config.CheckMySqlServerRunningState())
                    {
                        Response.Write("1"); // successfully on
                        return;
                    }
                }

                Response.Write("0|Unable to start MySQL server");
            }
            catch (Exception ex)
            {
                Response.Write("0|" + ex.Message);
            }
        }

        void StopMySqlServer()
        {
            Response.Clear();

            try
            {
                config.GetMySqlInstancePath(out string mysqld_path, out string mysqladmin_path);

                if (string.IsNullOrEmpty(mysqld_path))
                {
                    Response.Write("0|mysqladmin.exe file path is not defined");
                    return;
                }

                if (!File.Exists(mysqld_path))
                {
                    Response.Write("0|Unable to locate mysqladmin.exe");
                    return;
                }

                if (!config.CheckMySqlServerRunningState())
                {
                    Response.Write("2"); // already off
                    return;
                }

                config.StopMySqlServer();

                for (int checkAttempt = 0; checkAttempt < 3; checkAttempt++)
                {
                    System.Threading.Thread.Sleep(500);

                    bool isRunning = config.CheckMySqlServerRunningState();

                    if (!isRunning)
                    {
                        Response.Write("1"); // successfully turned off
                        return;
                    }
                }

                Response.Write("0|Unable to stop MySQL server");
            }
            catch (Exception ex)
            {
                Response.Write("0|" + ex.Message);
            }
        }

        void SaveMySqlInstancePath()
        {
            string mysqldpath = Request.Form["mysqldpath"] + "";
            string mysqladminpath = Request.Form["mysqladminpath"] + "";

            if (string.IsNullOrEmpty(mysqldpath))
            {
                Response.Write("0|mysqld.exe file path is not defined");
                return;
            }

            if (!File.Exists(mysqldpath))
            {
                Response.Write("0|Unable to locate mysqld.exe");
                return;
            }

            if (string.IsNullOrEmpty(mysqladminpath))
            {
                Response.Write("0|mysqladmin.exe file path is not defined");
                return;
            }

            if (!File.Exists(mysqladminpath))
            {
                Response.Write("0|Unable to locate mysqladmin.exe");
                return;
            }

            config.SaveMysqlInstancePath(mysqldpath, mysqladminpath);

            Response.Clear();
            Response.Write("1");
        }

        void SaveConnectionString()
        {
            Response.Clear();

            try
            {
                string connStr = Request.Form["connectionString"] + "";

                if (string.IsNullOrEmpty(connStr))
                {
                    Response.Write("0|Connection string cannot be empty");
                    return;
                }

                config.SaveConnStr(connStr);

                MySqlConnectionStringBuilder consb = new MySqlConnectionStringBuilder(connStr);
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
                        }
                    }
                }

                // Test connection
                using (var conn = config.GetNewConnection())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "select now();";
                    string timenow = cmd.ExecuteScalar() + "";
                }

                Response.Write("1");
            }
            catch (Exception ex)
            {
                Response.Write("0|" + ex.Message);
            }
        }

        void BackupSimple()
        {
            Response.Clear();

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
                    Response.Write("0|No database selected");
                    return;
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

                Response.Write("1|" + filename); // Return filename for download link
            }
            catch (Exception ex)
            {
                Response.Write("0|" + ex.Message);
            }
        }

        void BackupMemory()
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
                    Response.BufferOutput = false;

                    mb.ExportToStream(Response.OutputStream);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.ContentType = "text/plain";
                Response.StatusCode = 500; // // This makes response.ok = false
                Response.Write("0|" + ex.Message);
            }
        }

        void RestoreSimple()
        {
            Response.Clear();

            try
            {
                if (Request.Files.Count == 0)
                {
                    Response.Write("0|No file uploaded");
                    return;
                }

                HttpPostedFile uploadedFile = Request.Files[0];

                if (uploadedFile == null || uploadedFile.ContentLength == 0)
                {
                    Response.Write("0|No file uploaded");
                    return;
                }

                if (uploadedFile.FileName.ToLower().EndsWith(".zip"))
                {
                    Response.Write("0|Zip file is not supported");
                    return;
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
                    Response.Write("0|No database selected");
                    return;
                }

                filename = $"{databaseName}_(restore)_{DateTime.Now:yyyy-MM-dd_hhmmss}.txt";
                filePath = Path.Combine(folder, filename);
                uploadedFile.SaveAs(filePath);

                using (var conn = config.GetNewConnection())
                using (var cmd = conn.CreateCommand())
                using (var mb = new MySqlBackup(cmd))
                {
                    conn.Open();
                    mb.ImportFromFile(filePath);
                }

                Response.Write("1|" + filename); // Return filename for download link
            }
            catch (Exception ex)
            {
                Response.Write("0|" + ex.Message);
            }
        }

        void RestoreMemory()
        {
            Response.Clear();

            try
            {
                if (Request.Files.Count == 0)
                {
                    Response.Write("0|No file uploaded");
                    return;
                }

                HttpPostedFile uploadedFile = Request.Files[0];

                if (uploadedFile == null || uploadedFile.ContentLength == 0)
                {
                    Response.Write("0|No file uploaded");
                    return;
                }

                using (var ms = new MemoryStream())
                {
                    uploadedFile.InputStream.CopyTo(ms);
                    ms.Position = 0;

                    using (var conn = config.GetNewConnection())
                    using (var cmd = conn.CreateCommand())
                    using (var mb = new MySqlBackup(cmd))
                    {
                        conn.Open();
                        mb.ImportFromMemoryStream(ms);
                    }
                }

                Response.Write("1");
            }
            catch (Exception ex)
            {
                Response.Write("0|" + ex.Message);
            }
        }
    }
}