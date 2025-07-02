<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="ParallelPipelineExport.aspx.cs" Inherits="System.pages.ParallelPipelineExport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .export-dashboard {
            border: 2px solid #ddd;
            border-radius: 8px;
            padding: 20px;
            margin: 10px 0;
            transition: all 0.3s ease;
        }

        /* Theme colors */
        .theme-running {
            background-color: #e3f2fd;
            border-color: #2196f3;
        }

        .theme-completed {
            background-color: #e8f5e8;
            border-color: #4caf50;
        }

        .theme-cancelled {
            background-color: #ffebee;
            border-color: #f44336;
        }

        .theme-error {
            background-color: #ffebee;
            border-color: #f44336;
        }

        .field-row {
            display: flex;
            margin: 8px 0;
            align-items: center;
        }

        .field-label {
            font-weight: bold;
            min-width: 150px;
            margin-right: 10px;
        }

        .field-value {
            flex: 1;
            padding: 4px 8px;
            background: rgba(255, 255, 255, 0.5);
            border-radius: 4px;
        }

        .progress-container {
            margin: 15px 0;
        }

        .progress-bar {
            width: 100%;
            height: 25px;
            background-color: #f0f0f0;
            border-radius: 12px;
            overflow: hidden;
            border: 1px solid #ddd;
        }

        .progress-fill {
            height: 100%;
            background: linear-gradient(90deg, #4caf50, #66bb6a);
            border-radius: 12px;
            transition: width 0.3s ease;
            display: flex;
            align-items: center;
            justify-content: center;
            color: white;
            font-weight: bold;
            font-size: 12px;
        }

        .button-group {
            margin: 15px 0;
            display: flex;
            gap: 10px;
        }

        .btn {
            padding: 10px 20px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-weight: bold;
            transition: all 0.2s ease;
        }

        .btn-cancel {
            background-color: #f44336;
            color: white;
        }

            .btn-cancel:hover {
                background-color: #d32f2f;
            }

            .btn-cancel:disabled {
                background-color: #ccc;
                cursor: not-allowed;
            }

        .btn-restart {
            background-color: #2196f3;
            color: white;
        }

            .btn-restart:hover {
                background-color: #1976d2;
            }

        .status-indicator {
            display: inline-block;
            width: 12px;
            height: 12px;
            border-radius: 50%;
            margin-right: 8px;
        }

        .status-running {
            background-color: #2196f3;
        }

        .status-completed {
            background-color: #4caf50;
        }

        .status-cancelled {
            background-color: #f44336;
        }

        .status-error {
            background-color: #f44336;
        }

        .error-message {
            color: #d32f2f;
            font-weight: bold;
            margin-top: 10px;
            padding: 10px;
            background-color: rgba(244, 67, 54, 0.1);
            border-radius: 4px;
        }

        .network-error {
            color: #ff9800;
            font-weight: bold;
            text-align: center;
            padding: 10px;
            background-color: rgba(255, 152, 0, 0.1);
            border-radius: 4px;
            margin: 10px 0;
        }

        #span-percent_complete {
            color: white;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">
        <h1>Parallel Pipeline Export</h1>

        <asp:Panel ID="panelSetup" runat="server">

            <asp:CheckBox ID="cbParallel" runat="server" Checked="true" />
            Enable Parallel Processing Export
            <br />
            <br />

            <asp:Button ID="btStart" runat="server" Text="Start Export" OnClick="btStart_Click" />

        </asp:Panel>

        <asp:Panel ID="panelResult" runat="server" Visible="false">

            <div id="divresult"></div>

            <asp:Literal ID="ltTaskId" runat="server"></asp:Literal>

            <script>
                var apicallid = 0;
                var intervalTimer = null;
                var networkErrorCount = 0;

                // Function 1: Draw UI
                function drawUI() {
                    const container = document.getElementById('divresult');

                    container.innerHTML = `
        <div id="export-dashboard" class="export-dashboard">
            <h3>Export Task #<span id="span-id"></span></h3>

            <div class="progress-container">
                <div class="field-label">Progress:</div>
                <div class="progress-bar">
                    <div id="progress-fill" class="progress-fill" style="width: 0%;">
                        <span id="span-percent_complete">0</span>%
                    </div>
                </div>
            </div>

            <div class="field-row">
                <div class="field-label">Status:</div>
                <div class="field-value">
                    <span class="status-indicator" id="status-indicator"></span>
                    <span id="span-status">Initializing...</span>
                </div>
            </div>

            <div class="field-row">
                <div class="field-label">Is Parallel:</div>
                <div class="field-value">
                    <span id="span-is_parallel"></span>
                </div>
            </div>
            
            <div class="field-row">
                <div class="field-label">Filename:</div>
                <div class="field-value">
                    <span id="span-filename"></span>
                </div>
            </div>
            
            <div class="field-row">
                <div class="field-label">File Path:</div>
                <div class="field-value">
                    <span id="span-filepath"></span>
                </div>
            </div>

            <div class="field-row">
                <div class="field-label">SHA 256:</div>
                <div class="field-value">
                    <span id="span-sha256"></span>
                </div>
            </div>
            
            <div class="field-row">
                <div class="field-label">Total Rows:</div>
                <div class="field-value">
                    <span id="span-total_rows">0</span>
                </div>
            </div>
            
            <div class="field-row">
                <div class="field-label">Current Row:</div>
                <div class="field-value">
                    <span id="span-current_row">0</span>
                </div>
            </div>

            <div class="field-row">
                <div class="field-label">Time Start:</div>
                <div class="field-value">
                    <span id="span-time_start">---</span>
                </div>
            </div>

            <div class="field-row">
                <div class="field-label">Time End:</div>
                <div class="field-value">
                    <span id="span-time_end">---</span>
                </div>
            </div>

            <div class="field-row">
                <div class="field-label">Time Used:</div>
                <div class="field-value">
                    <span id="span-time_used">---</span>
                </div>
            </div>

            <div class="button-group">
                <button id="btn-cancel" class="btn btn-cancel" onclick="cancelExport()">
                    Cancel Export
                </button>
                <button id="btn-restart" class="btn btn-restart" onclick="startMonitoring()" style="display: none;">
                    Restart Monitoring
                </button>
                <button id="btn-restart" class="btn btn-restart" onclick="resetProcess()">
                    Reset Process
                </button>
            </div>
            
            <div id="error-container"></div>
            <div id="network-error-container"></div>
        </div>
    `;
                }

                // Function 2: Fill values and apply theme
                function fillValues(data) {
                    // Fill all span elements with data
                    document.getElementById('span-id').textContent = data.id || '';
                    document.getElementById('span-is_parallel').textContent = data.is_parallel ? 'Yes' : 'No';
                    document.getElementById('span-filename').textContent = data.filename || '';
                    document.getElementById('span-filepath').textContent = data.filepath || '';
                    document.getElementById('span-sha256').textContent = data.sha256 || '';
                    document.getElementById('span-total_rows').textContent = data.total_rows || 0;
                    document.getElementById('span-current_row').textContent = data.current_row || 0;
                    document.getElementById('span-percent_complete').textContent = data.percent_complete || 0;
                    document.getElementById('span-time_start').textContent = data.time_start_display || "---";
                    document.getElementById('span-time_end').textContent = data.time_end_display || "---";
                    document.getElementById('span-time_used').textContent = data.time_used || "---";

                    // Update progress bar
                    const progressFill = document.getElementById('progress-fill');
                    const percentage = data.percent_complete || 0;
                    progressFill.style.width = percentage + '%';

                    // Determine status and apply theme
                    let status = 'running';
                    let statusText = 'Running';

                    if (data.request_cancel) {
                        status = 'cancelled';
                        statusText = 'Cancelling...';
                    } else if (data.has_error) {
                        status = 'error';
                        statusText = 'Error';
                    } else if (data.is_completed) {
                        status = 'completed';
                        statusText = 'Completed';
                    }

                    // Update status display
                    document.getElementById('span-status').textContent = statusText;
                    const statusIndicator = document.getElementById('status-indicator');
                    statusIndicator.className = 'status-indicator status-' + status;

                    // Apply theme to dashboard
                    const dashboard = document.getElementById('export-dashboard');
                    dashboard.className = 'export-dashboard theme-' + status;

                    // Handle error message
                    const errorContainer = document.getElementById('error-container');
                    if (data.has_error && data.error_msg) {
                        errorContainer.innerHTML = `<div class="error-message">Error: ${data.error_msg}</div>`;
                    } else {
                        errorContainer.innerHTML = '';
                    }

                    // Handle button states and monitoring control
                    const cancelBtn = document.getElementById('btn-cancel');
                    const restartBtn = document.getElementById('btn-restart');

                    // FIXED: Only stop monitoring when specific completion conditions are met
                    if (data.is_completed === true || data.request_cancel === true || data.has_error === true) {
                        cancelBtn.disabled = true;
                        cancelBtn.style.display = 'none';
                        restartBtn.style.display = 'inline-block';

                        // Stop monitoring only when we have definitive completion status
                        console.log('Stopping monitoring - is_completed:', data.is_completed, 'request_cancel:', data.request_cancel, 'has_error:', data.has_error);
                        stopMonitoring();
                    } else {
                        // Keep monitoring active for any other state
                        cancelBtn.disabled = false;
                        cancelBtn.style.display = 'inline-block';
                        restartBtn.style.display = 'none';
                    }

                    // Clear network error if we got successful data
                    networkErrorCount = 0;
                    document.getElementById('network-error-container').innerHTML = '';
                }

                // Function 3: Fetch API to get status
                async function fetchStatus() {
                    try {
                        apicallid++;
                        const response = await fetch(`/apiParallelPipelineExport?taskid=${taskid}&apicallid=${apicallid}`);

                        if (!response.ok) {
                            throw new Error(`HTTP ${response.status}: ${response.statusText}`);
                        }

                        const data = await response.json();

                        if (data.api_call_id != apicallid) {
                            return;
                        }

                        // Debug logging to see what we're receiving
                        console.log('Received data:', {
                            is_completed: data.is_completed,
                            request_cancel: data.request_cancel,
                            has_error: data.has_error,
                            percent_complete: data.percent_complete
                        });

                        fillValues(data);

                    } catch (error) {
                        console.error('Error fetching status:', error);
                        networkErrorCount++;

                        const networkErrorContainer = document.getElementById('network-error-container');
                        networkErrorContainer.innerHTML = `
            <div class="network-error">
                Network Error (${networkErrorCount}): ${error.message}
                <br>Monitoring will stop after 3 consecutive errors.
            </div>
        `;

                        // Stop monitoring after 3 consecutive network errors
                        if (networkErrorCount >= 3) {
                            stopMonitoring();
                            const restartBtn = document.getElementById('btn-restart');
                            restartBtn.style.display = 'inline-block';
                        }
                    }
                }

                // Function 4: Cancel export
                async function cancelExport() {
                    try {
                        apicallid++;
                        const response = await fetch(`/apiParallelPipelineExport?taskid=${taskid}&action=cancel&apicallid=${apicallid}`);

                        if (!response.ok) {
                            throw new Error(`HTTP ${response.status}: ${response.statusText}`);
                        }

                        const data = await response.json();
                        fillValues(data);

                    } catch (error) {
                        console.error('Error cancelling export:', error);
                        alert('Failed to cancel export: ' + error.message);
                    }
                }

                // Start monitoring with interval timer
                function startMonitoring() {
                    // Clear any existing timer
                    if (intervalTimer) {
                        clearInterval(intervalTimer);
                    }

                    // Reset network error count
                    networkErrorCount = 0;
                    document.getElementById('network-error-container').innerHTML = '';

                    // Hide restart button
                    const restartBtn = document.getElementById('btn-restart');
                    restartBtn.style.display = 'none';

                    // Show cancel button
                    const cancelBtn = document.getElementById('btn-cancel');
                    cancelBtn.style.display = 'inline-block';
                    cancelBtn.disabled = false;

                    // Start fetching immediately
                    fetchStatus();

                    // Set up interval timer (every 500 milliseconds)
                    intervalTimer = setInterval(fetchStatus, 500);

                    console.log('Started monitoring with interval timer');
                }

                // Stop monitoring
                function stopMonitoring() {
                    if (intervalTimer) {
                        clearInterval(intervalTimer);
                        intervalTimer = null;
                        console.log('Stopped monitoring');
                    }
                }

                function resetProcess() {
                    window.location = "/ParallelPipelineExport";
                }

                // Initialize the UI and start monitoring when page loads
                document.addEventListener('DOMContentLoaded', function () {
                    drawUI();
                    startMonitoring();
                });

                // Clean up interval when page unloads
                window.addEventListener('beforeunload', function () {
                    stopMonitoring();
                });
            </script>
        </asp:Panel>

    </div>

</asp:Content>
