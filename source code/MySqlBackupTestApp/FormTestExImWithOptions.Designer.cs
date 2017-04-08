namespace MySqlBackupTestApp
{
    partial class FormExImWithOptions
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
            this.button_Backup = new System.Windows.Forms.Button();
            this.button_Restore = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbExRecordDumpTime = new System.Windows.Forms.CheckBox();
            this.cbExEnableEncryption = new System.Windows.Forms.CheckBox();
            this.cbExAddCreateDatabase = new System.Windows.Forms.CheckBox();
            this.txtExPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbExAddDropCreateTable = new System.Windows.Forms.CheckBox();
            this.cbExExportRows = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nmExMaxSqlLength = new System.Windows.Forms.NumericUpDown();
            this.cbExExportRoutines = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtImPwd = new System.Windows.Forms.TextBox();
            this.cbImEnableEncryption = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtImTargetDatabase = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtImDefaultCharSet = new System.Windows.Forms.TextBox();
            this.cbImIgnoreSqlErrors = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtImErrorLogFile = new System.Windows.Forms.TextBox();
            this.cbExExportRoutinesWithoutDefiner = new System.Windows.Forms.CheckBox();
            this.cbExResetAutoIncrement = new System.Windows.Forms.CheckBox();
            this.btImErrorFile = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_WrapInTransaction = new System.Windows.Forms.CheckBox();
            this.comboBox_RowsExportMode = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nmExMaxSqlLength)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Backup
            // 
            this.button_Backup.Location = new System.Drawing.Point(16, 38);
            this.button_Backup.Margin = new System.Windows.Forms.Padding(8);
            this.button_Backup.Name = "button_Backup";
            this.button_Backup.Size = new System.Drawing.Size(203, 29);
            this.button_Backup.TabIndex = 0;
            this.button_Backup.Text = "Backup / Export";
            this.button_Backup.UseVisualStyleBackColor = true;
            this.button_Backup.Click += new System.EventHandler(this.button_Backup_Click);
            // 
            // button_Restore
            // 
            this.button_Restore.Location = new System.Drawing.Point(332, 38);
            this.button_Restore.Margin = new System.Windows.Forms.Padding(8);
            this.button_Restore.Name = "button_Restore";
            this.button_Restore.Size = new System.Drawing.Size(203, 29);
            this.button_Restore.TabIndex = 1;
            this.button_Restore.Text = "Restore / Import";
            this.button_Restore.UseVisualStyleBackColor = true;
            this.button_Restore.Click += new System.EventHandler(this.button_Restore_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Export / Import with Options";
            // 
            // cbExRecordDumpTime
            // 
            this.cbExRecordDumpTime.AutoSize = true;
            this.cbExRecordDumpTime.Checked = true;
            this.cbExRecordDumpTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExRecordDumpTime.Location = new System.Drawing.Point(16, 153);
            this.cbExRecordDumpTime.Name = "cbExRecordDumpTime";
            this.cbExRecordDumpTime.Size = new System.Drawing.Size(134, 19);
            this.cbExRecordDumpTime.TabIndex = 3;
            this.cbExRecordDumpTime.Text = "Record Dump Time";
            this.cbExRecordDumpTime.UseVisualStyleBackColor = true;
            // 
            // cbExEnableEncryption
            // 
            this.cbExEnableEncryption.AutoSize = true;
            this.cbExEnableEncryption.Location = new System.Drawing.Point(16, 207);
            this.cbExEnableEncryption.Name = "cbExEnableEncryption";
            this.cbExEnableEncryption.Size = new System.Drawing.Size(125, 19);
            this.cbExEnableEncryption.TabIndex = 4;
            this.cbExEnableEncryption.Text = "Enable Encryption";
            this.cbExEnableEncryption.UseVisualStyleBackColor = true;
            // 
            // cbExAddCreateDatabase
            // 
            this.cbExAddCreateDatabase.AutoSize = true;
            this.cbExAddCreateDatabase.Location = new System.Drawing.Point(16, 78);
            this.cbExAddCreateDatabase.Name = "cbExAddCreateDatabase";
            this.cbExAddCreateDatabase.Size = new System.Drawing.Size(144, 19);
            this.cbExAddCreateDatabase.TabIndex = 5;
            this.cbExAddCreateDatabase.Text = "Add Create Database";
            this.cbExAddCreateDatabase.UseVisualStyleBackColor = true;
            // 
            // txtExPassword
            // 
            this.txtExPassword.Location = new System.Drawing.Point(85, 232);
            this.txtExPassword.Name = "txtExPassword";
            this.txtExPassword.Size = new System.Drawing.Size(127, 21);
            this.txtExPassword.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 235);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Password:";
            // 
            // cbExAddDropCreateTable
            // 
            this.cbExAddDropCreateTable.AutoSize = true;
            this.cbExAddDropCreateTable.Checked = true;
            this.cbExAddDropCreateTable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExAddDropCreateTable.Location = new System.Drawing.Point(16, 103);
            this.cbExAddDropCreateTable.Name = "cbExAddDropCreateTable";
            this.cbExAddDropCreateTable.Size = new System.Drawing.Size(150, 19);
            this.cbExAddDropCreateTable.TabIndex = 8;
            this.cbExAddDropCreateTable.Text = "Add Drop/Create Table";
            this.cbExAddDropCreateTable.UseVisualStyleBackColor = true;
            // 
            // cbExExportRows
            // 
            this.cbExExportRows.AutoSize = true;
            this.cbExExportRows.Checked = true;
            this.cbExExportRows.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExExportRows.Location = new System.Drawing.Point(16, 128);
            this.cbExExportRows.Name = "cbExExportRows";
            this.cbExExportRows.Size = new System.Drawing.Size(95, 19);
            this.cbExExportRows.TabIndex = 9;
            this.cbExExportRows.Text = "Export Rows";
            this.cbExExportRows.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 261);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "Max SQL Length:";
            // 
            // nmExMaxSqlLength
            // 
            this.nmExMaxSqlLength.Location = new System.Drawing.Point(119, 259);
            this.nmExMaxSqlLength.Maximum = new decimal(new int[] {
            -1981284352,
            -1966660860,
            0,
            0});
            this.nmExMaxSqlLength.Name = "nmExMaxSqlLength";
            this.nmExMaxSqlLength.Size = new System.Drawing.Size(93, 21);
            this.nmExMaxSqlLength.TabIndex = 12;
            this.nmExMaxSqlLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nmExMaxSqlLength.Value = new decimal(new int[] {
            3145728,
            0,
            0,
            0});
            // 
            // cbExExportRoutines
            // 
            this.cbExExportRoutines.AutoSize = true;
            this.cbExExportRoutines.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cbExExportRoutines.Checked = true;
            this.cbExExportRoutines.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExExportRoutines.Location = new System.Drawing.Point(16, 286);
            this.cbExExportRoutines.Name = "cbExExportRoutines";
            this.cbExExportRoutines.Size = new System.Drawing.Size(190, 34);
            this.cbExExportRoutines.TabIndex = 13;
            this.cbExExportRoutines.Text = "Export Procedures, Functions,\r\nTriggers, Events, Views";
            this.cbExExportRoutines.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(329, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 15);
            this.label4.TabIndex = 17;
            this.label4.Text = "Password:";
            // 
            // txtImPwd
            // 
            this.txtImPwd.Location = new System.Drawing.Point(401, 103);
            this.txtImPwd.Name = "txtImPwd";
            this.txtImPwd.Size = new System.Drawing.Size(127, 21);
            this.txtImPwd.TabIndex = 16;
            // 
            // cbImEnableEncryption
            // 
            this.cbImEnableEncryption.AutoSize = true;
            this.cbImEnableEncryption.Location = new System.Drawing.Point(332, 78);
            this.cbImEnableEncryption.Name = "cbImEnableEncryption";
            this.cbImEnableEncryption.Size = new System.Drawing.Size(125, 19);
            this.cbImEnableEncryption.TabIndex = 15;
            this.cbImEnableEncryption.Text = "Enable Encryption";
            this.cbImEnableEncryption.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 15);
            this.label7.TabIndex = 22;
            this.label7.Text = "Target Database";
            // 
            // txtImTargetDatabase
            // 
            this.txtImTargetDatabase.Location = new System.Drawing.Point(115, 26);
            this.txtImTargetDatabase.Name = "txtImTargetDatabase";
            this.txtImTargetDatabase.Size = new System.Drawing.Size(100, 21);
            this.txtImTargetDatabase.TabIndex = 23;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtImDefaultCharSet);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtImTargetDatabase);
            this.groupBox1.Location = new System.Drawing.Point(332, 155);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 98);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import to New/Another Database";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(154, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 15);
            this.label5.TabIndex = 26;
            this.label5.Text = "(Optional)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 15);
            this.label8.TabIndex = 24;
            this.label8.Text = "Default Char Set";
            // 
            // txtImDefaultCharSet
            // 
            this.txtImDefaultCharSet.Location = new System.Drawing.Point(115, 53);
            this.txtImDefaultCharSet.Name = "txtImDefaultCharSet";
            this.txtImDefaultCharSet.Size = new System.Drawing.Size(100, 21);
            this.txtImDefaultCharSet.TabIndex = 25;
            // 
            // cbImIgnoreSqlErrors
            // 
            this.cbImIgnoreSqlErrors.AutoSize = true;
            this.cbImIgnoreSqlErrors.Location = new System.Drawing.Point(332, 130);
            this.cbImIgnoreSqlErrors.Name = "cbImIgnoreSqlErrors";
            this.cbImIgnoreSqlErrors.Size = new System.Drawing.Size(125, 19);
            this.cbImIgnoreSqlErrors.TabIndex = 25;
            this.cbImIgnoreSqlErrors.Text = "Ignore SQL Errors";
            this.cbImIgnoreSqlErrors.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(329, 265);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(134, 15);
            this.label9.TabIndex = 26;
            this.label9.Text = "Error Log File Location:";
            // 
            // txtImErrorLogFile
            // 
            this.txtImErrorLogFile.BackColor = System.Drawing.Color.White;
            this.txtImErrorLogFile.Location = new System.Drawing.Point(370, 284);
            this.txtImErrorLogFile.Name = "txtImErrorLogFile";
            this.txtImErrorLogFile.ReadOnly = true;
            this.txtImErrorLogFile.Size = new System.Drawing.Size(207, 21);
            this.txtImErrorLogFile.TabIndex = 27;
            // 
            // cbExExportRoutinesWithoutDefiner
            // 
            this.cbExExportRoutinesWithoutDefiner.AutoSize = true;
            this.cbExExportRoutinesWithoutDefiner.Checked = true;
            this.cbExExportRoutinesWithoutDefiner.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExExportRoutinesWithoutDefiner.Location = new System.Drawing.Point(16, 324);
            this.cbExExportRoutinesWithoutDefiner.Name = "cbExExportRoutinesWithoutDefiner";
            this.cbExExportRoutinesWithoutDefiner.Size = new System.Drawing.Size(200, 19);
            this.cbExExportRoutinesWithoutDefiner.TabIndex = 29;
            this.cbExExportRoutinesWithoutDefiner.Text = "Export Routines Without Definer";
            this.cbExExportRoutinesWithoutDefiner.UseVisualStyleBackColor = true;
            // 
            // cbExResetAutoIncrement
            // 
            this.cbExResetAutoIncrement.AutoSize = true;
            this.cbExResetAutoIncrement.Location = new System.Drawing.Point(16, 178);
            this.cbExResetAutoIncrement.Name = "cbExResetAutoIncrement";
            this.cbExResetAutoIncrement.Size = new System.Drawing.Size(144, 19);
            this.cbExResetAutoIncrement.TabIndex = 30;
            this.cbExResetAutoIncrement.Text = "Reset Auto-Increment";
            this.cbExResetAutoIncrement.UseVisualStyleBackColor = true;
            // 
            // btImErrorFile
            // 
            this.btImErrorFile.Image = global::MySqlBackupTestApp.Properties.Resources.folder;
            this.btImErrorFile.Location = new System.Drawing.Point(332, 283);
            this.btImErrorFile.Name = "btImErrorFile";
            this.btImErrorFile.Size = new System.Drawing.Size(32, 23);
            this.btImErrorFile.TabIndex = 28;
            this.btImErrorFile.UseVisualStyleBackColor = true;
            this.btImErrorFile.Click += new System.EventHandler(this.btImErrorFile_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_WrapInTransaction);
            this.groupBox2.Controls.Add(this.comboBox_RowsExportMode);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(16, 355);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(315, 72);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "New features in v2.0.7";
            // 
            // checkBox_WrapInTransaction
            // 
            this.checkBox_WrapInTransaction.AutoSize = true;
            this.checkBox_WrapInTransaction.Location = new System.Drawing.Point(11, 49);
            this.checkBox_WrapInTransaction.Name = "checkBox_WrapInTransaction";
            this.checkBox_WrapInTransaction.Size = new System.Drawing.Size(222, 19);
            this.checkBox_WrapInTransaction.TabIndex = 2;
            this.checkBox_WrapInTransaction.Text = "Wrap Exported Rows in Transaction";
            this.checkBox_WrapInTransaction.UseVisualStyleBackColor = true;
            // 
            // comboBox_RowsExportMode
            // 
            this.comboBox_RowsExportMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_RowsExportMode.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_RowsExportMode.FormattingEnabled = true;
            this.comboBox_RowsExportMode.Location = new System.Drawing.Point(123, 18);
            this.comboBox_RowsExportMode.Name = "comboBox_RowsExportMode";
            this.comboBox_RowsExportMode.Size = new System.Drawing.Size(181, 23);
            this.comboBox_RowsExportMode.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "Rows Export Mode";
            // 
            // FormExImWithOptions
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(694, 494);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cbExResetAutoIncrement);
            this.Controls.Add(this.cbExExportRoutinesWithoutDefiner);
            this.Controls.Add(this.btImErrorFile);
            this.Controls.Add(this.txtImErrorLogFile);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cbImIgnoreSqlErrors);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtImPwd);
            this.Controls.Add(this.cbImEnableEncryption);
            this.Controls.Add(this.cbExExportRoutines);
            this.Controls.Add(this.nmExMaxSqlLength);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbExExportRows);
            this.Controls.Add(this.cbExAddDropCreateTable);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtExPassword);
            this.Controls.Add(this.cbExAddCreateDatabase);
            this.Controls.Add(this.cbExEnableEncryption);
            this.Controls.Add(this.cbExRecordDumpTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_Restore);
            this.Controls.Add(this.button_Backup);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FormExImWithOptions";
            this.Text = "Form Export Import With Options";
            ((System.ComponentModel.ISupportInitialize)(this.nmExMaxSqlLength)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Backup;
        private System.Windows.Forms.Button button_Restore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbExRecordDumpTime;
        private System.Windows.Forms.CheckBox cbExEnableEncryption;
        private System.Windows.Forms.CheckBox cbExAddCreateDatabase;
        private System.Windows.Forms.TextBox txtExPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbExAddDropCreateTable;
        private System.Windows.Forms.CheckBox cbExExportRows;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nmExMaxSqlLength;
        private System.Windows.Forms.CheckBox cbExExportRoutines;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtImPwd;
        private System.Windows.Forms.CheckBox cbImEnableEncryption;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtImTargetDatabase;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtImDefaultCharSet;
        private System.Windows.Forms.CheckBox cbImIgnoreSqlErrors;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtImErrorLogFile;
        private System.Windows.Forms.Button btImErrorFile;
        private System.Windows.Forms.CheckBox cbExExportRoutinesWithoutDefiner;
        private System.Windows.Forms.CheckBox cbExResetAutoIncrement;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBox_RowsExportMode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBox_WrapInTransaction;
    }
}