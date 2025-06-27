<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="Benchmark.aspx.cs" Inherits="System.pages.Benchmark" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        
        .maintb tr td:first-child {
            text-align: right;
        }

        .progress-container {
            width: 100%;
            height: 30px;
            background-color: #e0e0e0;
            border-radius: 15px;
            overflow: hidden;
            margin-bottom: 20px;
        }

        .progress-bar {
            height: 100%;
            background-color: #4CAF50;
            width: 0%;
            transition: width 0.3s ease;
            display: flex;
            align-items: center;
            justify-content: center;
            color: white;
            font-weight: bold;
        }

        .main-info {
            background-color: #f9f9f9;
            padding: 15px;
            border-radius: 5px;
            margin-bottom: 20px;
        }

            .main-info div {
                margin: 8px 0;
            }

            .main-info label {
                font-weight: bold;
                display: inline-block;
                width: 120px;
            }

        .stage-section {
            margin: 20px 0;
            border-top: 2px solid #e0e0e0;
            padding-top: 15px;
        }

        .stage-title {
            font-size: 18px;
            font-weight: bold;
            margin-bottom: 15px;
            color: #333;
        }

        .rounds-container {
            display: flex;
            gap: 10px;
            flex-wrap: wrap;
        }

        .round-block {
            flex: 1;
            min-width: 280px;
            border: 1px solid #ddd;
            border-radius: 5px;
            padding: 15px;
            transition: all 0.3s ease;
        }

            .round-block.pending {
                background-color: #f5f5f5;
                border-color: #ccc;
            }

            .round-block.started {
                background-color: #e3f2fd;
                border-color: #2196F3;
            }

            .round-block.completed {
                background-color: #e8f5e9;
                border-color: #4CAF50;
            }

            .round-block.error {
                background-color: #ffebee;
                border-color: #f44336;
            }

        .round-title {
            font-weight: bold;
            margin-bottom: 10px;
            font-size: 16px;
        }

        .round-info div {
            margin: 5px 0;
            font-size: 14px;
        }

        .round-info label {
            font-weight: 600;
            display: inline-block;
            width: 100px;
            color: #666;
        }

        .error-message {
            color: #d32f2f;
            margin-top: 10px;
            padding: 10px;
            background-color: #ffebee;
            border-radius: 4px;
            display: none;
        }

            .error-message.show {
                display: block;
            }

        .status-indicator {
            display: inline-block;
            padding: 2px 8px;
            border-radius: 3px;
            font-size: 12px;
            font-weight: bold;
        }

            .status-indicator.pending {
                background-color: #e0e0e0;
                color: #666;
            }

            .status-indicator.started {
                background-color: #2196F3;
                color: white;
            }

            .status-indicator.completed {
                background-color: #4CAF50;
                color: white;
            }

            .status-indicator.error {
                background-color: #f44336;
                color: white;
                font-weight: bold;
            }

        .error-banner {
            animation: slideDown 0.3s ease-out;
        }

        @keyframes slideDown {
            from {
                opacity: 0;
                transform: translateY(-20px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        .error-message.show {
            color: #f44336;
            font-weight: bold;
            margin-top: 10px;
        }

        .remarks-section {
            margin-top: 30px;
            border: 1px solid #ddd;
            border-radius: 8px;
            background-color: #f9f9f9;
            padding: 20px;
        }

        .remarks-title {
            font-size: 18px;
            font-weight: bold;
            margin-bottom: 15px;
            color: #333;
            border-bottom: 2px solid #4CAF50;
            padding-bottom: 10px;
        }

        .remarks-content {
            background-color: #f5f5f5;
            border: 1px solid #e0e0e0;
            border-radius: 4px;
            padding: 15px;
            margin: 0;
            font-family: 'Consolas', 'Monaco', 'Courier New', monospace;
            font-size: 12px;
            line-height: 1.5;
            overflow-x: auto;
            white-space: pre-wrap;
            word-wrap: break-word;
            max-height: 400px;
            overflow-y: auto;
        }

            /* Scrollbar styling for the remarks */
            .remarks-content::-webkit-scrollbar {
                width: 8px;
                height: 8px;
            }

            .remarks-content::-webkit-scrollbar-track {
                background: #f1f1f1;
            }

            .remarks-content::-webkit-scrollbar-thumb {
                background: #888;
                border-radius: 4px;
            }

                .remarks-content::-webkit-scrollbar-thumb:hover {
                    background: #555;
                }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">
        <asp:Panel ID="panelSetup" runat="server">

            <h1>Benchmark</h1>

            Performance test and comparison of Backup and Restore of using MySqlBackup.NET, MySqlDump and MySql (instance).<br />
            The test requires this ASP.NET application to be run with "LocalSystem" / "System" / "Administrator" privilege.<br />
            <br />
            Please manually enter the file path of the following instance:
            <table class="maintb">
                <tr>
                    <td style="padding-top: 10px; padding-bottom: 0; vertical-align: top;">Initial Schema</td>
                    <td>
                        <asp:TextBox ID="txtInitialSchema" runat="server" Width="600px"></asp:TextBox><br />
                        *This database is expected to have already been populated with rows of data.
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px; padding-bottom: 0; vertical-align: top;">MySqlDump</td>
                    <td>
                        <asp:TextBox ID="txtFilePathMySqlDump" runat="server" Width="600px"></asp:TextBox><br />
                        *The executable file path of mysqldump.exe
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px; padding-bottom: 0; vertical-align: top;">MySql</td>
                    <td>
                        <asp:TextBox ID="txtFilePathMySql" runat="server" Width="600px"></asp:TextBox><br />
                        *The executable file path of mysql.exe
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <div style="border: 1px solid #71c668;">

                            <div style="border-bottom: 1px solid #71c668; padding: 10px; background: #ecf6ff;">
                                Select mysql.exe Instance Execution Method:
                            </div>

                            <div style="padding: 10px;">
                                <asp:CheckBox ID="cbMySqlInstanceExecuteDirect" runat="server" ClientIDMode="Static" Checked="true" Style="vertical-align: middle;" />
                                <label for="cbMySqlInstanceExecuteDirect" style="vertical-align: middle;">Execute mysql.exe directly with SOURCE command</label>
                                <br />
                                <asp:CheckBox ID="cbMySqlInstanceExecuteCmdShell" runat="server" ClientIDMode="Static" Style="vertical-align: middle;" />
                                <label for="cbMySqlInstanceExecuteCmdShell" style="vertical-align: middle;">Execute mysql.exe through CMD shell with file redirection ( < )</label>

                                <script>
                                    const directCheckbox = document.getElementById('cbMySqlInstanceExecuteDirect');
                                    const cmdShellCheckbox = document.getElementById('cbMySqlInstanceExecuteCmdShell');

                                    directCheckbox.addEventListener('change', function () {
                                        if (this.checked) {
                                            cmdShellCheckbox.checked = false;
                                        }
                                    });

                                    cmdShellCheckbox.addEventListener('change', function () {
                                        if (this.checked) {
                                            directCheckbox.checked = false;
                                        }
                                    });
                                </script>

                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="cbSkipGetSystemInfo" runat="server" Checked="true" />
                        Skip Getting System Info (Save 5-10 seconds of initialization)</td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="cbCleanDatabaseAfterUse" runat="server" />
                        Clean Up Database After Use</td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="cbRunStage1" runat="server" Checked="true" />
                        Stage 1: Backup/Export - MySqlBackup.NET</td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="cbRunStage2" runat="server" Checked="true" />
                        Stage 2: Backup/Export - MySqlDump.exe</td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="cbRunStage3" runat="server" Checked="true" />
                        Stage 3: Restore/Import - MySqlBackup.NET</td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="cbRunStage4" runat="server" Checked="true" />
                        Stage 4: Restore/Import - mysql.exe</td>
                </tr>
            </table>

            <asp:Button ID="btRun" runat="server" ClientIDMode="Static" Text="Begin Performance Benchmark Test" OnClick="btRun_Click" OnClientClick="hideButton(this);" />
        </asp:Panel>

        <asp:Panel ID="panelResult" runat="server" Visible="false">

            <div id="div-benchmark-report"></div>

            <asp:Literal ID="literalTaskId" runat="server"></asp:Literal>

            <script>

                let intervalId = null;

                const stageNames = {
                    1: "Export/Backup - MySqlBackup.NET",
                    2: "Export/Backup - MySqlDump (mysqldump.exe)",
                    3: "Import/Restore - MySqlBackup.NET",
                    4: "Import/Restore - MySql (mysql.exe) Instance"
                };

                function drawUI() {
                    const container = document.getElementById('div-benchmark-report');

                    let html = `
        <h1>Benchmark Results</h1>

        <div class="progress-container">
            <div class="progress-bar" id="main-progress-bar">0%</div>
        </div>
        
        <div class="main-info">
            <div><label>Task ID:</label> <span id="span-main-taskid">${taskid}</span></div>
            <div><label>Status:</label> <span id="span-main-status" class="status-indicator pending">Pending</span></div>
            <div><label>Time Start:</label> <span id="span-main-TimeStart">-</span></div>
            <div><label>Time End:</label> <span id="span-main-TimeEnd">-</span></div>
            <div><label>Time Used:</label> <span id="span-main-TimeUsed">-</span></div>
            <div><label>Has Error:</label> <span id="span-main-HasError">No</span></div>
            <div class="error-message" id="main-error-message"></div>
        </div>
    `;

                    // Create stages
                    for (let stage = 1; stage <= 4; stage++) {
                        html += `
            <div class="stage-section">
                <div class="stage-title">Stage ${stage}: ${stageNames[stage]}</div>
                <div class="rounds-container">
        `;

                        // Create rounds for each stage
                        for (let round = 1; round <= 3; round++) {
                            html += `
                <div class="round-block pending" id="block-${stage}-${round}">
                    <div class="round-title">Round ${round}</div>
                    <div class="round-info">
                        <div><label>Status:</label> <span id="span-${stage}-${round}-status" class="status-indicator pending">Pending</span></div>
                        <div><label>Time Start:</label> <span id="span-${stage}-${round}-TimeStart">-</span></div>
                        <div><label>Time End:</label> <span id="span-${stage}-${round}-TimeEnd">-</span></div>
                        <div><label>Time Used:</label> <span id="span-${stage}-${round}-TimeUsed">-</span></div>
                        <div><label>Has Error:</label> <span id="span-${stage}-${round}-HasError">No</span></div>
                        <div class="error-message" id="error-${stage}-${round}"></div>
                    </div>
                </div>
            `;
                        }

                        html += `
                </div>
            </div>
        `;
                    }

                    // Add remarks section at the bottom
                    html += `
        <div class="remarks-section">
            <div class="remarks-title">Process Logs</div>
            <pre id="span-main-Remarks" class="remarks-content">Waiting for process to start...</pre>
        </div>
    `;

                    container.innerHTML = html;
                }

                function updateProgress(pr) {
                    let completedTasks = 0;
                    const totalTasks = 12;

                    // Count completed tasks
                    if (pr.dicTask) {
                        for (let i = 1; i <= totalTasks; i++) {
                            if (pr.dicTask[i] && pr.dicTask[i].Completed) {
                                completedTasks++;
                            }
                        }
                    }

                    const percentage = Math.round((completedTasks / totalTasks) * 100);
                    const progressBar = document.getElementById('main-progress-bar');
                    progressBar.style.width = percentage + '%';
                    progressBar.textContent = percentage + '%';
                }

                function fillValues(pr) {
                    // Update main info
                    if (pr.Started) {
                        if (pr.HasError) {
                            document.getElementById('span-main-status').textContent = 'Error';
                            document.getElementById('span-main-status').className = 'status-indicator error';
                        } else if (pr.Completed) {
                            document.getElementById('span-main-status').textContent = 'Completed';
                            document.getElementById('span-main-status').className = 'status-indicator completed';
                        } else {
                            document.getElementById('span-main-status').textContent = 'In Progress';
                            document.getElementById('span-main-status').className = 'status-indicator started';
                        }
                    }

                    document.getElementById('span-main-TimeStart').textContent = pr.TimeStartDisplay;
                    document.getElementById('span-main-TimeEnd').textContent = pr.TimeEndDisplay;
                    document.getElementById('span-main-TimeUsed').textContent = pr.TimeUsedDisplay;
                    document.getElementById('span-main-HasError').textContent = pr.HasError ? 'Yes' : 'No';

                    if (pr.HasError && pr.LastError) {
                        const errorMsg = document.getElementById('main-error-message');
                        errorMsg.textContent = pr.LastError.Message || 'Unknown error';
                        errorMsg.classList.add('show');
                    }

                    // Update remarks/logs
                    if (pr.Remarks) {
                        let span_main_Remarks = document.getElementById('span-main-Remarks');
                        span_main_Remarks.textContent = pr.Remarks;
                        span_main_Remarks.scrollTop = span_main_Remarks.scrollHeight;
                    }

                    // Update progress
                    updateProgress(pr);

                    // Update each task
                    if (pr.dicTask) {
                        for (let taskId in pr.dicTask) {
                            const task = pr.dicTask[taskId];
                            const stage = task.Stage;
                            const round = task.Round;

                            // Update block status
                            const block = document.getElementById(`block-${stage}-${round}`);
                            if (block) {
                                block.className = 'round-block ';
                                if (task.HasError) {
                                    block.className += 'error';
                                } else if (task.Completed) {
                                    block.className += 'completed';
                                } else if (task.Started) {
                                    block.className += 'started';
                                } else {
                                    block.className += 'pending';
                                }
                            }

                            // Update status indicator
                            const statusSpan = document.getElementById(`span-${stage}-${round}-status`);
                            if (statusSpan) {
                                if (task.HasError) {
                                    statusSpan.textContent = 'Error';
                                    statusSpan.className = 'status-indicator error';
                                } else if (task.Completed) {
                                    statusSpan.textContent = 'Completed';
                                    statusSpan.className = 'status-indicator completed';
                                } else if (task.Started) {
                                    statusSpan.textContent = 'Running';
                                    statusSpan.className = 'status-indicator started';
                                } else {
                                    statusSpan.textContent = 'Pending';
                                    statusSpan.className = 'status-indicator pending';
                                }
                            }

                            // Update times
                            const timeStartSpan = document.getElementById(`span-${stage}-${round}-TimeStart`);
                            if (timeStartSpan) timeStartSpan.textContent = task.TimeStartDisplay;

                            const timeEndSpan = document.getElementById(`span-${stage}-${round}-TimeEnd`);
                            if (timeEndSpan) timeEndSpan.textContent = task.TimeEndDisplay;

                            const timeUsedSpan = document.getElementById(`span-${stage}-${round}-TimeUsed`);
                            if (timeUsedSpan) timeUsedSpan.textContent = task.TimeUsedDisplay;

                            const hasErrorSpan = document.getElementById(`span-${stage}-${round}-HasError`);
                            if (hasErrorSpan) hasErrorSpan.textContent = task.HasError ? 'Yes' : 'No';

                            // Show error message if exists
                            if (task.HasError && task.ErrorMsg) {
                                const errorDiv = document.getElementById(`error-${stage}-${round}`);
                                if (errorDiv) {
                                    errorDiv.textContent = task.ErrorMsg;
                                    errorDiv.classList.add('show');
                                }
                            }
                        }
                    }
                }

                function fetchProgress() {
                    console.log(`Fetching progress for task ${taskid} at ${new Date().toLocaleTimeString()}`);

                    fetch(`/apiBenchmark?id=${taskid}`)
                        .then(response => {
                            console.log('Response status:', response.status);
                            if (!response.ok) {
                                throw new Error(`HTTP error! status: ${response.status}`);
                            }
                            return response.json();
                        })
                        .then(data => {
                            console.log('Received data:', data);

                            // Check if this is an API error
                            if (data.Error) {
                                console.error('API Error:', data.Error);

                                // Stop the polling
                                if (intervalId) {
                                    clearInterval(intervalId);
                                    intervalId = null;
                                }

                                // Show error to user
                                showApiError(data.Error);
                                return;
                            }

                            // Normal processing
                            fillValues(data);

                            if (data.Completed && intervalId) {
                                clearInterval(intervalId);
                                intervalId = null;
                                console.log('Task completed, stopping polling');
                            }
                        })
                        .catch(error => {
                            console.error('Fetch error:', error);

                            // Stop polling on network errors
                            if (intervalId) {
                                clearInterval(intervalId);
                                intervalId = null;
                            }

                            // Show error to user
                            showNetworkError(error.message);
                        });
                }

                function showApiError(errorMessage) {
                    // Update the main status to show error
                    const statusSpan = document.getElementById('span-main-status');
                    statusSpan.textContent = 'Error';
                    statusSpan.className = 'status-indicator error';

                    // Show the error message
                    const mainErrorDiv = document.getElementById('main-error-message');
                    mainErrorDiv.textContent = `API Error: ${errorMessage}`;
                    mainErrorDiv.classList.add('show');

                    // Optional: Show a more prominent error message
                    const container = document.getElementById('div-benchmark-report');
                    const errorBanner = document.createElement('div');
                    errorBanner.className = 'error-banner';
                    errorBanner.innerHTML = `
        <div style="background-color: #f44336; color: white; padding: 20px; margin: 20px 0; border-radius: 4px;">
            <h3 style="margin: 0 0 10px 0;">Error: Process Stopped</h3>
            <p style="margin: 0;">${errorMessage}</p>
            <button onclick="retryFetch()" style="margin-top: 10px; padding: 5px 15px; background: white; color: #f44336; border: none; border-radius: 3px; cursor: pointer;">Retry</button>
        </div>
    `;
                    container.insertBefore(errorBanner, container.firstChild);
                }

                // Add this function to show network errors
                function showNetworkError(errorMessage) {
                    const statusSpan = document.getElementById('span-main-status');
                    statusSpan.textContent = 'Connection Error';
                    statusSpan.className = 'status-indicator error';

                    const mainErrorDiv = document.getElementById('main-error-message');
                    mainErrorDiv.textContent = `Network Error: ${errorMessage}`;
                    mainErrorDiv.classList.add('show');

                    // Show retry option
                    const container = document.getElementById('div-benchmark-report');
                    const errorBanner = document.createElement('div');
                    errorBanner.className = 'error-banner';
                    errorBanner.innerHTML = `
        <div style="background-color: #ff9800; color: white; padding: 20px; margin: 20px 0; border-radius: 4px;">
            <h3 style="margin: 0 0 10px 0;">Connection Error</h3>
            <p style="margin: 0;">Unable to connect to the server. The connection may have been lost.</p>
            <button onclick="retryFetch()" style="margin-top: 10px; padding: 5px 15px; background: white; color: #ff9800; border: none; border-radius: 3px; cursor: pointer;">Retry</button>
        </div>
    `;
                    container.insertBefore(errorBanner, container.firstChild);
                }

                // Add retry functionality
                function retryFetch() {
                    // Remove any error banners
                    const errorBanners = document.querySelectorAll('.error-banner');
                    errorBanners.forEach(banner => banner.remove());

                    // Reset error messages
                    const errorMessages = document.querySelectorAll('.error-message.show');
                    errorMessages.forEach(msg => {
                        msg.textContent = '';
                        msg.classList.remove('show');
                    });

                    // Restart polling
                    if (!intervalId) {
                        fetchProgress(); // Initial fetch
                        intervalId = setInterval(fetchProgress, 1000);
                        console.log('Restarted polling');
                    }
                }

                // Initialize
                drawUI();

                // Start polling
                fetchProgress(); // Initial fetch

                intervalId = setInterval(fetchProgress, 1000); // Poll every second
            </script>

        </asp:Panel>

    </div>

</asp:Content>
