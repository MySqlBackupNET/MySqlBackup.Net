<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="ProgressReport3.aspx.cs" Inherits="System.pages.ProgressReport3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        #divThemes {
            display: none;
            margin: 15px 0;
        }
    </style>
    <link id="linkThemeFile" href="/cssjs/MySqlBackup-Progress-Widget-Theme/steampunk.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">

        <h1>Progress Report 3 - Web Socket</h1>

        <button type="button" id="btnBackup">Backup</button>
        <button type="button" id="btnRestore">Restore</button>
        <button type="button" id="btnStop">Stop</button>
        <input type="file" id="fileRestore" />
        <button type="button" onclick="changeTheme();">Change Theme</button>

        <div id="divThemes">
            <button type="button" onclick="changeTheme('light');">Light</button>
            <button type="button" onclick="changeTheme('dark');">Dark</button>
            <button type="button" onclick="changeTheme('cyberpunk');">Cyberpunk</button>
            <button type="button" onclick="changeTheme('retro');">
                Terminal<br />
                Alien 1986</button>
            <button type="button" onclick="changeTheme('steampunk');">
                Steampunk<br />
                Victorian</button>
            <button type="button" onclick="changeTheme('solarfire');">
                Solar<br />
                Fire</button>
            <button type="button" onclick="changeTheme('hud');">
                Futuristic<br />
                HUD</button>
        </div>

        <div id="progress_bar_container">
            <div id="progress_bar_indicator">
                <span id="labelPercent" style="display: none;">0</span> %
            </div>
        </div>

        <div class="div_task_status">
            <table>
                <tr>
                    <td>Task ID</td>
                    <td>
                        <span id="labelTaskId"></span>
                    </td>
                </tr>
                <tr>
                    <td>Time</td>
                    <td>Start: <span id="lableTimeStart"></span>
                        /
                    End: <span id="lableTimeEnd"></span>
                        /
                    Elapse: <span id="lableTimeElapse"></span>
                    </td>
                </tr>
                <tr>
                    <td>Task Status</td>
                    <td colspan="3">
                        <span id="labelTaskStatus"></span><span id="labelTaskMessage"></span>
                    </td>
                </tr>
                <tr>
                    <td>Backup File</td>
                    <td>
                        Download: <span id="labelSqlFilename"></span>
                        <br />
                        SHA 256: <span id="labelSha256"></span>
                    </td>
                </tr>
                <tr>
                    <td>Current Table</td>
                    <td>
                        <span id="labelCurTableName"></span>(<span id="labelCurTableIndex"></span> / <span id="labelTotalTables"></span>)
                    </td>
                </tr>
                <tr>
                    <td>All Tables Rows</td>
                    <td>
                        <span id="labelCurrentRowsAllTables"></span>/ <span id="labelTotalRowsAllTable">0</span>
                    </td>
                </tr>
                <tr>
                    <td>Current Table Rows</td>
                    <td>
                        <span id="labelCurrentRowsCurrentTables"></span>/ <span id="labelTotalRowsCurrentTable">0</span>
                    </td>
                </tr>
                <tr>
                    <td>Total Bytes</td>
                    <td>
                        <span id="labelCurrentBytes"></span>/ <span id="lableTotalBytes"></span>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <script>

        let divThemes = document.querySelector("#divThemes");

        let linkThemeFile = document.getElementById("linkThemeFile");
        let fileRestore = document.querySelector("#fileRestore");

        let labelSqlFilename = document.querySelector("#labelSqlFilename");
        let labelSha256 = document.querySelector("#labelSha256");
        let progress_bar_indicator = document.querySelector("#progress_bar_indicator");
        let labelPercent = document.querySelector("#labelPercent");
        let labelTaskId = document.querySelector("#labelTaskId");
        let lableTimeStart = document.querySelector("#lableTimeStart");
        let lableTimeEnd = document.querySelector("#lableTimeEnd");
        let lableTimeElapse = document.querySelector("#lableTimeElapse");
        let taskStatus = document.querySelector("#labelTaskStatus");
        let taskMessage = document.querySelector("#labelTaskMessage");
        let labelCurTableName = document.querySelector("#labelCurTableName");
        let labelCurTableIndex = document.querySelector("#labelCurTableIndex");
        let labelTotalTables = document.querySelector("#labelTotalTables");
        let labelCurrentRowsAllTables = document.querySelector("#labelCurrentRowsAllTables");
        let labelTotalRowsAllTable = document.querySelector("#labelTotalRowsAllTable");
        let labelCurrentRowsCurrentTables = document.querySelector("#labelCurrentRowsCurrentTables");
        let labelTotalRowsCurrentTable = document.querySelector("#labelTotalRowsCurrentTable");
        let labelCurrentBytes = document.querySelector("#labelCurrentBytes");
        let lableTotalBytes = document.querySelector("#lableTotalBytes");

        // Routed URL API Endpoint
        let urlApiEndpoint = "/apiProgressReport3";

        function changeTheme(themename) {

            if (themename)
                linkThemeFile.href = `/cssjs/MySqlBackup-Progress-Widget-Theme/${themename}.css`;

            divThemes.style.display = divThemes.style.display == "block" ? "none" : "block";
        }

        // Global variables
        let currentTaskId = 0;
        let webSocket = null;
        let isConnecting = false;

        // Button references (add to existing button declarations)
        const btnBackup = document.querySelector('#btnBackup');
        const btnRestore = document.querySelector('#btnRestore');
        const btnStop = document.querySelector('#btnStop');
        const buttons = { btnBackup, btnRestore, btnStop };

        // Add event listeners
        btnBackup.addEventListener('click', startBackup);
        btnRestore.addEventListener('click', startRestore);
        btnStop.addEventListener('click', stopTask);

        // HTTP API Functions
        async function fetchAPI(url, options = {}) {
            try {
                const response = await fetch(url, {
                    credentials: 'include',
                    ...options
                });

                if (response.ok) {
                    const contentType = response.headers.get('content-type');
                    if (contentType && contentType.includes('application/json')) {
                        return { ok: true, data: await response.json() };
                    } else {
                        return { ok: true, text: await response.text() };
                    }
                } else {
                    const errorText = await response.text();
                    return { ok: false, error: errorText, status: response.status };
                }
            } catch (err) {
                return { ok: false, error: err.message };
            }
        }

        async function startBackup() {
            resetUIValues();
            disableButtons();

            try {
                const formData = new FormData();
                formData.append('action', 'start_backup');

                const result = await fetchAPI(urlApiEndpoint, {
                    method: 'POST',
                    body: formData
                });

                if (result.ok && result.data) {
                    currentTaskId = result.data.TaskId;
                    showGoodMessage("Success", result.data.Status);
                    connectWebSocket(currentTaskId);
                } else {
                    showErrorMessage("Error", result.error || "Failed to start backup");
                    enableButtons();
                }
            } catch (err) {
                showErrorMessage("Error", err.message);
                enableButtons();
            }
        }

        async function startRestore() {
            resetUIValues();

            if (!fileRestore.files || fileRestore.files.length === 0) {
                showErrorMessage("Error", "Please select a file to restore");
                return;
            }

            disableButtons();

            try {
                const formData = new FormData();
                formData.append('action', 'start_restore');
                formData.append('file', fileRestore.files[0]);

                const result = await fetchAPI(urlApiEndpoint, {
                    method: 'POST',
                    body: formData
                });

                if (result.ok && result.data) {
                    currentTaskId = result.data.TaskId;
                    showGoodMessage("Success", result.data.Status);
                    connectWebSocket(currentTaskId);
                } else {
                    showErrorMessage("Error", result.error || "Failed to start restore");
                    enableButtons();
                }
            } catch (err) {
                showErrorMessage("Error", err.message);
                enableButtons();
            }
        }

        async function stopTask() {
            if (!currentTaskId || currentTaskId === 0) {
                showErrorMessage("Error", "No active task to stop");
                return;
            }

            try {
                const formData = new FormData();
                formData.append('action', 'stop');
                formData.append('taskid', currentTaskId);

                const result = await fetchAPI(urlApiEndpoint, {
                    method: 'POST',
                    body: formData
                });

                if (result.ok) {
                    showGoodMessage("Success", "Stop request sent to server");
                } else {
                    showErrorMessage("Error", result.error || "Failed to stop task");
                }
            } catch (err) {
                showErrorMessage("Error", err.message);
            }
        }

        // WebSocket Functions
        function connectWebSocket(taskId) {
            if (isConnecting || (webSocket && webSocket.readyState === WebSocket.OPEN)) {
                return;
            }

            isConnecting = true;

            // Determine WebSocket URL (ws:// or wss://)
            const protocol = window.location.protocol === 'https:' ? 'wss:' : 'ws:';
            const wsUrl = `${protocol}//${window.location.host}/${urlApiEndpoint}`;

            try {
                webSocket = new WebSocket(wsUrl);

                webSocket.onopen = function () {
                    console.log('WebSocket connected');
                    isConnecting = false;

                    // Subscribe to task updates
                    webSocket.send(`subscribe:${taskId}`);
                };

                webSocket.onmessage = function (event) {
                    try {
                        const data = JSON.parse(event.data);

                        if (data.Error) {
                            showErrorMessage("WebSocket Error", data.Error);
                            return;
                        }

                        updateUIValues(data);

                        // Close WebSocket when task is completed
                        if (data.IsCompleted || data.HasError || data.IsCancelled) {
                            closeWebSocket();
                        }

                    } catch (err) {
                        console.error('Error parsing WebSocket message:', err);
                    }
                };

                webSocket.onclose = function (event) {
                    console.log('WebSocket closed:', event.code, event.reason);
                    isConnecting = false;
                    webSocket = null;
                    enableButtons();
                };

                webSocket.onerror = function (error) {
                    console.error('WebSocket error:', error);
                    isConnecting = false;
                    showErrorMessage("Connection Error", "WebSocket connection failed");
                    enableButtons();
                };

            } catch (err) {
                console.error('Failed to create WebSocket:', err);
                isConnecting = false;
                showErrorMessage("Connection Error", "Failed to establish WebSocket connection");
                enableButtons();
            }
        }

        function closeWebSocket() {
            if (webSocket) {
                if (webSocket.readyState === WebSocket.OPEN) {
                    webSocket.close(1000, "Task completed");
                }
                webSocket = null;
            }
            isConnecting = false;
        }

        // UI Update Functions
        function resetUIValues() {
            labelTaskId.textContent = "--";
            lableTimeStart.textContent = "---";
            lableTimeEnd.textContent = "---";
            lableTimeElapse.textContent = "---";
            labelPercent.textContent = "0";
            labelCurTableName.textContent = "---";
            labelCurTableIndex.textContent = "--";
            labelTotalTables.textContent = "--";
            labelCurrentRowsAllTables.textContent = "--";
            labelTotalRowsAllTable.textContent = "--";
            labelCurrentRowsCurrentTables.textContent = "--";
            labelTotalRowsCurrentTable.textContent = "--";
            labelCurrentBytes.textContent = "--";
            lableTotalBytes.textContent = "--";

            // Reset progress bar
            progress_bar_indicator.style.width = '0%';
            labelPercent.style.display = "none";

            // Reset status
            taskStatus.textContent = "---";
            taskMessage.textContent = "";
            labelSqlFilename.textContent = "---";
            labelSha256.textContent = "---";

            // Reset status styling
            const statusCell = taskStatus.closest('td');
            if (statusCell) {
                statusCell.className = "";
            }
        }

        function updateUIValues(taskInfo) {
            // Basic task information
            labelTaskId.textContent = taskInfo.TaskId || "--";
            lableTimeStart.textContent = taskInfo.TimeStartDisplay || "---";
            lableTimeEnd.textContent = taskInfo.TimeEndDisplay || "---";
            lableTimeElapse.textContent = taskInfo.TimeUsedDisplay || "---";

            // Progress bar and percentage
            const percent = taskInfo.PercentCompleted || 0;
            labelPercent.style.display = "block";
            labelPercent.textContent = percent;
            progress_bar_indicator.style.width = percent + '%';

            // Task status with color coding
            const statusCell = taskStatus.closest('td');

            if (taskInfo.HasError) {
                taskStatus.textContent = "Error: ";
                taskMessage.textContent = taskInfo.ErrorMsg || "";
                if (statusCell) statusCell.className = "status-error";
            }
            else if (taskInfo.IsCancelled) {
                taskStatus.textContent = "Cancelled";
                taskMessage.textContent = "";
                if (statusCell) statusCell.className = "status-error";
            }
            else if (taskInfo.IsCompleted) {
                taskStatus.textContent = "Completed";
                taskMessage.textContent = "";
                if (statusCell) statusCell.className = "status-complete";
            }
            else {
                taskStatus.textContent = `Running (${taskInfo.TaskTypeName})`;
                taskMessage.textContent = "";
                if (statusCell) statusCell.className = "status-running";
            }

            // File information
            if (taskInfo.FileName && taskInfo.FileName.length > 0) {
                if (taskInfo.FileDownloadWebPath) {
                    labelSqlFilename.innerHTML = `<a href='${taskInfo.FileDownloadWebPath}' target='_blank'>${taskInfo.FileName}</a>`;
                } else {
                    labelSqlFilename.textContent = taskInfo.FileName;
                }
            } else {
                labelSqlFilename.textContent = "---";
            }
            labelSha256.textContent = taskInfo.FileSha256 || "---";

            // Task type specific data
            if (taskInfo.TaskType === 1) {
                // Backup
                labelCurTableName.textContent = taskInfo.CurrentTableName || "---";
                labelCurTableIndex.textContent = taskInfo.CurrentTableIndex || "--";
                labelTotalTables.textContent = taskInfo.TotalTables || "--";
                labelCurrentRowsAllTables.textContent = taskInfo.CurrentRows || "--";
                labelTotalRowsAllTable.textContent = taskInfo.TotalRows || "--";
                labelCurrentRowsCurrentTables.textContent = taskInfo.CurrentRowsCurrentTable || "--";
                labelTotalRowsCurrentTable.textContent = taskInfo.TotalRowsCurrentTable || "--";
            }
            else if (taskInfo.TaskType === 2) {
                // Restore
                labelCurrentBytes.textContent = formatBytes(taskInfo.CurrentBytes) || "--";
                lableTotalBytes.textContent = formatBytes(taskInfo.TotalBytes) || "--";
            }
        }

        // Utility Functions
        function formatBytes(bytes) {
            if (!bytes || bytes === 0) return '0 B';

            const k = 1024;
            const sizes = ['B', 'KB', 'MB', 'GB', 'TB'];
            const i = Math.floor(Math.log(bytes) / Math.log(k));

            return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
        }

        function disableButtons() {
            btnBackup.disabled = true;
            btnRestore.disabled = true;
            btnStop.disabled = false; // Keep stop button enabled
        }

        function enableButtons() {
            btnBackup.disabled = false;
            btnRestore.disabled = false;
            btnStop.disabled = false;
            currentTaskId = 0;
        }

        // Cleanup on page unload
        window.addEventListener('beforeunload', function () {
            closeWebSocket();
        });

        // Auto-reconnect logic (optional)
        function attemptReconnect(taskId, retries = 3) {
            if (retries <= 0) {
                showErrorMessage("Connection Failed", "Unable to establish WebSocket connection after multiple attempts");
                enableButtons();
                return;
            }

            console.log(`Attempting to reconnect... (${4 - retries}/3)`);

            setTimeout(() => {
                if (!webSocket || webSocket.readyState === WebSocket.CLOSED) {
                    connectWebSocket(taskId);

                    // Check if connection was successful after a delay
                    setTimeout(() => {
                        if (!webSocket || webSocket.readyState !== WebSocket.OPEN) {
                            attemptReconnect(taskId, retries - 1);
                        }
                    }, 2000);
                }
            }, 1000);
        }

    </script>
</asp:Content>
