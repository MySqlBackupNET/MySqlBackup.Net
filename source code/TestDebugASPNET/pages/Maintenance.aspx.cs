using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.pages
{
    public partial class Maintenance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btDeleteTestDatabase_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendLine("The following databases were deleted...");
            sb.AppendLine();

            using (MySqlConnection conn = config.GetNewConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SHOW DATABASES LIKE 'test%';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        List<string> databases = new List<string>();
                        while (reader.Read())
                        {
                            databases.Add(reader.GetString(0));
                        }

                        reader.Close();

                        foreach (var dbName in databases)
                        {
                            using (var dropCmd = conn.CreateCommand())
                            {
                                dropCmd.CommandText = $"DROP DATABASE `{dbName}`;";
                                dropCmd.ExecuteNonQuery();
                                sb.AppendLine(dbName);
                            }
                        }
                    }
                }
            }

            ph1.Controls.Add(new LiteralControl(sb.ToString()));
        }

        protected void btDeleteTempDumpFiles_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("The following files were deleted:");
            sb.AppendLine();

            string folder = Server.MapPath("~/temp");
            System.IO.Directory.CreateDirectory(folder);

            foreach (var file in Directory.EnumerateFiles(folder))
            {
                try
                {
                    File.Delete(file);
                    sb.AppendLine(file);
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"Failed to delete: {file}");
                    sb.AppendLine(ex.Message);
                }
            }

            ph1.Controls.Add(new LiteralControl(sb.ToString()));
        }
    }
}