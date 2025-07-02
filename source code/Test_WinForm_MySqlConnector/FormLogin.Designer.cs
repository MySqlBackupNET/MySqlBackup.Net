namespace Test_WinForm_MySqlConnector
{
    partial class FormLogin
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
            this.btLogin = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.NumericUpDown();
            this.cbConvertZeroDatetime = new System.Windows.Forms.CheckBox();
            this.cbTreatTinyAsBoolean = new System.Windows.Forms.CheckBox();
            this.txtOther = new System.Windows.Forms.TextBox();
            this.cbAutoSave = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort)).BeginInit();
            this.SuspendLayout();
            // 
            // btLogin
            // 
            this.btLogin.Location = new System.Drawing.Point(188, 331);
            this.btLogin.Name = "btLogin";
            this.btLogin.Size = new System.Drawing.Size(85, 34);
            this.btLogin.TabIndex = 9;
            this.btLogin.Text = "Login";
            this.btLogin.UseVisualStyleBackColor = true;
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(97, 331);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(85, 34);
            this.btCancel.TabIndex = 10;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 238);
            this.label1.TabIndex = 1;
            this.label1.Text = "server / host\r\n\r\nusername\r\n\r\npassword\r\n\r\nport\r\n\r\nconvert zero datetime\r\n\r\ntreat t" +
    "iny as boolean\r\n\r\nother connection\r\noptions";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(172, 22);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(142, 25);
            this.txtServer.TabIndex = 1;
            this.txtServer.Text = "localhost";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(172, 57);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(142, 25);
            this.txtUser.TabIndex = 2;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(172, 91);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(142, 25);
            this.txtPassword.TabIndex = 3;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(172, 126);
            this.txtPort.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(142, 25);
            this.txtPort.TabIndex = 4;
            this.txtPort.Value = new decimal(new int[] {
            3306,
            0,
            0,
            0});
            // 
            // cbConvertZeroDatetime
            // 
            this.cbConvertZeroDatetime.AutoSize = true;
            this.cbConvertZeroDatetime.Location = new System.Drawing.Point(172, 162);
            this.cbConvertZeroDatetime.Name = "cbConvertZeroDatetime";
            this.cbConvertZeroDatetime.Size = new System.Drawing.Size(46, 21);
            this.cbConvertZeroDatetime.TabIndex = 5;
            this.cbConvertZeroDatetime.Text = "Yes";
            this.cbConvertZeroDatetime.UseVisualStyleBackColor = true;
            // 
            // cbTreatTinyAsBoolean
            // 
            this.cbTreatTinyAsBoolean.AutoSize = true;
            this.cbTreatTinyAsBoolean.Location = new System.Drawing.Point(172, 195);
            this.cbTreatTinyAsBoolean.Name = "cbTreatTinyAsBoolean";
            this.cbTreatTinyAsBoolean.Size = new System.Drawing.Size(46, 21);
            this.cbTreatTinyAsBoolean.TabIndex = 6;
            this.cbTreatTinyAsBoolean.Text = "Yes";
            this.cbTreatTinyAsBoolean.UseVisualStyleBackColor = true;
            // 
            // txtOther
            // 
            this.txtOther.Location = new System.Drawing.Point(172, 235);
            this.txtOther.Name = "txtOther";
            this.txtOther.Size = new System.Drawing.Size(142, 25);
            this.txtOther.TabIndex = 7;
            // 
            // cbAutoSave
            // 
            this.cbAutoSave.AutoSize = true;
            this.cbAutoSave.Location = new System.Drawing.Point(99, 287);
            this.cbAutoSave.Name = "cbAutoSave";
            this.cbAutoSave.Size = new System.Drawing.Size(246, 21);
            this.cbAutoSave.TabIndex = 8;
            this.cbAutoSave.Text = "Auto Save Options (except password)";
            this.cbAutoSave.UseVisualStyleBackColor = true;
            // 
            // FormLogin
            // 
            this.ClientSize = new System.Drawing.Size(354, 377);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.cbAutoSave);
            this.Controls.Add(this.btLogin);
            this.Controls.Add(this.txtOther);
            this.Controls.Add(this.cbTreatTinyAsBoolean);
            this.Controls.Add(this.cbConvertZeroDatetime);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.label1);
            this.Name = "FormLogin";
            this.Text = "Login";
            ((System.ComponentModel.ISupportInitialize)(this.txtPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btLogin;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.NumericUpDown txtPort;
        private System.Windows.Forms.CheckBox cbConvertZeroDatetime;
        private System.Windows.Forms.CheckBox cbTreatTinyAsBoolean;
        private System.Windows.Forms.TextBox txtOther;
        private System.Windows.Forms.CheckBox cbAutoSave;
    }
}
