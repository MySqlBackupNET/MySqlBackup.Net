using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace MySqlBackupTestApp
{
    public partial class FormToolCreateSampleTable : Form
    {
        string tableName = "";
        long totalRows = 0L;
        long currentRow = 0L;
        bool stop = false;
        BackgroundWorker bw = new BackgroundWorker();
        Timer timer1;

        public FormToolCreateSampleTable()
        {
            InitializeComponent();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            timer1 = new Timer();
            timer1.Interval = 50;
            timer1.Tick += timer1_Tick;
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            txtTable.Text = @"(
  `int` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `varchar` VARCHAR(45),
  `text` TEXT,
  `datetime` DATETIME,
  `date` DATE,
  `time` TIME,
  `decimal` DECIMAL(10,5),
  `tinyint` TINYINT UNSIGNED,
  `timestamp` TIMESTAMP,
  `char36` CHAR(36),
  `binary16` BInary(16),
  `float` FLOAT,
  `double` DOUBLE,
  `blob` BLOB,
  PRIMARY KEY (`int`)
)
ENGINE = InnoDB;";
        }

        private void btCreate_Click(object sender, EventArgs e)
        {
            string sql = "CREATE TABLE `" + txtTableName.Text + "`" + txtTable.Text;
            Execute(sql);
        }

        private void btDrop_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();

                    cmd.CommandText = "DROP TABLE IF EXISTS `" + txtTableName.Text + "`;";
                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
            MessageBox.Show("Done.");
        }

        private void btStopInsert_Click(object sender, EventArgs e)
        {
            stop = true;
        }

        private void btInsert_Click(object sender, EventArgs e)
        {
            stop = false;
            totalRows = (long)numericUpDown1.Value;
            tableName = txtTableName.Text;
            timer1.Start();
            progressBar1.Maximum = (int)numericUpDown1.Value;
            bw.RunWorkerAsync();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO `");
            sb.AppendFormat(tableName);
            sb.AppendFormat("` (`varchar`,`text`,`datetime`,`date`,`time`,`decimal`,`tinyint`,`timestamp`,`char36`,`binary16`,`float`,`double`,`blob`,`bool`) VALUES");

            StringBuilder sb2 = new StringBuilder();
            sb2.AppendFormat("('");
            sb2.AppendFormat(CryptoExpress.RandomString(45)); // varchar
            sb2.AppendFormat("','");
            sb2.AppendFormat(CryptoExpress.RandomString(45)); // text
            sb2.AppendFormat("','");
            sb2.AppendFormat(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); // datetime
            sb2.AppendFormat("','");
            sb2.AppendFormat(DateTime.Now.ToString("yyyy-MM-dd")); // date
            sb2.AppendFormat("','");
            sb2.AppendFormat(DateTime.Now.ToString("HH:mm:ss")); // time
            sb2.AppendFormat("',3487.2398,1,CURRENT_TIMESTAMP,'00000000000000000000000000000000',"); // decimal, tinyint, timestamp
            sb2.AppendFormat(CryptoExpress.ConvertByteArrayToHexString(new byte[16]));
            sb2.AppendFormat(",243.234,456.456,");
            sb2.AppendFormat(CryptoExpress.ConvertByteArrayToHexString(new byte[16]));
            sb2.AppendFormat(",1)");

            string head = sb.ToString();
            string values = sb2.ToString();

            int maxlength = 1024*1024;

            using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();

                    cmd.CommandText = "start transaction;";
                    cmd.ExecuteNonQuery();

                    StringBuilder sb3 = new StringBuilder();

                    for (long i = 0; i < totalRows; i++)
                    {
                        if (stop)
                            break;

                        currentRow = i + 1;

                        if (sb3.Length == 0)
                        {
                            sb3.AppendFormat(head);
                            sb3.AppendFormat(values);
                        }
                        else if (sb3.Length + values.Length < maxlength)
                        {
                            sb3.AppendFormat(",");
                            sb3.AppendFormat(values);
                        }
                        else
                        {
                            sb3.AppendFormat(";");
                            cmd.CommandText = sb3.ToString();
                            cmd.ExecuteNonQuery();

                            sb3 = new StringBuilder();
                            sb3.AppendFormat(head);
                            sb3.AppendFormat(values);
                        }
                    }

                    if (sb3.Length > 0)
                    {
                        sb3.AppendFormat(";");
                        cmd.CommandText = sb3.ToString();
                        cmd.ExecuteNonQuery();
                    }

                    cmd.CommandText = "commit;";
                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
            System.Threading.Thread.Sleep(700);
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer1.Stop();
            MessageBox.Show("Finished.");
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value = (int)currentRow;
            lbTotal.Text = currentRow + " / " + totalRows;
        }

        void Execute(string sql)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
                {
                    MySqlScript script = new MySqlScript(conn);
                    script.Query = sql;
                    script.Execute();
                    script = null;
                }
                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btFunctionCreate_Click(object sender, EventArgs e)
        {
            Execute(txtFunction.Text);
        }

        private void btProcedureCreate_Click(object sender, EventArgs e)
        {
            Execute(txtProcedure.Text);
        }

        private void btTriggerCreate_Click(object sender, EventArgs e)
        {
            Execute(txtTrigger.Text);
        }

        private void btEventCreate_Click(object sender, EventArgs e)
        {
            Execute(txtEvent.Text);
        }

        private void btViewCreate_Click(object sender, EventArgs e)
        {
            Execute(txtView.Text);
        }

        private void btViewReset_Click(object sender, EventArgs e)
        {
            txtView.Text = @"CREATE VIEW `viewsample1` 
AS SELECT 'Hello View' AS `View Sample`;";
        }

        private void btEventReset_Click(object sender, EventArgs e)
        {
            txtEvent.Text = @"DELIMITER |
CREATE EVENT `eventsample1`
ON SCHEDULE EVERY 1 WEEK STARTS '2014-01-01 00:00:00'
DO BEGIN
END |";
        }

        private void btTriggerReset_Click(object sender, EventArgs e)
        {
            txtTrigger.Text = @"DELIMITER |
CREATE TRIGGER `triggersample1` 
BEFORE INSERT ON `tablesample1` 
FOR EACH ROW BEGIN
Update `table1` SET `bool` = 1 WHERE 1 = 2;
END |";
        }

        private void btProcedureReset_Click(object sender, EventArgs e)
        {
            txtProcedure.Text = @"DELIMITER |
CREATE PROCEDURE `proceduresample1`()
    DETERMINISTIC
    COMMENT 'A procedure'
BEGIN
SELECT 'Hello World !';
END |";
        }

        private void btFunctionReset_Click(object sender, EventArgs e)
        {
            txtFunction.Text = @"DELIMITER |
CREATE FUNCTION `functionsample1`() RETURNS int(11)
    DETERMINISTIC
BEGIN
DECLARE b INT;
SET b = 1;
RETURN b;
END |";
        }

    }
}
