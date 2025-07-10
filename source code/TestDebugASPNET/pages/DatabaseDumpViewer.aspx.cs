using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.pages
{
    public partial class DatabaseDumpViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadData(false);
            }
        }

        void LoadData(bool showError)
        {
            try
            {
                string output = "";

                using (var conn = config.GetNewConnection())
                using (var cmd = conn.CreateCommand())
                using (var mb = new MySqlBackup(cmd))
                {
                    if (string.IsNullOrEmpty(conn.Database))
                    {
                        throw new Exception("Database is not defined");
                    }

                    conn.Open();
                    mb.ExportInfo.ExportRows = false;
                    output = mb.ExportToString();
                }

                ltOutput.Text = output;
            }
            catch (Exception ex)
            {
                if (showError)
                {
                    ltOutput.Text = $"Error: {ex.Message}";
                    ((masterPage1)this.Master).ShowMessage("Error", ex.Message, false);
                }
            }
        }

        protected void btView_Click(object sender, EventArgs e)
        {
            LoadData(true);
        }
    }
}