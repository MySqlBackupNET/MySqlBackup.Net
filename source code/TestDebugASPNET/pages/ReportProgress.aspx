<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="ReportProgress.aspx.cs" Inherits="System.pages.ReportProgress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .progress-container {
            margin: 20px;
        }

        .task-item {
            border: 1px solid #ddd;
            border-radius: 8px;
            padding: 15px;
            margin-bottom: 15px;
            background: #f9f9f9;
            /* Animation properties for smooth transitions */
            transition: all 0.3s ease, opacity 0.4s ease, transform 0.4s ease, margin-bottom 0.4s ease, padding 0.4s ease;
            transform-origin: top;
        }

            .task-item.active {
                background: #e8f4fd;
                border-color: #4a90e2;
            }

            .task-item.completed {
                background: #e8f5e9;
                border-color: #4caf50;
            }

            .task-item.error {
                background: #ffebee;
                border-color: #f44336;
            }

            .task-item.cancelled {
                background: #c7c7c7;
                border-color: #757575;
            }

            /* Fade out animation for deletion */
            .task-item.fade-out-only {
                opacity: 0;
                transition: opacity 0.4s ease;
            }

            /* Bounce back effect for remaining items */
            .task-item.bounce-back {
                transform: translateY(-10px);
                transition: transform 0.2s ease;
            }

            /* Disable pointer events during animation to prevent user interaction */
            .task-item.animating {
                pointer-events: none;
            }

        .task-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 10px;
        }

        .task-title {
            font-weight: bold;
            font-size: 14pt;
        }

        .task-status {
            padding: 5px 10px;
            border-radius: 5px;
            font-size: 9pt;
            font-weight: bold;
        }

        .status-active {
            background: #2196f3;
            color: white;
        }

        .status-completed {
            background: #4caf50;
            color: white;
        }

        .status-error {
            background: #f44336;
            color: white;
        }

        .status-cancelled {
            background: #757575;
            color: white;
        }

        .progress-bar-container {
            width: 100%;
            height: 20px;
            background: #e0e0e0;
            border-radius: 10px;
            overflow: hidden;
            margin: 10px 0;
        }

        .progress-bar {
            height: 100%;
            background: #2196f3;
            transition: width 0.3s ease;
            text-align: center;
            line-height: 20px;
            color: white;
            font-size: 10pt;
        }

        .task-details {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 10px;
            margin-top: 10px;
        }

        .detail-item {
            font-size: 9pt;
        }

        .detail-label {
            font-weight: bold;
            color: #666;
        }

        .no-tasks {
            text-align: center;
            padding: 40px;
            color: #666;
            font-size: 12pt;
            opacity: 0;
            animation: fadeIn 0.5s ease forwards;
        }

        @keyframes fadeIn {
            to {
                opacity: 1;
            }
        }

        .controls {
            margin: 20px;
        }

        .task-actions {
            margin-top: 15px;
            display: flex;
            gap: 10px;
            flex-wrap: wrap;
        }

        .buttonmain {
            display: inline-block;
            padding: 6px 12px;
            background: #2196f3;
            color: white;
            text-decoration: none;
            border-radius: 4px;
            font-size: 9pt;
            cursor: pointer;
            border: none;
            transition: background 0.3s;
        }

            .buttonmain:hover {
                background: #1976d2;
            }

            .buttonmain.delete {
                background: #f44336;
            }

                .buttonmain.delete:hover {
                    background: #d32f2f;
                }

            .buttonmain.view {
                background: #4caf50;
            }

                .buttonmain.view:hover {
                    background: #388e3c;
                }

            .buttonmain.download {
                background: #ff9800;
            }

                .buttonmain.download:hover {
                    background: #f57c00;
                }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main-content">
        <h2>Progress Report</h2>

        <div class="controls">
            <button type="button" onclick="getAllTask();">Refresh All Tasks</button>
            <button type="button" onclick="getActiveTasks();">Show Active Only</button>
            <button type="button" onclick="startNewBackup();" id="btBackup">Start New Backup</button>
            <button type="button" onclick="startNewRestore();">Start New Restore</button>
            *This will create a record at [<a href="/DatabaseRecordList">Database Record</a>]
        </div>

        <div id="progressContainer" class="progress-container">
            <div class="no-tasks">Loading tasks...</div>
        </div>

        <iframe name='frame1' id='frame1' style='height: 0; width: 0; border: none; display: inline-block;'></iframe>
    </div>

    <script>
        let updateTimer = null;
        let activeTaskIds = [];

        // Initialize on page load
        window.addEventListener('load', function () {
            getAllTask();
        });

        function getAllTask() {
            // Reset timer
            if (updateTimer) {
                clearInterval(updateTimer);
                updateTimer = null;
            }

            fetch('/apiProgressReport?action=getalltasks')
                .then(response => response.json())
                .then(data => {
                    displayTasks(data);
                    checkForActiveTasks(data);
                })
                .catch(error => {
                    console.error('Error fetching tasks:', error);
                    showErrorMessage('Error', 'Failed to load tasks');
                });
        }

        function getActiveTasks() {
            // Reset timer
            if (updateTimer) {
                clearInterval(updateTimer);
                updateTimer = null;
            }

            fetch('/apiProgressReport?action=getactivetasks')
                .then(response => response.json())
                .then(data => {
                    displayTasks(data);
                    if (data.length > 0) {
                        startProgressMonitoring();
                    }
                })
                .catch(error => {
                    console.error('Error fetching active tasks:', error);
                    showErrorMessage('Error', 'Failed to load active tasks');
                });
        }

        function displayTasks(tasks) {
            const container = document.getElementById('progressContainer');

            if (!tasks || tasks.length === 0) {
                container.innerHTML = '<div class="no-tasks">No tasks found</div>';
                return;
            }

            container.innerHTML = '';

            tasks.forEach(task => {
                const taskElement = createTaskElement(task);
                container.appendChild(taskElement);
            });
        }

        function createTaskElement(task) {
            const div = document.createElement('div');
            div.className = 'task-item';
            div.id = 'task-' + task.id;

            // Determine status with proper logic for cancelled tasks
            let status = 'Active';
            let statusClass = 'status-active';

            if (task.is_completed) {
                if (task.has_error) {
                    status = 'Error';
                    statusClass = 'status-error';
                    div.className += ' error';
                } else if (task.is_cancelled) {
                    status = 'Cancelled';
                    statusClass = 'status-cancelled';
                    div.className += ' cancelled';
                } else {
                    status = 'Completed';
                    statusClass = 'status-completed';
                    div.className += ' completed';
                }
            } else {
                // Check if task is cancelled but not completed (edge case)
                if (task.is_cancelled) {
                    status = 'Cancelled';
                    statusClass = 'status-cancelled';
                    div.className += ' error';
                } else {
                    div.className += ' active';
                }
            }

            // Format dates
            const startTime = formatDateTime(task.start_time);
            const endTime = task.end_time && task.end_time !== '0001-01-01T00:00:00' ? formatDateTime(task.end_time) : 'N/A';

            // Calculate duration if completed
            let duration = 'N/A';
            if (task.is_completed && task.end_time && task.end_time !== '0001-01-01T00:00:00') {
                duration = calculateDuration(task.start_time, task.end_time);
            }

            // Create action buttons HTML
            let actionButtons = '';

            // Only show view/download buttons for successfully completed tasks (not cancelled or error)
            if (task.has_file) {
                if (task.is_completed && !task.has_error && !task.is_cancelled) {
                    if (task.dbfile_id && task.dbfile_id > 0) {
                        actionButtons = `
                <a class='buttonmain view' target='_blank' href='/DisplayFileContent?id=${task.dbfile_id}'>View</a>
                <a class='buttonmain download' target='frame1' href='/DisplayFileContent?id=${task.dbfile_id}&action=download' onclick='showBigLoading(3000);'>Download</a>
            `;
                    } else {
                        actionButtons = "<i>Preparing file... hold on...</i>";
                        setTimeout(() => { getAllTask(); }, 3000);
                    }
                }
            }

            // Show cancel button only for active (non-completed, non-cancelled) tasks
            if (!task.is_completed && !task.is_cancelled) {
                actionButtons += `
            <button type='button' class='buttonmain cancel' onclick='cancelTask(${task.id});'>Stop/Cancel Task</button>
        `;
            }

            div.innerHTML = `
        <div class="task-header">
            <div class="task-title">Task #${task.id} - ${task.operationName}</div>
            <div class="task-status ${statusClass}">${status}</div>
        </div>
        
        ${!task.is_completed && !task.is_cancelled ? `
            <div class="progress-bar-container">
                <div class="progress-bar" style="width: ${task.percent_complete}%">
                    ${task.percent_complete}%
                </div>
            </div>
        ` : ''}
        
        <div class="task-details">
            <div class="detail-item">
                <span class="detail-label">Filename:</span> ${task.filename || 'N/A'}
            </div>
            <div class="detail-item">
                <span class="detail-label">Start Time:</span> ${startTime}
            </div>
            <div class="detail-item">
                <span class="detail-label">End Time:</span> ${endTime}
            </div>
            <div class="detail-item">
                <span class="detail-label">Duration:</span> ${duration}
            </div>
            ${task.operation === 1 && !task.is_completed && !task.is_cancelled ? `
                <div class="detail-item">
                    <span class="detail-label">Current Table:</span><br />
                    <span id='span-current-table-${task.id}'>${task.current_table || 'N/A'} (${task.current_table_index}/${task.total_tables})</span>
                </div>
                <div class="detail-item">
                    <span class="detail-label">Current Row:</span><br />
                    <span id='span-current-row-${task.id}'>${task.current_row.toLocaleString()}/${task.total_rows.toLocaleString()}</span>
                </div>
            ` : ''}
            ${task.operation === 2 && !task.is_completed && !task.is_cancelled ? `
                <div class="detail-item">
                    <span class="detail-label">Current Bytes:</span><br />
                    <span id='span-current-bytes-${task.id}'>${task.current_bytes.toLocaleString()}/${task.total_bytes.toLocaleString()}</span>
                </div>
            ` : ''}
            ${task.remarks && task.remarks.length > 0 ? `
                <div class="detail-item" style="grid-column: 1 / -1;">
                    <span class="detail-label">Remarks:</span><br />
                    <span id='span-error-${task.id}'>${task.remarks}</span>
                </div>
            ` : ''}
        </div>
        
        <div class="task-actions">
            ${actionButtons}
            ${task.is_completed ? `<button type='button' class='buttonmain delete' onclick="deleteTask(${task.id})">Delete Task</button>`:""}
        </div>
    `;

            return div;
        }

        function cancelTask(taskId) {
            fetch(`/apiProgressReport?action=cancel&id=${taskId}`);
            setTimeout(getAllTask, 800);
        }

        function deleteTask(taskId) {
            spShowConfirmDialog(
                "Delete Task",
                "Are you sure you want to delete this task?",
                "",
                (customdata) => {
                    // Yes
                    confirmRunDelete(taskId);
                },
                (customdata) => {
                    // No, do nothing
                }
            );
        }

        function confirmRunDelete(taskId) {
            fetch(`/apiProgressReport?action=delete&id=${taskId}`, {
                method: 'POST'
            })
                .then(response => response.text())
                .then(result => {
                    if (result === '1') {
                        // Start the animated deletion
                        animateTaskDeletion(taskId);
                        showGoodMessage('Success', 'Task deleted successfully');
                    } else {
                        showErrorMessage('Error', 'Failed to delete task');
                    }
                })
                .catch(error => {
                    console.error('Error deleting task:', error);
                    showErrorMessage('Error', 'Failed to delete task');
                });
        }

        function animateTaskDeletion(taskId) {
            const taskElement = document.getElementById('task-' + taskId);
            if (!taskElement) return;

            // Get all task items and mark them as animating to prevent interaction
            const allTasks = document.querySelectorAll('.task-item');
            allTasks.forEach(task => task.classList.add('animating'));

            // Get all tasks that come after the deleted task
            const siblingTasks = [];
            let foundTarget = false;
            allTasks.forEach(task => {
                if (foundTarget && task.id !== 'task-' + taskId) {
                    siblingTasks.push(task);
                }
                if (task.id === 'task-' + taskId) {
                    foundTarget = true;
                }
            });

            // Step 1: Fade out using opacity only (no space collapse)
            taskElement.classList.add('fade-out-only');

            // Step 2: After fade completes, animate the height collapse
            setTimeout(() => {
                // Get the current height before starting collapse animation
                const currentHeight = taskElement.offsetHeight;
                const currentMargin = parseInt(getComputedStyle(taskElement).marginBottom);

                // Set explicit height and start collapse animation
                taskElement.style.height = currentHeight + 'px';
                taskElement.style.marginBottom = currentMargin + 'px';
                taskElement.style.overflow = 'hidden';

                // Force a reflow
                taskElement.offsetHeight;

                // Add transition and collapse
                taskElement.style.transition = 'height 0.3s ease, margin-bottom 0.3s ease, padding 0.3s ease';
                taskElement.style.height = '0px';
                taskElement.style.marginBottom = '0px';
                taskElement.style.paddingTop = '0px';
                taskElement.style.paddingBottom = '0px';

                // Step 3: After collapse animation, remove element and add bounce
                setTimeout(() => {
                    taskElement.remove();

                    // Add bounce-back effect to remaining elements
                    //setTimeout(() => {
                        siblingTasks.forEach(task => {
                            task.classList.add('bounce-back');
                        });

                        // Remove bounce effect and clean up
                        setTimeout(() => {
                            siblingTasks.forEach(task => {
                                task.classList.remove('bounce-back');
                            });

                            const remainingTasks = document.querySelectorAll('.task-item');
                            remainingTasks.forEach(task => task.classList.remove('animating'));

                            if (remainingTasks.length === 0) {
                                document.getElementById('progressContainer').innerHTML = '<div class="no-tasks">No tasks found</div>';
                            }
                        }, 200); // Wait for bounce animation to complete

                    //}, 50); // Small delay before bounce starts

                }, 250); // Wait for collapse animation to complete

            }, 300); // Wait for fade-out to complete
        }

        function checkForActiveTasks(tasks) {
            activeTaskIds = [];

            tasks.forEach(task => {
                if (!task.is_completed) {
                    activeTaskIds.push(task.id);
                }
            });

            if (activeTaskIds.length > 0) {
                startProgressMonitoring();
            }
        }

        function startProgressMonitoring() {
            // Clear existing timer
            if (updateTimer) {
                clearInterval(updateTimer);
            }

            // Update every 500ms for smooth progress
            updateTimer = setInterval(() => {
                updateActiveTasksProgress();
            }, 500);
        }

        function updateActiveTasksProgress() {
            fetch('/apiProgressReport?action=getactivetasks')
                .then(response => response.json())
                .then(data => {
                    if (data.length === 0) {
                        // No more active tasks, stop monitoring
                        clearInterval(updateTimer);
                        updateTimer = null;
                        setTimeout(getAllTask, 1000);
                        return;
                    }

                    // Get current active task IDs
                    const newActiveTaskIds = data.map(task => task.id);

                    // Check if the active task lists are different
                    const tasksChanged = activeTaskIds.length !== newActiveTaskIds.length ||
                        !activeTaskIds.every(id => newActiveTaskIds.includes(id)) ||
                        !newActiveTaskIds.every(id => activeTaskIds.includes(id));

                    if (tasksChanged) {
                        // Task list changed - some completed/started, refresh all
                        setTimeout(getAllTask, 500);
                        return;
                    }

                    // Same tasks, just update progress
                    data.forEach(task => {
                        updateTaskUI(task);
                    });
                })
                .catch(error => {
                    console.error('Error updating progress:', error);
                });
        }

        function updateTaskUI(task) {
            const taskElement = document.getElementById('task-' + task.id);
            if (!taskElement) return;

            // Update progress bar
            const progressBar = taskElement.querySelector('.progress-bar');
            if (progressBar) {
                progressBar.style.width = task.percent_complete + '%';
                progressBar.textContent = task.percent_complete + '%';
            }

            // Update current table info (for Backup)
            if (task.operation === 1) {
                const currentTableInfo = taskElement.querySelector(`#span-current-table-${task.id}`);
                if (currentTableInfo) {
                    currentTableInfo.textContent = `${task.current_table || 'N/A'} (${task.current_table_index}/${task.total_tables})`;
                }

                // Update current row info
                const currentRowInfo = taskElement.querySelector(`#span-current-row-${task.id}`);
                if (currentRowInfo) {
                    currentRowInfo.textContent = `${task.current_row.toLocaleString()}/${task.total_rows.toLocaleString()}`;
                }
            }

            // Update current bytes info (for Restore)
            if (task.operation === 2) {
                const currentBytesInfo = taskElement.querySelector(`#span-current-bytes-${task.id}`);
                if (currentBytesInfo) {
                    currentBytesInfo.textContent = `${task.current_bytes.toLocaleString()}/${task.total_bytes.toLocaleString()}`;
                }
            }
        }

        function startNewRestore() {
            window.location = "/ReportProgressRestoreFileUpload";
        }

        function startNewBackup() {

            let btBackup = document.getElementById("btBackup");
            btBackup.disabled = true;

            fetch('/apiProgressReport?action=backup', {
                method: 'POST'
            })
                .then(response => response.text())
                .then(result => {
                    if (result === '1') {
                        showGoodMessage('Success', 'Backup started successfully');
                        // Refresh to show new task
                        setTimeout(() => {
                            getAllTask();
                        }, 500);
                    } else {
                        const parts = result.split('|');
                        showErrorMessage('Error', parts[1] || 'Failed to start backup');
                    }
                })
                .catch(error => {
                    console.error('Error starting backup:', error);
                    showErrorMessage('Error', 'Failed to start backup');
                });

            setTimeout(() => {
                btBackup.disabled = false;
            }, 1000);
        }

        function formatDateTime(dateString) {
            if (!dateString || dateString === '0001-01-01T00:00:00') return 'N/A';

            const date = new Date(dateString);
            const year = date.getFullYear();
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const day = String(date.getDate()).padStart(2, '0');
            const hours = String(date.getHours()).padStart(2, '0');
            const minutes = String(date.getMinutes()).padStart(2, '0');
            const seconds = String(date.getSeconds()).padStart(2, '0');

            return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
        }

        function calculateDuration(startTime, endTime) {
            const start = new Date(startTime);
            const end = new Date(endTime);
            const diff = end - start;

            const seconds = Math.floor(diff / 1000);
            const minutes = Math.floor(seconds / 60);
            const hours = Math.floor(minutes / 60);

            if (hours > 0) {
                return `${hours}h ${minutes % 60}m ${seconds % 60}s`;
            } else if (minutes > 0) {
                return `${minutes}m ${seconds % 60}s`;
            } else {
                return `${seconds}s`;
            }
        }

        function notifyCancel() {
            spShowMessage("Cancelling", "Task is being requested to cancel", true);
        }
    </script>
</asp:Content>
