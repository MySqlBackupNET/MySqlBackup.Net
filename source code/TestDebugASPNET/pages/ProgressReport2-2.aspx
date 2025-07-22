<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="ProgressReport2-2.aspx.cs" Inherits="System.pages.ProgressReport2_2" %>

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

        <h1>Progress Report 2-2</h1>

        <button type="button" onclick="backup();">Backup</button>
        <button type="button" onclick="restore();">Restore</button>
        <button type="button" onclick="stopTask();">Stop</button>
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
                <span id="labelPercent">0</span> %
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
                        <span id="labelSqlFilename"></span>
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

        let taskid = 0;
        let apicallid = 0;
        let intervalTimer = null;
        let intervalMs = 1000;

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

        async function fetchData(formData) {

            try {
                let action = formData.get("action") || "";

                const response = await fetch('/apiProgressReport2', {
                    method: 'POST',
                    body: formData,
                    credentials: 'include'
                });

                if (response.ok) {

                    const responseText = await response.text();

                    if (responseText.startsWith("0|")) {
                        let err = responseText.substring(2);
                        return { ok: false, errMsg: err };
                    }
                    else {
                        if (!responseText || responseText.trim() === '') {
                            let err = "Empty response from server";
                            return { ok: false, errMsg: err };
                        }
                        else {

                            // Success

                            if (action == "backup" || action == "restore") {
                                let _thisTaskid = parseInt(responseText);
                                if (isNaN(_thisTaskid)) {
                                    let err = `Invalid Task ID: ${_thisTaskid}`;
                                    return { ok: false, errMsg: err };
                                }
                                else {
                                    return { ok: true, thisTaskid: _thisTaskid };
                                }
                            }
                            else if (action == "stoptask") {
                                if (responseText == "1") {
                                    return { ok: true };
                                }
                                else {
                                    let err = `Unable to stop task`;
                                    return { ok: false, errMsg: err };
                                }
                            }
                            else if (action == "gettaskstatus") {
                                try {
                                    let thisJsonObject = JSON.parse(responseText);
                                    return { ok: true, jsonObject: thisJsonObject };
                                }
                                catch (err) {
                                    return { ok: false, errMsg: err };
                                }
                            }
                        }
                    }
                }
                else {
                    const err = await response.text();
                    return { ok: false, errMsg: err };
                }
            }
            catch (err) {
                return { ok: false, errMsg: err };
            }
        }

        async function backup() {
            resetUIValues();

            const formData = new FormData();
            formData.append('action', 'backup');

            const result = await fetchData(formData);

            if (result.ok) {
                taskid = result.thisTaskid;
                intervalMs = 1000;
                startIntervalTimer();
                showGoodMessage("Success", "Backup Task Begin");
            } else {
                showErrorMessage("Error", result.errMsg);
            }
        }

        async function restore() {
            resetUIValues();

            if (!fileRestore.files || fileRestore.files.length === 0) {
                showErrorMessage("Error", "Please select a file to restore");
                return;
            }

            const formData = new FormData();
            formData.append('action', 'restore');
            formData.append('fileRestore', fileRestore.files[0]);

            const result = await fetchData(formData);

            if (result.ok) {
                taskid = result.thisTaskid;
                intervalMs = 1000;
                startIntervalTimer();
                showGoodMessage("Success", "Restore Task Begin");
            } else {
                showErrorMessage("Error", result.errMsg);
            }
        }

        async function stopTask() {
            if (!taskid || taskid === 0) {
                showErrorMessage("Error", "No active task to stop");
                return;
            }

            const formData = new FormData();
            formData.append("action", "stoptask");
            formData.append("taskid", taskid);

            const result = await fetchData(formData);

            if (result.ok) {
                showGoodMessage("The task is being called to stop.");
            } else {
                showErrorMessage("Error", result.errMsg);
                stopIntervalTimer();
            }
        }

        async function fetchTaskStatus() {
            apicallid++;

            const formData = new FormData();
            formData.append('action', 'gettaskstatus');
            formData.append('taskid', taskid);
            formData.append('apicallid', apicallid);

            const result = await fetchData(formData);

            if (result.ok) {
                if (result.jsonObject.ApiCallIndex != apicallid) {
                    // late echo response - ignore
                    return;
                }

                console.log("before updateUIValues");
                updateUIValues(result.jsonObject);
            } else {
                showErrorMessage("Error", result.errMsg);
                stopIntervalTimer();
            }
        }

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
            const statusCell = taskStatus.closest('td');
            taskStatus.textContent = "---";
            taskMessage.textContent = "---";
            statusCell.className = "";
        }

        function updateUIValues(jsonObject) {

            if (jsonObject.PercentCompleted > 0) {
                if (intervalMs == 1000) {
                    intervalMs = 100;
                    stopIntervalTimer();
                    setTimeout(() => { startIntervalTimer(); }, 500);
                }
            }

            if (jsonObject.IsCompleted || jsonObject.HasError || jsonObject.IsCancelled) {
                stopIntervalTimer();
            }

            labelTaskId.textContent = jsonObject.TaskId || "";
            lableTimeStart.textContent = jsonObject.TimeStartDisplay || "";
            lableTimeEnd.textContent = jsonObject.TimeEndDisplay || "";
            lableTimeElapse.textContent = jsonObject.TimeUsedDisplay || "";

            // Update progress bar and percentage
            const percent = jsonObject.PercentCompleted || 0;
            labelPercent.style.display = "block";
            labelPercent.textContent = percent;
            progress_bar_indicator.style.width = percent + '%';

            // Task status with color coding
            const statusCell = taskStatus.closest('td');

            if (jsonObject.HasError) {
                taskStatus.textContent = "Error: ";
                taskMessage.textContent = jsonObject.ErrorMsg || "";
                statusCell.className = "status-error";
            }
            else if (jsonObject.IsCancelled) {
                taskStatus.textContent = "Cancelled";
                taskMessage.textContent = "";
                statusCell.className = "status-error";
            }
            else if (jsonObject.IsCompleted) {
                taskStatus.textContent = "Completed";
                taskMessage.textContent = "";
                statusCell.className = "status-complete";
            }
            else {
                taskStatus.textContent = "Running";
                taskMessage.textContent = "";
                statusCell.className = "status-running";
            }

            if (jsonObject.FileName && jsonObject.FileName.length > 0) {
                labelSqlFilename.innerHTML = `File: <a href='/apiFiles?folder=backup&filename=${jsonObject.FileName}' class='mainbutton'>Download</a>`;
            } else {
                labelSqlFilename.textContent = jsonObject.FileName || "---";
            }
            labelSha256.textContent = jsonObject.SHA256 || "---";

            // Export/Backup specific data (table information)
            if (jsonObject.TaskType === 1) {
                labelCurTableName.textContent = jsonObject.CurrentTableName || "";
                labelCurTableIndex.textContent = jsonObject.CurrentTableIndex || "";
                labelTotalTables.textContent = jsonObject.TotalTables || "";
                labelCurrentRowsAllTables.textContent = jsonObject.CurrentRowIndex || "";
                labelTotalRowsAllTable.textContent = jsonObject.TotalRows || "0";
                labelCurrentRowsCurrentTables.textContent = jsonObject.CurrentRowCurrentTable || "";
                labelTotalRowsCurrentTable.textContent = jsonObject.TotalRowsCurrentTable || "0";

                // Clear bytes for backup
                labelCurrentBytes.textContent = "---";
                lableTotalBytes.textContent = "---";
            }

            // Import/Restore specific data (bytes information)
            if (jsonObject.TaskType === 2) {
                labelCurrentBytes.textContent = jsonObject.CurrentBytes || "";
                lableTotalBytes.textContent = jsonObject.TotalBytes || "";

                // Clear table info for restore
                labelCurTableName.textContent = "---";
                labelCurTableIndex.textContent = "--";
                labelTotalTables.textContent = "--";
                labelCurrentRowsAllTables.textContent = "--";
                labelTotalRowsAllTable.textContent = "--";
                labelCurrentRowsCurrentTables.textContent = "--";
                labelTotalRowsCurrentTable.textContent = "--";
            }
        }

        function startIntervalTimer() {
            stopIntervalTimer();

            intervalTimer = setInterval(() => {
                fetchTaskStatus();
            }, intervalMs);
        }

        function stopIntervalTimer() {
            if (intervalTimer) {
                clearInterval(intervalTimer);
            }
        }

        function changeTheme(themename) {

            if (themename)
                linkThemeFile.href = `/cssjs/MySqlBackup-Progress-Widget-Theme/${themename}.css`;

            divThemes.style.display = divThemes.style.display == "block" ? "none" : "block";
        }

    </script>

</asp:Content>
