using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.Json;

namespace System.pages
{
    public partial class apiParallelPipelineExport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (int.TryParse(Request["taskid"] + "", out int taskid))
            {
                if (int.TryParse(Request["apicallid"] + "", out int apicallid))
                {
                    if (ParallelPipelineExport.dicParallelExportTask.TryGetValue(taskid, out ParallelExportInfo p))
                    {
                        string action = Request["action"] + "";

                        if (action == "cancel")
                        {
                            p.request_cancel = true;
                        }

                        p.api_call_id = apicallid;

                        string json = JsonSerializer.Serialize(p);
                        Response.Write(json);
                    }
                }
            }
        }
    }
}