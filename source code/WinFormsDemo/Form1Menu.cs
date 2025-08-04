using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsDemo
{
    public partial class Form1Menu : baseForm
    {
        public Form1Menu()
        {
            InitializeComponent();
        }

        private void button_SimpleDemo_Click(object sender, EventArgs e)
        {
            FormSimpleDemo f = new FormSimpleDemo();
            f.Show();
        }

        private void button_progress_report_Click(object sender, EventArgs e)
        {
            FormProgressReport f = new FormProgressReport();
            f.Show();
        }
    }
}
