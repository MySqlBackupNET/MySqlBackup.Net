<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="ProgressReport2.aspx.cs" Inherits="System.pages.ProgressReport2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        input[type="file"] {
            border: 1px solid #949494;
            padding: 5px;
            background: #e8e8e8
        }

            input[type="file"]::file-selector-button {
                color: #797979;
                border: 2px solid #797979;
                padding: 6px 12px;
                font-family: 'Roboto Mono';
                border-radius: 3px;
            }

        .mb_progress_bar_container {
            background: #949494;
            border: 1px solid #b1b1b1;
            box-shadow: 0 0 10px #b4b4b4;
            border-radius: 16px;
            width: 750px;
        }

        .mb_progress_bar_indicator {
            border-radius: 16px;
            background: #0059b9;
            color: white;
            width: 0%;
            text-align: center;
            transition: width 0.3s linear;
            overflow: hidden;
        }

        .mb_percent {
            color: white;
            padding: 3px;
            display: inline-block;
        }

        .tableProgress {
            border-collapse: collapse;
        }

            .tableProgress td {
                padding: 5px;
                border: 1px solid #a3a3a3;
                vertical-align: top;
            }

                .tableProgress td:nth-child(2) {
                    width: 600px;
                }

        .status_complete {
            background: #d8ffd1;
            color: #0f6300;
            display: block;
            padding: 5px;
        }

        .status_error {
            background: #ffcfcf;
            color: maroon;
            display: block;
            padding: 5px;
        }

        .status_running {
            background: #daecff;
            color: #0059b9;
            display: block;
            padding: 5px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">
        <h1>Progress Report 2 - HTTP Request</h1>

        <button type="button" onclick="startBackup();">Backup</button>
        <button type="button" onclick="startRestore();">Restore</button>
        <button type="button" onclick="stopTask();">Stop</button>
        File Restore:
        <input type="file" id="fileRestore" />

        <hr />

        <div class="mb_progress_bar_container">
            <div id="mb_progress_bar_indicator" class="mb_progress_bar_indicator">
                <span id="mb_percent" class="mb_percent"></span>
            </div>
        </div>

        <h2>Task Type: <span id="mb_task_type"></span></h2>

        <table class="tableProgress">
            <tr>
                <td>Task ID</td>
                <td>
                    <span id="mb_task_id"></span>
                </td>
            </tr>
            <tr>
                <td>Status</td>
                <td>
                    <span id="mb_status"></span>
                </td>
            </tr>
            <tr>
                <td>SQL File</td>
                <td>
                    Download File: <span id="mb_filedownload"></span><br />
                    SHA 256: <span id="mb_file_sha256"></span>
                </td>
            </tr>
            <tr>
                <td>Time Start</td>
                <td>
                    <span id="mb_time_start"></span>
                </td>
            </tr>
            <tr>
                <td>Time End</td>
                <td>
                    <span id="mb_time_end"></span>
                </td>
            </tr>
            <tr>
                <td>Time Used</td>
                <td>
                    <span id="mb_time_used"></span>
                </td>
            </tr>
            <tr>
                <td>Rows</td>
                <td>
                    <span id="mb_current_rows"></span> / <span id="mb_total_rows"></span>
                </td>
            </tr>
            <tr>
                <td>Current Table<br />
                    Rows</td>
                <td>
                    <span id="mb_current_table_name"></span>
                    (<span id="mb_current_table_index"></span> / <span id="mb_total_tables"></span>)
                    <br />
                    <span id="mb_current_table_current_rows"></span> / <span id="mb_current_table_total_rows"></span>
                </td>
            </tr>
            <tr>
                <td>Bytes</td>
                <td>
                    <span id="mb_current_bytes"></span> / <span id="mb_total_bytes"></span>
                </td>
            </tr>
        </table>

    </div>

    <script>

        let fileRestore = document.querySelector("#fileRestore");

        let mb_task_id = document.querySelector("#mb_task_id");
        let mb_progress_bar_indicator = document.querySelector("#mb_progress_bar_indicator");
        let mb_percent = document.querySelector("#mb_percent");
        let mb_task_type = document.querySelector("#mb_task_type");
        let mb_status = document.querySelector("#mb_status");
        let mb_filedownload = document.querySelector("#mb_filedownload");
        let mb_file_sha256 = document.querySelector("#mb_file_sha256");
        let mb_time_start = document.querySelector("#mb_time_start");
        let mb_time_end = document.querySelector("#mb_time_end");
        let mb_time_used = document.querySelector("#mb_time_used");
        let mb_total_rows = document.querySelector("#mb_total_rows");
        let mb_current_rows = document.querySelector("#mb_current_rows");
        let mb_current_table_name = document.querySelector("#mb_current_table_name");
        let mb_total_tables = document.querySelector("#mb_total_tables");
        let mb_current_table_index = document.querySelector("#mb_current_table_index");
        let mb_current_table_total_rows = document.querySelector("#mb_current_table_total_rows");
        let mb_current_table_current_rows = document.querySelector("#mb_current_table_current_rows");
        let mb_total_bytes = document.querySelector("#mb_total_bytes");
        let mb_current_bytes = document.querySelector("#mb_current_bytes");

        let currentTaskId = 0;
        let urlApi = "/apiProgressReport2";
        let intervalTimer = null;
        let networkErrorCount = 0;
        let api_call_index = 0;

        async function startBackup() {

            try {
                resetUi();

                const formData = new FormData();
                formData.append("action", "start_backup");

                const result = await fetch(urlApi, {
                    method: "POST",
                    body: formData,
                    credentials: "include"
                });

                if (result.ok) {
                    let jsonObject = await result.json();
                    currentTaskId = jsonObject.TaskId;
                    showGoodMessage("Success", "The task has begun");
                    startProgressMonitoring();
                }
                else {
                    let errMsg = await result.text();
                    showErrorMessage("Error", errMsg);
                }
            }
            catch (err) {
                showErrorMessage("Error", err.message);
            }
        }

        async function startRestore() {
            try {

                if (!fileRestore.files[0]) {
                    showErrorMessage("Error", "Please select a file to restore");
                    return;
                }

                resetUi();

                const formData = new FormData();
                formData.append("action", "start_restore");
                formData.append("file_restore", fileRestore.files[0]);

                const result = await fetch(urlApi, {
                    method: "POST",
                    body: formData,
                    credentials: "include"
                });

                if (result.ok) {
                    let jsonObject = await result.json();
                    currentTaskId = jsonObject.TaskId;
                    showGoodMessage("Success", "The task has begun");
                    startProgressMonitoring();
                }
                else {
                    let errMsg = await result.text();
                    showErrorMessage("Error", errMsg);
                }
            }
            catch (err) {
                showErrorMessage("Error", err.message);
            }
        }


        async function stopTask() {
            try {
                if (!currentTaskId) {
                    showErrorMessage("Error", "No active task to stop");
                    return;
                }

                const formData = new FormData();
                formData.append("action", "stop_task");
                formData.append("taskid", currentTaskId);

                const result = await fetch(urlApi, {
                    method: "POST",
                    body: formData,
                    credentials: "include"
                });

                if (result.ok) {
                    showGoodMessage("Task is Stopping", "The task is being called to stop");
                }
                else {
                    let errMsg = await result.text();
                    showErrorMessage("Error", errMsg);
                }
            }
            catch (err) {
                showErrorMessage("Error", err.message);
            }
        }


        async function getStatus() {
            try {
                // increment the api call index
                api_call_index++;

                const formData = new FormData();
                formData.append("action", "get_status");
                formData.append("taskid", currentTaskId);
                formData.append("api_call_index", api_call_index);

                const result = await fetch(urlApi, {
                    method: "POST",
                    body: formData,
                    credentials: "include"
                });

                if (result.ok) {
                    let jsonObject = await result.json();

                    if (jsonObject.ApiCallIndex != api_call_index) {
                        // late echo, ignore
                        return;
                    }

                    updateUiValues(jsonObject);
                }
                else {
                    networkErrorCount++;
                    let errMsg = await result.text();
                    mb_status.textContent += `Error: ${errMsg}<br>`;
                    showErrorMessage("Error", errMsg);
                }
            }
            catch (err) {
                networkErrorCount++;
                mb_status.textContent += `Error: ${err}<br>`;
                showErrorMessage("Error", err);
            }

            if (networkErrorCount >= 3) {
                stopProgressMonitoring();
            }
        }

        function startProgressMonitoring() {
            networkErrorCount = 0;
            stopProgressMonitoring();
            intervalTimer = setInterval(
                () => { getStatus() },
                200);
        }

        function stopProgressMonitoring() {
            if (intervalTimer) {
                clearInterval(intervalTimer);
                intervalTimer = null;
            }
        }

        function updateUiValues(jsonObject) {

            if (jsonObject.IsCompleted || jsonObject.HasError || jsonObject.IsCancelled) {
                stopProgressMonitoring();
            }

            mb_task_id.textContent = currentTaskId;

            mb_progress_bar_indicator.style.width = jsonObject.PercentCompleted + "%";
            mb_percent.textContent = jsonObject.PercentCompleted + "%";
            mb_task_type.textContent = jsonObject.TaskTypeName;
            mb_status.textContent = jsonObject.Status;
            mb_time_start.textContent = jsonObject.TimeStartDisplay;
            mb_time_end.textContent = jsonObject.TimeEndDisplay;
            mb_time_used.textContent = jsonObject.TimeUsedDisplay;

            if (jsonObject.FileDownloadWebPath.length > 0) {
                mb_filedownload.innerHTML = `<a href='${jsonObject.FileDownloadWebPath}'>${jsonObject.FileName}</a>`;
                mb_file_sha256.textContent = jsonObject.FileSha256;
            }

            if (jsonObject.TaskType == 1) {
                mb_total_rows.textContent = jsonObject.TotalRows;
                mb_current_rows.textContent = jsonObject.CurrentRows;
                mb_current_table_name.textContent = jsonObject.CurrentTableName;
                mb_current_table_index.textContent = jsonObject.CurrentTableIndex;
                mb_total_tables.textContent = jsonObject.TotalTables;
                mb_current_table_total_rows.textContent = jsonObject.TotalRowsCurrentTable;
                mb_current_table_current_rows.textContent = jsonObject.CurrentRowsCurrentTable;
            }
            else if (jsonObject.TaskType == 2) {
                mb_total_bytes.textContent = jsonObject.TotalBytes;
                mb_current_bytes.textContent = jsonObject.CurrentBytes;
            }

            mb_status.className = "";

            if (jsonObject.HasError) {
                mb_status.textContent = `${jsonObject.Status} - ${jsonObject.ErrorMsg}`;
                mb_status.className = "status_error";
                showErrorMessage("Error", jsonObject.ErrorMsg);
            }
            else if (jsonObject.IsCancelled) {
                mb_status.className = "status_error";
                showErrorMessage("Cancelled", "Task is cancelled by user");
            }
            else if (jsonObject.IsCompleted) {
                mb_status.className = "status_complete";
                showGoodMessage("Completed", "The task has completed!");
            }
            else {
                mb_status.className = "status_running";
            }
        }

        function resetUi() {

            mb_task_id.textContent = "---";
            mb_progress_bar_indicator.style.width = "0%";
            mb_percent.textContent = "0%";
            mb_task_type.textContent = "---";
            mb_status.textContent = "---";
            mb_status.className = "";
            mb_filedownload.textContent = "---";
            mb_file_sha256.textContent = "---";
            mb_time_start.textContent = "---";
            mb_time_end.textContent = "---";
            mb_time_used.textContent = "---";
            mb_total_rows.textContent = "---";
            mb_current_rows.textContent = "---";
            mb_current_table_name.textContent = "---";
            mb_total_tables.textContent = "---";
            mb_current_table_index.textContent = "---";
            mb_current_table_total_rows.textContent = "---";
            mb_current_table_current_rows.textContent = "---";
            mb_total_bytes.textContent = "---";
            mb_current_bytes.textContent = "---";
        }

        resetUi();

    </script>

</asp:Content>
