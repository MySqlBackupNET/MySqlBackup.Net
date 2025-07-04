<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="ProgressReport2.aspx.cs" Inherits="System.pages.ProgressReport2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .task-container {
            display: flex;
            flex-direction: column;
            gap: 16px;
        }

        .task-block {
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            padding: 16px;
            position: relative;
            transition: all 0.3s ease;
        }

            .task-block:hover {
                box-shadow: 0 4px 8px rgba(0,0,0,0.1);
            }

            /* Status color themes */
            .task-block.running {
                background-color: #e3f2fd;
                border-color: #2196f3;
            }

            .task-block.completed {
                background-color: #e8f5e8;
                border-color: #4caf50;
            }

            .task-block.error {
                background-color: #ffebee;
                border-color: #f44336;
            }

            .task-block.cancelled {
                background-color: #f5f5f5;
                border-color: #9e9e9e;
            }

            .task-block.pending {
                background-color: #fff3e0;
                border-color: #ff9800;
            }

        /* Progress bar container */
        .progress-container {
            width: 100%;
            height: 8px;
            background-color: #e0e0e0;
            border-radius: 4px;
            overflow: hidden;
            margin-bottom: 16px;
        }

        .progress-bar {
            height: 100%;
            transition: width 0.3s ease;
            border-radius: 4px;
        }

            .progress-bar.running {
                background-color: #2196f3;
            }

            .progress-bar.completed {
                background-color: #4caf50;
            }

            .progress-bar.error {
                background-color: #f44336;
            }

            .progress-bar.cancelled {
                background-color: #9e9e9e;
            }

            .progress-bar.pending {
                background-color: #ff9800;
            }

        /* Task header */
        .task-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 12px;
        }

        .task-title {
            font-size: 16px;
            font-weight: 600;
            color: #333;
        }

        .task-id {
            font-size: 12px;
            color: #666;
            background-color: #f0f0f0;
            padding: 2px 6px;
            border-radius: 3px;
        }

        /* Task content grid */
        .task-content {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 16px;
            margin-bottom: 16px;
        }

        .task-info-group {
            display: flex;
            flex-direction: column;
            gap: 8px;
        }

        .task-info-item {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 4px 0;
        }

        .task-info-label {
            font-size: 12px;
            color: #666;
            font-weight: 500;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        .task-info-value {
            font-size: 14px;
            color: #333;
            font-weight: 400;
        }

        /* Progress details for backup */
        .progress-details {
            grid-column: 1 / -1;
            background-color: rgba(255,255,255,0.5);
            padding: 12px;
            border-radius: 4px;
            margin-top: 8px;
        }

        .progress-details-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 12px;
        }

        /* Error message */
        .error-message {
            grid-column: 1 / -1;
            background-color: rgba(244, 67, 54, 0.1);
            color: #d32f2f;
            padding: 8px 12px;
            border-radius: 4px;
            font-size: 13px;
            margin-top: 8px;
        }

        /* Status badge */
        .status-badge {
            padding: 4px 8px;
            border-radius: 12px;
            font-size: 11px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

            .status-badge.running {
                background-color: #2196f3;
                color: white;
            }

            .status-badge.completed {
                background-color: #4caf50;
                color: white;
            }

            .status-badge.error {
                background-color: #f44336;
                color: white;
            }

            .status-badge.cancelled {
                background-color: #9e9e9e;
                color: white;
            }

            .status-badge.pending {
                background-color: #ff9800;
                color: white;
            }

        /* Action buttons */
        .task-actions {
            display: flex;
            gap: 8px;
            flex-wrap: wrap;
        }

        .btn {
            padding: 8px 16px;
            border: none;
            border-radius: 4px;
            font-size: 13px;
            font-weight: 500;
            cursor: pointer;
            transition: all 0.2s ease;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            gap: 4px;
        }

            .btn:hover {
                transform: translateY(-1px);
                box-shadow: 0 2px 4px rgba(0,0,0,0.2);
            }

        .btn-cancel {
            background-color: #ff9800;
            color: white;
        }

            .btn-cancel:hover {
                background-color: #f57c00;
            }

        .btn-delete {
            background-color: #f44336;
            color: white;
        }

            .btn-delete:hover {
                background-color: #d32f2f;
            }

        .btn-download {
            background-color: #2196f3;
            color: white;
        }

            .btn-download:hover {
                background-color: #1976d2;
            }

        .btn:disabled {
            opacity: 0.6;
            cursor: not-allowed;
        }

            .btn:disabled:hover {
                transform: none;
                box-shadow: none;
            }

        /* Control panel */
        .control-panel {
            background-color: #f8f9fa;
            padding: 20px;
            border-radius: 8px;
            margin-bottom: 20px;
            border: 1px solid #e0e0e0;
        }

            .control-panel h3 {
                margin-top: 0;
                color: #333;
                font-size: 18px;
            }

        .control-group {
            display: flex;
            gap: 16px;
            align-items: center;
            margin-bottom: 16px;
        }

            .control-group:last-child {
                margin-bottom: 0;
            }

            .control-group label {
                font-weight: 500;
                color: #555;
                min-width: 120px;
            }

        .file-input-wrapper {
            position: relative;
            display: inline-block;
        }

            .file-input-wrapper input[type=file] {
                position: absolute;
                opacity: 0;
                width: 100%;
                height: 100%;
                cursor: pointer;
            }

        .file-input-display {
            padding: 8px 12px;
            border: 1px solid #ddd;
            border-radius: 4px;
            background-color: white;
            min-width: 200px;
            cursor: pointer;
            color: #666;
        }

            .file-input-display:hover {
                border-color: #bbb;
            }

        /* Responsive design */
        @media (max-width: 768px) {
            .task-content {
                grid-template-columns: 1fr;
            }

            .progress-details-grid {
                grid-template-columns: 1fr;
            }

            .task-header {
                flex-direction: column;
                align-items: flex-start;
                gap: 8px;
            }

            .control-group {
                flex-direction: column;
                align-items: flex-start;
            }
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">
        <h1>MySQL Backup/Restore Task Monitor</h1>

        <!-- Control Panel -->
        <div class="control-panel">
            <h3>Task Controls</h3>

            <div class="control-group">
                <label>Create Backup:</label>
                <button type="button" id="btnNewBackup" class="buttonmain">Start New Backup</button>
                <span style="color: #666; font-size: 12px;">Creates a new database backup task</span>
            </div>

            <div class="control-group">
                <label>Restore Database:</label>
                <div class="file-input-wrapper">
                    <input type="file" id="fileRestore" accept=".sql,.zip" />
                    <div class="file-input-display" id="fileRestoreDisplay">Choose backup file (.sql or .zip)</div>
                </div>
                <button type="button" id="btnNewRestore" class="buttonmain">Start Restore</button>
                <span style="color: #666; font-size: 12px;">Upload and restore from backup file</span>
            </div>
        </div>

        <!-- Task Container -->
        <div id="divtask" class="task-container">
            <!-- Tasks will be dynamically added here -->
        </div>
    </div>

    <script>
        // Global configuration
        var msTimeWhenActive = 250;
        var msTimeWhenNoActive = 1000;

        // Global variables
        var apiCallId = 0;
        var intervalTimer = null;
        var activeTasks = new Set(); // Track active task IDs
        var networkErrorState = false;

        // Initialize on page load
        document.addEventListener('DOMContentLoaded', function () {
            // Wait 1000ms then start monitoring

            // Add button event handlers
            document.getElementById('btnNewBackup').addEventListener('click', createNewBackup);
            document.getElementById('btnNewRestore').addEventListener('click', createNewRestore);

            // File input handler
            document.getElementById('fileRestore').addEventListener('change', function () {
                var display = document.getElementById('fileRestoreDisplay');
                if (this.files.length > 0) {
                    display.textContent = this.files[0].name;
                } else {
                    display.textContent = 'Choose backup file (.sql or .zip)';
                }
            });

            // Wait 1000ms then start monitoring
            setTimeout(function () {
                startMonitoring();
            }, 250);
        });

        // Start monitoring tasks
        function startMonitoring() {
            fetchAllTaskStatus();
            startIntervalTimer();
        }

        // Interval timer management
        function startIntervalTimer() {
            if (intervalTimer) {
                clearInterval(intervalTimer);
            }

            var intervalTime = activeTasks.size > 0 ? msTimeWhenActive : msTimeWhenNoActive;

            intervalTimer = setInterval(function () {
                if (!networkErrorState) {
                    if (activeTasks.size > 0) {
                        fetchSpecificAndActiveTaskStatus();
                    } else {
                        fetchAllTaskStatus();
                    }
                }
            }, intervalTime);
        }

        // Update interval based on active tasks
        function updateIntervalTimer() {
            var newInterval = activeTasks.size > 0 ? msTimeWhenActive : msTimeWhenNoActive;
            startIntervalTimer();
        }

        // 1st function: Draw task skeleton
        function drawTaskBlock(taskId) {
            var taskContainer = document.getElementById('divtask');

            var taskBlock = document.createElement('div');
            taskBlock.className = 'task-block';
            taskBlock.id = 'task-' + taskId;

            taskBlock.innerHTML = `
        <div class="progress-container">
            <div class="progress-bar" style="width: 0%;"></div>
        </div>
        
        <div class="task-header">
            <div class="task-title"></div>
            <div class="task-id">Task #${taskId}</div>
        </div>
        
        <div class="task-content">
            <div class="task-info-group">
                <div class="task-info-item">
                    <span class="task-info-label">Status</span>
                    <span class="status-badge">-</span>
                </div>
                <div class="task-info-item">
                    <span class="task-info-label">Progress</span>
                    <span class="task-info-value progress-percent">0%</span>
                </div>
                <div class="task-info-item">
                    <span class="task-info-label">Started</span>
                    <span class="task-info-value time-start">-</span>
                </div>
                <div class="task-info-item time-end-item" style="display: none;">
                    <span class="task-info-label">Completed</span>
                    <span class="task-info-value time-end">-</span>
                </div>
                <div class="task-info-item time-used-item">
                    <span class="task-info-label">Time Used</span>
                    <span class="task-info-value time-used">-</span>
                </div>
            </div>
            
            <div class="task-info-group">
                <div class="backup-specific" style="display: none;">
                    <div class="task-info-item">
                        <span class="task-info-label">Current Table</span>
                        <span class="task-info-value current-table">-</span>
                    </div>
                    <div class="task-info-item">
                        <span class="task-info-label">Table Progress</span>
                        <span class="task-info-value table-progress">-</span>
                    </div>
                    <div class="task-info-item">
                        <span class="task-info-label">Rows Processed</span>
                        <span class="task-info-value rows-processed">-</span>
                    </div>
                </div>
                <div class="restore-specific" style="display: none;">
                    <div class="task-info-item">
                        <span class="task-info-label">File Name</span>
                        <span class="task-info-value file-name">-</span>
                    </div>
                    <div class="task-info-item">
                        <span class="task-info-label">Bytes Processed</span>
                        <span class="task-info-value bytes-processed">-</span>
                    </div>
                </div>
            </div>
            
            <div class="progress-details" style="display: none;">
                <div class="progress-details-grid">
                    <!-- Backup details -->
                    <div class="backup-details" style="display: none;">
                        <div class="task-info-item">
                            <span class="task-info-label">Total Tables</span>
                            <span class="task-info-value total-tables">-</span>
                        </div>
                        <div class="task-info-item">
                            <span class="task-info-label">Current Table Index</span>
                            <span class="task-info-value current-table-index">-</span>
                        </div>
                        <div class="task-info-item">
                            <span class="task-info-label">Total Rows</span>
                            <span class="task-info-value total-rows">-</span>
                        </div>
                        <div class="task-info-item">
                            <span class="task-info-label">Current Row Index</span>
                            <span class="task-info-value current-row-index">-</span>
                        </div>
                    </div>
                    <!-- Restore details -->
                    <div class="restore-details" style="display: none;">
                        <div class="task-info-item">
                            <span class="task-info-label">Total Bytes</span>
                            <span class="task-info-value total-bytes">-</span>
                        </div>
                        <div class="task-info-item">
                            <span class="task-info-label">Current Bytes</span>
                            <span class="task-info-value current-bytes">-</span>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="error-message" style="display: none;"></div>
        </div>
        
        <div class="task-actions">
            <button type="button" class="btn btn-cancel" onclick="stopTask(${taskId})" style="display: none;">Cancel</button>
            <a href="#" class="btn btn-download" style="display: none;">Download</a>
            <button type="button" class="btn btn-delete" onclick="deleteTaskAndFile(${taskId})" style="display: none;">Delete</button>
        </div>
    `;

            taskContainer.appendChild(taskBlock);
        }

        // 2nd function: Fill/update task values
        function fillTaskValues(task) {
            var taskBlock = document.getElementById('task-' + task.TaskId);
            if (!taskBlock) return;

            // Update title
            taskBlock.querySelector('.task-title').textContent = task.TaskTypeName;

            // Update status and theme
            var statusClass = getStatusClass(task);
            taskBlock.className = 'task-block ' + statusClass;

            var statusBadge = taskBlock.querySelector('.status-badge');
            statusBadge.className = 'status-badge ' + statusClass;
            statusBadge.textContent = getStatusText(task);

            // Update progress bar
            var progressBar = taskBlock.querySelector('.progress-bar');
            progressBar.className = 'progress-bar ' + statusClass;
            progressBar.style.width = task.PercentCompleted + '%';

            // Update progress percentage
            taskBlock.querySelector('.progress-percent').textContent = task.PercentCompleted + '%';

            // Update times
            taskBlock.querySelector('.time-start').textContent = task.TimeStartDisplay || '-';

            if (task.IsCompleted) {
                var timeEndItem = taskBlock.querySelector('.time-end-item');
                timeEndItem.style.display = 'block';
                taskBlock.querySelector('.time-end').textContent = task.TimeEndDisplay || '-';
            }

            // Calculate and display time used
            if (task.IsStarted) {
                var timeUsed = calculateTimeUsed(task);
                taskBlock.querySelector('.time-used').textContent = timeUsed;
            }

            // Show/hide task type specific info
            if (task.TaskType === 1) { // Backup
                taskBlock.querySelector('.backup-specific').style.display = 'block';
                taskBlock.querySelector('.restore-specific').style.display = 'none';

                // Update backup specific info
                taskBlock.querySelector('.current-table').textContent = task.CurrentTableName || '-';
                taskBlock.querySelector('.table-progress').textContent =
                    task.CurrentTableIndex + ' of ' + task.TotalTables;
                taskBlock.querySelector('.rows-processed').textContent =
                    formatNumber(task.CurrentRowIndex) + ' / ' + formatNumber(task.TotalRows);

                // Show progress details for running tasks
                if (!task.IsCompleted && task.IsStarted) {
                    taskBlock.querySelector('.progress-details').style.display = 'block';
                    taskBlock.querySelector('.backup-details').style.display = 'grid';
                    taskBlock.querySelector('.restore-details').style.display = 'none';

                    taskBlock.querySelector('.total-tables').textContent = task.TotalTables;
                    taskBlock.querySelector('.current-table-index').textContent = task.CurrentTableIndex;
                    taskBlock.querySelector('.total-rows').textContent = formatNumber(task.TotalRows);
                    taskBlock.querySelector('.current-row-index').textContent = formatNumber(task.CurrentRowIndex);
                }
            } else if (task.TaskType === 2) { // Restore
                taskBlock.querySelector('.backup-specific').style.display = 'none';
                taskBlock.querySelector('.restore-specific').style.display = 'block';

                // Update restore specific info
                taskBlock.querySelector('.file-name').textContent = task.FileName || '-';
                taskBlock.querySelector('.bytes-processed').textContent =
                    formatBytes(task.CurrentBytes) + ' / ' + formatBytes(task.TotalBytes);

                // Show progress details for running tasks
                if (!task.IsCompleted && task.IsStarted) {
                    taskBlock.querySelector('.progress-details').style.display = 'block';
                    taskBlock.querySelector('.backup-details').style.display = 'none';
                    taskBlock.querySelector('.restore-details').style.display = 'grid';

                    taskBlock.querySelector('.total-bytes').textContent = formatNumber(task.TotalBytes);
                    taskBlock.querySelector('.current-bytes').textContent = formatNumber(task.CurrentBytes);
                }
            }

            // Handle error message
            if (task.HasError) {
                var errorMsg = taskBlock.querySelector('.error-message');
                errorMsg.style.display = 'block';
                errorMsg.textContent = task.ErrorMsg;
            }

            // Update action buttons
            updateActionButtons(taskBlock, task);

            // Track active tasks
            if (!task.IsCompleted) {
                activeTasks.add(task.TaskId);
            } else {
                activeTasks.delete(task.TaskId);
            }
        }

        // Update action buttons visibility
        function updateActionButtons(taskBlock, task) {
            var cancelBtn = taskBlock.querySelector('.btn-cancel');
            var downloadBtn = taskBlock.querySelector('.btn-download');
            var deleteBtn = taskBlock.querySelector('.btn-delete');

            if (!task.IsCompleted && !task.RequestCancel) {
                cancelBtn.style.display = 'inline-flex';
            } else {
                cancelBtn.style.display = 'none';
            }

            if (task.IsCompleted && task.FileDownloadUrl && !task.HasError) {
                downloadBtn.style.display = 'inline-flex';
                downloadBtn.href = task.FileDownloadUrl;
            } else {
                downloadBtn.style.display = 'none';
            }

            if (task.IsCompleted || task.HasError) {
                deleteBtn.style.display = 'inline-flex';
            } else {
                deleteBtn.style.display = 'none';
            }
        }

        // 3rd function: Fetch all task status
        function fetchAllTaskStatus() {
            apiCallId++;
            var currentCallId = apiCallId;

            fetch('/apiProgressReport2?action=getallstatus&apicallid=' + currentCallId)
                .then(response => response.json())
                .then(data => {
                    if (data.apicallid !== currentCallId) {
                        // Late echo, ignore
                        return;
                    }

                    networkErrorState = false;
                    hideNetworkError();

                    // Clear existing tasks
                    document.getElementById('divtask').innerHTML = '';
                    activeTasks.clear();

                    // Sort tasks by TaskId in descending order, then process each task
                    data.lstTask.sort((a, b) => b.TaskId - a.TaskId).forEach(task => {
                        drawTaskBlock(task.TaskId);
                        fillTaskValues(task);
                    });

                    updateIntervalTimer();
                })
                .catch(error => {
                    handleNetworkError();
                });
        }

        // 4th function: Fetch specific and active task status
        function fetchSpecificAndActiveTaskStatus() {
            var taskIds = Array.from(activeTasks).join(',');

            if (!taskIds) {
                fetchAllTaskStatus();
                return;
            }

            apiCallId++;
            var currentCallId = apiCallId;

            fetch('/apiProgressReport2?action=getspecificandactivestatus&apicallid=' + currentCallId + '&taskidstr=' + taskIds)
                .then(response => response.json())
                .then(data => {
                    if (data.apicallid !== currentCallId) {
                        // Late echo, ignore
                        return;
                    }

                    networkErrorState = false;
                    hideNetworkError();

                    // Process each task
                    data.lstTask.sort((a, b) => b.TaskId - a.TaskId).forEach(task => {
                        var taskBlock = document.getElementById('task-' + task.TaskId);

                        if (!taskBlock) {
                            // New task, draw it
                            drawTaskBlock(task.TaskId);
                        }

                        fillTaskValues(task);
                    });

                    updateIntervalTimer();
                })
                .catch(error => {
                    handleNetworkError();
                });
        }

        // Stop task function
        function stopTask(taskId) {
            showBigLoading(3000);

            fetch('/apiProgressReport2?action=stoptask&taskid=' + taskId)
                .then(response => {
                    closeBigLoading();
                    // Continue fetching status
                    if (activeTasks.size > 0) {
                        fetchSpecificAndActiveTaskStatus();
                    } else {
                        fetchAllTaskStatus();
                    }
                })
                .catch(error => {
                    closeBigLoading();
                    showErrorMessage('Error', 'Failed to stop task');
                });
        }

        // Delete task and file function
        function deleteTaskAndFile(taskId) {
            spShowConfirmDialog(
                'Delete Task',
                'Are you sure you want to delete this task and its files?',
                taskId,
                function (data) {
                    showBigLoading(3000);

                    fetch('/apiProgressReport2?action=deletetaskfile&taskid=' + data)
                        .then(response => {
                            closeBigLoading();
                            showGoodMessage('Success', 'Task and files deleted successfully');
                            // Remove from UI
                            var taskBlock = document.getElementById('task-' + data);
                            if (taskBlock) {
                                taskBlock.remove();
                            }
                            activeTasks.delete(parseInt(data));
                            updateIntervalTimer();
                        })
                        .catch(error => {
                            closeBigLoading();
                            showErrorMessage('Error', 'Failed to delete task');
                        });
                },
                null
            );
        }

        // Create new backup
        function createNewBackup() {
            showBigLoading(3000);

            fetch('/apiProgressReport2?action=backup')
                .then(response => {
                    closeBigLoading();
                    showGoodMessage('Success', 'Backup task started');
                    // Fetch all tasks to see the new one
                    fetchAllTaskStatus();
                })
                .catch(error => {
                    closeBigLoading();
                    showErrorMessage('Error', 'Failed to start backup');
                });
        }

        // Create new restore
        function createNewRestore() {
            var fileInput = document.getElementById('fileRestore');
            var file = fileInput.files[0];
            if (!file) {
                showErrorMessage('Error', 'Please select a file to restore');
                return;
            }

            var formData = new FormData();
            formData.append('file', file);

            showBigLoading(3000);

            var xhr = new XMLHttpRequest();

            xhr.open('POST', '/apiProgressReport2?action=restore', true);

            xhr.onload = function () {
                closeBigLoading();
                if (xhr.status === 200) {
                    showGoodMessage('Success', 'Restore task started');
                    fileInput.value = ''; // Clear file input
                    // Fetch all tasks to see the new one
                    fetchAllTaskStatus();
                } else {
                    showErrorMessage('Error', 'Failed to start restore');
                }
            };

            xhr.onerror = function () {
                closeBigLoading();
                showErrorMessage('Error', 'Failed to start restore');
            };

            xhr.send(formData);
        }

        // Network error handling
        function handleNetworkError() {
            if (!networkErrorState) {
                networkErrorState = true;
                showNetworkErrorButton();
                if (intervalTimer) {
                    clearInterval(intervalTimer);
                }
            }
        }

        function showNetworkErrorButton() {
            var existingBtn = document.getElementById('networkErrorBtn');
            if (!existingBtn) {
                var btn = document.createElement('button');
                btn.id = 'networkErrorBtn';
                btn.className = 'btn';
                btn.style.cssText = 'position: fixed; top: 20px; right: 20px; background: #ff5722;';
                btn.textContent = 'Resume Status Updates';
                btn.onclick = function () {
                    networkErrorState = false;
                    hideNetworkError();
                    startMonitoring();
                };
                document.body.appendChild(btn);
            }
        }

        function hideNetworkError() {
            var btn = document.getElementById('networkErrorBtn');
            if (btn) {
                btn.remove();
            }
        }

        // Helper functions
        function getStatusClass(task) {
            if (task.HasError) return 'error';
            if (task.IsCancelled) return 'cancelled';
            if (task.IsCompleted) return 'completed';
            if (task.IsStarted) return 'running';
            return '';
        }

        function getStatusText(task) {
            if (task.HasError) return 'Error';
            if (task.IsCancelled) return 'Cancelled';
            if (task.IsCompleted) return 'Completed';
            if (task.IsStarted) return 'Running';
            return 'Pending';
        }

        function calculateTimeUsed(task) {
            if (task.TimeUsedDisplay) {
                return task.TimeUsedDisplay;
            }

            var startTime = new Date(task.TimeStart);
            var endTime = task.IsCompleted ? new Date(task.TimeEnd) : new Date();
            var diff = endTime - startTime;

            var hours = Math.floor(diff / 3600000);
            var minutes = Math.floor((diff % 3600000) / 60000);
            var seconds = Math.floor((diff % 60000) / 1000);

            return String(hours).padStart(2, '0') + ':' +
                String(minutes).padStart(2, '0') + ':' +
                String(seconds).padStart(2, '0');
        }

        function formatNumber(num) {
            if (!num) return '0';
            return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        }

        function formatBytes(bytes) {
            if (!bytes) return '0 B';
            var sizes = ['B', 'KB', 'MB', 'GB'];
            var i = Math.floor(Math.log(bytes) / Math.log(1024));
            return (bytes / Math.pow(1024, i)).toFixed(2) + ' ' + sizes[i];
        }
    </script>
</asp:Content>
