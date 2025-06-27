<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="BackupRestoreOptions.aspx.cs" Inherits="System.pages.BackupRestoreOptions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        
        body {
            background: #dcdcdc;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">

        <h1>Backup and Restore MySQL With Options</h1>

        This is a lightweight debugging tool designed exclusively for testing MySqlBackup.NET functionality during development. This project facilitates rapid testing of backup operations and error handling.<br />
        <br />

        MySQL Connection String: 
    <br />
        <br />
        <asp:TextBox ID="txtConnStr" runat="server" Width="800px"></asp:TextBox>
        <br />
        <br />

        <asp:Button ID="btSaveConnStr" runat="server" Text="Save and Test Connection" OnClick="btSaveConnStr_Click" />
        <asp:Button ID="btCreateSampleData" runat="server" Text="Create Sample Data" OnClick="btCreateSampleData_Click" />
        <a href="/DatabaseRecordList" class="buttonmain">View Backup File List</a>

        <br />
        <br />

        *[Run Backup Async] and [Run Restore Async] will create a record at [<a href="/DatabaseRecordList">Database Record</a>]

        <h2>Basic Tests</h2>

        <table style="border-collapse: collapse;">
            <tr>
                <td style="vertical-align: top; padding: 0;">

                    <div style="border: 1px solid #7d6bbb; width: 500px;">
                        <div style="background: #7d6bbb; padding: 10px; color: white;">
                            BACKUP
                        </div>
                        <div style="padding: 10px; line-height: 320%;">

                            <asp:Button ID="btRunBackup" runat="server" Text="Run Simple Backup" OnClick="btRunBackup_Click" OnClientClick="showBackupLoading(0);" />
                            <asp:Button ID="btRunBackupAsync" runat="server" Text="Run Backup Async (Progress Report)" OnClick="btRunBackupAsync_Click" />
                            <asp:Button ID="btGetDatabaseInfo" runat="server" Text="Refresh Info" OnClick="btGetDatabaseInfo_Click" />
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

                            <asp:CheckBox ID="cbEnableLockTablesWrite" runat="server" />
                            Enable Lock Tables Write<br />

                            <asp:CheckBox ID="cbWrapWithinTransaction" runat="server" Checked="false" />
                            Wrap Within Transaction<br />

                            <asp:CheckBox ID="cbEnableComments" runat="server" Checked="true" />
                            Enable Comments<br />

                            <asp:CheckBox ID="cbRecordDumpTime" runat="server" Checked="true" />
                            Record Dump Time
                        <br />

                            <asp:CheckBox ID="cbInsertLineBreakBetweenInserts" runat="server" Checked="false" />
                            Insert Line Break Between Inserts<br />
                            (false = faster import process)<br />

                            Max SQL Length:
                        <asp:TextBox ID="txtMaxSqlLength" runat="server" TextMode="Number" Width="100px" Text="16777216" oninput="showSqlLength();" ClientIDMode="static"></asp:TextBox>
                            <span id="spanSqlLength"></span>
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
                            <asp:ListItem Value="3">Select Count</asp:ListItem>
                            <asp:ListItem Value="1">Skip</asp:ListItem>
                            <asp:ListItem Value="2">Information Schema</asp:ListItem>
                        </asp:DropDownList>
                            <br />
                            <div style="border-radius: 10px; background: #d9d9d9; line-height: 150%; font-size: 8pt; font-style: italic; padding: 10px;">
                                *Skip = If you are not doing progress report, don't obtain total rows<br />
                                *Select Count = Accurate, but slow<br />
                                *Information Schema = Fast, but inaccurate, estimation only<br />
                            </div>

                            Document Headers:<br />
                            <asp:TextBox ID="txtDocumentHeaders" runat="server" TextMode="MultiLine" Height="170px" Width="460px"></asp:TextBox>
                            <br />

                            Document Footers:<br />
                            <asp:TextBox ID="txtDocumentFooters" runat="server" TextMode="MultiLine" Height="170px" Width="460px"></asp:TextBox>
                            <br />

                            Include Tables (White List): 
                        <button type="button" onclick="clearCheckBox('cbListIncludeTables')">Clear</button>
                            <div style="max-height: 250px; overflow-x: scroll; border: 1px solid #969696;">
                                <asp:CheckBoxList ID="cbListIncludeTables" runat="server" CssClass="div-checklist-box" ClientIDMode="Static"></asp:CheckBoxList>
                            </div>

                            Exclude Tables (Black List):
                        
                        <button type="button" onclick="clearCheckBox('cbListExcludeTables')">Clear</button>
                            <div style="max-height: 250px; overflow-x: scroll; border: 1px solid #969696;">
                                <asp:CheckBoxList ID="cbListExcludeTables" runat="server" CssClass="div-checklist-box" ClientIDMode="Static"></asp:CheckBoxList>
                            </div>

                            Exclude Rows For Tables:
                        <button type="button" onclick="clearCheckBox('cbListExcludeRowsForTables')">Clear</button><br />
                            <div style="max-height: 250px; overflow-x: scroll; border: 1px solid #969696;">
                                <asp:CheckBoxList ID="cbListExcludeRowsForTables" runat="server" CssClass="div-checklist-box" ClientIDMode="Static"></asp:CheckBoxList>
                            </div>

                        </div>
                    </div>


                </td>
                <td style="width: 20px;"></td>
                <td style="vertical-align: top; padding: 0;">

                    <div style="border: 1px solid #b36bbb; width: 500px;">
                        <div style="background: #b36bbb; padding: 10px; color: white;">
                            RESTORE
                        </div>
                        <div style="padding: 10px; line-height: 320%;">

                            <asp:Button ID="btRunRestore" runat="server" Text="Run Simple Restore" OnClick="btRunRestore_Click" OnClientClick="return showRestoreLoading();" />
                            <a href="/ReportProgressRestoreFileUpload" class="buttonmain">Run Restore Async (Progress Report)</a>

                            <br />
                            *If you want to upload zip file, use [Run Restore Async]
                        <br />

                            Upload File:
                        <asp:FileUpload ID="fileUploadRestore" runat="server" ClientIDMode="Static" onchange="checkFile();" accept=".sql" />
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

    <script type="text/javascript">

        const txtInput = document.getElementById('txtMaxSqlLength');
        const spanOutput = document.getElementById('spanSqlLength');

        function clearCheckBox(divid) {
            document.querySelectorAll(`#${divid} input[type="checkbox"]`).forEach(cb => cb.checked = false);
        }

        function showBackupLoading() {
            showBigLoading(0);
        }

        function checkFile() {
            var fileInput = document.getElementById('fileUploadRestore');

            if (!fileInput.files || fileInput.files.length === 0) {
                return false;
            }

            var fileName = fileInput.files[0].name;
            if (!fileName.toLowerCase().endsWith('.sql')) {
                showMessage("Incorrect file type", "Supports only SQL (*.sql) file. If you want to upload zip file, use [Run Restore Async]", false);
                fileInput.value = null;
                return false;
            }

            return true;
        }

        function showRestoreLoading() {
            var fileInput = document.getElementById('fileUploadRestore');

            if (!fileInput.files || fileInput.files.length === 0) {
                showMessage("No file", "Please select a SQL dump file (*.sql) to continue.", false);
                return false;
            }

            if (!checkFile())
                return false;

            showBigLoading(0);
            return true;
        }

        function showSqlLength() {
            const bytes = parseFloat(txtInput.value);
            if (!isNaN(bytes)) {
                const bytesInAMB = 1024 * 1024;
                const megabytes = bytes / bytesInAMB;
                const formattedMegabytes = megabytes.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 3 });
                spanOutput.textContent = `(${formattedMegabytes} MB)`;
            }
            else {
                spanOutput.textContent = '';
            }
        }

        showSqlLength();

    </script>
</asp:Content>
