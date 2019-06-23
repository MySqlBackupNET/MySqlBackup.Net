namespace MySqlBackupTestApp
{
    partial class FormTestCustomTablesExport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colnSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colnTable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colnSql = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btSelectAll = new System.Windows.Forms.Button();
            this.btSelectNone = new System.Windows.Forms.Button();
            this.lbDb = new System.Windows.Forms.Label();
            this.btRefresh = new System.Windows.Forms.Button();
            this.btExportDic = new System.Windows.Forms.Button();
            this.btExportList = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(281, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Custom Export Of Tables And Rows";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colnSelect,
            this.colnTable,
            this.colnSql});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.Location = new System.Drawing.Point(16, 84);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(699, 309);
            this.dataGridView1.TabIndex = 2;
            // 
            // colnSelect
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.colnSelect.DefaultCellStyle = dataGridViewCellStyle1;
            this.colnSelect.HeaderText = "";
            this.colnSelect.Name = "colnSelect";
            this.colnSelect.ReadOnly = true;
            this.colnSelect.Width = 25;
            // 
            // colnTable
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.colnTable.DefaultCellStyle = dataGridViewCellStyle2;
            this.colnTable.HeaderText = "Tables";
            this.colnTable.Name = "colnTable";
            this.colnTable.ReadOnly = true;
            this.colnTable.Width = 170;
            // 
            // colnSql
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colnSql.DefaultCellStyle = dataGridViewCellStyle3;
            this.colnSql.HeaderText = "Custom SELECT SQL";
            this.colnSql.Name = "colnSql";
            this.colnSql.Width = 500;
            // 
            // btSelectAll
            // 
            this.btSelectAll.Location = new System.Drawing.Point(125, 55);
            this.btSelectAll.Name = "btSelectAll";
            this.btSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btSelectAll.TabIndex = 3;
            this.btSelectAll.Text = "Select All";
            this.btSelectAll.UseVisualStyleBackColor = true;
            this.btSelectAll.Click += new System.EventHandler(this.btSelectAll_Click);
            // 
            // btSelectNone
            // 
            this.btSelectNone.Location = new System.Drawing.Point(206, 55);
            this.btSelectNone.Name = "btSelectNone";
            this.btSelectNone.Size = new System.Drawing.Size(75, 23);
            this.btSelectNone.TabIndex = 4;
            this.btSelectNone.Text = "Select None";
            this.btSelectNone.UseVisualStyleBackColor = true;
            this.btSelectNone.Click += new System.EventHandler(this.btSelectNone_Click);
            // 
            // lbDb
            // 
            this.lbDb.AutoSize = true;
            this.lbDb.Location = new System.Drawing.Point(13, 35);
            this.lbDb.Name = "lbDb";
            this.lbDb.Size = new System.Drawing.Size(56, 14);
            this.lbDb.TabIndex = 5;
            this.lbDb.Text = "Database:";
            // 
            // btRefresh
            // 
            this.btRefresh.Location = new System.Drawing.Point(16, 55);
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(103, 23);
            this.btRefresh.TabIndex = 6;
            this.btRefresh.Text = "Refresh/Reload";
            this.btRefresh.UseVisualStyleBackColor = true;
            this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // btExportDic
            // 
            this.btExportDic.BackColor = System.Drawing.Color.GreenYellow;
            this.btExportDic.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btExportDic.Location = new System.Drawing.Point(328, 38);
            this.btExportDic.Name = "btExportDic";
            this.btExportDic.Size = new System.Drawing.Size(119, 40);
            this.btExportDic.TabIndex = 7;
            this.btExportDic.Text = "Export\r\n(Using Dictionary)";
            this.btExportDic.UseVisualStyleBackColor = false;
            this.btExportDic.Click += new System.EventHandler(this.btExportDic_Click);
            // 
            // btExportList
            // 
            this.btExportList.BackColor = System.Drawing.Color.Yellow;
            this.btExportList.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btExportList.Location = new System.Drawing.Point(453, 38);
            this.btExportList.Name = "btExportList";
            this.btExportList.Size = new System.Drawing.Size(119, 40);
            this.btExportList.TabIndex = 8;
            this.btExportList.Text = "Export\r\n(Using List)";
            this.btExportList.UseVisualStyleBackColor = false;
            this.btExportList.Click += new System.EventHandler(this.btExportList_Click);
            // 
            // FormTestCustomTablesExport
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(727, 405);
            this.Controls.Add(this.btExportList);
            this.Controls.Add(this.btExportDic);
            this.Controls.Add(this.btRefresh);
            this.Controls.Add(this.lbDb);
            this.Controls.Add(this.btSelectNone);
            this.Controls.Add(this.btSelectAll);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FormTestCustomTablesExport";
            this.Text = "FormTestCustomTablesExport";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btSelectAll;
        private System.Windows.Forms.Button btSelectNone;
        private System.Windows.Forms.Label lbDb;
        private System.Windows.Forms.Button btRefresh;
        private System.Windows.Forms.Button btExportDic;
        private System.Windows.Forms.Button btExportList;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colnSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colnTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colnSql;
    }
}