namespace MySqlBackupTestApp
{
    partial class FormTestModifyHeadersFooters
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtHeaders = new System.Windows.Forms.TextBox();
            this.txtFooters = new System.Windows.Forms.TextBox();
            this.btGetHeadersFooters = new System.Windows.Forms.Button();
            this.btExportWithNewHeadersFooters = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Modify Headers and Footers";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btExportWithNewHeadersFooters);
            this.panel1.Controls.Add(this.btGetHeadersFooters);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(739, 43);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.txtFooters, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtHeaders, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 43);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(739, 350);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Modify Headers here:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(145, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "Modify Footers here:";
            // 
            // txtHeaders
            // 
            this.txtHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHeaders.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHeaders.Location = new System.Drawing.Point(3, 28);
            this.txtHeaders.Multiline = true;
            this.txtHeaders.Name = "txtHeaders";
            this.txtHeaders.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtHeaders.Size = new System.Drawing.Size(733, 144);
            this.txtHeaders.TabIndex = 2;
            // 
            // txtFooters
            // 
            this.txtFooters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFooters.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFooters.Location = new System.Drawing.Point(3, 203);
            this.txtFooters.Multiline = true;
            this.txtFooters.Name = "txtFooters";
            this.txtFooters.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFooters.Size = new System.Drawing.Size(733, 144);
            this.txtFooters.TabIndex = 3;
            // 
            // btGetHeadersFooters
            // 
            this.btGetHeadersFooters.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btGetHeadersFooters.Location = new System.Drawing.Point(229, 6);
            this.btGetHeadersFooters.Name = "btGetHeadersFooters";
            this.btGetHeadersFooters.Size = new System.Drawing.Size(150, 30);
            this.btGetHeadersFooters.TabIndex = 1;
            this.btGetHeadersFooters.Text = "Get Headers/Footers";
            this.btGetHeadersFooters.UseVisualStyleBackColor = true;
            this.btGetHeadersFooters.Click += new System.EventHandler(this.btGetHeadersFooters_Click);
            // 
            // btExportWithNewHeadersFooters
            // 
            this.btExportWithNewHeadersFooters.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btExportWithNewHeadersFooters.Location = new System.Drawing.Point(385, 6);
            this.btExportWithNewHeadersFooters.Name = "btExportWithNewHeadersFooters";
            this.btExportWithNewHeadersFooters.Size = new System.Drawing.Size(319, 30);
            this.btExportWithNewHeadersFooters.TabIndex = 2;
            this.btExportWithNewHeadersFooters.Text = "Export/Backup Database with New Headers/Footers";
            this.btExportWithNewHeadersFooters.UseVisualStyleBackColor = true;
            this.btExportWithNewHeadersFooters.Click += new System.EventHandler(this.btExportWithNewHeadersFooters_Click);
            // 
            // FormTestModifyHeadersFooters
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(739, 393);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormTestModifyHeadersFooters";
            this.Text = "FormTestModifyHeadersFooters";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btExportWithNewHeadersFooters;
        private System.Windows.Forms.Button btGetHeadersFooters;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtFooters;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtHeaders;
    }
}