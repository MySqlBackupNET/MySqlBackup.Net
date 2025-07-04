<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="ProgressReportSimple.aspx.cs" Inherits="System.pages.ProgressReportSimple" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        table.maintb {
            border-collapse: collapse;
        }

        .maintb th, td {
            border: 1px solid #ddd;
            padding: 8px 12px;
            text-align: left;
            vertical-align: top;
        }

        .maintb th {
            background-color: #f8f9fa;
            font-weight: bold;
            text-align: center;
        }

        .label {
            background-color: #f5f5f5;
            font-weight: bold;
            width: 160px;
            word-wrap: break-word;
        }

        .value {
            background-color: #fff;
            width: 300px;
            word-wrap: break-word;
            word-break: break-all;
            overflow-wrap: break-word;
        }

        .maintb tr:hover {
            background-color: #f9f9f9;
        }

        .status-field {
            color: #666;
            font-style: italic;
        }

        .error-field {
            color: #d32f2f;
        }

        .progress-field {
            color: #1976d2;
        }

        .section-header {
            background-color: #e3f2fd;
            font-weight: bold;
            text-align: center;
            font-size: 1.1em;
        }

        .progress-container {
            width: 100%;
            height: 20px;
            background-color: #f0f0f0;
            border-radius: 4px;
            overflow: hidden;
            position: relative;
        }

        .progress-bar {
            width: var(--progress-width, 0%);
            height: 100%;
            background-color: #1976d2;
            transition: width 0.3s ease-in-out;
            border-radius: 4px;
            display: flex;
            align-items: center;
            justify-content: center;
            color: white;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="div-center-framed-content">
        <h1>Progress Report - Simple API Demo</h1>

        <button type="button" onclick="runBackup();">Run Backup</button>
        <button type="button" onclick="runRestore();">Run Restore</button>
        <button type="button" onclick="runStop();">Stop Process</button>

        <div style="margin: 25px 0;">
            Restore File:
            <input type="file" id="fileRestore" />
        </div>

        <table class="maintb">
            <!-- Header -->
            <tr>
                <th colspan="4" class="section-header">Progress Report Details</th>
            </tr>
            <!-- Progress Bar Row -->
            <tr>
                <td class="label">Progress</td>
                <td colspan="3" class="value">
                    <div class="progress-container">
                        <div class="progress-bar" id="progress-bar"></div>
                    </div>
                </td>
            </tr>
            <!-- Basic Info Row -->
            <tr>
                <td class="label">SHA256</td>
                <td colspan="3"><span id="span-SHA256"></span></td>
            </tr>
            <tr>
                <td class="label">Task ID</td>
                <td class="value"><span id="span-TaskId"></span></td>
                <td class="label">Task Type</td>
                <td class="value"><span id="span-TaskTypeName"></span></td>
            </tr>
            <tr>
                <td class="label">API Call Index</td>
                <td class="value"><span id="span-ApiCallIndex"></span></td>
                <td class="label">File Name</td>
                <td class="value"><span id="span-FileName"></span></td>
            </tr>
            <!-- Status Row -->
            <tr>
                <td class="label">Is Started</td>
                <td class="value"><span id="span-IsStarted" class="status-field"></span></td>
                <td class="label">Is Completed</td>
                <td class="value"><span id="span-IsCompleted" class="status-field"></span></td>
            </tr>
            <tr>
                <td class="label">Is Cancelled</td>
                <td class="value"><span id="span-IsCancelled" class="status-field"></span></td>
                <td class="label">Request Cancel</td>
                <td class="value"><span id="span-RequestCancel" class="status-field"></span></td>
            </tr>
            <!-- Error Row -->
            <tr>
                <td class="label">Has Error</td>
                <td class="value"><span id="span-HasError" class="error-field"></span></td>
                <td class="label">Error Message</td>
                <td class="value"><span id="span-ErrorMsg" class="error-field"></span></td>
            </tr>
            <!-- Time Info Row -->
            <tr>
                <td class="label">Time Start</td>
                <td class="value"><span id="span-TimeStartDisplay"></span></td>
                <td class="label">Time End</td>
                <td class="value"><span id="span-TimeEndDisplay"></span></td>
            </tr>
            <tr>
                <td class="label">Time Used</td>
                <td class="value"><span id="span-TimeUsedDisplay"></span></td>
                <td class="label">Percent Completed</td>
                <td class="value"><span id="span-PercentCompleted" class="progress-field"></span></td>
            </tr>
            <!-- Export Progress Row -->
            <tr>
                <td class="label">Total Tables</td>
                <td class="value"><span id="span-TotalTables" class="progress-field"></span></td>
                <td class="label">Current Table Index</td>
                <td class="value"><span id="span-CurrentTableIndex" class="progress-field"></span></td>
            </tr>
            <tr>
                <td class="label">Current Table Name</td>
                <td class="value"><span id="span-CurrentTableName" class="progress-field"></span></td>
                <td class="label">Total Rows (Current Table)</td>
                <td class="value"><span id="span-TotalRowsCurrentTable" class="progress-field"></span></td>
            </tr>
            <tr>
                <td class="label">Current Row (Current Table)</td>
                <td class="value"><span id="span-CurrentRowCurrentTable" class="progress-field"></span></td>
                <td class="label">Total Rows</td>
                <td class="value"><span id="span-TotalRows" class="progress-field"></span></td>
            </tr>
            <tr>
                <td class="label">Current Row Index</td>
                <td class="value"><span id="span-CurrentRowIndex" class="progress-field"></span></td>
                <td class="label">Total Bytes</td>
                <td class="value"><span id="span-TotalBytes" class="progress-field"></span></td>
            </tr>
            <tr>
                <td class="label">Current Bytes</td>
                <td class="value"><span id="span-CurrentBytes" class="progress-field"></span></td>
                <td class="label">Download URL</td>
                <td class="value"><span id="span-FileDownloadUrl"></span></td>
            </tr>
        </table>
    </div>

    <script>
        let currentTaskId = 0;
        let apiCallIndex = 0;
        let statusInterval = null;

        function runBackup() {
            fetch('/apiProgressReport2?action=backup', {
                method: 'POST'
            })
                .then(response => response.text())
                .then(taskId => {
                    currentTaskId = parseInt(taskId);
                    startStatusUpdate();
                })
                .catch(error => {
                    console.error('Backup request failed:', error);
                    updateUI({ HasError: true, ErrorMsg: 'Failed to start backup: ' + error.message });
                });
        }

        function runRestore() {
            const fileInput = document.getElementById('fileRestore');
            if (!fileInput.files.length) {
                showMessage("No file", "Please select a file", false);
                return;
            }

            const formData = new FormData();
            formData.append('file', fileInput.files[0]);

            fetch('/apiProgressReport2?action=restore', {
                method: 'POST',
                body: formData
            })
                .then(response => response.text())
                .then(taskId => {
                    currentTaskId = parseInt(taskId);
                    startStatusUpdate();
                })
                .catch(error => {
                    console.error('Restore request failed:', error);
                    updateUI({ HasError: true, ErrorMsg: 'Failed to start restore: ' + error.message });
                });
        }

        function runStop() {
            if (!currentTaskId) {
                showMessage("No Task", "No active task to stop", false);
                return;
            }

            fetch(`/apiProgressReport2?action=stoptask&taskid=${currentTaskId}`, {
                method: 'POST'
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Failed to stop task');
                    }
                })
                .catch(error => {
                    console.error('Stop task request failed:', error);
                    updateUI({ HasError: true, ErrorMsg: 'Failed to stop task: ' + error.message });
                });
        }

        function startStatusUpdate() {
            // Clear any existing interval
            if (statusInterval) {
                clearInterval(statusInterval);
            }

            // Start new interval
            apiCallIndex++;
            statusInterval = setInterval(() => {
                fetch(`/apiProgressReport2?action=gettaskstatus&apicallid=${apiCallIndex}&taskid=${currentTaskId}`)
                    .then(response => response.json())
                    .then(task => {
                        if (task) {
                            updateUI(task);
                            if (task.IsCompleted || task.HasError || task.IsCancelled) {
                                clearInterval(statusInterval);
                                statusInterval = null;
                            }
                        }
                    })
                    .catch(error => {
                        console.error('Status update failed:', error);
                        updateUI({ HasError: true, ErrorMsg: 'Status update failed: ' + error.message });
                        clearInterval(statusInterval);
                        statusInterval = null;
                    });
            }, 250);
        }

        function updateUI(task) {
            const fields = [
                'TaskId', 'TaskTypeName', 'ApiCallIndex', 'FileName',
                'IsStarted', 'IsCompleted', 'IsCancelled', 'RequestCancel',
                'HasError', 'ErrorMsg', 'TimeStartDisplay', 'TimeEndDisplay',
                'TimeUsedDisplay', 'PercentCompleted', 'TotalTables',
                'CurrentTableIndex', 'CurrentTableName', 'TotalRowsCurrentTable',
                'CurrentRowCurrentTable', 'TotalRows', 'CurrentRowIndex',
                'TotalBytes', 'CurrentBytes', 'FileDownloadUrl', 'SHA256'
            ];

            fields.forEach(field => {
                const element = document.getElementById(`span-${field}`);
                if (element) {
                    element.textContent = task[field] || '';
                }
            });

            let downloadUrl = task["FileDownloadUrl"];
            if (downloadUrl.length > 0)
                document.getElementById(`span-FileDownloadUrl`).innerHTML = `<a href='${downloadUrl}'>Download</a>`;

            // Update progress bar
            const progressBar = document.getElementById('progress-bar');
            if (progressBar) {
                const percent = task.PercentCompleted || 0;
                progressBar.style.setProperty('--progress-width', `${percent}%`);
                progressBar.textContent = `${percent}%`;
            }
        }

        // Initialize on page load
        document.addEventListener('DOMContentLoaded', () => {
            // Clear initial UI
            updateUI({});
        });
    </script>
</asp:Content>
