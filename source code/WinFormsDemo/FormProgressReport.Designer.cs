namespace WinFormsDemo
{
    partial class FormProgressReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtConstr = new System.Windows.Forms.TextBox();
            this.btBackup = new System.Windows.Forms.Button();
            this.btRestore = new System.Windows.Forms.Button();
            this.btStop = new System.Windows.Forms.Button();
            this.btOpenBackupFile = new System.Windows.Forms.Button();
            this.txtBackupFile = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btClearLog = new System.Windows.Forms.Button();
            this.btTestConnection = new System.Windows.Forms.Button();
            this.progressBar_TotalRows = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar_RowsCurrentTable = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            this.progressBar_TotalBytes = new System.Windows.Forms.ProgressBar();
            this.label7 = new System.Windows.Forms.Label();
            this.progressBar_TotalTables = new System.Windows.Forms.ProgressBar();
            this.lbTotalRows = new System.Windows.Forms.Label();
            this.lbRowsCurrentTable = new System.Windows.Forms.Label();
            this.lbTotalTables = new System.Windows.Forms.Label();
            this.lbTotalBytes = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lbTimeStart = new System.Windows.Forms.Label();
            this.lbTimeEnd = new System.Windows.Forms.Label();
            this.lbTimeUsed = new System.Windows.Forms.Label();
            this.lbIsCompleted = new System.Windows.Forms.Label();
            this.lbIsCancelled = new System.Windows.Forms.Label();
            this.lbHasError = new System.Windows.Forms.Label();
            this.lbErrorMsg = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lbTaskType = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "MySQL Connection String:";
            // 
            // txtConstr
            // 
            this.txtConstr.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConstr.Location = new System.Drawing.Point(12, 28);
            this.txtConstr.Name = "txtConstr";
            this.txtConstr.Size = new System.Drawing.Size(857, 22);
            this.txtConstr.TabIndex = 1;
            // 
            // btBackup
            // 
            this.btBackup.Location = new System.Drawing.Point(12, 56);
            this.btBackup.Name = "btBackup";
            this.btBackup.Size = new System.Drawing.Size(104, 35);
            this.btBackup.TabIndex = 2;
            this.btBackup.Text = "Backup";
            this.btBackup.UseVisualStyleBackColor = true;
            this.btBackup.Click += new System.EventHandler(this.btBackup_Click);
            // 
            // btRestore
            // 
            this.btRestore.Location = new System.Drawing.Point(122, 56);
            this.btRestore.Name = "btRestore";
            this.btRestore.Size = new System.Drawing.Size(104, 35);
            this.btRestore.TabIndex = 3;
            this.btRestore.Text = "Restore";
            this.btRestore.UseVisualStyleBackColor = true;
            this.btRestore.Click += new System.EventHandler(this.btRestore_Click);
            // 
            // btStop
            // 
            this.btStop.Enabled = false;
            this.btStop.Location = new System.Drawing.Point(232, 56);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(104, 35);
            this.btStop.TabIndex = 4;
            this.btStop.Text = "Stop";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // btOpenBackupFile
            // 
            this.btOpenBackupFile.Location = new System.Drawing.Point(342, 56);
            this.btOpenBackupFile.Name = "btOpenBackupFile";
            this.btOpenBackupFile.Size = new System.Drawing.Size(130, 35);
            this.btOpenBackupFile.TabIndex = 6;
            this.btOpenBackupFile.Text = "Open Backup File";
            this.btOpenBackupFile.UseVisualStyleBackColor = true;
            this.btOpenBackupFile.Click += new System.EventHandler(this.btOpenBackupFile_Click);
            // 
            // txtBackupFile
            // 
            this.txtBackupFile.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBackupFile.Location = new System.Drawing.Point(12, 124);
            this.txtBackupFile.Name = "txtBackupFile";
            this.txtBackupFile.ReadOnly = true;
            this.txtBackupFile.Size = new System.Drawing.Size(857, 22);
            this.txtBackupFile.TabIndex = 7;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 639);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(884, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.MarqueeAnimationSpeed = 20;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(150, 16);
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "SQL Backup File";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 376);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Task Log:";
            // 
            // txtLog
            // 
            this.txtLog.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.Color.DarkGray;
            this.txtLog.Location = new System.Drawing.Point(12, 395);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(857, 232);
            this.txtLog.TabIndex = 11;
            this.txtLog.WordWrap = false;
            // 
            // btClearLog
            // 
            this.btClearLog.Location = new System.Drawing.Point(604, 56);
            this.btClearLog.Name = "btClearLog";
            this.btClearLog.Size = new System.Drawing.Size(100, 35);
            this.btClearLog.TabIndex = 12;
            this.btClearLog.Text = "Clear Log";
            this.btClearLog.UseVisualStyleBackColor = true;
            this.btClearLog.Click += new System.EventHandler(this.btClearLog_Click);
            // 
            // btTestConnection
            // 
            this.btTestConnection.Location = new System.Drawing.Point(478, 56);
            this.btTestConnection.Name = "btTestConnection";
            this.btTestConnection.Size = new System.Drawing.Size(120, 35);
            this.btTestConnection.TabIndex = 13;
            this.btTestConnection.Text = "Test Connection";
            this.btTestConnection.UseVisualStyleBackColor = true;
            this.btTestConnection.Click += new System.EventHandler(this.btTestConnection_Click);
            // 
            // progressBar_TotalRows
            // 
            this.progressBar_TotalRows.Location = new System.Drawing.Point(478, 177);
            this.progressBar_TotalRows.Name = "progressBar_TotalRows";
            this.progressBar_TotalRows.Size = new System.Drawing.Size(380, 23);
            this.progressBar_TotalRows.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(478, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 15;
            this.label4.Text = "Total Rows:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(478, 262);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 16);
            this.label5.TabIndex = 17;
            this.label5.Text = "Rows Current Table:";
            // 
            // progressBar_RowsCurrentTable
            // 
            this.progressBar_RowsCurrentTable.Location = new System.Drawing.Point(478, 281);
            this.progressBar_RowsCurrentTable.Name = "progressBar_RowsCurrentTable";
            this.progressBar_RowsCurrentTable.Size = new System.Drawing.Size(380, 23);
            this.progressBar_RowsCurrentTable.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(478, 314);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 16);
            this.label6.TabIndex = 21;
            this.label6.Text = "Total Bytes:";
            // 
            // progressBar_TotalBytes
            // 
            this.progressBar_TotalBytes.Location = new System.Drawing.Point(478, 333);
            this.progressBar_TotalBytes.Name = "progressBar_TotalBytes";
            this.progressBar_TotalBytes.Size = new System.Drawing.Size(380, 23);
            this.progressBar_TotalBytes.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(478, 210);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 16);
            this.label7.TabIndex = 19;
            this.label7.Text = "Total Tables:";
            // 
            // progressBar_TotalTables
            // 
            this.progressBar_TotalTables.Location = new System.Drawing.Point(478, 229);
            this.progressBar_TotalTables.Name = "progressBar_TotalTables";
            this.progressBar_TotalTables.Size = new System.Drawing.Size(380, 23);
            this.progressBar_TotalTables.TabIndex = 18;
            // 
            // lbTotalRows
            // 
            this.lbTotalRows.AutoSize = true;
            this.lbTotalRows.Location = new System.Drawing.Point(556, 158);
            this.lbTotalRows.Name = "lbTotalRows";
            this.lbTotalRows.Size = new System.Drawing.Size(76, 16);
            this.lbTotalRows.TabIndex = 22;
            this.lbTotalRows.Text = "lbTotalRows";
            // 
            // lbRowsCurrentTable
            // 
            this.lbRowsCurrentTable.AutoSize = true;
            this.lbRowsCurrentTable.Location = new System.Drawing.Point(607, 262);
            this.lbRowsCurrentTable.Name = "lbRowsCurrentTable";
            this.lbRowsCurrentTable.Size = new System.Drawing.Size(125, 16);
            this.lbRowsCurrentTable.TabIndex = 23;
            this.lbRowsCurrentTable.Text = "lbRowsCurrentTable";
            // 
            // lbTotalTables
            // 
            this.lbTotalTables.AutoSize = true;
            this.lbTotalTables.Location = new System.Drawing.Point(561, 210);
            this.lbTotalTables.Name = "lbTotalTables";
            this.lbTotalTables.Size = new System.Drawing.Size(81, 16);
            this.lbTotalTables.TabIndex = 24;
            this.lbTotalTables.Text = "lbTotalTables";
            // 
            // lbTotalBytes
            // 
            this.lbTotalBytes.AutoSize = true;
            this.lbTotalBytes.Location = new System.Drawing.Point(556, 314);
            this.lbTotalBytes.Name = "lbTotalBytes";
            this.lbTotalBytes.Size = new System.Drawing.Size(76, 16);
            this.lbTotalBytes.TabIndex = 25;
            this.lbTotalBytes.Text = "lbTotalBytes";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.19647F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.80353F));
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label14, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lbTimeStart, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbTimeEnd, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbTimeUsed, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbIsCompleted, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lbIsCancelled, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lbHasError, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.lbErrorMsg, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label15, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbTaskType, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(15, 158);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(397, 208);
            this.tableLayoutPanel1.TabIndex = 26;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 16);
            this.label8.TabIndex = 0;
            this.label8.Text = "Time Start:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 16);
            this.label9.TabIndex = 1;
            this.label9.Text = "Time End:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 78);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 16);
            this.label10.TabIndex = 2;
            this.label10.Text = "Time Used:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 104);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 16);
            this.label11.TabIndex = 3;
            this.label11.Text = "Is Completed:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 130);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 16);
            this.label12.TabIndex = 4;
            this.label12.Text = "Is Cancelled:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 156);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(68, 16);
            this.label13.TabIndex = 5;
            this.label13.Text = "Has Error:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 182);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(70, 16);
            this.label14.TabIndex = 6;
            this.label14.Text = "Error Msg:";
            // 
            // lbTimeStart
            // 
            this.lbTimeStart.AutoSize = true;
            this.lbTimeStart.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTimeStart.Location = new System.Drawing.Point(106, 26);
            this.lbTimeStart.Name = "lbTimeStart";
            this.lbTimeStart.Size = new System.Drawing.Size(73, 16);
            this.lbTimeStart.TabIndex = 7;
            this.lbTimeStart.Text = "lbTimeStart";
            // 
            // lbTimeEnd
            // 
            this.lbTimeEnd.AutoSize = true;
            this.lbTimeEnd.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTimeEnd.Location = new System.Drawing.Point(106, 52);
            this.lbTimeEnd.Name = "lbTimeEnd";
            this.lbTimeEnd.Size = new System.Drawing.Size(66, 16);
            this.lbTimeEnd.TabIndex = 8;
            this.lbTimeEnd.Text = "lbTimeEnd";
            // 
            // lbTimeUsed
            // 
            this.lbTimeUsed.AutoSize = true;
            this.lbTimeUsed.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTimeUsed.Location = new System.Drawing.Point(106, 78);
            this.lbTimeUsed.Name = "lbTimeUsed";
            this.lbTimeUsed.Size = new System.Drawing.Size(74, 16);
            this.lbTimeUsed.TabIndex = 9;
            this.lbTimeUsed.Text = "lbTimeUsed";
            // 
            // lbIsCompleted
            // 
            this.lbIsCompleted.AutoSize = true;
            this.lbIsCompleted.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbIsCompleted.Location = new System.Drawing.Point(106, 104);
            this.lbIsCompleted.Name = "lbIsCompleted";
            this.lbIsCompleted.Size = new System.Drawing.Size(91, 16);
            this.lbIsCompleted.TabIndex = 10;
            this.lbIsCompleted.Text = "lbIsCompleted";
            // 
            // lbIsCancelled
            // 
            this.lbIsCancelled.AutoSize = true;
            this.lbIsCancelled.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbIsCancelled.Location = new System.Drawing.Point(106, 130);
            this.lbIsCancelled.Name = "lbIsCancelled";
            this.lbIsCancelled.Size = new System.Drawing.Size(86, 16);
            this.lbIsCancelled.TabIndex = 11;
            this.lbIsCancelled.Text = "lbIsCancelled";
            // 
            // lbHasError
            // 
            this.lbHasError.AutoSize = true;
            this.lbHasError.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHasError.Location = new System.Drawing.Point(106, 156);
            this.lbHasError.Name = "lbHasError";
            this.lbHasError.Size = new System.Drawing.Size(74, 16);
            this.lbHasError.TabIndex = 12;
            this.lbHasError.Text = "lbHasError";
            // 
            // lbErrorMsg
            // 
            this.lbErrorMsg.AutoSize = true;
            this.lbErrorMsg.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbErrorMsg.Location = new System.Drawing.Point(106, 182);
            this.lbErrorMsg.Name = "lbErrorMsg";
            this.lbErrorMsg.Size = new System.Drawing.Size(75, 16);
            this.lbErrorMsg.TabIndex = 13;
            this.lbErrorMsg.Text = "lbErrorMsg";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 16);
            this.label15.TabIndex = 14;
            this.label15.Text = "Task Type:";
            // 
            // lbTaskType
            // 
            this.lbTaskType.AutoSize = true;
            this.lbTaskType.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTaskType.Location = new System.Drawing.Point(106, 0);
            this.lbTaskType.Name = "lbTaskType";
            this.lbTaskType.Size = new System.Drawing.Size(68, 16);
            this.lbTaskType.TabIndex = 15;
            this.lbTaskType.Text = "lbTaskType";
            // 
            // FormProgressReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 661);
            this.Controls.Add(this.lbTotalBytes);
            this.Controls.Add(this.lbTotalTables);
            this.Controls.Add(this.lbRowsCurrentTable);
            this.Controls.Add(this.lbTotalRows);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.progressBar_TotalBytes);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.progressBar_TotalTables);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.progressBar_RowsCurrentTable);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.progressBar_TotalRows);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btTestConnection);
            this.Controls.Add(this.btClearLog);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.txtBackupFile);
            this.Controls.Add(this.btOpenBackupFile);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.btRestore);
            this.Controls.Add(this.btBackup);
            this.Controls.Add(this.txtConstr);
            this.Controls.Add(this.label1);
            this.Name = "FormProgressReport";
            this.Text = "FormSimpleDemo";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtConstr;
        private System.Windows.Forms.Button btBackup;
        private System.Windows.Forms.Button btRestore;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.Button btOpenBackupFile;
        private System.Windows.Forms.TextBox txtBackupFile;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btClearLog;
        private System.Windows.Forms.Button btTestConnection;
        private System.Windows.Forms.ProgressBar progressBar_TotalRows;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ProgressBar progressBar_RowsCurrentTable;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ProgressBar progressBar_TotalBytes;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ProgressBar progressBar_TotalTables;
        private System.Windows.Forms.Label lbTotalRows;
        private System.Windows.Forms.Label lbRowsCurrentTable;
        private System.Windows.Forms.Label lbTotalTables;
        private System.Windows.Forms.Label lbTotalBytes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lbTimeStart;
        private System.Windows.Forms.Label lbTimeEnd;
        private System.Windows.Forms.Label lbTimeUsed;
        private System.Windows.Forms.Label lbIsCompleted;
        private System.Windows.Forms.Label lbIsCancelled;
        private System.Windows.Forms.Label lbHasError;
        private System.Windows.Forms.Label lbErrorMsg;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lbTaskType;
    }
}