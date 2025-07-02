﻿using System;
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

            lbVersion.Text = string.Format(@"
MySqlBackup.NET Testing Tools {0}
Date: {1}

MySqlBackup.NET
A MySQL Database Backup Tool for .NET

*This program is obsoleted. Please use the ASP.NET version for testing

www.mysqlbackup.net

Freeware

Loaded MySqlBackup.DLL Version: {2}", Program.Version, Program.DateVersion, MySqlConnector.MySqlBackup.Version);
        }
    }
}
