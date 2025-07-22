/**
 * MySqlBackupProgress.js - Portable Progress Report Widget for MySqlBackup.NET
 * 
 * Usage:
 * 1. Include this script in your page
 * 2. Initialize with one line: MySqlBackupProgress.init('container-id', options)
 * 
 * Example:
 *   MySqlBackupProgress.init('my-backup-widget', {
 *     apiUrl: '/apiProgressReport',
 *     onComplete: function(data) { console.log('Done!', data); }
 *   });
 */

(function (window) {
    'use strict';

    const MySqlBackupProgress = {

        init(containerId, options = {}) {
            const widget = this.create();
            return widget.init(containerId, options);
        },

        create() {

            // MySqlBackupProgress Portable Widget
            const MySqlBackupProgressWidget = {
                // Default configuration
                config: {
                    widgetId: 0,
                    apiUrl: '',
                    updateInterval: 1000,
                    fastUpdateInterval: 100,
                    container: null,
                    onComplete: null,
                    onError: null,
                    onProgress: null
                },

                // Internal state
                state: {
                    taskId: 0,
                    apiCallId: 0,
                    intervalTimer: null,
                    intervalMs: 1000,
                    elements: {}
                },

                // Initialize the widget
                init(containerId, options = {}) {
                    // Merge options with defaults
                    this.config = { ...this.config, ...options };
                    this.config.container = document.getElementById(containerId);

                    if (!this.config.container) {
                        console.error('MySqlBackupProgress: Container element not found:', containerId);
                        return false;
                    }

                    // Create the widget
                    this.render();
                    this.bindEvents();

                    return this;
                },

                // Render the widget HTML
                render() {
                    const html = `
                        <div class="div_task_status">
                            <h2>MySQL Backup Progress</h2>

                            <div class="progress_bar_container">
                                <div class="progress_bar_indicator" id="progress_bar_indicator-${this.config.widgetId}">
                                    <span class="labelPercent" id="labelPercent-${this.config.widgetId}">0%</span>
                                </div>
                            </div>

                            <div class="mbp-controls">
                                <button type="button" id="mbp-btn-backup-${this.config.widgetId}">Backup</button>
                                <button type="button" id="mbp-btn-restore-${this.config.widgetId}">Restore</button>
                                <button type="button" id="mbp-btn-stop-${this.config.widgetId}">Stop</button>
                                <input type="file" id="mbp-file-restore-${this.config.widgetId}" accept=".sql,.zip" />
                            </div>

                            <table>
                                <tr>
                                    <td>Task ID</td>
                                    <td><span id="mbp-task-id-${this.config.widgetId}">--</span></td>
                                </tr>
                                <tr>
                                    <td>Status</td>
                                    <td id="mbp-status-cell-${this.config.widgetId}">
                                        <span id="mbp-status-${this.config.widgetId}">Ready</span>
                                        <span id="mbp-status-msg-${this.config.widgetId}"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Time</td>
                                    <td>
                                        Start: <span id="mbp-time-start-${this.config.widgetId}">--</span>
                                        End: <span id="mbp-time-end-${this.config.widgetId}">--</span>
                                        Duration: <span id="mbp-time-duration-${this.config.widgetId}">--</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>File</td>
                                    <td>
                                        <span id="mbp-filename-${this.config.widgetId}">--</span><br>
                                        SHA256: <span id="mbp-sha256-${this.config.widgetId}">--</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Current Table</td>
                                    <td>
                                        <span id="mbp-table-name-${this.config.widgetId}">--</span>
                                        (<span id="mbp-table-index-${this.config.widgetId}">--</span> / <span id="mbp-table-total-${this.config.widgetId}">--</span>)
                                    </td>
                                </tr>
                                <tr>
                                    <td>All Tables Rows</td>
                                    <td>
                                        <span id="mbp-rows-current-${this.config.widgetId}">--</span> / <span id="mbp-rows-total-${this.config.widgetId}">--</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Current Table Rows</td>
                                    <td>
                                        <span id="mbp-table-rows-current-${this.config.widgetId}">--</span> / <span id="mbp-table-rows-total-${this.config.widgetId}">--</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Total Bytes</td>
                                    <td>
                                        <span id="mbp-bytes-current-${this.config.widgetId}">--</span> / <span id="mbp-bytes-total-${this.config.widgetId}">--</span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    `;

                    this.config.container.innerHTML = html;

                    // Cache element references
                    this.state.elements = {
                        progressBar: document.getElementById(`progress_bar_indicator-${this.config.widgetId}`),
                        percent: document.getElementById(`labelPercent-${this.config.widgetId}`),
                        fileRestore: document.getElementById(`mbp-file-restore-${this.config.widgetId}`),
                        taskId: document.getElementById(`mbp-task-id-${this.config.widgetId}`),
                        status: document.getElementById(`mbp-status-${this.config.widgetId}`),
                        statusMsg: document.getElementById(`mbp-status-msg-${this.config.widgetId}`),
                        statusCell: document.getElementById(`mbp-status-cell-${this.config.widgetId}`),
                        timeStart: document.getElementById(`mbp-time-start-${this.config.widgetId}`),
                        timeEnd: document.getElementById(`mbp-time-end-${this.config.widgetId}`),
                        timeDuration: document.getElementById(`mbp-time-duration-${this.config.widgetId}`),
                        filename: document.getElementById(`mbp-filename-${this.config.widgetId}`),
                        sha256: document.getElementById(`mbp-sha256-${this.config.widgetId}`),
                        tableName: document.getElementById(`mbp-table-name-${this.config.widgetId}`),
                        tableIndex: document.getElementById(`mbp-table-index-${this.config.widgetId}`),
                        tableTotal: document.getElementById(`mbp-table-total-${this.config.widgetId}`),
                        rowsCurrent: document.getElementById(`mbp-rows-current-${this.config.widgetId}`),
                        rowsTotal: document.getElementById(`mbp-rows-total-${this.config.widgetId}`),
                        tableRowsCurrent: document.getElementById(`mbp-table-rows-current-${this.config.widgetId}`),
                        tableRowsTotal: document.getElementById(`mbp-table-rows-total-${this.config.widgetId}`),
                        bytesCurrent: document.getElementById(`mbp-bytes-current-${this.config.widgetId}`),
                        bytesTotal: document.getElementById(`mbp-bytes-total-${this.config.widgetId}`)
                    };
                },

                // Bind events
                bindEvents() {
                    document.getElementById(`mbp-btn-backup-${this.config.widgetId}`).addEventListener('click', () => this.backup());
                    document.getElementById(`mbp-btn-restore-${this.config.widgetId}`).addEventListener('click', () => this.restore());
                    document.getElementById(`mbp-btn-stop-${this.config.widgetId}`).addEventListener('click', () => this.stopTask());
                },

                // FetchAPI Helper
                async fetchData(formData) {

                    try {
                        let action = formData.get("action") || "";

                        const response = await fetch(this.config.apiUrl, {
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
                },

                // Backup function
                async backup() {

                    this.resetUI();

                    const formData = new FormData();
                    formData.append('action', 'backup');

                    const result = await this.fetchData(formData);

                    if (result.ok) {
                        this.state.taskId = result.thisTaskid;
                        this.state.intervalMs = 1000;
                        this.startMonitoring();
                        showGoodMessage("Success", "Backup Task Begin");
                    } else {
                        showErrorMessage("Error", result.errMsg);
                    }
                },

                // Restore function
                async restore() {
                    this.resetUI();

                    if (!this.state.elements.fileRestore.files || this.state.elements.fileRestore.files.length === 0) {
                        showErrorMessage("Error", "Please select a file to restore");
                        return;
                    }

                    const formData = new FormData();
                    formData.append('action', 'restore');
                    formData.append('fileRestore', this.state.elements.fileRestore.files[0]);

                    const result = await this.fetchData(formData);

                    if (result.ok) {
                        this.state.taskId = result.thisTaskid;
                        this.state.intervalMs = 1000;
                        this.startMonitoring();
                        showGoodMessage("Success", "Restore Task Begin");
                    } else {
                        showErrorMessage("Error", result.errMsg);
                    }
                },

                // Stop function
                async stopTask() {
                    if (!this.state.taskId || this.state.taskId === 0) {
                        showErrorMessage("Error", "No active task to stop");
                        return;
                    }

                    const formData = new FormData();
                    formData.append("action", "stoptask");
                    formData.append("taskid", this.state.taskId);

                    const result = await this.fetchData(formData);

                    if (result.ok) {
                        showGoodMessage("The task is being called to stop.");
                    } else {
                        showErrorMessage("Error", result.errMsg);
                        this.stopMonitoring();
                    }
                },

                // Monitor progress
                async fetchStatus() {
                    this.state.apiCallId++;

                    const formData = new FormData();
                    formData.append('action', 'gettaskstatus');
                    formData.append('taskid', this.state.taskId);
                    formData.append('apicallid', this.state.apiCallId);

                    const result = await this.fetchData(formData);

                    if (result.ok) {
                        if (result.jsonObject.ApiCallIndex != this.state.apiCallId) {
                            // late echo response - ignore
                            return;
                        }

                        console.log("before updateUIValues");
                        console.log(result.jsonObject);
                        this.updateUI(result.jsonObject);
                    } else {
                        showErrorMessage("Error", result.errMsg);
                        this.stopMonitoring();
                    }
                },

                // Update UI with progress data
                updateUI(data) {
                    // Optimize update frequency when task starts
                    if (data.PercentCompleted > 0 && this.state.intervalMs === this.config.updateInterval) {
                        this.state.intervalMs = this.config.fastUpdateInterval;
                        this.stopMonitoring();
                        setTimeout(() => this.startMonitoring(), 500);
                    }

                    // Stop monitoring when task completes
                    if (data.IsCompleted || data.HasError || data.IsCancelled) {
                        this.stopMonitoring();

                        if (this.config.onComplete && data.IsCompleted && !data.HasError && !data.IsCancelled) {
                            this.config.onComplete(data);
                        }
                    }

                    // Update progress bar
                    const percent = data.PercentCompleted || 0;
                    this.state.elements.progressBar.style.width = percent + '%';
                    this.state.elements.percent.textContent = percent + '%';

                    // Update basic info
                    this.state.elements.taskId.textContent = data.TaskId || '--';
                    this.state.elements.timeStart.textContent = data.TimeStartDisplay || '--';
                    this.state.elements.timeEnd.textContent = data.TimeEndDisplay || '--';
                    this.state.elements.timeDuration.textContent = data.TimeUsedDisplay || '--';

                    // Update status
                    if (data.HasError) {
                        this.state.elements.status.textContent = 'Error';
                        this.state.elements.statusMsg.textContent = data.ErrorMsg || '';
                        this.state.elements.statusCell.className = 'status-error';
                        if (this.config.onError) {
                            this.config.onError(data.ErrorMsg);
                        }
                    } else if (data.IsCancelled) {
                        this.state.elements.status.textContent = 'Cancelled';
                        this.state.elements.statusMsg.textContent = '';
                        this.state.elements.statusCell.className = 'status-error';
                    } else if (data.IsCompleted) {
                        this.state.elements.status.textContent = 'Completed';
                        this.state.elements.statusMsg.textContent = '';
                        this.state.elements.statusCell.className = 'status-complete';
                    } else {
                        this.state.elements.status.textContent = 'Running';
                        this.state.elements.statusMsg.textContent = '';
                        this.state.elements.statusCell.className = 'status-running';
                    }

                    // Update file info
                    if (data.FileName && data.FileName.length > 0) {
                        this.state.elements.filename.innerHTML =
                            `<a href="${data.FileDownloadUrl}" target="_blank">${data.FileName}</a>`;
                    } else {
                        this.state.elements.filename.textContent = data.FileName || '--';
                    }
                    this.state.elements.sha256.textContent = data.SHA256 || '--';

                    // Update backup-specific info
                    if (data.TaskType === 1) {
                        this.state.elements.tableName.textContent = data.CurrentTableName || '--';
                        this.state.elements.tableIndex.textContent = data.CurrentTableIndex || '--';
                        this.state.elements.tableTotal.textContent = data.TotalTables || '--';
                        this.state.elements.rowsCurrent.textContent = data.CurrentRowIndex || '--';
                        this.state.elements.rowsTotal.textContent = data.TotalRows || '--';
                        this.state.elements.tableRowsCurrent.textContent = data.CurrentRowCurrentTable || '--';
                        this.state.elements.tableRowsTotal.textContent = data.TotalRowsCurrentTable || '--';

                        // Clear restore fields
                        this.state.elements.bytesCurrent.textContent = '--';
                        this.state.elements.bytesTotal.textContent = '--';
                    }

                    // Update restore-specific info
                    if (data.TaskType === 2) {
                        this.state.elements.bytesCurrent.textContent = this.formatBytes(data.CurrentBytes) || '--';
                        this.state.elements.bytesTotal.textContent = this.formatBytes(data.TotalBytes) || '--';

                        // Clear backup fields
                        this.state.elements.tableName.textContent = '--';
                        this.state.elements.tableIndex.textContent = '--';
                        this.state.elements.tableTotal.textContent = '--';
                        this.state.elements.rowsCurrent.textContent = '--';
                        this.state.elements.rowsTotal.textContent = '--';
                        this.state.elements.tableRowsCurrent.textContent = '--';
                        this.state.elements.tableRowsTotal.textContent = '--';
                    }

                    // Call progress callback
                    if (this.config.onProgress) {
                        this.config.onProgress(data);
                    }
                },

                // Start monitoring
                startMonitoring() {
                    this.stopMonitoring();
                    this.state.intervalTimer = setInterval(() => this.fetchStatus(), this.state.intervalMs);
                },

                // Stop monitoring
                stopMonitoring() {
                    if (this.state.intervalTimer) {
                        clearInterval(this.state.intervalTimer);
                        this.state.intervalTimer = null;
                    }
                },

                // Reset UI
                resetUI() {
                    const elementsToReset = [
                        'taskId', 'timeStart', 'timeEnd', 'timeDuration',
                        'filename', 'sha256', 'tableName', 'tableIndex', 'tableTotal',
                        'rowsCurrent', 'rowsTotal', 'tableRowsCurrent', 'tableRowsTotal',
                        'bytesCurrent', 'bytesTotal'
                    ];

                    elementsToReset.forEach(elementKey => {
                        if (this.state.elements[elementKey]) {
                            this.state.elements[elementKey].textContent = '--';
                        }
                    });

                    // Reset progress bar
                    this.state.elements.progressBar.style.width = '0%';
                    this.state.elements.percent.textContent = '0%';
                    this.state.elements.status.textContent = 'Ready';
                    this.state.elements.statusMsg.textContent = '';
                    this.state.elements.statusCell.className = '';
                },

                // Show error
                showError(message) {
                    this.state.elements.status.textContent = 'Error';
                    this.state.elements.statusMsg.textContent = message;
                    this.state.elements.statusCell.className = 'mbp-status-error';
                    console.error('MySqlBackupProgress:', message);
                },

                // Format bytes
                formatBytes(bytes) {
                    if (!bytes || bytes === 0) return '0 Bytes';
                    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
                    const i = Math.floor(Math.log(bytes) / Math.log(1024));
                    return Math.round(bytes / Math.pow(1024, i) * 100) / 100 + ' ' + sizes[i];
                },

                // Public methods
                getStatus() {
                    return {
                        taskId: this.state.taskId,
                        isRunning: this.state.intervalTimer !== null
                    };
                },

                destroy() {
                    this.stopMonitoring();
                    if (this.config.container) {
                        this.config.container.innerHTML = '';
                    }
                }
            };

            return MySqlBackupProgressWidget;
        }

    };

    // Expose to global scope
    window.MySqlBackupProgress = MySqlBackupProgress;

})(window);