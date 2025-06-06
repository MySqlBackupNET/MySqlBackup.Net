namespace Backup_All_Databases
{
    partial class Form1
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtConnString = new System.Windows.Forms.TextBox();
            this.btLoadSettings = new System.Windows.Forms.Button();
            this.btSaveSettings = new System.Windows.Forms.Button();
            this.checkBoxBackupAllDatabases = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btIncludeClear = new System.Windows.Forms.Button();
            this.btIncludeSelectAll = new System.Windows.Forms.Button();
            this.checkedListIncludeDatabases = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btExcludeClear = new System.Windows.Forms.Button();
            this.btExcludeSelectAll = new System.Windows.Forms.Button();
            this.checkedListExcludeDatabases = new System.Windows.Forms.CheckedListBox();
            this.btSetBackupFolder = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDestinationFolder = new System.Windows.Forms.TextBox();
            this.numericUpDownMaxBackupCount = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.btLoadDatabaseList = new System.Windows.Forms.Button();
            this.btTestConnection = new System.Windows.Forms.Button();
            this.btRunBackup = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.nmBackupHour = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.nmBackupMinute = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.btTimer = new System.Windows.Forms.Button();
            this.btTaskScheduler = new System.Windows.Forms.Button();
            this.lbNextBackup = new System.Windows.Forms.Label();
            this.btDeleteConfig = new System.Windows.Forms.Button();
            this.btInfo = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxBackupCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmBackupHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmBackupMinute)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.SlateGray;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(4);
            this.label1.Size = new System.Drawing.Size(844, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Recommended to run this program with Windows Task Scheduler. The settings will be" +
    " saved and encrypted.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "MySQL Connection String:";
            // 
            // txtConnString
            // 
            this.txtConnString.Location = new System.Drawing.Point(207, 87);
            this.txtConnString.Name = "txtConnString";
            this.txtConnString.PasswordChar = '*';
            this.txtConnString.Size = new System.Drawing.Size(472, 21);
            this.txtConnString.TabIndex = 3;
            // 
            // btLoadSettings
            // 
            this.btLoadSettings.Location = new System.Drawing.Point(98, 37);
            this.btLoadSettings.Name = "btLoadSettings";
            this.btLoadSettings.Size = new System.Drawing.Size(80, 41);
            this.btLoadSettings.TabIndex = 2;
            this.btLoadSettings.Text = "Load Settings";
            this.btLoadSettings.UseVisualStyleBackColor = true;
            this.btLoadSettings.Click += new System.EventHandler(this.btLoadSettings_Click);
            // 
            // btSaveSettings
            // 
            this.btSaveSettings.Location = new System.Drawing.Point(12, 37);
            this.btSaveSettings.Name = "btSaveSettings";
            this.btSaveSettings.Size = new System.Drawing.Size(80, 41);
            this.btSaveSettings.TabIndex = 1;
            this.btSaveSettings.Text = "Save Settings";
            this.btSaveSettings.UseVisualStyleBackColor = true;
            this.btSaveSettings.Click += new System.EventHandler(this.btSaveSettings_Click);
            // 
            // checkBoxBackupAllDatabases
            // 
            this.checkBoxBackupAllDatabases.AutoSize = true;
            this.checkBoxBackupAllDatabases.Location = new System.Drawing.Point(140, 175);
            this.checkBoxBackupAllDatabases.Name = "checkBoxBackupAllDatabases";
            this.checkBoxBackupAllDatabases.Size = new System.Drawing.Size(166, 20);
            this.checkBoxBackupAllDatabases.TabIndex = 4;
            this.checkBoxBackupAllDatabases.Text = "Backup All Databases";
            this.checkBoxBackupAllDatabases.UseVisualStyleBackColor = true;
            this.checkBoxBackupAllDatabases.CheckedChanged += new System.EventHandler(this.checkBoxBackupAllDatabases_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btIncludeClear);
            this.groupBox1.Controls.Add(this.btIncludeSelectAll);
            this.groupBox1.Controls.Add(this.checkedListIncludeDatabases);
            this.groupBox1.Location = new System.Drawing.Point(12, 206);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 449);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Include Databases";
            // 
            // btIncludeClear
            // 
            this.btIncludeClear.Font = new System.Drawing.Font("Cascadia Code", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btIncludeClear.Location = new System.Drawing.Point(93, 22);
            this.btIncludeClear.Name = "btIncludeClear";
            this.btIncludeClear.Size = new System.Drawing.Size(81, 23);
            this.btIncludeClear.TabIndex = 2;
            this.btIncludeClear.Text = "Clear";
            this.btIncludeClear.UseVisualStyleBackColor = true;
            this.btIncludeClear.Click += new System.EventHandler(this.btIncludeClear_Click);
            // 
            // btIncludeSelectAll
            // 
            this.btIncludeSelectAll.Font = new System.Drawing.Font("Cascadia Code", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btIncludeSelectAll.Location = new System.Drawing.Point(6, 22);
            this.btIncludeSelectAll.Name = "btIncludeSelectAll";
            this.btIncludeSelectAll.Size = new System.Drawing.Size(81, 23);
            this.btIncludeSelectAll.TabIndex = 1;
            this.btIncludeSelectAll.Text = "Select All";
            this.btIncludeSelectAll.UseVisualStyleBackColor = true;
            this.btIncludeSelectAll.Click += new System.EventHandler(this.btIncludeSelectAll_Click);
            // 
            // checkedListIncludeDatabases
            // 
            this.checkedListIncludeDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListIncludeDatabases.FormattingEnabled = true;
            this.checkedListIncludeDatabases.Location = new System.Drawing.Point(6, 52);
            this.checkedListIncludeDatabases.Name = "checkedListIncludeDatabases";
            this.checkedListIncludeDatabases.Size = new System.Drawing.Size(388, 388);
            this.checkedListIncludeDatabases.TabIndex = 0;
            this.checkedListIncludeDatabases.SelectedIndexChanged += new System.EventHandler(this.checkedListIncludeDatabases_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btExcludeClear);
            this.groupBox2.Controls.Add(this.btExcludeSelectAll);
            this.groupBox2.Controls.Add(this.checkedListExcludeDatabases);
            this.groupBox2.Location = new System.Drawing.Point(419, 206);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(400, 449);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Exclude Databases";
            // 
            // btExcludeClear
            // 
            this.btExcludeClear.Font = new System.Drawing.Font("Cascadia Code", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btExcludeClear.Location = new System.Drawing.Point(93, 22);
            this.btExcludeClear.Name = "btExcludeClear";
            this.btExcludeClear.Size = new System.Drawing.Size(81, 23);
            this.btExcludeClear.TabIndex = 4;
            this.btExcludeClear.Text = "Clear";
            this.btExcludeClear.UseVisualStyleBackColor = true;
            this.btExcludeClear.Click += new System.EventHandler(this.btExcludeClear_Click);
            // 
            // btExcludeSelectAll
            // 
            this.btExcludeSelectAll.Font = new System.Drawing.Font("Cascadia Code", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btExcludeSelectAll.Location = new System.Drawing.Point(6, 22);
            this.btExcludeSelectAll.Name = "btExcludeSelectAll";
            this.btExcludeSelectAll.Size = new System.Drawing.Size(81, 23);
            this.btExcludeSelectAll.TabIndex = 3;
            this.btExcludeSelectAll.Text = "Select All";
            this.btExcludeSelectAll.UseVisualStyleBackColor = true;
            this.btExcludeSelectAll.Click += new System.EventHandler(this.btExcludeSelectAll_Click);
            // 
            // checkedListExcludeDatabases
            // 
            this.checkedListExcludeDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListExcludeDatabases.FormattingEnabled = true;
            this.checkedListExcludeDatabases.Location = new System.Drawing.Point(6, 52);
            this.checkedListExcludeDatabases.Name = "checkedListExcludeDatabases";
            this.checkedListExcludeDatabases.Size = new System.Drawing.Size(388, 388);
            this.checkedListExcludeDatabases.TabIndex = 1;
            this.checkedListExcludeDatabases.SelectedIndexChanged += new System.EventHandler(this.checkedListExcludeDatabases_SelectedIndexChanged);
            // 
            // btSetBackupFolder
            // 
            this.btSetBackupFolder.Location = new System.Drawing.Point(184, 37);
            this.btSetBackupFolder.Name = "btSetBackupFolder";
            this.btSetBackupFolder.Size = new System.Drawing.Size(122, 41);
            this.btSetBackupFolder.TabIndex = 7;
            this.btSetBackupFolder.Text = "Set Destination Folder";
            this.btSetBackupFolder.UseVisualStyleBackColor = true;
            this.btSetBackupFolder.Click += new System.EventHandler(this.btSetBackupFolder_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(189, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Destination Backup Folder:";
            // 
            // txtDestinationFolder
            // 
            this.txtDestinationFolder.Location = new System.Drawing.Point(207, 118);
            this.txtDestinationFolder.Name = "txtDestinationFolder";
            this.txtDestinationFolder.ReadOnly = true;
            this.txtDestinationFolder.Size = new System.Drawing.Size(472, 21);
            this.txtDestinationFolder.TabIndex = 9;
            // 
            // numericUpDownMaxBackupCount
            // 
            this.numericUpDownMaxBackupCount.Location = new System.Drawing.Point(355, 176);
            this.numericUpDownMaxBackupCount.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownMaxBackupCount.Name = "numericUpDownMaxBackupCount";
            this.numericUpDownMaxBackupCount.Size = new System.Drawing.Size(55, 21);
            this.numericUpDownMaxBackupCount.TabIndex = 10;
            this.numericUpDownMaxBackupCount.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(352, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(189, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Number of Backups to Keep:";
            // 
            // btLoadDatabaseList
            // 
            this.btLoadDatabaseList.Location = new System.Drawing.Point(12, 154);
            this.btLoadDatabaseList.Name = "btLoadDatabaseList";
            this.btLoadDatabaseList.Size = new System.Drawing.Size(119, 41);
            this.btLoadDatabaseList.TabIndex = 12;
            this.btLoadDatabaseList.Text = "Load Database List";
            this.btLoadDatabaseList.UseVisualStyleBackColor = true;
            this.btLoadDatabaseList.Click += new System.EventHandler(this.btLoadDatabaseList_Click);
            // 
            // btTestConnection
            // 
            this.btTestConnection.Location = new System.Drawing.Point(312, 37);
            this.btTestConnection.Name = "btTestConnection";
            this.btTestConnection.Size = new System.Drawing.Size(100, 41);
            this.btTestConnection.TabIndex = 13;
            this.btTestConnection.Text = "Test MySQL Connection";
            this.btTestConnection.UseVisualStyleBackColor = true;
            this.btTestConnection.Click += new System.EventHandler(this.btTestConnection_Click);
            // 
            // btRunBackup
            // 
            this.btRunBackup.Location = new System.Drawing.Point(418, 37);
            this.btRunBackup.Name = "btRunBackup";
            this.btRunBackup.Size = new System.Drawing.Size(67, 41);
            this.btRunBackup.TabIndex = 14;
            this.btRunBackup.Text = "Run Backup";
            this.btRunBackup.UseVisualStyleBackColor = true;
            this.btRunBackup.Click += new System.EventHandler(this.btRunBackup_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(622, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 16);
            this.label5.TabIndex = 15;
            this.label5.Text = "Runs Daily Backup At:";
            // 
            // nmBackupHour
            // 
            this.nmBackupHour.Location = new System.Drawing.Point(625, 176);
            this.nmBackupHour.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.nmBackupHour.Name = "nmBackupHour";
            this.nmBackupHour.Size = new System.Drawing.Size(40, 21);
            this.nmBackupHour.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(666, 178);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 16);
            this.label6.TabIndex = 17;
            this.label6.Text = "Hour";
            // 
            // nmBackupMinute
            // 
            this.nmBackupMinute.Location = new System.Drawing.Point(704, 176);
            this.nmBackupMinute.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.nmBackupMinute.Name = "nmBackupMinute";
            this.nmBackupMinute.Size = new System.Drawing.Size(40, 21);
            this.nmBackupMinute.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(744, 178);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 16);
            this.label7.TabIndex = 19;
            this.label7.Text = "Minute";
            // 
            // btTimer
            // 
            this.btTimer.Location = new System.Drawing.Point(587, 37);
            this.btTimer.Name = "btTimer";
            this.btTimer.Size = new System.Drawing.Size(92, 41);
            this.btTimer.TabIndex = 20;
            this.btTimer.Text = "Start/Stop Timer";
            this.btTimer.UseVisualStyleBackColor = true;
            this.btTimer.Click += new System.EventHandler(this.btTimer_Click);
            // 
            // btTaskScheduler
            // 
            this.btTaskScheduler.Location = new System.Drawing.Point(491, 37);
            this.btTaskScheduler.Name = "btTaskScheduler";
            this.btTaskScheduler.Size = new System.Drawing.Size(90, 41);
            this.btTaskScheduler.TabIndex = 21;
            this.btTaskScheduler.Text = "Task Scheduler";
            this.btTaskScheduler.UseVisualStyleBackColor = true;
            this.btTaskScheduler.Click += new System.EventHandler(this.btTaskScheduler_Click);
            // 
            // lbNextBackup
            // 
            this.lbNextBackup.AutoSize = true;
            this.lbNextBackup.Location = new System.Drawing.Point(685, 87);
            this.lbNextBackup.Name = "lbNextBackup";
            this.lbNextBackup.Size = new System.Drawing.Size(91, 16);
            this.lbNextBackup.TabIndex = 22;
            this.lbNextBackup.Text = "lbNextBackup";
            // 
            // btDeleteConfig
            // 
            this.btDeleteConfig.Location = new System.Drawing.Point(684, 37);
            this.btDeleteConfig.Name = "btDeleteConfig";
            this.btDeleteConfig.Size = new System.Drawing.Size(95, 41);
            this.btDeleteConfig.TabIndex = 23;
            this.btDeleteConfig.Text = "Delete Config File";
            this.btDeleteConfig.UseVisualStyleBackColor = true;
            this.btDeleteConfig.Click += new System.EventHandler(this.btDeleteConfig_Click);
            // 
            // btInfo
            // 
            this.btInfo.Location = new System.Drawing.Point(785, 37);
            this.btInfo.Name = "btInfo";
            this.btInfo.Size = new System.Drawing.Size(49, 41);
            this.btInfo.TabIndex = 24;
            this.btInfo.Text = "Info";
            this.btInfo.UseVisualStyleBackColor = true;
            this.btInfo.Click += new System.EventHandler(this.btInfo_Click);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(844, 661);
            this.Controls.Add(this.btInfo);
            this.Controls.Add(this.btDeleteConfig);
            this.Controls.Add(this.lbNextBackup);
            this.Controls.Add(this.btTaskScheduler);
            this.Controls.Add(this.btTimer);
            this.Controls.Add(this.nmBackupMinute);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.nmBackupHour);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btRunBackup);
            this.Controls.Add(this.btTestConnection);
            this.Controls.Add(this.btLoadDatabaseList);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownMaxBackupCount);
            this.Controls.Add(this.txtDestinationFolder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btSetBackupFolder);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBoxBackupAllDatabases);
            this.Controls.Add(this.btSaveSettings);
            this.Controls.Add(this.btLoadSettings);
            this.Controls.Add(this.txtConnString);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Backup All Database (V1.0)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxBackupCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmBackupHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmBackupMinute)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtConnString;
        private System.Windows.Forms.Button btLoadSettings;
        private System.Windows.Forms.Button btSaveSettings;
        private System.Windows.Forms.CheckBox checkBoxBackupAllDatabases;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox checkedListIncludeDatabases;
        private System.Windows.Forms.CheckedListBox checkedListExcludeDatabases;
        private System.Windows.Forms.Button btSetBackupFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDestinationFolder;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxBackupCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btLoadDatabaseList;
        private System.Windows.Forms.Button btTestConnection;
        private System.Windows.Forms.Button btRunBackup;
        private System.Windows.Forms.Button btIncludeClear;
        private System.Windows.Forms.Button btIncludeSelectAll;
        private System.Windows.Forms.Button btExcludeClear;
        private System.Windows.Forms.Button btExcludeSelectAll;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nmBackupHour;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nmBackupMinute;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btTimer;
        private System.Windows.Forms.Button btTaskScheduler;
        private System.Windows.Forms.Label lbNextBackup;
        private System.Windows.Forms.Button btDeleteConfig;
        private System.Windows.Forms.Button btInfo;
    }
}

