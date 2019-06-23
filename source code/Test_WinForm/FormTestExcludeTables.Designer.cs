namespace MySqlBackupTestApp
{
    partial class FormTestExcludeTables
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
            this.btGetTables = new System.Windows.Forms.Button();
            this.btAll = new System.Windows.Forms.Button();
            this.btNone = new System.Windows.Forms.Button();
            this.btExport = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.lbTotal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(398, 51);
            this.label1.TabIndex = 0;
            this.label1.Text = "Exclude Tables (Black List)\r\n-------------------------------------------\r\nSelecte" +
    "d tables will not be exported/backup (Including rows).";
            // 
            // btGetTables
            // 
            this.btGetTables.Location = new System.Drawing.Point(16, 83);
            this.btGetTables.Name = "btGetTables";
            this.btGetTables.Size = new System.Drawing.Size(97, 27);
            this.btGetTables.TabIndex = 1;
            this.btGetTables.Text = "Get Tables";
            this.btGetTables.UseVisualStyleBackColor = true;
            this.btGetTables.Click += new System.EventHandler(this.btGetTables_Click);
            // 
            // btAll
            // 
            this.btAll.Location = new System.Drawing.Point(119, 83);
            this.btAll.Name = "btAll";
            this.btAll.Size = new System.Drawing.Size(58, 27);
            this.btAll.TabIndex = 2;
            this.btAll.Text = "All";
            this.btAll.UseVisualStyleBackColor = true;
            this.btAll.Click += new System.EventHandler(this.btAll_Click);
            // 
            // btNone
            // 
            this.btNone.Location = new System.Drawing.Point(183, 83);
            this.btNone.Name = "btNone";
            this.btNone.Size = new System.Drawing.Size(58, 27);
            this.btNone.TabIndex = 3;
            this.btNone.Text = "None";
            this.btNone.UseVisualStyleBackColor = true;
            this.btNone.Click += new System.EventHandler(this.btNone_Click);
            // 
            // btExport
            // 
            this.btExport.Location = new System.Drawing.Point(247, 83);
            this.btExport.Name = "btExport";
            this.btExport.Size = new System.Drawing.Size(199, 27);
            this.btExport.TabIndex = 4;
            this.btExport.Text = "Backup/Export Database";
            this.btExport.UseVisualStyleBackColor = true;
            this.btExport.Click += new System.EventHandler(this.btExport_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBox1.ColumnWidth = 270;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.HorizontalScrollbar = true;
            this.checkedListBox1.Location = new System.Drawing.Point(16, 116);
            this.checkedListBox1.MultiColumn = true;
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(632, 324);
            this.checkedListBox1.TabIndex = 5;
            // 
            // lbTotal
            // 
            this.lbTotal.AutoSize = true;
            this.lbTotal.Location = new System.Drawing.Point(452, 88);
            this.lbTotal.Name = "lbTotal";
            this.lbTotal.Size = new System.Drawing.Size(100, 17);
            this.lbTotal.TabIndex = 6;
            this.lbTotal.Text = "Total Tables: 0";
            // 
            // FormTestExcludeTables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 450);
            this.Controls.Add(this.lbTotal);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.btExport);
            this.Controls.Add(this.btNone);
            this.Controls.Add(this.btAll);
            this.Controls.Add(this.btGetTables);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormTestExcludeTables";
            this.Text = "FormTestExcludeTables";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btGetTables;
        private System.Windows.Forms.Button btAll;
        private System.Windows.Forms.Button btNone;
        private System.Windows.Forms.Button btExport;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Label lbTotal;
    }
}