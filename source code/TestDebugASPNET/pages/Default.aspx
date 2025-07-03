<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="System.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

        .maintb {
            border-collapse: collapse;
        }

            .maintb td {
                padding: 10px;
                vertical-align: top;
                border: 1px solid #adadad;
            }

            .maintb tr {
                height: 180px;
            }

            .maintb a {
                text-decoration: none;
                padding: 5px;
                border-radius: 5px;
                color: #1e39ba;
                font-weight: bold;
                transition: all linear 0.2s;
                border: 1px solid white;
            }

                .maintb a:hover {
                    background: #d8d8d8;
                    box-shadow: 3px 3px 3px #c7c7c7;
                    border: 1px solid #939393;
                }

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

        <h1>Setup and Simple Basic Demo</h1>

        This is a lightweight debugging tool designed exclusively for testing MySqlBackup.NET functionality during development. This project facilitates rapid testing of backup operations and error handling.<br />
        <br />

        MySQL Connection String: 
        <br />
        <br />
        <asp:TextBox ID="txtConnStr" runat="server" Width="900px" placeholder="server=localhost;user=root;pwd=;convertzerodatetime=true;treattinyasboolean=true;database=;"></asp:TextBox><br />
        *This connection string will be used for all database operations in this app.<br />
        *If the database that specified in the connection string is not existed, upon saving the connection, it will be created.
        <br />
        <br />

        <asp:Button ID="btSaveConnStr" runat="server" Text="Save and Test Connection" OnClick="btSaveConnStr_Click" />

        <br />
        <br />

        <table class="maintb">
            <tr>
                <td>
                    <asp:Button ID="btBackup" runat="server" OnClick="btBackup_Click" Text="Backup MySQL" OnClientClick="showBigLoading(3000);" /><br /><br />
                    <asp:Button ID="btBackupMemory" runat="server" OnClick="btBackupMemory_Click" Text="Backup in Memory" OnClientClick="showBigLoading(3000);" />
                </td>
                <td style="width: 550px;">
                    <asp:PlaceHolder ID="phBackup" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btRestore" runat="server" OnClick="btRestore_Click" Text="Restore MySQL" OnClientClick="showBigLoading(0);" /><br /><br />
                    <button type="button" onclick="restoreInMemory();">Restore in Memory</button>
                </td>
                <td>Select SQL Dump File (Any text file, no ZIP file):
                    <asp:FileUpload ID="fileUploadSql" runat="server" /><br />
                    <br />
                    <asp:PlaceHolder ID="phRestore" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
        </table>

        <h2>Quick and Simple Code Demo: MySQL Backup and Restore</h2>

        <div class="code-container">
            <div class="code-header">
                C#
            </div>
            <pre><code class="language-csharp">string constr = "server=localhost;user=root;pwd=1234;database=test1;convertzerodatetime=true;";
string filePath = @"C:\backup.sql";

// Backup , Export
using (var conn = new MySqlConnection(constr))
using (var cmd = conn.CreateCommand())
using (var mb = new MySqlBackup(cmd))
{
    conn.Open();
    mb.ExportToFile(filePath);
}

// Restore , Import
using (var conn = new MySqlConnection(constr))
using (var cmd = conn.CreateCommand())
using (var mb = new MySqlBackup(cmd))
{
    conn.Open();
    mb.ImportFromFile(filePath);
}</code></pre>
        </div>
    </div>

    <input type="hidden" id="postbackAction" name="postbackAction" />

    <script>
        hljs.highlightAll();
    </script>

    <script>
        function restoreInMemory() {

            spShowConfirmDialog(
                "Restore in Memory",
                "Are you sure to restore?",
                "",
                () => {
                    // Yes
                    showBigLoading(0);
                    document.querySelector("#postbackAction").value = "restore-memory";
                    document.querySelector('form').submit();
                },
                () => {
                    // No, do nothing
                }
            );
        }
    </script>
</asp:Content>
