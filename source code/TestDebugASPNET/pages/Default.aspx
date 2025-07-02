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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">

        <h1>Setup and Simple Basic Demo</h1>

        This is a lightweight debugging tool designed exclusively for testing MySqlBackup.NET functionality during development. This project facilitates rapid testing of backup operations and error handling.<br />
        <br />

        MySQL Connection String: 
        <br />
        <br />
        <asp:TextBox ID="txtConnStr" runat="server" Width="800px"></asp:TextBox>
        <br />
        <br />

        <asp:Button ID="btSaveConnStr" runat="server" Text="Save and Test Connection" OnClick="btSaveConnStr_Click" />

        <br />
        <br />

        <table class="maintb">
            <tr>
                <td>
                    <asp:Button ID="btBackup" runat="server" OnClick="btBackup_Click" Text="Backup MySQL" OnClientClick="showBigLoading(0);" />
                </td>
                <td style="width: 550px;">
                    <asp:PlaceHolder ID="phBackup" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btRestore" runat="server" OnClick="btRestore_Click" Text="Restore MySQL" OnClientClick="showBigLoading(0);" />
                </td>
                <td>Select SQL Dump File (Any text file, no ZIP file):
                    <asp:FileUpload ID="fileUploadSql" runat="server" /><br />
                    <br />
                    <asp:PlaceHolder ID="phRestore" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
        </table>

    </div>

</asp:Content>
