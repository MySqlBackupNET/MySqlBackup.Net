namespace MySqlBackupTestApp
{
    partial class FormTestZip
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTestZip));
            this.label2 = new System.Windows.Forms.Label();
            this.btExportMemoryZip = new System.Windows.Forms.Button();
            this.btImportUnzipMemoryStream = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btImportUnzipFile = new System.Windows.Forms.Button();
            this.btExportFileZip = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 21);
            this.label2.TabIndex = 5;
            this.label2.Text = "Test - Zip";
            // 
            // btExportMemoryZip
            // 
            this.btExportMemoryZip.Location = new System.Drawing.Point(315, 269);
            this.btExportMemoryZip.Name = "btExportMemoryZip";
            this.btExportMemoryZip.Size = new System.Drawing.Size(240, 31);
            this.btExportMemoryZip.TabIndex = 6;
            this.btExportMemoryZip.Text = "Export to Memory Stream and Zip";
            this.btExportMemoryZip.UseVisualStyleBackColor = true;
            this.btExportMemoryZip.Click += new System.EventHandler(this.btExportMemoryZip_Click);
            // 
            // btImportUnzipMemoryStream
            // 
            this.btImportUnzipMemoryStream.Location = new System.Drawing.Point(315, 306);
            this.btImportUnzipMemoryStream.Name = "btImportUnzipMemoryStream";
            this.btImportUnzipMemoryStream.Size = new System.Drawing.Size(240, 31);
            this.btImportUnzipMemoryStream.TabIndex = 7;
            this.btImportUnzipMemoryStream.Text = "Unzip to Memory Stream and Import";
            this.btImportUnzipMemoryStream.UseVisualStyleBackColor = true;
            this.btImportUnzipMemoryStream.Click += new System.EventHandler(this.btImportUnzipMemoryStream_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(525, 192);
            this.label1.TabIndex = 8;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // btImportUnzipFile
            // 
            this.btImportUnzipFile.Location = new System.Drawing.Point(39, 306);
            this.btImportUnzipFile.Name = "btImportUnzipFile";
            this.btImportUnzipFile.Size = new System.Drawing.Size(240, 31);
            this.btImportUnzipFile.TabIndex = 10;
            this.btImportUnzipFile.Text = "Unzip to Physical File and Import";
            this.btImportUnzipFile.UseVisualStyleBackColor = true;
            this.btImportUnzipFile.Click += new System.EventHandler(this.btImportUnzipFile_Click);
            // 
            // btExportFileZip
            // 
            this.btExportFileZip.Location = new System.Drawing.Point(39, 269);
            this.btExportFileZip.Name = "btExportFileZip";
            this.btExportFileZip.Size = new System.Drawing.Size(240, 31);
            this.btExportFileZip.TabIndex = 9;
            this.btExportFileZip.Text = "Export to Physical File and Zip";
            this.btExportFileZip.UseVisualStyleBackColor = true;
            this.btExportFileZip.Click += new System.EventHandler(this.btExportFileZip_Click);
            // 
            // FormTestZip
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(596, 363);
            this.Controls.Add(this.btImportUnzipFile);
            this.Controls.Add(this.btExportFileZip);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btImportUnzipMemoryStream);
            this.Controls.Add(this.btExportMemoryZip);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FormTestZip";
            this.Text = "FormTestZip";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btExportMemoryZip;
        private System.Windows.Forms.Button btImportUnzipMemoryStream;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btImportUnzipFile;
        private System.Windows.Forms.Button btExportFileZip;

    }
}