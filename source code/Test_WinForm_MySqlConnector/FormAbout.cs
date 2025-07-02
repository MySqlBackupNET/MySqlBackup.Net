using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MySqlBackupTestApp
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();

            lbVersion.Text = $@"
MySqlBackup.NET Testing Tools v2.4
Date: {Program.DateVersion}

Designed for v2.4

MySqlBackup.NET
A MySQL Database Backup Tool for .NET

*This program is obsoleted. Please use the ASP.NET version for testing

www.mysqlbackup.net

Freeware";
        }
    }
}
