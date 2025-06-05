namespace Backup_All_Databases
{
    partial class FormBackup
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
            this.components = new System.ComponentModel.Container();
            this.btRunBackup = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btOpenLogFile = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btRunBackup
            // 
            this.btRunBackup.Location = new System.Drawing.Point(12, 12);
            this.btRunBackup.Name = "btRunBackup";
            this.btRunBackup.Size = new System.Drawing.Size(95, 33);
            this.btRunBackup.TabIndex = 15;
            this.btRunBackup.Text = "Run Backup";
            this.btRunBackup.UseVisualStyleBackColor = true;
            this.btRunBackup.Click += new System.EventHandler(this.btRunBackup_Click);
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(113, 12);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(95, 33);
            this.btCancel.TabIndex = 16;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.textBox1.ForeColor = System.Drawing.Color.Gainsboro;
            this.textBox1.Location = new System.Drawing.Point(12, 51);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(960, 598);
            this.textBox1.TabIndex = 17;
            // 
            // toolTip1
            // 
            this.toolTip1.BackColor = System.Drawing.Color.Gold;
            this.toolTip1.IsBalloon = true;
            // 
            // btOpenLogFile
            // 
            this.btOpenLogFile.Location = new System.Drawing.Point(214, 12);
            this.btOpenLogFile.Name = "btOpenLogFile";
            this.btOpenLogFile.Size = new System.Drawing.Size(130, 33);
            this.btOpenLogFile.TabIndex = 18;
            this.btOpenLogFile.Text = "Open Log File";
            this.btOpenLogFile.UseVisualStyleBackColor = true;
            this.btOpenLogFile.Click += new System.EventHandler(this.btOpenLogFile_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(350, 12);
            this.progressBar1.MarqueeAnimationSpeed = 10;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(223, 33);
            this.progressBar1.Step = 5;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 19;
            this.progressBar1.Visible = false;
            // 
            // FormBackup
            // 
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btOpenLogFile);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btRunBackup);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBackup";
            this.ShowInTaskbar = false;
            this.Text = "Backup";
            this.Load += new System.EventHandler(this.FormBackup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btRunBackup;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btOpenLogFile;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}
