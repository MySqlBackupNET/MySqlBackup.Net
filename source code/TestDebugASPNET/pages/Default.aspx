<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="System.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        input[type=text] {
            background: #fffdd3
        }

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

        .mysqlstat {
            padding: 7px;
            text-align: center;
            border-radius: 15px;
        }

        .dots-flow {
            width: 150px;
            height: 30px;
            background-color: #e0e0e0;
            border-radius: 15px;
            position: relative;
            overflow: hidden;
        }

        .dot-runner {
            width: 20px;
            height: 20px;
            background: radial-gradient(circle, #4CAF50, #45a049);
            border-radius: 50%;
            position: absolute;
            top: 5px;
            animation: dot-run 1.5s linear infinite;
        }

        @keyframes dot-run {
            0% {
                left: -20px;
            }

            100% {
                left: 150px;
            }
        }
    </style>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.9.0/styles/vs2015.min.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.9.0/highlight.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <button type="submit" disabled style="display: none" aria-hidden="true"></button>

    <div class="div-center-framed-content">

        <table style="border-collapse: collapse; width: 100%;">
            <tr>
                <td>
                    <h1>Setup and Simple Basic Demo</h1>
                </td>
                <td style="width: 200px;">
                    <div id="divServerOnline" style="background: #c7ffbc; display: none;" class="mysqlstat">
                        MySQL Server Online
                    </div>
                    <div id="divServerOffline" style="background: #ffd2d2; display: none;" class="mysqlstat">
                        MySQL Server Offline
                    </div>
                    <div id="divServerCheckAnimation" class="dots-flow">
                        <div class="dot-runner"></div>
                    </div>
                </td>
            </tr>
        </table>


        This is a lightweight debugging tool designed exclusively for testing MySqlBackup.NET functionality during development. This project facilitates rapid testing of backup operations and error handling.
        
        <div style="height: 10px;"></div>

        <button type="button" onclick="saveConnectionString();">Save and Test MySQL Connection</button>
        MySQL Connection String:

        <div style="height: 10px;"></div>

        <asp:TextBox ID="txtConnStr" runat="server" Width="900px" ClientIDMode="Static"></asp:TextBox><br />
        *This connection string will be used for all database operations in this app.<br />
        *If the database that specified in the connection string is not existed, upon saving the connection, it will be created.
        
        <div style="height: 3px; background: #d3d3d3; margin: 20px 0;"></div>

        <button type="button" onclick="saveInstancePath();">Save MySQL Instance Path</button>
        <button type="button" onclick="startMySqlServer();">Start MySQL Server</button>
        <button type="button" onclick="stopMySqlServer();">Stop MySQL Server</button>
        <button type="button" onclick="checkMySqlStatus();">Check MySQL Server State</button>

        <div style="height: 10px;"></div>

        *The path to <strong>mysqld.exe</strong> and <strong>mysqladmin</strong>

        <table>
            <tr>
                <td>mysqld.exe</td>
                <td>
                    <asp:TextBox ID="txtMysqldExePath" runat="server" Width="770px" ClientIDMode="Static"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>mysqladmin.exe</td>
                <td>
                    <asp:TextBox ID="txtMySqlAdminExePath" runat="server" Width="770px" ClientIDMode="Static"></asp:TextBox>
                </td>
            </tr>
        </table>

        <div style="height: 10px;"></div>

        <h2>Basic Backup & Restore:</h2>

        <table class="maintb">
            <tr>
                <td>
                    <asp:Button ID="btBackup" runat="server" Text="Backup (ASP.NET Web Forms)" OnClick="btBackup_Click" OnClientClick="showBigLoading(0);" />
                    <div style="height: 7px;"></div>
                    <button type="button" onclick="backupSimple();">Backup (JS/Fetchapi)</button>
                    <div style="height: 7px;"></div>
                    <button type="button" onclick="backupMemory();">Backup (JS/Fetchapi/Memory)</button>
                </td>
                <td style="width: 550px;">
                    <div id="divBackupResult">
                        <asp:PlaceHolder ID="phBackup" runat="server"></asp:PlaceHolder>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btRestore" runat="server" Text="Restore (ASP.NET Web Forms)" OnClick="btRestore_Click" OnClientClick="showBigLoading(0);" />
                    <div style="height: 7px;"></div>
                    <button type="button" onclick="restoreSimple();">Restore (JS/Fetchapi)</button>
                    <div style="height: 7px;"></div>
                    <button type="button" onclick="restoreInMemory();">Restore (JS/Fetchapi/Memory)</button>
                </td>
                <td>Select SQL Dump File (Any text file, no ZIP file):
                    <asp:FileUpload ID="fileUploadSql" runat="server" ClientIDMode="Static" />
                    <div id="divRestoreResult">
                        <asp:PlaceHolder ID="phRestore" runat="server"></asp:PlaceHolder>
                    </div>
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

</script>

    <script>
        async function saveConnectionString() {
            try {
                showBigLoading(0);

                const connectionString = document.getElementById('txtConnStr').value;

                if (!connectionString.trim()) {
                    showErrorMessage("Validation Error", "Connection string cannot be empty");
                    closeBigLoading();
                    return;
                }

                const formData = new FormData();
                formData.append('connectionString', connectionString);

                const response = await fetch('/apiMain?action=saveconnectionstring', {
                    method: 'POST',
                    body: formData
                });

                const result = await response.text();
                closeBigLoading();

                if (result.trim() === '1') {
                    showGoodMessage("Success", "Connection string saved and tested successfully");
                } else {
                    const errorParts = result.split('|');
                    const errorMessage = errorParts.length > 1 ? errorParts[1] : 'Failed to save connection string';
                    showErrorMessage("Error", errorMessage);
                }
            } catch (error) {
                console.error('Error saving connection string:', error);
                showErrorMessage("Error", "Failed to save connection string: " + error.message);
                closeBigLoading();
            }
        }

        // Backup Simple
        async function backupSimple() {
            try {
                showBigLoading(3000);

                const response = await fetch('/apiMain?action=backupsimple', {
                    method: 'POST'
                });

                const result = await response.text();
                closeBigLoading();

                const divBackupResult = document.getElementById('divBackupResult');

                if (result.startsWith('1|')) {
                    const filename = result.substring(2);
                    const downloadLink = `/apiFiles?folder=temp&filename=${filename}`;

                    divBackupResult.innerHTML = `
                <span style='color: darkgreen;'>
                Backup Success!<br />
                <br />
                Download File: <a href='${downloadLink}'>${filename}</a><br />
                <br />
                File saved successfully</span>`;

                    showGoodMessage("Success", "Backup completed successfully!");
                } else {
                    const errorParts = result.split('|');
                    const errorMessage = errorParts.length > 1 ? errorParts[1] : 'Backup failed';

                    divBackupResult.innerHTML = `<pre style='color: maroon;'>Task Failed: ${errorMessage}</pre>`;
                    showErrorMessage("Error", errorMessage);
                }
            } catch (error) {
                console.error('Error during backup:', error);
                document.getElementById('divBackupResult').innerHTML = `<pre style='color: maroon;'>Task Failed: ${error.message}</pre>`;
                showErrorMessage("Error", "Backup failed: " + error.message);
                closeBigLoading();
            }
        }

        // Backup Memory (Direct Download)
        async function backupMemory() {
            try {
                showBigLoading(0);

                // First, check if the backup will succeed
                const response = await fetch('/apiMain?action=backupmemory', {
                    method: 'GET'
                });

                closeBigLoading();

                if (!response.ok) {
                    // If response is not ok, try to read error message
                    const errorText = await response.text();

                    const errorMsg = errorText.startsWith('0|') ? errorText.substring(2) : `HTTP ${response.status}`;
                    document.getElementById('divBackupResult').innerHTML = `<pre style='color: maroon;'>Task Failed: ${errorMsg}</pre>`;
                    showErrorMessage("Error", "Backup failed: " + errorMsg);
                    closeBigLoading();
                }
                else {
                    // Extract filename from Content-Disposition header
                    let filename = 'backup.sql'; // Fallback filename
                    const disposition = response.headers.get('Content-Disposition');
                    if (disposition && disposition.includes('attachment')) {
                        const matches = disposition.match(/filename="([^"]+)"/);
                        if (matches && matches[1]) {
                            filename = matches[1];
                        }
                    }

                    // If successful, trigger the download
                    const blob = await response.blob();
                    const url = window.URL.createObjectURL(blob);
                    const link = document.createElement('a');
                    link.href = url;
                    link.download = filename;
                    document.body.appendChild(link);
                    link.click();
                    document.body.removeChild(link);
                    window.URL.revokeObjectURL(url);

                    closeBigLoading();

                    // Show success message
                    const divBackupResult = document.getElementById('divBackupResult');
                    divBackupResult.innerHTML = `
                        <span style='color: darkgreen;'>
                            Backup Success!<br />
                            <br />
                            File downloaded directly to your browser
                        </span>`;
                    showGoodMessage("Success", "Backup download initiated");
                }
            } catch (error) {
                console.error('Error during backup:', error);
                document.getElementById('divBackupResult').innerHTML = `<pre style='color: maroon;'>Task Failed: ${error.message}</pre>`;
                showErrorMessage("Error", "Backup failed: " + error.message);
                closeBigLoading();
            }
        }

        // Restore Simple
        async function restoreSimple() {
            try {
                const fileInput = document.getElementById('fileUploadSql');

                if (!fileInput.files || fileInput.files.length === 0) {
                    showErrorMessage("Validation Error", "Please select a file to restore");
                    return;
                }

                const file = fileInput.files[0];

                if (file.name.toLowerCase().endsWith('.zip')) {
                    showErrorMessage("Error", "Zip files are not supported");
                    return;
                }

                showBigLoading(0);

                const formData = new FormData();
                formData.append('file', file);

                const response = await fetch('/apiMain?action=restoresimple', {
                    method: 'POST',
                    body: formData
                });

                const result = await response.text();
                closeBigLoading();

                const divRestoreResult = document.getElementById('divRestoreResult');

                if (result.startsWith('1|')) {
                    const filename = result.substring(2);
                    const downloadLink = `/apiFiles?folder=temp&filename=${filename}`;

                    divRestoreResult.innerHTML = `
                <span style='color: darkgreen;'>
                Restore Success!<br />
                <br />
                Download File: <a href='${downloadLink}'>${filename}</a><br />
                <br />
                File saved successfully</span>`;

                    showGoodMessage("Success", "Restore completed successfully!");
                } else {
                    const errorParts = result.split('|');
                    const errorMessage = errorParts.length > 1 ? errorParts[1] : 'Restore failed';

                    divRestoreResult.innerHTML = `<pre style='color: maroon;'>Task Failed: ${errorMessage}</pre>`;
                    showErrorMessage("Error", errorMessage);
                }
            } catch (error) {
                console.error('Error during restore:', error);
                document.getElementById('divRestoreResult').innerHTML = `<pre style='color: maroon;'>Task Failed: ${error.message}</pre>`;
                showErrorMessage("Error", "Restore failed: " + error.message);
                closeBigLoading();
            }
        }

        // Restore in Memory
        async function restoreInMemory() {
            try {
                const fileInput = document.getElementById('fileUploadSql');

                if (!fileInput.files || fileInput.files.length === 0) {
                    showErrorMessage("Validation Error", "Please select a file to restore");
                    return;
                }

                showBigLoading(0);

                const file = fileInput.files[0];
                const formData = new FormData();
                formData.append('file', file);

                const response = await fetch('/apiMain?action=restorememory', {
                    method: 'POST',
                    body: formData
                });

                const result = await response.text();
                closeBigLoading();

                const divRestoreResult = document.getElementById('divRestoreResult');

                if (result.trim() === '1') {
                    divRestoreResult.innerHTML = `
    <span style='color: darkgreen;'>
    Restore Success!<br />
    <br />
    File processed in memory</span>`;

                    showGoodMessage("Success", "Restore completed successfully!");
                } else {
                    const errorParts = result.split('|');
                    const errorMessage = errorParts.length > 1 ? errorParts[1] : 'Restore failed';

                    divRestoreResult.innerHTML = `<pre style='color: maroon;'>Task Failed: ${errorMessage}</pre>`;
                    showErrorMessage("Error", errorMessage);
                }
            } catch (error) {
                console.error('Error during restore:', error);
                document.getElementById('divRestoreResult').innerHTML = `<pre style='color: maroon;'>Task Failed: ${error.message}</pre>`;
                showErrorMessage("Error", "Restore failed: " + error.message);
                closeBigLoading();
            }
        }

        // Start MySQL Server
        async function startMySqlServer() {
            try {

                showBigLoading(0);

                const response = await fetch('/apiMain?action=startserver');
                const result = await response.text();
                closeBigLoading();

                if (result.trim() === '1') {
                    showGoodMessage("Success", "MySQL server started successfully");
                    // Update status display
                    document.getElementById('divServerOnline').style.display = 'block';
                    document.getElementById('divServerOffline').style.display = 'none';
                } else if (result.trim() === '2') {
                    showGoodMessage("Already Running", "MySQL server is already running");
                    document.getElementById('divServerOnline').style.display = 'block';
                    document.getElementById('divServerOffline').style.display = 'none';
                } else {
                    // Handle error response (format: "0|error message")
                    const errorParts = result.split('|');
                    const errorMessage = errorParts.length > 1 ? errorParts[1] : 'Failed to start MySQL server';
                    showErrorMessage("Error", errorMessage);
                    document.getElementById('divServerOffline').style.display = 'block';
                    document.getElementById('divServerOnline').style.display = 'none';
                }
            } catch (error) {
                console.error('Error starting server:', error);
                showErrorMessage("Error", "Failed to start MySQL server: " + error.message);
                closeBigLoading();
                document.getElementById('divServerOffline').style.display = 'block';
                document.getElementById('divServerOnline').style.display = 'none';
            }
        }

        // Stop MySQL Server
        async function stopMySqlServer() {
            try {
                showBigLoading(0);

                const response = await fetch('/apiMain?action=stopserver');
                const result = await response.text();
                closeBigLoading();

                if (result.trim() === '1') {
                    showGoodMessage("Success", "MySQL server stopped successfully");
                    // Update status display
                    document.getElementById('divServerOffline').style.display = 'block';
                    document.getElementById('divServerOnline').style.display = 'none';
                } else if (result.trim() === '2') {
                    showGoodMessage("Already Stopped", "MySQL server is already stopped");
                    document.getElementById('divServerOffline').style.display = 'block';
                    document.getElementById('divServerOnline').style.display = 'none';
                } else {
                    // Handle error response (format: "0|error message")
                    const errorParts = result.split('|');
                    const errorMessage = errorParts.length > 1 ? errorParts[1] : 'Failed to stop MySQL server';
                    showErrorMessage("Error", errorMessage);
                }
            } catch (error) {
                console.error('Error stopping server:', error);
                showErrorMessage("Error", "Failed to stop MySQL server: " + error.message);
                closeBigLoading();
            }
        }

        // Save MySQL Instance Path
        async function saveInstancePath() {
            try {
                // Get the path values from the textboxes
                const mysqldPath = document.getElementById('txtMysqldExePath').value;
                const mysqladminPath = document.getElementById('txtMySqlAdminExePath').value;

                // Validate paths are not empty
                if (!mysqldPath.trim() || !mysqladminPath.trim()) {
                    showErrorMessage("Validation Error", "Please enter both mysqld.exe and mysqladmin.exe paths");
                    closeBigLoading();
                    return;
                }

                // Create form data
                const formData = new FormData();
                formData.append('mysqldpath', mysqldPath);
                formData.append('mysqladminpath', mysqladminPath);

                const response = await fetch('/apiMain?action=saveinstancepath', {
                    method: 'POST',
                    body: formData
                });

                const result = await response.text();
                closeBigLoading();

                if (result.trim() === '1') {
                    showGoodMessage("Success", "MySQL instance paths saved successfully");
                } else {
                    showErrorMessage("Error", "Failed to save MySQL instance paths");
                }
            } catch (error) {
                console.error('Error saving instance path:', error);
                showErrorMessage("Error", "Failed to save instance paths: " + error.message);
                closeBigLoading();
            }
        }

        let isFirstCheck = true;

        async function checkMySqlStatus() {
            try {
                // Show loading state (optional)
                const onlineDiv = document.getElementById('divServerOnline');
                const offlineDiv = document.getElementById('divServerOffline');
                const divServerCheckAnimation = document.getElementById('divServerCheckAnimation');

                // Hide both divs initially
                onlineDiv.style.display = 'none';
                offlineDiv.style.display = 'none';
                divServerCheckAnimation.style.display = 'block';

                // Make API call
                const response = await fetch('/apiMain?action=checkserverstatus');
                const statusText = await response.text();

                divServerCheckAnimation.style.display = "none";

                // Check the response and show appropriate div
                if (statusText.trim() === '1') {
                    onlineDiv.style.display = 'block';
                    if (!isFirstCheck)
                        showGoodMessage("Online", "MySQL server is running");
                } else if (statusText.trim() === '0') {
                    if (!isFirstCheck)
                        showErrorMessage("Offline", "MySQL server is not running");
                    offlineDiv.style.display = 'block';
                } else {
                    // Handle unexpected response
                    console.error('Unexpected response:', statusText);
                    if (!isFirstCheck)
                        showErrorMessage('Unexpected response:', statusText);
                    offlineDiv.style.display = 'block'; // Default to offline
                }

            } catch (error) {
                console.error('Error checking server status:', error);
                if (!isFirstCheck)
                    showErrorMessage("Error", error);
                // Show offline div on error
                document.getElementById('divServerOffline').style.display = 'block';
                document.getElementById('divServerOnline').style.display = 'none';
            }

            if (isFirstCheck)
                isFirstCheck = false;
        }

        setTimeout(() => {
            checkMySqlStatus();
            hljs.highlightAll();
        }, 500);
    </script>
</asp:Content>
