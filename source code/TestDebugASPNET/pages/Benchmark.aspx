<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="Benchmark.aspx.cs" Inherits="System.pages.Benchmark" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .divconfig textarea {
            white-space: pre-wrap;
            word-wrap: break-word;
            word-break: break-all;
            width: 800px;
            height: 40px;
        }

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

        ul ul {
            list-style-type: "-";
            padding-left: 20px; /* Controls indentation distance */
        }

            ul ul li {
                padding-left: 10px; /* Controls distance between symbol and text */
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">
        <asp:Panel ID="panelSetup" runat="server">

            <h1>Benchmark</h1>
            Performance test and comparison of Backup and Restore using MySqlBackup.NET, MySqlDump, and MySQL (instance).
            <br />
            Notes:
            <ul>
                <li>Running this test in the Visual Studio debugging environment will slow down the process by 2-4 times.</li>
                <li>For optimum speed, build this project with the [Release] profile and publish it to a folder. Use Windows IIS to run this ASP.NET app.</li>
                <li>Executing mysqldump.exe and mysql.exe in ASP.NET:
                    <ul>
                        <li><strong>Local Windows 10/11 IIS:</strong> Works with default ApplicationPoolIdentity (no privilege changes needed).</li>
                        <li><strong>Shared Web Hosting:</strong> Executable restrictions prevent running mysql.exe and mysqldump.exe.</li>
                        <li><strong>Self-owned Windows Server/VPS:</strong> May (may not) require granting file system permissions to ApplicationPoolIdentity or changing to a custom account with MySQL access.</li>
                    </ul>
                </li>
            </ul>

            <hr />

            Please manually enter the file path of the following instance:
            <table class="maintb divconfig">
                <tr>
                    <td style="padding-top: 10px; padding-bottom: 0; vertical-align: top;">Initial Schema</td>
                    <td>
                        <asp:TextBox ID="txtInitialSchema" runat="server" Width="200px"></asp:TextBox><br />
                        *This database is expected to have already been populated with rows of data.
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px; padding-bottom: 0; vertical-align: top;">MySqlDump</td>
                    <td>
                        <asp:TextBox ID="txtFilePathMySqlDump" runat="server" Width="800px"></asp:TextBox><br />
                        *The executable file path of mysqldump.exe
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px; padding-bottom: 0; vertical-align: top;">MySql</td>
                    <td>
                        <asp:TextBox ID="txtFilePathMySql" runat="server" Width="800px"></asp:TextBox><br />
                        *The executable file path of mysql.exe
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px; padding-bottom: 0; vertical-align: top;">Output Folder</td>
                    <td>
                        <asp:TextBox ID="txtOutputFolder" runat="server" TextMode="MultiLine"></asp:TextBox><br />
                        *Destination folder of exported dump files
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px; padding-bottom: 0; vertical-align: top;">Report File</td>
                    <td>
                        <asp:TextBox ID="txtReportFilePath" runat="server" TextMode="MultiLine"></asp:TextBox><br />
                        *Full file path location for saving the report
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <div style="border: 1px solid #71c668;">

                            <div style="border-bottom: 1px solid #71c668; padding: 10px; background: #ecf6ff;">
                                Select mysql.exe Instance Execution Method:
                            </div>

                            <div style="padding: 10px; line-height: 300%;">

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
                        <asp:TextBox ID="txtTotalRound" runat="server" TextMode="Number" Width="40px" Text="3" min="1" max="3"></asp:TextBox>
                        Total round for each stage (min: 1, max: 3)</td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="cbGetSystemInfo" runat="server" Checked="true" />
                        Get System Info</td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="cbRunStage1" runat="server" Checked="true" />
                        Run Stage 1: Backup/Export - MySqlBackup.NET - Single Thread</td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="cbRunStage2" runat="server" Checked="true" />
                        Run Stage 2: Backup/Export - MySqlBackup.NET - Parallel Processing</td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="cbRunStage3" runat="server" Checked="true" />
                        Run Stage 3: Backup/Export - MySqlDump.exe</td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="cbRunStage4" runat="server" Checked="true" />
                        Run Stage 4: Restore/Import - MySqlBackup.NET</td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="cbRunStage5" runat="server" Checked="true" />
                        Run Stage 5: Restore/Import - mysql.exe</td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="cbCleanDatabaseAfterUse" runat="server" Checked="true" />
                        Clean Up Database After Use</td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="cbDeleteDumpFile" runat="server" Checked="true" />
                        Delete dump file immediately after each process. Note: The first dump file must be kept for the import test</td>
                </tr>
            </table>

            <div style="height: 10px;"></div>

            <asp:Button ID="btRun" runat="server" ClientIDMode="Static" Text="Begin Performance Benchmark Test" OnClick="btRun_Click" OnClientClick="hideButton(this);" />
        </asp:Panel>

        <asp:Panel ID="panelResult" runat="server" Visible="false">

            <div id="div-benchmark-report"></div>

            <asp:Literal ID="literalTaskId" runat="server"></asp:Literal>

            <script>
                let intervalId = null;
                let totalRounds = 3; // Default value, will be updated from backend data

                const stageNames = {
                    1: "Export/Backup - MySqlBackup.NET",
                    2: "Export/Backup - MySqlBackup.NET - Parallel Processing",
                    3: "Export/Backup - MySqlDump (mysqldump.exe)",
                    4: "Import/Restore - MySqlBackup.NET",
                    5: "Import/Restore - MySql (mysql.exe) Instance"
                };

                function getActiveStages(pr) {
                    const activeStages = [];
                    if (pr.dicStageInfo) {
                        for (let stageId in pr.dicStageInfo) {
                            const stage = pr.dicStageInfo[stageId];
                            if (stage.RunStage) {
                                activeStages.push(parseInt(stageId));
                            }
                        }
                    }
                    return activeStages.sort();
                }

                function getTotalRoundsFromTasks(pr) {
                    // Determine total rounds from the actual tasks
                    let maxRound = 0;
                    if (pr.dicTask) {
                        for (let taskId in pr.dicTask) {
                            const task = pr.dicTask[taskId];
                            if (task.ActiveTask && task.Round > maxRound) {
                                maxRound = task.Round;
                            }
                        }
                    }
                    return maxRound > 0 ? maxRound : 3; // Default to 3 if no tasks found
                }

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
        
        <div id="stages-container">
            <!-- Stages will be dynamically added here -->
        </div>

        <div class="remarks-section">
            <div class="remarks-title">Process Logs</div>
            <pre id="span-main-Remarks" class="remarks-content">Waiting for process to start...</pre>
        </div>
    `;

                    container.innerHTML = html;
                }

                function updateStagesUI(activeStages, totalRounds) {
                    const stagesContainer = document.getElementById('stages-container');

                    let html = '';

                    for (let stage of activeStages) {
                        html += `
            <div class="stage-section" id="stage-section-${stage}">
                <div class="stage-title">Stage ${stage}: ${stageNames[stage]}</div>
                <div class="rounds-container">
        `;

                        // Create rounds based on actual totalRounds
                        for (let round = 1; round <= totalRounds; round++) {
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

                    stagesContainer.innerHTML = html;
                }

                function updateProgress(pr) {
                    const activeStages = getActiveStages(pr);
                    const actualTotalRounds = getTotalRoundsFromTasks(pr);
                    const totalTasks = activeStages.length * actualTotalRounds;
                    let completedTasks = 0;

                    // Count completed tasks for active stages only
                    if (pr.dicTask) {
                        for (let taskId in pr.dicTask) {
                            const task = pr.dicTask[taskId];
                            if (task.ActiveTask && task.Completed && activeStages.includes(task.Stage)) {
                                completedTasks++;
                            }
                        }
                    }

                    const percentage = totalTasks > 0 ? Math.round((completedTasks / totalTasks) * 100) : 0;
                    const progressBar = document.getElementById('main-progress-bar');
                    if (progressBar) {
                        progressBar.style.width = percentage + '%';
                        progressBar.textContent = percentage + '%';
                    }
                }

                function fillValues(pr) {
                    const activeStages = getActiveStages(pr);
                    const actualTotalRounds = getTotalRoundsFromTasks(pr);

                    // Update totalRounds if it has changed
                    if (totalRounds !== actualTotalRounds) {
                        totalRounds = actualTotalRounds;
                    }

                    // Update stages UI if not already done or if stages/rounds changed
                    const stagesContainer = document.getElementById('stages-container');
                    if (stagesContainer.children.length === 0) {
                        updateStagesUI(activeStages, totalRounds);
                    }

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

                    document.getElementById('span-main-TimeStart').textContent = pr.TimeStartDisplay || '-';
                    document.getElementById('span-main-TimeEnd').textContent = pr.TimeEndDisplay || '-';
                    document.getElementById('span-main-TimeUsed').textContent = pr.TimeUsedDisplay || '-';
                    document.getElementById('span-main-HasError').textContent = pr.HasError ? 'Yes' : 'No';

                    if (pr.HasError && pr.LastError) {
                        const errorMsg = document.getElementById('main-error-message');
                        if (errorMsg) {
                            errorMsg.textContent = pr.LastError.Message || 'Unknown error';
                            errorMsg.classList.add('show');
                        }
                    }

                    // Update remarks/logs
                    if (pr.Remarks) {
                        let span_main_Remarks = document.getElementById('span-main-Remarks');
                        if (span_main_Remarks) {
                            span_main_Remarks.textContent = pr.Remarks;
                            span_main_Remarks.scrollTop = span_main_Remarks.scrollHeight;
                        }
                    }

                    // Update progress
                    updateProgress(pr);

                    // Update each task for active stages only
                    if (pr.dicTask) {
                        for (let taskId in pr.dicTask) {
                            const task = pr.dicTask[taskId];

                            // Skip inactive tasks
                            if (!task.ActiveTask) {
                                continue;
                            }

                            const stage = task.Stage;
                            const round = task.Round;

                            // Skip if this stage is not active
                            if (!activeStages.includes(stage)) {
                                continue;
                            }

                            // Skip if this round is beyond what we're displaying
                            if (round > totalRounds) {
                                continue;
                            }

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
                            if (timeStartSpan) timeStartSpan.textContent = task.TimeStartDisplay || '-';

                            const timeEndSpan = document.getElementById(`span-${stage}-${round}-TimeEnd`);
                            if (timeEndSpan) timeEndSpan.textContent = task.TimeEndDisplay || '-';

                            const timeUsedSpan = document.getElementById(`span-${stage}-${round}-TimeUsed`);
                            if (timeUsedSpan) timeUsedSpan.textContent = task.TimeUsedDisplay || '-';

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
                    if (statusSpan) {
                        statusSpan.textContent = 'Error';
                        statusSpan.className = 'status-indicator error';
                    }

                    // Show the error message
                    const mainErrorDiv = document.getElementById('main-error-message');
                    if (mainErrorDiv) {
                        mainErrorDiv.textContent = `API Error: ${errorMessage}`;
                        mainErrorDiv.classList.add('show');
                    }

                    // Optional: Show a more prominent error message
                    const container = document.getElementById('div-benchmark-report');
                    if (container) {
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
                }

                // Add this function to show network errors
                function showNetworkError(errorMessage) {
                    const statusSpan = document.getElementById('span-main-status');
                    if (statusSpan) {
                        statusSpan.textContent = 'Connection Error';
                        statusSpan.className = 'status-indicator error';
                    }

                    const mainErrorDiv = document.getElementById('main-error-message');
                    if (mainErrorDiv) {
                        mainErrorDiv.textContent = `Network Error: ${errorMessage}`;
                        mainErrorDiv.classList.add('show');
                    }

                    // Show retry option
                    const container = document.getElementById('div-benchmark-report');
                    if (container) {
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

                // Add input validation for total rounds
                function hideButton(button) {
                    const totalRoundInput = document.getElementById('txtTotalRound');
                    if (totalRoundInput) {
                        let value = parseInt(totalRoundInput.value) || 1;
                        if (value < 1) value = 1;
                        if (value > 3) value = 3;
                        totalRoundInput.value = value;
                    }

                    button.style.display = 'none';
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
