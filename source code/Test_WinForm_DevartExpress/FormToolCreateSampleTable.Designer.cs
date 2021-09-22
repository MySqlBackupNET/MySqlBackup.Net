namespace MySqlBackupTestApp
{
    partial class FormToolCreateSampleTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormToolCreateSampleTable));
            this.t = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btCreate = new System.Windows.Forms.Button();
            this.btDrop = new System.Windows.Forms.Button();
            this.lbTotal = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btStopInsert = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btInsert = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btReset = new System.Windows.Forms.Button();
            this.txtTable = new System.Windows.Forms.TextBox();
            this.txtTableName = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btFunctionCreate = new System.Windows.Forms.Button();
            this.btFunctionReset = new System.Windows.Forms.Button();
            this.txtFunction = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btProcedureCreate = new System.Windows.Forms.Button();
            this.btProcedureReset = new System.Windows.Forms.Button();
            this.txtProcedure = new System.Windows.Forms.TextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.btTriggerCreate = new System.Windows.Forms.Button();
            this.btTriggerReset = new System.Windows.Forms.Button();
            this.txtTrigger = new System.Windows.Forms.TextBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.btEventCreate = new System.Windows.Forms.Button();
            this.btEventReset = new System.Windows.Forms.Button();
            this.txtEvent = new System.Windows.Forms.TextBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.btViewCreate = new System.Windows.Forms.Button();
            this.btViewReset = new System.Windows.Forms.Button();
            this.txtView = new System.Windows.Forms.TextBox();
            this.t.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.SuspendLayout();
            // 
            // t
            // 
            this.t.Controls.Add(this.tabPage1);
            this.t.Controls.Add(this.tabPage3);
            this.t.Controls.Add(this.tabPage4);
            this.t.Controls.Add(this.tabPage5);
            this.t.Controls.Add(this.tabPage6);
            this.t.Controls.Add(this.tabPage7);
            this.t.Dock = System.Windows.Forms.DockStyle.Fill;
            this.t.Location = new System.Drawing.Point(0, 0);
            this.t.Name = "t";
            this.t.SelectedIndex = 0;
            this.t.Size = new System.Drawing.Size(566, 400);
            this.t.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Controls.Add(this.btCreate);
            this.tabPage1.Controls.Add(this.btDrop);
            this.tabPage1.Controls.Add(this.lbTotal);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.btStopInsert);
            this.tabPage1.Controls.Add(this.progressBar1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.numericUpDown1);
            this.tabPage1.Controls.Add(this.btInsert);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.btReset);
            this.tabPage1.Controls.Add(this.txtTable);
            this.tabPage1.Controls.Add(this.txtTableName);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(558, 371);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Table";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btCreate
            // 
            this.btCreate.Location = new System.Drawing.Point(25, 35);
            this.btCreate.Name = "btCreate";
            this.btCreate.Size = new System.Drawing.Size(75, 33);
            this.btCreate.TabIndex = 28;
            this.btCreate.Text = "Create";
            this.btCreate.UseVisualStyleBackColor = true;
            this.btCreate.Click += new System.EventHandler(this.btCreate_Click);
            // 
            // btDrop
            // 
            this.btDrop.Location = new System.Drawing.Point(25, 74);
            this.btDrop.Name = "btDrop";
            this.btDrop.Size = new System.Drawing.Size(75, 33);
            this.btDrop.TabIndex = 27;
            this.btDrop.Text = "Drop";
            this.btDrop.UseVisualStyleBackColor = true;
            this.btDrop.Click += new System.EventHandler(this.btDrop_Click);
            // 
            // lbTotal
            // 
            this.lbTotal.AutoSize = true;
            this.lbTotal.Location = new System.Drawing.Point(489, 337);
            this.lbTotal.Name = "lbTotal";
            this.lbTotal.Size = new System.Drawing.Size(15, 16);
            this.lbTotal.TabIndex = 26;
            this.lbTotal.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(394, 337);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 16);
            this.label3.TabIndex = 25;
            this.label3.Text = "Total Inserted:";
            // 
            // btStopInsert
            // 
            this.btStopInsert.Location = new System.Drawing.Point(69, 334);
            this.btStopInsert.Name = "btStopInsert";
            this.btStopInsert.Size = new System.Drawing.Size(168, 23);
            this.btStopInsert.TabIndex = 24;
            this.btStopInsert.Text = "Stop Insert";
            this.btStopInsert.UseVisualStyleBackColor = true;
            this.btStopInsert.Click += new System.EventHandler(this.btStopInsert_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(243, 334);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(145, 23);
            this.progressBar1.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(394, 308);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 22;
            this.label1.Text = "Rows";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(243, 306);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            -559939584,
            902409669,
            54,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(145, 22);
            this.numericUpDown1.TabIndex = 21;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown1.ThousandsSeparator = true;
            this.numericUpDown1.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // btInsert
            // 
            this.btInsert.Location = new System.Drawing.Point(69, 303);
            this.btInsert.Name = "btInsert";
            this.btInsert.Size = new System.Drawing.Size(168, 26);
            this.btInsert.TabIndex = 20;
            this.btInsert.Text = "Insert Sample Rows";
            this.btInsert.UseVisualStyleBackColor = true;
            this.btInsert.Click += new System.EventHandler(this.btInsert_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(115, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "CREATE TABLE IF NOT EXISTS ";
            // 
            // btReset
            // 
            this.btReset.Location = new System.Drawing.Point(25, 113);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(75, 33);
            this.btReset.TabIndex = 19;
            this.btReset.Text = "Reset";
            this.btReset.UseVisualStyleBackColor = true;
            this.btReset.Click += new System.EventHandler(this.btReset_Click);
            // 
            // txtTable
            // 
            this.txtTable.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTable.Location = new System.Drawing.Point(118, 42);
            this.txtTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTable.Multiline = true;
            this.txtTable.Name = "txtTable";
            this.txtTable.ReadOnly = true;
            this.txtTable.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTable.Size = new System.Drawing.Size(415, 247);
            this.txtTable.TabIndex = 16;
            this.txtTable.Text = resources.GetString("txtTable.Text");
            this.txtTable.WordWrap = false;
            // 
            // txtTableName
            // 
            this.txtTableName.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTableName.Location = new System.Drawing.Point(341, 13);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size(183, 22);
            this.txtTableName.TabIndex = 18;
            this.txtTableName.Text = "tableA";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btFunctionCreate);
            this.tabPage3.Controls.Add(this.btFunctionReset);
            this.tabPage3.Controls.Add(this.txtFunction);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(558, 371);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Function";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btFunctionCreate
            // 
            this.btFunctionCreate.Location = new System.Drawing.Point(25, 35);
            this.btFunctionCreate.Name = "btFunctionCreate";
            this.btFunctionCreate.Size = new System.Drawing.Size(75, 33);
            this.btFunctionCreate.TabIndex = 28;
            this.btFunctionCreate.Text = "Create";
            this.btFunctionCreate.UseVisualStyleBackColor = true;
            this.btFunctionCreate.Click += new System.EventHandler(this.btFunctionCreate_Click);
            // 
            // btFunctionReset
            // 
            this.btFunctionReset.Location = new System.Drawing.Point(25, 113);
            this.btFunctionReset.Name = "btFunctionReset";
            this.btFunctionReset.Size = new System.Drawing.Size(75, 33);
            this.btFunctionReset.TabIndex = 19;
            this.btFunctionReset.Text = "Reset";
            this.btFunctionReset.UseVisualStyleBackColor = true;
            this.btFunctionReset.Click += new System.EventHandler(this.btFunctionReset_Click);
            // 
            // txtFunction
            // 
            this.txtFunction.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFunction.Location = new System.Drawing.Point(120, 35);
            this.txtFunction.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFunction.Multiline = true;
            this.txtFunction.Name = "txtFunction";
            this.txtFunction.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFunction.Size = new System.Drawing.Size(400, 250);
            this.txtFunction.TabIndex = 16;
            this.txtFunction.Text = "DELIMITER |\r\nCREATE FUNCTION `functionsample1`() RETURNS int(11)\r\n    DETERMINIST" +
    "IC\r\nBEGIN\r\nDECLARE b INT;\r\nSET b = 1;\r\nRETURN b;\r\nEND |";
            this.txtFunction.WordWrap = false;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btProcedureCreate);
            this.tabPage4.Controls.Add(this.btProcedureReset);
            this.tabPage4.Controls.Add(this.txtProcedure);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(558, 371);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Procedure";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btProcedureCreate
            // 
            this.btProcedureCreate.Location = new System.Drawing.Point(25, 35);
            this.btProcedureCreate.Name = "btProcedureCreate";
            this.btProcedureCreate.Size = new System.Drawing.Size(75, 33);
            this.btProcedureCreate.TabIndex = 28;
            this.btProcedureCreate.Text = "Create";
            this.btProcedureCreate.UseVisualStyleBackColor = true;
            this.btProcedureCreate.Click += new System.EventHandler(this.btProcedureCreate_Click);
            // 
            // btProcedureReset
            // 
            this.btProcedureReset.Location = new System.Drawing.Point(25, 113);
            this.btProcedureReset.Name = "btProcedureReset";
            this.btProcedureReset.Size = new System.Drawing.Size(75, 33);
            this.btProcedureReset.TabIndex = 19;
            this.btProcedureReset.Text = "Reset";
            this.btProcedureReset.UseVisualStyleBackColor = true;
            this.btProcedureReset.Click += new System.EventHandler(this.btProcedureReset_Click);
            // 
            // txtProcedure
            // 
            this.txtProcedure.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProcedure.Location = new System.Drawing.Point(120, 35);
            this.txtProcedure.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProcedure.Multiline = true;
            this.txtProcedure.Name = "txtProcedure";
            this.txtProcedure.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtProcedure.Size = new System.Drawing.Size(400, 250);
            this.txtProcedure.TabIndex = 16;
            this.txtProcedure.Text = "DELIMITER |\r\nCREATE PROCEDURE `proceduresample1`()\r\n    DETERMINISTIC\r\n    COMMEN" +
    "T \'A procedure\'\r\nBEGIN\r\nSELECT \'Hello World !\';\r\nEND |";
            this.txtProcedure.WordWrap = false;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.btTriggerCreate);
            this.tabPage5.Controls.Add(this.btTriggerReset);
            this.tabPage5.Controls.Add(this.txtTrigger);
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(558, 371);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Trigger";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // btTriggerCreate
            // 
            this.btTriggerCreate.Location = new System.Drawing.Point(25, 35);
            this.btTriggerCreate.Name = "btTriggerCreate";
            this.btTriggerCreate.Size = new System.Drawing.Size(75, 33);
            this.btTriggerCreate.TabIndex = 28;
            this.btTriggerCreate.Text = "Create";
            this.btTriggerCreate.UseVisualStyleBackColor = true;
            this.btTriggerCreate.Click += new System.EventHandler(this.btTriggerCreate_Click);
            // 
            // btTriggerReset
            // 
            this.btTriggerReset.Location = new System.Drawing.Point(25, 113);
            this.btTriggerReset.Name = "btTriggerReset";
            this.btTriggerReset.Size = new System.Drawing.Size(75, 33);
            this.btTriggerReset.TabIndex = 19;
            this.btTriggerReset.Text = "Reset";
            this.btTriggerReset.UseVisualStyleBackColor = true;
            this.btTriggerReset.Click += new System.EventHandler(this.btTriggerReset_Click);
            // 
            // txtTrigger
            // 
            this.txtTrigger.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTrigger.Location = new System.Drawing.Point(120, 35);
            this.txtTrigger.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTrigger.Multiline = true;
            this.txtTrigger.Name = "txtTrigger";
            this.txtTrigger.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTrigger.Size = new System.Drawing.Size(400, 250);
            this.txtTrigger.TabIndex = 16;
            this.txtTrigger.Text = "DELIMITER |\r\nCREATE TRIGGER `triggerA` \r\nBEFORE INSERT ON `tableA` \r\nFOR EACH ROW" +
    " BEGIN\r\nUpdate `tableA` SET `bool` = 1 WHERE 1 = 2;\r\nEND |";
            this.txtTrigger.WordWrap = false;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.btEventCreate);
            this.tabPage6.Controls.Add(this.btEventReset);
            this.tabPage6.Controls.Add(this.txtEvent);
            this.tabPage6.Location = new System.Drawing.Point(4, 25);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(558, 371);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Event";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // btEventCreate
            // 
            this.btEventCreate.Location = new System.Drawing.Point(25, 35);
            this.btEventCreate.Name = "btEventCreate";
            this.btEventCreate.Size = new System.Drawing.Size(75, 33);
            this.btEventCreate.TabIndex = 28;
            this.btEventCreate.Text = "Create";
            this.btEventCreate.UseVisualStyleBackColor = true;
            this.btEventCreate.Click += new System.EventHandler(this.btEventCreate_Click);
            // 
            // btEventReset
            // 
            this.btEventReset.Location = new System.Drawing.Point(25, 113);
            this.btEventReset.Name = "btEventReset";
            this.btEventReset.Size = new System.Drawing.Size(75, 33);
            this.btEventReset.TabIndex = 19;
            this.btEventReset.Text = "Reset";
            this.btEventReset.UseVisualStyleBackColor = true;
            this.btEventReset.Click += new System.EventHandler(this.btEventReset_Click);
            // 
            // txtEvent
            // 
            this.txtEvent.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEvent.Location = new System.Drawing.Point(120, 35);
            this.txtEvent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEvent.Multiline = true;
            this.txtEvent.Name = "txtEvent";
            this.txtEvent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtEvent.Size = new System.Drawing.Size(400, 250);
            this.txtEvent.TabIndex = 16;
            this.txtEvent.Text = "DELIMITER |\r\nCREATE EVENT `eventA`\r\nON SCHEDULE EVERY 1 WEEK STARTS \'2014-01-01 0" +
    "0:00:00\'\r\nDO BEGIN\r\nEND |";
            this.txtEvent.WordWrap = false;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.btViewCreate);
            this.tabPage7.Controls.Add(this.btViewReset);
            this.tabPage7.Controls.Add(this.txtView);
            this.tabPage7.Location = new System.Drawing.Point(4, 25);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(558, 371);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "View";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // btViewCreate
            // 
            this.btViewCreate.Location = new System.Drawing.Point(25, 35);
            this.btViewCreate.Name = "btViewCreate";
            this.btViewCreate.Size = new System.Drawing.Size(75, 33);
            this.btViewCreate.TabIndex = 28;
            this.btViewCreate.Text = "Create";
            this.btViewCreate.UseVisualStyleBackColor = true;
            this.btViewCreate.Click += new System.EventHandler(this.btViewCreate_Click);
            // 
            // btViewReset
            // 
            this.btViewReset.Location = new System.Drawing.Point(25, 113);
            this.btViewReset.Name = "btViewReset";
            this.btViewReset.Size = new System.Drawing.Size(75, 33);
            this.btViewReset.TabIndex = 19;
            this.btViewReset.Text = "Reset";
            this.btViewReset.UseVisualStyleBackColor = true;
            this.btViewReset.Click += new System.EventHandler(this.btViewReset_Click);
            // 
            // txtView
            // 
            this.txtView.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtView.Location = new System.Drawing.Point(120, 35);
            this.txtView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtView.Multiline = true;
            this.txtView.Name = "txtView";
            this.txtView.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtView.Size = new System.Drawing.Size(400, 250);
            this.txtView.TabIndex = 16;
            this.txtView.Text = "CREATE VIEW `viewA` \r\nAS SELECT \'Hello View\' AS `View Sample`;";
            this.txtView.WordWrap = false;
            // 
            // FormToolCreateSampleTable
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(566, 400);
            this.Controls.Add(this.t);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormToolCreateSampleTable";
            this.Text = "FormToolCreateSampleTable";
            this.t.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl t;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.Button btCreate;
        private System.Windows.Forms.Button btDrop;
        private System.Windows.Forms.Label lbTotal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btStopInsert;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button btInsert;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btReset;
        private System.Windows.Forms.TextBox txtTable;
        private System.Windows.Forms.TextBox txtTableName;
        private System.Windows.Forms.Button btFunctionCreate;
        private System.Windows.Forms.Button btFunctionReset;
        private System.Windows.Forms.TextBox txtFunction;
        private System.Windows.Forms.Button btProcedureCreate;
        private System.Windows.Forms.Button btProcedureReset;
        private System.Windows.Forms.TextBox txtProcedure;
        private System.Windows.Forms.Button btTriggerCreate;
        private System.Windows.Forms.Button btTriggerReset;
        private System.Windows.Forms.TextBox txtTrigger;
        private System.Windows.Forms.Button btEventCreate;
        private System.Windows.Forms.Button btEventReset;
        private System.Windows.Forms.TextBox txtEvent;
        private System.Windows.Forms.Button btViewCreate;
        private System.Windows.Forms.Button btViewReset;
        private System.Windows.Forms.TextBox txtView;
    }
}