namespace MySqlBackupTestApp
{
    partial class FormTestViewDependencies
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTestViewDependencies));
            this.btRun = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDatabaseName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtStep1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtStep2 = new System.Windows.Forms.TextBox();
            this.txtStep4 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtSHA1 = new System.Windows.Forms.TextBox();
            this.txtSHA2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btRun
            // 
            this.btRun.Location = new System.Drawing.Point(12, 12);
            this.btRun.Name = "btRun";
            this.btRun.Size = new System.Drawing.Size(98, 30);
            this.btRun.TabIndex = 0;
            this.btRun.Text = "Run Test";
            this.btRun.UseVisualStyleBackColor = true;
            this.btRun.Click += new System.EventHandler(this.btRun_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(116, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Database Name:";
            // 
            // txtDatabaseName
            // 
            this.txtDatabaseName.Location = new System.Drawing.Point(227, 16);
            this.txtDatabaseName.Name = "txtDatabaseName";
            this.txtDatabaseName.Size = new System.Drawing.Size(100, 22);
            this.txtDatabaseName.TabIndex = 2;
            this.txtDatabaseName.Text = "test_view1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(523, 64);
            this.label2.TabIndex = 3;
            this.label2.Text = resources.GetString("label2.Text");
            this.label2.UseMnemonic = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(172, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Step 1: Pre-Arranged SQL";
            // 
            // txtStep1
            // 
            this.txtStep1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStep1.Location = new System.Drawing.Point(12, 194);
            this.txtStep1.Multiline = true;
            this.txtStep1.Name = "txtStep1";
            this.txtStep1.Size = new System.Drawing.Size(353, 267);
            this.txtStep1.TabIndex = 6;
            this.txtStep1.Text = resources.GetString("txtStep1.Text");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(381, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Step 2 (Output) SHA128:";
            // 
            // txtStep2
            // 
            this.txtStep2.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStep2.Location = new System.Drawing.Point(384, 194);
            this.txtStep2.Multiline = true;
            this.txtStep2.Name = "txtStep2";
            this.txtStep2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtStep2.Size = new System.Drawing.Size(393, 267);
            this.txtStep2.TabIndex = 8;
            // 
            // txtStep4
            // 
            this.txtStep4.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStep4.Location = new System.Drawing.Point(797, 194);
            this.txtStep4.Multiline = true;
            this.txtStep4.Name = "txtStep4";
            this.txtStep4.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtStep4.Size = new System.Drawing.Size(393, 267);
            this.txtStep4.TabIndex = 10;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(794, 150);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(158, 16);
            this.label11.TabIndex = 9;
            this.label11.Text = "Step 4 (Output) SHA128:";
            // 
            // txtSHA1
            // 
            this.txtSHA1.Location = new System.Drawing.Point(384, 169);
            this.txtSHA1.Name = "txtSHA1";
            this.txtSHA1.Size = new System.Drawing.Size(393, 22);
            this.txtSHA1.TabIndex = 13;
            // 
            // txtSHA2
            // 
            this.txtSHA2.Location = new System.Drawing.Point(797, 169);
            this.txtSHA2.Name = "txtSHA2";
            this.txtSHA2.Size = new System.Drawing.Size(393, 22);
            this.txtSHA2.TabIndex = 14;
            // 
            // FormTestViewDependencies
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1205, 474);
            this.Controls.Add(this.txtSHA2);
            this.Controls.Add(this.txtSHA1);
            this.Controls.Add(this.txtStep4);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtStep2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtStep1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDatabaseName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btRun);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FormTestViewDependencies";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export / Import VIEW with Dependencies within VIEWs";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btRun;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDatabaseName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtStep1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtStep2;
        private System.Windows.Forms.TextBox txtStep4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtSHA1;
        private System.Windows.Forms.TextBox txtSHA2;
    }
}