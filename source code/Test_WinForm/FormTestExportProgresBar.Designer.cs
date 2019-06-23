namespace MySqlBackupTestApp
{
    partial class FormTestExportProgresBar
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
            this.btExport = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.lbCurrentTableName = new System.Windows.Forms.Label();
            this.pbTable = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.pbRowInCurTable = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.pbRowInAllTable = new System.Windows.Forms.ProgressBar();
            this.lbTableCount = new System.Windows.Forms.Label();
            this.lbRowInCurTable = new System.Windows.Forms.Label();
            this.lbRowInAllTable = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nmExInterval = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbTotalRows_Tables = new System.Windows.Forms.Label();
            this.txtProgress = new System.Windows.Forms.TextBox();
            this.comboBox_RowsExportMode = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbGetTotalRowsMode = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nmExInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // btExport
            // 
            this.btExport.Location = new System.Drawing.Point(34, 51);
            this.btExport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btExport.Name = "btExport";
            this.btExport.Size = new System.Drawing.Size(118, 33);
            this.btExport.TabIndex = 0;
            this.btExport.Text = "Start Export";
            this.btExport.UseVisualStyleBackColor = true;
            this.btExport.Click += new System.EventHandler(this.btExport_Click);
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(173, 51);
            this.btCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(118, 33);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel Export";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // lbCurrentTableName
            // 
            this.lbCurrentTableName.AutoSize = true;
            this.lbCurrentTableName.Location = new System.Drawing.Point(31, 246);
            this.lbCurrentTableName.Name = "lbCurrentTableName";
            this.lbCurrentTableName.Size = new System.Drawing.Size(38, 16);
            this.lbCurrentTableName.TabIndex = 2;
            this.lbCurrentTableName.Text = "Table";
            // 
            // pbTable
            // 
            this.pbTable.Location = new System.Drawing.Point(34, 265);
            this.pbTable.Name = "pbTable";
            this.pbTable.Size = new System.Drawing.Size(306, 23);
            this.pbTable.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 320);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Rows in Current Table";
            // 
            // pbRowInCurTable
            // 
            this.pbRowInCurTable.Location = new System.Drawing.Point(34, 339);
            this.pbRowInCurTable.Name = "pbRowInCurTable";
            this.pbRowInCurTable.Size = new System.Drawing.Size(306, 23);
            this.pbRowInCurTable.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 396);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Rows in All Tables";
            // 
            // pbRowInAllTable
            // 
            this.pbRowInAllTable.Location = new System.Drawing.Point(34, 415);
            this.pbRowInAllTable.Name = "pbRowInAllTable";
            this.pbRowInAllTable.Size = new System.Drawing.Size(306, 23);
            this.pbRowInAllTable.TabIndex = 7;
            // 
            // lbTableCount
            // 
            this.lbTableCount.AutoSize = true;
            this.lbTableCount.Location = new System.Drawing.Point(31, 291);
            this.lbTableCount.Name = "lbTableCount";
            this.lbTableCount.Size = new System.Drawing.Size(34, 16);
            this.lbTableCount.TabIndex = 8;
            this.lbTableCount.Text = "1 / 1";
            // 
            // lbRowInCurTable
            // 
            this.lbRowInCurTable.AutoSize = true;
            this.lbRowInCurTable.Location = new System.Drawing.Point(31, 365);
            this.lbRowInCurTable.Name = "lbRowInCurTable";
            this.lbRowInCurTable.Size = new System.Drawing.Size(34, 16);
            this.lbRowInCurTable.TabIndex = 9;
            this.lbRowInCurTable.Text = "1 / 1";
            // 
            // lbRowInAllTable
            // 
            this.lbRowInAllTable.AutoSize = true;
            this.lbRowInAllTable.Location = new System.Drawing.Point(31, 441);
            this.lbRowInAllTable.Name = "lbRowInAllTable";
            this.lbRowInAllTable.Size = new System.Drawing.Size(34, 16);
            this.lbRowInAllTable.TabIndex = 10;
            this.lbRowInAllTable.Text = "1 / 1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(244, 19);
            this.label1.TabIndex = 11;
            this.label1.Text = "Test - Progress Report - Export";
            // 
            // nmExInterval
            // 
            this.nmExInterval.Location = new System.Drawing.Point(264, 101);
            this.nmExInterval.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nmExInterval.Name = "nmExInterval";
            this.nmExInterval.Size = new System.Drawing.Size(76, 22);
            this.nmExInterval.TabIndex = 21;
            this.nmExInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nmExInterval.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(227, 16);
            this.label5.TabIndex = 20;
            this.label5.Text = "Progress Report Interval (Miliseconds)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 32);
            this.label4.TabIndex = 22;
            this.label4.Text = "Total Tables:\r\nTotal Rows:";
            // 
            // lbTotalRows_Tables
            // 
            this.lbTotalRows_Tables.AutoSize = true;
            this.lbTotalRows_Tables.Location = new System.Drawing.Point(117, 197);
            this.lbTotalRows_Tables.Name = "lbTotalRows_Tables";
            this.lbTotalRows_Tables.Size = new System.Drawing.Size(0, 16);
            this.lbTotalRows_Tables.TabIndex = 23;
            // 
            // txtProgress
            // 
            this.txtProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProgress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtProgress.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProgress.ForeColor = System.Drawing.Color.YellowGreen;
            this.txtProgress.Location = new System.Drawing.Point(368, 9);
            this.txtProgress.Multiline = true;
            this.txtProgress.Name = "txtProgress";
            this.txtProgress.ReadOnly = true;
            this.txtProgress.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtProgress.Size = new System.Drawing.Size(454, 537);
            this.txtProgress.TabIndex = 25;
            // 
            // comboBox_RowsExportMode
            // 
            this.comboBox_RowsExportMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_RowsExportMode.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_RowsExportMode.FormattingEnabled = true;
            this.comboBox_RowsExportMode.Location = new System.Drawing.Point(152, 132);
            this.comboBox_RowsExportMode.Name = "comboBox_RowsExportMode";
            this.comboBox_RowsExportMode.Size = new System.Drawing.Size(188, 23);
            this.comboBox_RowsExportMode.TabIndex = 28;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 16);
            this.label6.TabIndex = 27;
            this.label6.Text = "Row Export Mode:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 168);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(136, 16);
            this.label7.TabIndex = 29;
            this.label7.Text = "Get Total Rows Mode:";
            // 
            // cbGetTotalRowsMode
            // 
            this.cbGetTotalRowsMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGetTotalRowsMode.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbGetTotalRowsMode.FormattingEnabled = true;
            this.cbGetTotalRowsMode.Items.AddRange(new object[] {
            "Information Schema",
            "Select Count",
            "Skip"});
            this.cbGetTotalRowsMode.Location = new System.Drawing.Point(173, 166);
            this.cbGetTotalRowsMode.Name = "cbGetTotalRowsMode";
            this.cbGetTotalRowsMode.Size = new System.Drawing.Size(167, 23);
            this.cbGetTotalRowsMode.TabIndex = 30;
            // 
            // FormTestExportProgresBar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(834, 561);
            this.Controls.Add(this.cbGetTotalRowsMode);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBox_RowsExportMode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtProgress);
            this.Controls.Add(this.lbTotalRows_Tables);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nmExInterval);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbRowInAllTable);
            this.Controls.Add(this.lbRowInCurTable);
            this.Controls.Add(this.lbTableCount);
            this.Controls.Add(this.pbRowInAllTable);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pbRowInCurTable);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pbTable);
            this.Controls.Add(this.lbCurrentTableName);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btExport);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormTestExportProgresBar";
            this.Text = "FormTestExportProgresBar";
            ((System.ComponentModel.ISupportInitialize)(this.nmExInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btExport;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label lbCurrentTableName;
        private System.Windows.Forms.ProgressBar pbTable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar pbRowInCurTable;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pbRowInAllTable;
        private System.Windows.Forms.Label lbTableCount;
        private System.Windows.Forms.Label lbRowInCurTable;
        private System.Windows.Forms.Label lbRowInAllTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nmExInterval;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbTotalRows_Tables;
        private System.Windows.Forms.TextBox txtProgress;
        private System.Windows.Forms.ComboBox comboBox_RowsExportMode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbGetTotalRowsMode;
    }
}