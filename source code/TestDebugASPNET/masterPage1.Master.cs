using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System
{
    public partial class masterPage1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void WriteMessageBar(string message, bool isGoodMsg)
        {
            string css = isGoodMsg ? "divGoodMsgBar" : "divErrorMsgBar";

            string htmlMsgBar = $"<div class='{css}'>{message}</div>";

            phMessageBar.Controls.Add(new LiteralControl(htmlMsgBar));
        }

        public void ShowGoodMessage(string message)
        {
            ShowMessage("Ok", message, true);
        }

        public void ShowErrorMessage(string message)
        {
            ShowMessage("Error", message, false);
            WriteMessageBar(message, false);
        }

        public void ShowMessage(string title, string msg, bool isGoodMsg)
        {
            string escapedTitle = title.Replace("'", "\\'");
            string escapedMsg = msg.Replace("'", "\\'");
            string isGood = isGoodMsg ? "true" : "false";

            string s = $"<script>showMessage('{escapedTitle}', '{escapedMsg}', {isGood});</script>";

            phBottomScript.Controls.Add(new LiteralControl(s));
        }
    }
}