namespace WinFormsDemo
{
    partial class Form1Menu
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
            this.button_SimpleDemo = new System.Windows.Forms.Button();
            this.button_progress_report = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(318, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Demo for MySqlBackup.NET 2.6.x (and perhaps above)";
            // 
            // button_SimpleDemo
            // 
            this.button_SimpleDemo.Location = new System.Drawing.Point(45, 67);
            this.button_SimpleDemo.Name = "button_SimpleDemo";
            this.button_SimpleDemo.Size = new System.Drawing.Size(423, 44);
            this.button_SimpleDemo.TabIndex = 1;
            this.button_SimpleDemo.Text = "Simple Demo - No Progress Report";
            this.button_SimpleDemo.UseVisualStyleBackColor = true;
            this.button_SimpleDemo.Click += new System.EventHandler(this.button_SimpleDemo_Click);
            // 
            // button_progress_report
            // 
            this.button_progress_report.Location = new System.Drawing.Point(45, 131);
            this.button_progress_report.Name = "button_progress_report";
            this.button_progress_report.Size = new System.Drawing.Size(423, 44);
            this.button_progress_report.TabIndex = 2;
            this.button_progress_report.Text = "Progress Reporting";
            this.button_progress_report.UseVisualStyleBackColor = true;
            this.button_progress_report.Click += new System.EventHandler(this.button_progress_report_Click);
            // 
            // Form1Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 353);
            this.Controls.Add(this.button_progress_report);
            this.Controls.Add(this.button_SimpleDemo);
            this.Controls.Add(this.label1);
            this.Name = "Form1Menu";
            this.Text = "Form1Menu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_SimpleDemo;
        private System.Windows.Forms.Button button_progress_report;
    }
}

