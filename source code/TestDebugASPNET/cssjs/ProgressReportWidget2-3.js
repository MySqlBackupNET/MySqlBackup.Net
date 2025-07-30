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
                                <button type="button" id="mbp-btn-remove-${this.config.widgetId}">Remove</button>
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
                    document.getElementById(`mbp-btn-remove-${this.config.widgetId}`).addEventListener('click', () => this.remove());
                },

                async backup() {
                    this.resetUI();

                    try {
                        const formData = new FormData();
                        formData.append('action', 'start_backup');

                        const response = await fetch(this.config.apiUrl, {
                            method: 'POST',
                            body: formData
                        });

                        if (response.ok) {
                            const jsonObject = await response.json();
                            this.state.taskId = jsonObject.TaskId;
                            this.state.intervalMs = this.config.updateInterval;
                            this.startMonitoring();
                            this.showSuccess("Backup task started");
                        } else {
                            const errorText = await response.text();
                            this.showError(`HTTP ${response.status}: ${errorText}`);
                        }
                    } catch (error) {
                        this.showError(error.message);
                    }
                },

                async restore() {
                    this.resetUI();

                    if (!this.state.elements.fileRestore.files || this.state.elements.fileRestore.files.length === 0) {
                        this.showError("Please select a file to restore");
                        return;
                    }

                    try {
                        const formData = new FormData();
                        formData.append('action', 'start_restore');
                        formData.append('file', this.state.elements.fileRestore.files[0]);

                        const response = await fetch(this.config.apiUrl, {
                            method: 'POST',
                            body: formData
                        });

                        if (response.ok) {
                            const jsonObject = await response.json();
                            this.state.taskId = jsonObject.TaskId;
                            this.state.intervalMs = this.config.updateInterval;
                            this.startMonitoring();
                            this.showSuccess("Restore task started");
                        } else {
                            const errorText = await response.text();
                            this.showError(`HTTP ${response.status}: ${errorText}`);
                        }
                    } catch (error) {
                        this.showError(error.message);
                    }
                },

                async stopTask() {
                    if (!this.state.taskId || this.state.taskId === 0) {
                        this.showError("No active task to stop");
                        return;
                    }

                    try {
                        const formData = new FormData();
                        formData.append("action", "stop_task");
                        formData.append("taskid", this.state.taskId);

                        const response = await fetch(this.config.apiUrl, {
                            method: 'POST',
                            body: formData
                        });

                        if (response.ok) {
                            this.showSuccess("Task is being stopped");
                        } else {
                            const errorText = await response.text();
                            this.showError(`HTTP ${response.status}: ${errorText}`);
                            this.stopMonitoring();
                        }
                    } catch (error) {
                        this.showError(error.message);
                        this.stopMonitoring();
                    }
                },

                async fetchStatus() {
                    this.state.apiCallId++;

                    try {
                        const formData = new FormData();
                        formData.append('action', 'get_status');
                        formData.append('taskid', this.state.taskId);
                        formData.append('api_call_index', this.state.apiCallId);

                        const response = await fetch(this.config.apiUrl, {
                            method: 'POST',
                            body: formData
                        });

                        if (response.ok) {
                            const jsonObject = await response.json();

                            if (jsonObject.ApiCallIndex != this.state.apiCallId) {
                                // late echo response - ignore
                                return;
                            }

                            console.log("before updateUIValues");
                            console.log(jsonObject);
                            this.updateUI(jsonObject);
                        } else {
                            const errorText = await response.text();
                            this.showError(`HTTP ${response.status}: ${errorText}`);
                            this.stopMonitoring();
                        }
                    } catch (error) {
                        this.showError(error.message);
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
                    this.state.elements.taskId.textContent = data.TaskId;
                    this.state.elements.timeStart.textContent = data.TimeStartDisplay;
                    this.state.elements.timeEnd.textContent = data.TimeEndDisplay;
                    this.state.elements.timeDuration.textContent = data.TimeUsedDisplay;

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
                            `<a href="${data.FileDownloadWebPath}" target="_blank">${data.FileName}</a>`;
                    } else {
                        this.state.elements.filename.textContent = data.FileName;
                    }
                    this.state.elements.sha256.textContent = data.FileSha256;

                    // Update backup-specific info
                    if (data.TaskType === 1) {
                        this.state.elements.tableName.textContent = data.CurrentTableName;
                        this.state.elements.tableIndex.textContent = data.CurrentTableIndex;
                        this.state.elements.tableTotal.textContent = data.TotalTables;
                        this.state.elements.rowsCurrent.textContent = data.CurrentRows;
                        this.state.elements.rowsTotal.textContent = data.TotalRows;
                        this.state.elements.tableRowsCurrent.textContent = data.CurrentRowsCurrentTable;
                        this.state.elements.tableRowsTotal.textContent = data.TotalRowsCurrentTable;
                    }

                    // Update restore-specific info
                    if (data.TaskType === 2) {
                        this.state.elements.bytesCurrent.textContent = this.formatBytes(data.CurrentBytes);
                        this.state.elements.bytesTotal.textContent = this.formatBytes(data.TotalBytes);
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

                remove() {
                    // Stop any running monitoring
                    this.stopMonitoring();

                    // Clear the task state
                    this.state.taskId = 0;
                    this.state.apiCallId = 0;

                    // Remove all event listeners by cloning and replacing elements
                    const buttonsToClean = [
                        `mbp-btn-backup-${this.config.widgetId}`,
                        `mbp-btn-restore-${this.config.widgetId}`,
                        `mbp-btn-stop-${this.config.widgetId}`,
                        `mbp-btn-remove-${this.config.widgetId}`
                    ];

                    buttonsToClean.forEach(buttonId => {
                        const button = document.getElementById(buttonId);
                        if (button) {
                            const newButton = button.cloneNode(true);
                            button.parentNode.replaceChild(newButton, button);
                        }
                    });

                    // Clear element references
                    this.state.elements = {};

                    // Remove the widget from DOM
                    if (this.config.container) {
                        this.config.container.innerHTML = '';
                        this.config.container.removeAttribute('data-mysqlbackup-widget');
                    }

                    // Clear configuration
                    this.config.container = null;
                    this.config.onComplete = null;
                    this.config.onError = null;
                    this.config.onProgress = null;

                    console.log('MySqlBackupProgress: Widget removed from DOM');
                },

                showSuccess(msg) {
                    try {
                        showGoodMessage("Ok", msg);
                    }
                    catch (err) {
                        console.log(msg);
                        conosle.log(err);
                    }
                },

                showError(msg) {
                    try {
                        showErrorMessage("Error", msg);
                    }
                    catch (err) {
                        console.log(msg);
                        conosle.log(err);
                    }
                },

                // Also update the destroy() method to be more thorough:
                destroy() {
                    this.remove(); // Use the remove method for complete cleanup
                }
            };

            return MySqlBackupProgressWidget;
        }

    };

    // Expose to global scope
    window.MySqlBackupProgress = MySqlBackupProgress;

})(window);