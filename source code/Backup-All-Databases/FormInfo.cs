using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Backup_All_Databases
{
    public partial class FormInfo : Backup_All_Databases.baseform
    {
        public FormInfo()
        {
            InitializeComponent();
        }

        private void FormInfo_Load(object sender, EventArgs e)
        {
            textBox1.Select(0, 0);
            textBox1.AppendText(Environment.NewLine + Environment.NewLine + "Current loaded font: " + this.Font.Name);
        }
    }
}
