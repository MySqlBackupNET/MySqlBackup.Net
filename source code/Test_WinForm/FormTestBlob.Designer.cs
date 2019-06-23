namespace MySqlBackupTestApp
{
    partial class FormTestBlob
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTestBlob));
            this.btCreateTable = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btTest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbHash1 = new System.Windows.Forms.Label();
            this.lbHash2 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dropBlobExportMode = new System.Windows.Forms.ComboBox();
            this.lbResult = new System.Windows.Forms.Label();
            this.lbDumpSize = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btCreateTable
            // 
            this.btCreateTable.Location = new System.Drawing.Point(15, 108);
            this.btCreateTable.Margin = new System.Windows.Forms.Padding(4);
            this.btCreateTable.Name = "btCreateTable";
            this.btCreateTable.Size = new System.Drawing.Size(172, 28);
            this.btCreateTable.TabIndex = 0;
            this.btCreateTable.Text = "Create Table";
            this.btCreateTable.UseVisualStyleBackColor = true;
            this.btCreateTable.Click += new System.EventHandler(this.btCreateTable_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(16, 140);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(486, 98);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "CREATE TABLE IF NOT EXISTS `testblob` (\r\n  `id` INTEGER UNSIGNED NOT NULL AUTO_IN" +
    "CREMENT,\r\n  `blobdata` MEDIUMBLOB,\r\n  PRIMARY KEY (`id`)\r\n)\r\nENGINE = InnoDB;";
            // 
            // btTest
            // 
            this.btTest.Location = new System.Drawing.Point(15, 246);
            this.btTest.Margin = new System.Windows.Forms.Padding(4);
            this.btTest.Name = "btTest";
            this.btTest.Size = new System.Drawing.Size(277, 28);
            this.btTest.TabIndex = 2;
            this.btTest.Text = "Select a file to be saved into database";
            this.btTest.UseVisualStyleBackColor = true;
            this.btTest.Click += new System.EventHandler(this.btTest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 278);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(269, 192);
            this.label1.TabIndex = 3;
            this.label1.Text = "*Note: Please test this with small size file.\r\n\r\n\r\nbyte array of files before sav" +
    "e into database\r\n\r\nSHA128 Hash:\r\n\r\nbyte array after getting out from database\r\n\r" +
    "\nSQL Dump Size: \r\nSHA128 Hash:\r\nRESULT:";
            // 
            // lbHash1
            // 
            this.lbHash1.AutoSize = true;
            this.lbHash1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHash1.Location = new System.Drawing.Point(122, 358);
            this.lbHash1.Name = "lbHash1";
            this.lbHash1.Size = new System.Drawing.Size(65, 16);
            this.lbHash1.TabIndex = 4;
            this.lbHash1.Text = "lbHash1";
            // 
            // lbHash2
            // 
            this.lbHash2.AutoSize = true;
            this.lbHash2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHash2.Location = new System.Drawing.Point(122, 438);
            this.lbHash2.Name = "lbHash2";
            this.lbHash2.Size = new System.Drawing.Size(65, 16);
            this.lbHash2.TabIndex = 5;
            this.lbHash2.Text = "lbHash2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(299, 252);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(178, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "ExportInfo.BlobExportMode=";
            // 
            // dropBlobExportMode
            // 
            this.dropBlobExportMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropBlobExportMode.FormattingEnabled = true;
            this.dropBlobExportMode.Items.AddRange(new object[] {
            "Hexdecimal String",
            "Binary Char"});
            this.dropBlobExportMode.Location = new System.Drawing.Point(483, 249);
            this.dropBlobExportMode.Name = "dropBlobExportMode";
            this.dropBlobExportMode.Size = new System.Drawing.Size(153, 24);
            this.dropBlobExportMode.TabIndex = 38;
            // 
            // lbResult
            // 
            this.lbResult.AutoSize = true;
            this.lbResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbResult.Location = new System.Drawing.Point(122, 454);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(65, 16);
            this.lbResult.TabIndex = 39;
            this.lbResult.Text = "lbResult";
            // 
            // lbDumpSize
            // 
            this.lbDumpSize.AutoSize = true;
            this.lbDumpSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDumpSize.Location = new System.Drawing.Point(122, 422);
            this.lbDumpSize.Name = "lbDumpSize";
            this.lbDumpSize.Size = new System.Drawing.Size(91, 16);
            this.lbDumpSize.TabIndex = 40;
            this.lbDumpSize.Text = "lbDumpSize";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(12, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(640, 80);
            this.label3.TabIndex = 41;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(15, 40);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(471, 22);
            this.textBox2.TabIndex = 42;
            this.textBox2.Text = "https://github.com/MySqlBackupNET/MySqlBackup.Net/issues";
            // 
            // FormTestBlob
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(707, 496);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbDumpSize);
            this.Controls.Add(this.lbResult);
            this.Controls.Add(this.dropBlobExportMode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbHash2);
            this.Controls.Add(this.lbHash1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btTest);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btCreateTable);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormTestBlob";
            this.Text = "FormTestBlob";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCreateTable;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btTest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbHash1;
        private System.Windows.Forms.Label lbHash2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox dropBlobExportMode;
        private System.Windows.Forms.Label lbResult;
        private System.Windows.Forms.Label lbDumpSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
    }
}