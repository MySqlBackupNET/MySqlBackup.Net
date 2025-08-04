namespace WinFormsDemo
{
    partial class FormSimpleDemo
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
            this.statusStrip1.SuspendLayout();
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
            this.btBackup.Location = new System.Drawing.Point(12, 68);
            this.btBackup.Name = "btBackup";
            this.btBackup.Size = new System.Drawing.Size(109, 39);
            this.btBackup.TabIndex = 2;
            this.btBackup.Text = "Backup";
            this.btBackup.UseVisualStyleBackColor = true;
            this.btBackup.Click += new System.EventHandler(this.btBackup_Click);
            // 
            // btRestore
            // 
            this.btRestore.Location = new System.Drawing.Point(144, 68);
            this.btRestore.Name = "btRestore";
            this.btRestore.Size = new System.Drawing.Size(109, 39);
            this.btRestore.TabIndex = 3;
            this.btRestore.Text = "Restore";
            this.btRestore.UseVisualStyleBackColor = true;
            this.btRestore.Click += new System.EventHandler(this.btRestore_Click);
            // 
            // btStop
            // 
            this.btStop.Enabled = false;
            this.btStop.Location = new System.Drawing.Point(272, 68);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(109, 39);
            this.btStop.TabIndex = 4;
            this.btStop.Text = "Stop";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // btOpenBackupFile
            // 
            this.btOpenBackupFile.Location = new System.Drawing.Point(399, 68);
            this.btOpenBackupFile.Name = "btOpenBackupFile";
            this.btOpenBackupFile.Size = new System.Drawing.Size(153, 39);
            this.btOpenBackupFile.TabIndex = 6;
            this.btOpenBackupFile.Text = "Open Backup File";
            this.btOpenBackupFile.UseVisualStyleBackColor = true;
            this.btOpenBackupFile.Click += new System.EventHandler(this.btOpenBackupFile_Click);
            // 
            // txtBackupFile
            // 
            this.txtBackupFile.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBackupFile.Location = new System.Drawing.Point(9, 148);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 539);
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
            this.label2.Location = new System.Drawing.Point(9, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "SQL Backup File";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Task Log:";
            // 
            // txtLog
            // 
            this.txtLog.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.Color.DarkGray;
            this.txtLog.Location = new System.Drawing.Point(12, 216);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(857, 320);
            this.txtLog.TabIndex = 11;
            this.txtLog.WordWrap = false;
            // 
            // btClearLog
            // 
            this.btClearLog.Location = new System.Drawing.Point(92, 183);
            this.btClearLog.Name = "btClearLog";
            this.btClearLog.Size = new System.Drawing.Size(100, 27);
            this.btClearLog.TabIndex = 12;
            this.btClearLog.Text = "Clear Log";
            this.btClearLog.UseVisualStyleBackColor = true;
            this.btClearLog.Click += new System.EventHandler(this.btClearLog_Click);
            // 
            // btTestConnection
            // 
            this.btTestConnection.Location = new System.Drawing.Point(569, 68);
            this.btTestConnection.Name = "btTestConnection";
            this.btTestConnection.Size = new System.Drawing.Size(153, 39);
            this.btTestConnection.TabIndex = 13;
            this.btTestConnection.Text = "Test Connection";
            this.btTestConnection.UseVisualStyleBackColor = true;
            this.btTestConnection.Click += new System.EventHandler(this.btTestConnection_Click);
            // 
            // FormSimpleDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 561);
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
            this.Name = "FormSimpleDemo";
            this.Text = "FormSimpleDemo";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
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
    }
}