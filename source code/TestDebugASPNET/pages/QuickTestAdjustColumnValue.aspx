<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="QuickTestAdjustColumnValue.aspx.cs" Inherits="System.pages.QuickTestAdjustColumnValue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .code-container {
            background-color: #1e1e1e;
            border: 1px solid #3c3c3c;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.3);
        }

        .code-header {
            background-color: #2d2d30;
            padding: 10px 15px;
            border-bottom: 1px solid #3c3c3c;
            font-size: 14px;
            color: #cccccc;
            display: flex;
            align-items: center;
        }

            .code-header::before {
                content: "📄";
                margin-right: 8px;
            }

        pre code, pre code span {
            font-size: 10pt;
            line-height: 1.5;
        }
    </style>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.9.0/styles/vs2015.min.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.9.0/highlight.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">

        <h1>Adjust Column Value</h1>

        <asp:Button ID="btRun" runat="server" ClientIDMode="Static" Text="Run Test" OnClick="btRun_Click" OnClientClick="showBigLoading(0); hideButton(this);" />
        <asp:CheckBox ID="cbNoTryCatch" runat="server" />
        Run Without Try Catch
        <asp:CheckBox ID="cbCleanDatabaseAfterUse" runat="server" Checked="true" />
        Clean Up Database After Use 
        
        <br />
        Rows Export Mode:
        <asp:CheckBox ID="cbInsert" runat="server" Checked="true" />
        Insert
        <asp:CheckBox ID="cbInsertIgnore" runat="server" Checked="false" />
        Insert Ignore
        <asp:CheckBox ID="cbReplace" runat="server" Checked="false" />
        Replace
        <asp:CheckBox ID="cbUpdate" runat="server" Checked="false" />
        Update
        <asp:CheckBox ID="cbInsertUpdate" runat="server" Checked="false" />
        Insert Update
        <br />
        <br />

        <asp:Panel ID="panelDemoGuide" runat="server">

            <h2>Quick and Simple Code Demo: MySQL Backup and Restore"</h2>

            <div class="code-container">
                <div class="code-header">
                    C#
                </div>
                <pre><code class="language-csharp">string constr = "server=localhost;user=root;pwd=1234;database=test1;convertzerodatetime=true;";
string filePath = @"C:\backup.sql";

// Backup with column value adjustment
using (var conn = new MySqlConnection(constr))
using (var cmd = conn.CreateCommand())
using (var mb = new MySqlBackup(cmd))
{
    conn.Open();
    
    // Add column value adjustment to mask the 'name' column
    mb.ExportInfo.AddTableColumnValueAdjustment("users", "name", MaskName);
    mb.ExportToFile(filePath);
}

// Masking function
object MaskName(object obInput)
{
    if (obInput == null) return null;
    string input = obInput.ToString();
    if (string.IsNullOrEmpty(input)) return "";
    
    // If input is less than 6 characters, mask everything with asterisks
    if (input.Length < 6)
    {
        return new string('*', input.Length);
    }
    
    // Return first 3 characters + asterisks + last 3 characters
    string first3 = input.Substring(0, 3);
    string last3 = input.Substring(input.Length - 3);
    return $"{first3}************{last3}";
}


/* 
BEFORE (original data in database):

INSERT INTO `users` (`id`, `name`, `email`) VALUES
(1, 'John Smith', 'john@email.com'),
(2, 'Jane Williams', 'jane@email.com'),
(3, 'Bob Johnson', 'bob@email.com');


AFTER (exported backup file with masked names):

INSERT INTO `users` (`id`, `name`, `email`) VALUES
(1, 'Joh************ith', 'john@email.com'),
(2, 'Jan************ams', 'jane@email.com'),
(3, 'Bob************son', 'bob@email.com');
*/</code></pre>
            </div>
    </div>

    <script>
            hljs.highlightAll();
    </script>

    </asp:Panel>
        <asp:Panel ID="panelResult" runat="server" Visible="false">
            <pre class="light-formatted"><asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder></pre>
        </asp:Panel>

    </div>

    <script type="text/javascript">
        function hideButton(bt) {
                   bt.style.display = "none";
               }
    </script>

</asp:Content>
