using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.Json;

namespace System.pages
{
    public partial class apiBenchmark : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (int.TryParse(Request["id"] + "", out int id))
                {
                    if (BenchmarkTest.dicProgress.ContainsKey(id))
                    {
                        var benchmarkReport = BenchmarkTest.dicProgress[id];

                        string json = JsonSerializer.Serialize(benchmarkReport);

                        Response.Clear();
                        Response.ContentType = "application/json";
                        Response.Write(json);
                    }
                }
                else
                {
                    Response.Clear();
                    Response.ContentType = "application/json";
                    Response.Write("{ \"Error\": \"Unknown task id\" }");
                }
            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write("{ \"Error\": \"" + ex.Message + "\" }");
            }
        }
    }
}