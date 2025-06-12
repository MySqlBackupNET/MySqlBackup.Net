<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="System.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="main-content">
        This is a lightweight debugging tool designed exclusively for testing MySqlBackup.NET functionality during development. This project facilitates rapid testing of backup operations and error handling.

        <br />
        <br />


        MySQL Connection String: 
        <br />
        <br />
        <asp:TextBox ID="txtConnStr" runat="server" Width="800px"></asp:TextBox>
        <br />
        <br />

        <asp:Button ID="btSaveConnStr" runat="server" Text="Save and Test Connection" OnClick="btSaveConnStr_Click" />
        <asp:Button ID="btCreateSampleData" runat="server" Text="Create Sample Data" OnClick="btCreateSampleData_Click" />

        <br />
        <br />

        <h2>Basic Tests</h2>

        <table style="border-collapse: collapse;">
            <tr>
                <td style="vertical-align: top; padding: 0;">

                    <div style="border: 1px solid #7d6bbb; width: 400px;">
                        <div style="background: #7d6bbb; padding: 10px; color: white;">
                            BACKUP
                        </div>
                        <div style="padding: 10px; line-height: 320%;">

                            <asp:Button ID="btRunBackup" runat="server" Text="Run Backup" OnClick="btRunBackup_Click" />
                            <asp:Button ID="btGetDatabaseInfo" runat="server" Text="Refresh Database Info" OnClick="btGetDatabaseInfo_Click" />
                            <br />
                            <asp:CheckBox ID="cbAddDropDatabase" runat="server" />
                            Add Drop Database<br />

                            <asp:CheckBox ID="cbAddCreateDatabase" runat="server" />
                            Add Create Database<br />

                            <asp:CheckBox ID="cbAddDropTable" runat="server" Checked="true" />
                            Add Drop Table<br />

                            <asp:CheckBox ID="cbExportTableStructure" runat="server" Checked="true" />
                            Export Table Structure<br />

                            <asp:CheckBox ID="cbExportRows" runat="server" Checked="true" />
                            Export Rows<br />

                            <asp:CheckBox ID="cbExportProcedures" runat="server" Checked="true" />
                            Export Procedures<br />

                            <asp:CheckBox ID="cbExportFunctions" runat="server" Checked="true" />
                            Export Functions<br />

                            <asp:CheckBox ID="cbExportTriggers" runat="server" Checked="true" />
                            Export Triggers<br />

                            <asp:CheckBox ID="cbExportViews" runat="server" Checked="true" />
                            Export Views<br />

                            <asp:CheckBox ID="cbExportRoutinesWithoutDefiner" runat="server" Checked="true" />
                            Export Routines Without Definer<br />

                            <asp:CheckBox ID="cbResetAutoIncrement" runat="server" />
                            Reset Auto-Increment<br />

                            Script Delimiter:
                            <asp:TextBox ID="txtScriptDelimiter" runat="server" Width="60px"></asp:TextBox>
                            <br />

                            <asp:CheckBox ID="cbWrapWithinTransaction" runat="server" Checked="false" />
                            Wrap Within Transaction<br />

                            <asp:CheckBox ID="cbEnableComments" runat="server" Checked="true" />
                            Enable Comments<br />

                            <asp:CheckBox ID="cbInsertLineBreakBetweenInserts" runat="server" Checked="false" />
                            Insert Line Break Between Inserts<br />
                            (false = faster import process)<br />

                            Max SQL Length:
                            <asp:TextBox ID="txtMaxSqlLength" runat="server" TextMode="Number" Width="100px"></asp:TextBox>
                            <br />

                            Rows Export Mode:
                            <asp:DropDownList ID="dropRowsExportMode" runat="server">
                                <asp:ListItem Value="1">Insert</asp:ListItem>
                                <asp:ListItem Value="2">Insert Ignore</asp:ListItem>
                                <asp:ListItem Value="3">Replace</asp:ListItem>
                                <asp:ListItem Value="4">On Duplicate Key Update</asp:ListItem>
                                <asp:ListItem Value="5">Update</asp:ListItem>
                            </asp:DropDownList>
                            <br />

                            Get Total Rows Mode:
                            <asp:DropDownList ID="dropGetTotalRowsMode" runat="server">
                                <asp:ListItem Value="1">Skip (Fastest)</asp:ListItem>
                                <asp:ListItem Value="2">Information Schema (Fast, but inaccurate)</asp:ListItem>
                                <asp:ListItem Value="3">Select Count (Slow, but accurate)</asp:ListItem>
                            </asp:DropDownList>
                            <br />

                            Document Headers:<br />
                            <asp:TextBox ID="txtDocumentHeaders" runat="server" TextMode="MultiLine" Height="170px" Width="360px"></asp:TextBox>
                            <br />

                            Document Footers:<br />
                            <asp:TextBox ID="txtDocumentFooters" runat="server" TextMode="MultiLine" Height="170px" Width="360px"></asp:TextBox>
                            <br />

                            Include Tables:<br />
                            <asp:CheckBoxList ID="cbListIncludeTables" runat="server"></asp:CheckBoxList>
                            <br />
                            <br />

                            Exclude Tables:<br />
                            <asp:CheckBoxList ID="cbListExcludeTables" runat="server"></asp:CheckBoxList>
                            <br />
                            <br />

                            Exclude Rows For Tables:<br />
                            <asp:CheckBoxList ID="cbListExcludeRowsForTables" runat="server"></asp:CheckBoxList>

                        </div>
                    </div>


                </td>
                <td style="width: 20px;"></td>
                <td style="vertical-align: top; padding: 0;">

                    <div style="border: 1px solid #b36bbb; width: 400px;">
                        <div style="background: #b36bbb; padding: 10px; color: white;">
                            RESTORE
                        </div>
                        <div style="padding: 10px; line-height: 320%;">

                            <asp:Button ID="btRunRestore" runat="server" Text="Run Restore" OnClick="btRunRestore_Click" />

                            Upload File:
                                <input type="file" id="fileRestore" name="fileRestore" />
                            <br />

                            <asp:CheckBox ID="cbIgnoreSqlError" runat="server" />
                            Ignore SQL Error
                            <br />

                            <asp:PlaceHolder ID="phOutputLog" runat="server"></asp:PlaceHolder>

                        </div>
                    </div>

                </td>
            </tr>
        </table>
    </div>

</asp:Content>
