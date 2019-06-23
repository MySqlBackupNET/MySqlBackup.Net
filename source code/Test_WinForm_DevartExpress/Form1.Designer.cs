namespace TestApp_DevartExpressMySql
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
            this.btBackup = new System.Windows.Forms.Button();
            this.btRestore = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btLoadSample = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btExecute = new System.Windows.Forms.Button();
            this.btSelect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtConStr = new System.Windows.Forms.TextBox();
            this.txtSql = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btBackup
            // 
            this.btBackup.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btBackup.Location = new System.Drawing.Point(3, 6);
            this.btBackup.Name = "btBackup";
            this.btBackup.Size = new System.Drawing.Size(180, 25);
            this.btBackup.TabIndex = 0;
            this.btBackup.Text = "MySqlBackup - Backup";
            this.btBackup.UseVisualStyleBackColor = true;
            this.btBackup.Click += new System.EventHandler(this.btBackup_Click);
            // 
            // btRestore
            // 
            this.btRestore.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRestore.Location = new System.Drawing.Point(189, 6);
            this.btRestore.Name = "btRestore";
            this.btRestore.Size = new System.Drawing.Size(180, 25);
            this.btRestore.TabIndex = 1;
            this.btRestore.Text = "MySqlBackup - Restore";
            this.btRestore.UseVisualStyleBackColor = true;
            this.btRestore.Click += new System.EventHandler(this.btRestore_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btLoadSample);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btExecute);
            this.panel1.Controls.Add(this.btSelect);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtConStr);
            this.panel1.Controls.Add(this.btBackup);
            this.panel1.Controls.Add(this.btRestore);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(901, 96);
            this.panel1.TabIndex = 2;
            // 
            // btLoadSample
            // 
            this.btLoadSample.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLoadSample.Location = new System.Drawing.Point(3, 65);
            this.btLoadSample.Name = "btLoadSample";
            this.btLoadSample.Size = new System.Drawing.Size(180, 25);
            this.btLoadSample.TabIndex = 8;
            this.btLoadSample.Text = "Load Sample SQL";
            this.btLoadSample.UseVisualStyleBackColor = true;
            this.btLoadSample.Click += new System.EventHandler(this.btLoadSample_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(375, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "MySQL Connection String:";
            // 
            // btExecute
            // 
            this.btExecute.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btExecute.Location = new System.Drawing.Point(189, 35);
            this.btExecute.Name = "btExecute";
            this.btExecute.Size = new System.Drawing.Size(180, 25);
            this.btExecute.TabIndex = 6;
            this.btExecute.Text = "DevartExpress - Execute";
            this.btExecute.UseVisualStyleBackColor = true;
            this.btExecute.Click += new System.EventHandler(this.btExecute_Click);
            // 
            // btSelect
            // 
            this.btSelect.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSelect.Location = new System.Drawing.Point(3, 35);
            this.btSelect.Name = "btSelect";
            this.btSelect.Size = new System.Drawing.Size(180, 25);
            this.btSelect.TabIndex = 5;
            this.btSelect.Text = "DevartExpress - Select";
            this.btSelect.UseVisualStyleBackColor = true;
            this.btSelect.Click += new System.EventHandler(this.btSelect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(189, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "SQL:";
            // 
            // txtConStr
            // 
            this.txtConStr.Location = new System.Drawing.Point(375, 35);
            this.txtConStr.Name = "txtConStr";
            this.txtConStr.Size = new System.Drawing.Size(504, 22);
            this.txtConStr.TabIndex = 3;
            this.txtConStr.Text = "server=localhost;user=root;pwd=1234;database=test;";
            // 
            // txtSql
            // 
            this.txtSql.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSql.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSql.Location = new System.Drawing.Point(0, 96);
            this.txtSql.Multiline = true;
            this.txtSql.Name = "txtSql";
            this.txtSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSql.Size = new System.Drawing.Size(901, 229);
            this.txtSql.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 325);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(901, 220);
            this.dataGridView1.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(901, 545);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtSql);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btBackup;
        private System.Windows.Forms.Button btRestore;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtConStr;
        private System.Windows.Forms.TextBox txtSql;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btExecute;
        private System.Windows.Forms.Button btSelect;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btLoadSample;
    }
}

