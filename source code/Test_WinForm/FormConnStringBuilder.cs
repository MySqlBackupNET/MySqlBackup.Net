using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Reflection;

namespace MySqlBackupTestApp
{
    public partial class FormConnStringBuilder : Form
    {
        public string ConnStr = "";

        public FormConnStringBuilder(string cstr)
        {
            InitializeComponent();
            
            List<string> lst = new List<string>();

            MySqlConnectionStringBuilder msb = new MySqlConnectionStringBuilder(cstr);

            var props = typeof(MySqlConnectionStringBuilder).GetProperties();

            foreach (var prop in props)
            {
                if (!prop.CanWrite)
                    continue;

                switch(prop.Name)
                {
                    case "Server":
                    case "UserID":
                    case "Password":
                    case "Database":
                    case "ConnectionString":
                        continue;
                }
                lst.Add(prop.Name);
            }

            lst.Sort(delegate (string x, string y)
                {
                    return x.CompareTo(y);
                });

            lst.Insert(0, "Server");
            lst.Insert(1, "UserID");
            lst.Insert(2, "Password");
            lst.Insert(3, "Database");

            int row = 0;

            foreach (var n in lst)
            {
                Label lb = new Label();
                lb.AutoSize = true;
                lb.Text = n;
                TextBox tb = new TextBox();
                tb.Width = 300;
                tb.Tag = n;

                foreach (var prop in props)
                {
                    if (prop.CanRead && prop.CanWrite)
                    {
                        if (prop.Name == n)
                        {
                            try
                            {
                                tb.Text = prop.GetValue(msb) + "";
                            }
                            catch { }
                        }
                    }
                }

                row++;
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                tableLayoutPanel1.Controls.Add(lb, 0, row);
                tableLayoutPanel1.Controls.Add(tb, 1, row);
            }
        }

        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtOk_Click(object sender, EventArgs e)
        {
            var lst = tableLayoutPanel1.Controls;

            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();

            var props = typeof(MySqlConnectionStringBuilder).GetProperties();

            foreach (var a in lst)
            {
                if (a.GetType() == typeof(Label))
                    continue;

                TextBox tb = (TextBox)a;
                if (tb.TextLength == 0)
                    continue;

                string tag = tb.Tag + "";

                foreach (var prop in props)
                {
                    if (tag != prop.Name)
                        continue;

                    Type ob = prop.PropertyType;

                    if (ob == typeof(string))
                    {
                        prop.SetValue(sb, tb.Text);
                        continue;
                    }
                    else if (ob == typeof(bool))
                    {
                        bool b = false;
                        bool.TryParse(tb.Text, out b);
                        prop.SetValue(sb, b);
                        continue;
                    }
                    else if (ob == typeof(uint))
                    {
                        uint i = 0;
                        uint.TryParse(tb.Text, out i);
                        prop.SetValue(sb, i);
                        continue;
                    }
                    else if (ob == typeof(ulong))
                    {
                        ulong i = 0;
                        ulong.TryParse(tb.Text, out i);
                        prop.SetValue(sb, i);
                        continue;
                    }
                    else if (ob == typeof(int))
                    {
                        int i = 0;
                        int.TryParse(tb.Text, out i);
                        prop.SetValue(sb, i);
                        continue;
                    }
                    else if (ob == typeof(long))
                    {
                        long i = 0;
                        long.TryParse(tb.Text, out i);
                        prop.SetValue(sb, i);
                        continue;
                    }

                    try
                    {
                        prop.SetValue(sb, tb.Text);
                    }
                    catch { }
                    continue;
                }
            }

            ConnStr = sb.GetConnectionString(true);

            this.DialogResult = DialogResult.OK;
        }
    }
}
