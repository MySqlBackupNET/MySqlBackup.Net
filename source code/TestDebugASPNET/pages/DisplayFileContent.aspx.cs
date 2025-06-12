using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.pages
{
    public partial class DisplayFileContent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Request["id"], out int id);
            string text = EngineSQLite.GetRecordFileContent(id);
            Response.Clear();
            Response.ContentType = "text/plain; charset=utf-8";
            Response.Write(text);
        }
    }
}