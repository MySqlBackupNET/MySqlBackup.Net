namespace MySqlBackupTestApp
{
    partial class FormTestImportCaptureError
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
            this.btStart = new System.Windows.Forms.Button();
            this.btSetLogFilePath = new System.Windows.Forms.Button();
            this.txtLogFilePath = new System.Windows.Forms.TextBox();
            this.btResetLogFilePath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.nmErrorCount = new System.Windows.Forms.NumericUpDown();
            this.cbIgnoreSqlError = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtError = new System.Windows.Forms.TextBox();
            this.btClearLogFilePath = new System.Windows.Forms.Button();
            this.txtLastErrorSqlSyntax = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btViewDump = new System.Windows.Forms.Button();
            this.btExport = new System.Windows.Forms.Button();
            this.btViewErrorLog = new System.Windows.Forms.Button();
            this.btStartFile = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nmErrorCount)).BeginInit();
            this.SuspendLayout();
            // 
            // btStart
            // 
            this.btStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btStart.Location = new System.Drawing.Point(155, 11);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(166, 28);
            this.btStart.TabIndex = 0;
            this.btStart.Text = "Run Error Test (Sample Dump)";
            this.btStart.UseVisualStyleBackColor = false;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // btSetLogFilePath
            // 
            this.btSetLogFilePath.Location = new System.Drawing.Point(327, 12);
            this.btSetLogFilePath.Name = "btSetLogFilePath";
            this.btSetLogFilePath.Size = new System.Drawing.Size(174, 27);
            this.btSetLogFilePath.TabIndex = 1;
            this.btSetLogFilePath.Text = "New Error Log File Path\r\n";
            this.btSetLogFilePath.UseVisualStyleBackColor = true;
            this.btSetLogFilePath.Click += new System.EventHandler(this.btSetLogFilePath_Click);
            // 
            // txtLogFilePath
            // 
            this.txtLogFilePath.Location = new System.Drawing.Point(110, 45);
            this.txtLogFilePath.Name = "txtLogFilePath";
            this.txtLogFilePath.Size = new System.Drawing.Size(712, 22);
            this.txtLogFilePath.TabIndex = 2;
            // 
            // btResetLogFilePath
            // 
            this.btResetLogFilePath.Location = new System.Drawing.Point(507, 11);
            this.btResetLogFilePath.Name = "btResetLogFilePath";
            this.btResetLogFilePath.Size = new System.Drawing.Size(174, 28);
            this.btResetLogFilePath.TabIndex = 3;
            this.btResetLogFilePath.Text = "Reset Log File Path";
            this.btResetLogFilePath.UseVisualStyleBackColor = true;
            this.btResetLogFilePath.Click += new System.EventHandler(this.btResetLogFilePath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Generate Total Error:";
            // 
            // nmErrorCount
            // 
            this.nmErrorCount.Location = new System.Drawing.Point(151, 78);
            this.nmErrorCount.Name = "nmErrorCount";
            this.nmErrorCount.Size = new System.Drawing.Size(69, 22);
            this.nmErrorCount.TabIndex = 5;
            this.nmErrorCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // cbIgnoreSqlError
            // 
            this.cbIgnoreSqlError.AutoSize = true;
            this.cbIgnoreSqlError.Location = new System.Drawing.Point(240, 78);
            this.cbIgnoreSqlError.Name = "cbIgnoreSqlError";
            this.cbIgnoreSqlError.Size = new System.Drawing.Size(126, 20);
            this.cbIgnoreSqlError.TabIndex = 6;
            this.cbIgnoreSqlError.Text = "Ignore SQL Error";
            this.cbIgnoreSqlError.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 223);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(588, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "View Error Log Content / View Sample Error SQL Dump Content / Export SQL Dump fro" +
    "m Database";
            // 
            // txtError
            // 
            this.txtError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtError.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtError.Location = new System.Drawing.Point(12, 242);
            this.txtError.Multiline = true;
            this.txtError.Name = "txtError";
            this.txtError.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtError.Size = new System.Drawing.Size(892, 257);
            this.txtError.TabIndex = 8;
            // 
            // btClearLogFilePath
            // 
            this.btClearLogFilePath.Location = new System.Drawing.Point(687, 11);
            this.btClearLogFilePath.Name = "btClearLogFilePath";
            this.btClearLogFilePath.Size = new System.Drawing.Size(174, 28);
            this.btClearLogFilePath.TabIndex = 9;
            this.btClearLogFilePath.Text = "Clear Log File Path";
            this.btClearLogFilePath.UseVisualStyleBackColor = true;
            this.btClearLogFilePath.Click += new System.EventHandler(this.btClearLogFilePath_Click);
            // 
            // txtLastErrorSqlSyntax
            // 
            this.txtLastErrorSqlSyntax.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLastErrorSqlSyntax.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastErrorSqlSyntax.Location = new System.Drawing.Point(12, 163);
            this.txtLastErrorSqlSyntax.Multiline = true;
            this.txtLastErrorSqlSyntax.Name = "txtLastErrorSqlSyntax";
            this.txtLastErrorSqlSyntax.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLastErrorSqlSyntax.Size = new System.Drawing.Size(892, 57);
            this.txtLastErrorSqlSyntax.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Last Error SQL Syntax:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 16);
            this.label4.TabIndex = 12;
            this.label4.Text = "Error File Path:";
            // 
            // btViewDump
            // 
            this.btViewDump.Location = new System.Drawing.Point(194, 106);
            this.btViewDump.Name = "btViewDump";
            this.btViewDump.Size = new System.Drawing.Size(284, 28);
            this.btViewDump.TabIndex = 13;
            this.btViewDump.Text = "View Sample Error SQL Dump Content";
            this.btViewDump.UseVisualStyleBackColor = true;
            this.btViewDump.Click += new System.EventHandler(this.btViewDump_Click);
            // 
            // btExport
            // 
            this.btExport.Location = new System.Drawing.Point(484, 106);
            this.btExport.Name = "btExport";
            this.btExport.Size = new System.Drawing.Size(245, 28);
            this.btExport.TabIndex = 14;
            this.btExport.Text = "Export SQL Dump from Database";
            this.btExport.UseVisualStyleBackColor = true;
            this.btExport.Click += new System.EventHandler(this.btExport_Click);
            // 
            // btViewErrorLog
            // 
            this.btViewErrorLog.Location = new System.Drawing.Point(12, 106);
            this.btViewErrorLog.Name = "btViewErrorLog";
            this.btViewErrorLog.Size = new System.Drawing.Size(176, 28);
            this.btViewErrorLog.TabIndex = 15;
            this.btViewErrorLog.Text = "View Error Log Content\r\n";
            this.btViewErrorLog.UseVisualStyleBackColor = true;
            this.btViewErrorLog.Click += new System.EventHandler(this.btViewErrorLog_Click);
            // 
            // btStartFile
            // 
            this.btStartFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btStartFile.Location = new System.Drawing.Point(12, 11);
            this.btStartFile.Name = "btStartFile";
            this.btStartFile.Size = new System.Drawing.Size(137, 28);
            this.btStartFile.TabIndex = 16;
            this.btStartFile.Text = "Run Error Test (File)";
            this.btStartFile.UseVisualStyleBackColor = false;
            this.btStartFile.Click += new System.EventHandler(this.BtStartFile_Click);
            // 
            // FormTestImportCaptureError
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(916, 511);
            this.Controls.Add(this.btStartFile);
            this.Controls.Add(this.btViewErrorLog);
            this.Controls.Add(this.btExport);
            this.Controls.Add(this.btViewDump);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtLastErrorSqlSyntax);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btClearLogFilePath);
            this.Controls.Add(this.txtError);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbIgnoreSqlError);
            this.Controls.Add(this.nmErrorCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btResetLogFilePath);
            this.Controls.Add(this.txtLogFilePath);
            this.Controls.Add(this.btSetLogFilePath);
            this.Controls.Add(this.btStart);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FormTestImportCaptureError";
            this.Text = "FormTestImportCaptureError";
            ((System.ComponentModel.ISupportInitialize)(this.nmErrorCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Button btSetLogFilePath;
        private System.Windows.Forms.TextBox txtLogFilePath;
        private System.Windows.Forms.Button btResetLogFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nmErrorCount;
        private System.Windows.Forms.CheckBox cbIgnoreSqlError;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtError;
        private System.Windows.Forms.Button btClearLogFilePath;
        private System.Windows.Forms.TextBox txtLastErrorSqlSyntax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btViewDump;
        private System.Windows.Forms.Button btExport;
        private System.Windows.Forms.Button btViewErrorLog;
        private System.Windows.Forms.Button btStartFile;
    }
}