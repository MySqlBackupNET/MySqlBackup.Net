namespace Backup_All_Databases
{
    partial class FormTaskScheduler
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
            this.btDeleteTaskScheduler = new System.Windows.Forms.Button();
            this.btCreateTaskScheduler = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbUser = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btDeleteTaskScheduler
            // 
            this.btDeleteTaskScheduler.Location = new System.Drawing.Point(313, 227);
            this.btDeleteTaskScheduler.Name = "btDeleteTaskScheduler";
            this.btDeleteTaskScheduler.Size = new System.Drawing.Size(187, 39);
            this.btDeleteTaskScheduler.TabIndex = 1;
            this.btDeleteTaskScheduler.Text = "Delete Task Scheduler";
            this.btDeleteTaskScheduler.UseVisualStyleBackColor = true;
            this.btDeleteTaskScheduler.Click += new System.EventHandler(this.btDeleteTaskScheduler_Click);
            // 
            // btCreateTaskScheduler
            // 
            this.btCreateTaskScheduler.BackColor = System.Drawing.Color.GreenYellow;
            this.btCreateTaskScheduler.Location = new System.Drawing.Point(97, 227);
            this.btCreateTaskScheduler.Name = "btCreateTaskScheduler";
            this.btCreateTaskScheduler.Size = new System.Drawing.Size(187, 39);
            this.btCreateTaskScheduler.TabIndex = 0;
            this.btCreateTaskScheduler.Text = "Create Task Scheduler";
            this.btCreateTaskScheduler.UseVisualStyleBackColor = false;
            this.btCreateTaskScheduler.Click += new System.EventHandler(this.btCreateTaskScheduler_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(94, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(406, 32);
            this.label1.TabIndex = 2;
            this.label1.Text = "This task require this program to \"Run As Administrator\".\r\nTask scheduler will be" +
    " run with current windows user:";
            // 
            // lbUser
            // 
            this.lbUser.Font = new System.Drawing.Font("Cascadia Code", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUser.Location = new System.Drawing.Point(94, 87);
            this.lbUser.Name = "lbUser";
            this.lbUser.Size = new System.Drawing.Size(406, 35);
            this.lbUser.TabIndex = 3;
            this.lbUser.Text = "lbUser";
            this.lbUser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(94, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(406, 60);
            this.label3.TabIndex = 4;
            this.label3.Text = "*Do not run the task scheduler with \"SYSTEM\"\r\ndue to the key stored in windows us" +
    "er profile.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormTaskScheduler
            // 
            this.ClientSize = new System.Drawing.Size(576, 323);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbUser);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btDeleteTaskScheduler);
            this.Controls.Add(this.btCreateTaskScheduler);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTaskScheduler";
            this.ShowInTaskbar = false;
            this.Text = "Task Scheduler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCreateTaskScheduler;
        private System.Windows.Forms.Button btDeleteTaskScheduler;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbUser;
        private System.Windows.Forms.Label label3;
    }
}
