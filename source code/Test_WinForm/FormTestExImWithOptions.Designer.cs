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
            this.cbExAddCreateDatabase = new System.Windows.Forms.CheckBox();
            this.cbAddDropTable = new System.Windows.Forms.CheckBox();
            this.cbExExportRows = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nmExMaxSqlLength = new System.Windows.Forms.NumericUpDown();
            this.cbExExportRoutines = new System.Windows.Forms.CheckBox();
            this.cbImIgnoreSqlErrors = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtImErrorLogFile = new System.Windows.Forms.TextBox();
            this.cbExExportRoutinesWithoutDefiner = new System.Windows.Forms.CheckBox();
            this.cbExResetAutoIncrement = new System.Windows.Forms.CheckBox();
            this.btImErrorFile = new System.Windows.Forms.Button();
            this.checkBox_WrapInTransaction = new System.Windows.Forms.CheckBox();
            this.comboBox_RowsExportMode = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbAddCreateTable = new System.Windows.Forms.CheckBox();
            this.cbAddDropDatabase = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dropTextEncoding = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dropBlobExportMode = new System.Windows.Forms.ComboBox();
            this.cbAllowBinaryChar = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nmExMaxSqlLength)).BeginInit();
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
            this.button_Restore.Location = new System.Drawing.Point(387, 38);
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
            this.cbExRecordDumpTime.Location = new System.Drawing.Point(15, 153);
            this.cbExRecordDumpTime.Name = "cbExRecordDumpTime";
            this.cbExRecordDumpTime.Size = new System.Drawing.Size(134, 19);
            this.cbExRecordDumpTime.TabIndex = 3;
            this.cbExRecordDumpTime.Text = "Record Dump Time";
            this.cbExRecordDumpTime.UseVisualStyleBackColor = true;
            // 
            // cbExAddCreateDatabase
            // 
            this.cbExAddCreateDatabase.AutoSize = true;
            this.cbExAddCreateDatabase.Location = new System.Drawing.Point(185, 78);
            this.cbExAddCreateDatabase.Name = "cbExAddCreateDatabase";
            this.cbExAddCreateDatabase.Size = new System.Drawing.Size(144, 19);
            this.cbExAddCreateDatabase.TabIndex = 5;
            this.cbExAddCreateDatabase.Text = "Add Create Database";
            this.cbExAddCreateDatabase.UseVisualStyleBackColor = true;
            // 
            // cbAddDropTable
            // 
            this.cbAddDropTable.AutoSize = true;
            this.cbAddDropTable.Checked = true;
            this.cbAddDropTable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAddDropTable.Location = new System.Drawing.Point(15, 103);
            this.cbAddDropTable.Name = "cbAddDropTable";
            this.cbAddDropTable.Size = new System.Drawing.Size(110, 19);
            this.cbAddDropTable.TabIndex = 8;
            this.cbAddDropTable.Text = "Add Drop Table";
            this.cbAddDropTable.UseVisualStyleBackColor = true;
            // 
            // cbExExportRows
            // 
            this.cbExExportRows.AutoSize = true;
            this.cbExExportRows.Checked = true;
            this.cbExExportRows.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExExportRows.Location = new System.Drawing.Point(15, 128);
            this.cbExExportRows.Name = "cbExExportRows";
            this.cbExExportRows.Size = new System.Drawing.Size(95, 19);
            this.cbExExportRows.TabIndex = 9;
            this.cbExExportRows.Text = "Export Rows";
            this.cbExExportRows.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "Max SQL Length:";
            // 
            // nmExMaxSqlLength
            // 
            this.nmExMaxSqlLength.Location = new System.Drawing.Point(120, 178);
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
            this.cbExExportRoutines.Location = new System.Drawing.Point(15, 214);
            this.cbExExportRoutines.Name = "cbExExportRoutines";
            this.cbExExportRoutines.Size = new System.Drawing.Size(190, 34);
            this.cbExExportRoutines.TabIndex = 13;
            this.cbExExportRoutines.Text = "Export Procedures, Functions,\r\nTriggers, Events, Views";
            this.cbExExportRoutines.UseVisualStyleBackColor = true;
            // 
            // cbImIgnoreSqlErrors
            // 
            this.cbImIgnoreSqlErrors.AutoSize = true;
            this.cbImIgnoreSqlErrors.Location = new System.Drawing.Point(393, 78);
            this.cbImIgnoreSqlErrors.Name = "cbImIgnoreSqlErrors";
            this.cbImIgnoreSqlErrors.Size = new System.Drawing.Size(125, 19);
            this.cbImIgnoreSqlErrors.TabIndex = 25;
            this.cbImIgnoreSqlErrors.Text = "Ignore SQL Errors";
            this.cbImIgnoreSqlErrors.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(390, 107);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(134, 15);
            this.label9.TabIndex = 26;
            this.label9.Text = "Error Log File Location:";
            // 
            // txtImErrorLogFile
            // 
            this.txtImErrorLogFile.BackColor = System.Drawing.Color.White;
            this.txtImErrorLogFile.Location = new System.Drawing.Point(431, 126);
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
            this.cbExExportRoutinesWithoutDefiner.Location = new System.Drawing.Point(15, 254);
            this.cbExExportRoutinesWithoutDefiner.Name = "cbExExportRoutinesWithoutDefiner";
            this.cbExExportRoutinesWithoutDefiner.Size = new System.Drawing.Size(200, 19);
            this.cbExExportRoutinesWithoutDefiner.TabIndex = 29;
            this.cbExExportRoutinesWithoutDefiner.Text = "Export Routines Without Definer";
            this.cbExExportRoutinesWithoutDefiner.UseVisualStyleBackColor = true;
            // 
            // cbExResetAutoIncrement
            // 
            this.cbExResetAutoIncrement.AutoSize = true;
            this.cbExResetAutoIncrement.Location = new System.Drawing.Point(185, 154);
            this.cbExResetAutoIncrement.Name = "cbExResetAutoIncrement";
            this.cbExResetAutoIncrement.Size = new System.Drawing.Size(144, 19);
            this.cbExResetAutoIncrement.TabIndex = 30;
            this.cbExResetAutoIncrement.Text = "Reset Auto-Increment";
            this.cbExResetAutoIncrement.UseVisualStyleBackColor = true;
            // 
            // btImErrorFile
            // 
            this.btImErrorFile.Image = global::MySqlBackupTestApp.Properties.Resources.folder;
            this.btImErrorFile.Location = new System.Drawing.Point(393, 125);
            this.btImErrorFile.Name = "btImErrorFile";
            this.btImErrorFile.Size = new System.Drawing.Size(32, 23);
            this.btImErrorFile.TabIndex = 28;
            this.btImErrorFile.UseVisualStyleBackColor = true;
            this.btImErrorFile.Click += new System.EventHandler(this.btImErrorFile_Click);
            // 
            // checkBox_WrapInTransaction
            // 
            this.checkBox_WrapInTransaction.AutoSize = true;
            this.checkBox_WrapInTransaction.Location = new System.Drawing.Point(15, 279);
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
            this.comboBox_RowsExportMode.Location = new System.Drawing.Point(130, 307);
            this.comboBox_RowsExportMode.Name = "comboBox_RowsExportMode";
            this.comboBox_RowsExportMode.Size = new System.Drawing.Size(178, 23);
            this.comboBox_RowsExportMode.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 310);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "Rows Export Mode";
            // 
            // cbAddCreateTable
            // 
            this.cbAddCreateTable.AutoSize = true;
            this.cbAddCreateTable.Checked = true;
            this.cbAddCreateTable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAddCreateTable.Location = new System.Drawing.Point(185, 103);
            this.cbAddCreateTable.Name = "cbAddCreateTable";
            this.cbAddCreateTable.Size = new System.Drawing.Size(120, 19);
            this.cbAddCreateTable.TabIndex = 32;
            this.cbAddCreateTable.Text = "Add Create Table";
            this.cbAddCreateTable.UseVisualStyleBackColor = true;
            // 
            // cbAddDropDatabase
            // 
            this.cbAddDropDatabase.AutoSize = true;
            this.cbAddDropDatabase.Location = new System.Drawing.Point(15, 78);
            this.cbAddDropDatabase.Name = "cbAddDropDatabase";
            this.cbAddDropDatabase.Size = new System.Drawing.Size(134, 19);
            this.cbAddDropDatabase.TabIndex = 33;
            this.cbAddDropDatabase.Text = "Add Drop Database";
            this.cbAddDropDatabase.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 339);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 15);
            this.label2.TabIndex = 34;
            this.label2.Text = "Text Encoding:";
            // 
            // dropTextEncoding
            // 
            this.dropTextEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropTextEncoding.FormattingEnabled = true;
            this.dropTextEncoding.Items.AddRange(new object[] {
            "UTF8 (without unicode byte order)",
            "UTF8",
            "ASCII"});
            this.dropTextEncoding.Location = new System.Drawing.Point(130, 336);
            this.dropTextEncoding.Name = "dropTextEncoding";
            this.dropTextEncoding.Size = new System.Drawing.Size(178, 23);
            this.dropTextEncoding.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 368);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 15);
            this.label4.TabIndex = 36;
            this.label4.Text = "BLOB Export Mode:";
            // 
            // dropBlobExportMode
            // 
            this.dropBlobExportMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropBlobExportMode.FormattingEnabled = true;
            this.dropBlobExportMode.Items.AddRange(new object[] {
            "Hexdecimal String",
            "Binary String"});
            this.dropBlobExportMode.Location = new System.Drawing.Point(130, 365);
            this.dropBlobExportMode.Name = "dropBlobExportMode";
            this.dropBlobExportMode.Size = new System.Drawing.Size(178, 23);
            this.dropBlobExportMode.TabIndex = 37;
            // 
            // cbAllowBinaryChar
            // 
            this.cbAllowBinaryChar.AutoSize = true;
            this.cbAllowBinaryChar.Location = new System.Drawing.Point(15, 405);
            this.cbAllowBinaryChar.Name = "cbAllowBinaryChar";
            this.cbAllowBinaryChar.Size = new System.Drawing.Size(221, 19);
            this.cbAllowBinaryChar.TabIndex = 38;
            this.cbAllowBinaryChar.Text = "Allow BlobExportMode = BinaryChar";
            this.cbAllowBinaryChar.UseVisualStyleBackColor = true;
            // 
            // FormExImWithOptions
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(694, 509);
            this.Controls.Add(this.cbAllowBinaryChar);
            this.Controls.Add(this.dropBlobExportMode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox_RowsExportMode);
            this.Controls.Add(this.checkBox_WrapInTransaction);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dropTextEncoding);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbAddDropDatabase);
            this.Controls.Add(this.cbAddCreateTable);
            this.Controls.Add(this.cbExResetAutoIncrement);
            this.Controls.Add(this.cbExExportRoutinesWithoutDefiner);
            this.Controls.Add(this.btImErrorFile);
            this.Controls.Add(this.txtImErrorLogFile);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cbImIgnoreSqlErrors);
            this.Controls.Add(this.cbExExportRoutines);
            this.Controls.Add(this.nmExMaxSqlLength);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbExExportRows);
            this.Controls.Add(this.cbAddDropTable);
            this.Controls.Add(this.cbExAddCreateDatabase);
            this.Controls.Add(this.cbExRecordDumpTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_Restore);
            this.Controls.Add(this.button_Backup);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FormExImWithOptions";
            this.Text = "Form Export Import With Options";
            ((System.ComponentModel.ISupportInitialize)(this.nmExMaxSqlLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Backup;
        private System.Windows.Forms.Button button_Restore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbExRecordDumpTime;
        private System.Windows.Forms.CheckBox cbExAddCreateDatabase;
        private System.Windows.Forms.CheckBox cbAddDropTable;
        private System.Windows.Forms.CheckBox cbExExportRows;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nmExMaxSqlLength;
        private System.Windows.Forms.CheckBox cbExExportRoutines;
        private System.Windows.Forms.CheckBox cbImIgnoreSqlErrors;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtImErrorLogFile;
        private System.Windows.Forms.Button btImErrorFile;
        private System.Windows.Forms.CheckBox cbExExportRoutinesWithoutDefiner;
        private System.Windows.Forms.CheckBox cbExResetAutoIncrement;
        private System.Windows.Forms.ComboBox comboBox_RowsExportMode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBox_WrapInTransaction;
        private System.Windows.Forms.CheckBox cbAddCreateTable;
        private System.Windows.Forms.CheckBox cbAddDropDatabase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox dropTextEncoding;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox dropBlobExportMode;
        private System.Windows.Forms.CheckBox cbAllowBinaryChar;
    }
}