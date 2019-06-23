using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Devart.Data.MySql;

namespace TestApp_DevartExpressMySql
{
    public partial class Form1 : Form
    {
        Timer timer1 = new Timer();
        string file = System.IO.Path.Combine(Application.StartupPath, "cache_constr.txt");

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            timer1.Interval = 3000;
            timer1.Tick += Timer1_Tick;
            try
            {
                txtConStr.Text = System.IO.File.ReadAllText(file);
            }
            catch { }
            btLoadSample_Click(null, null);

            txtConStr.TextChanged += TxtConStr_TextChanged;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            System.IO.File.WriteAllText(file, txtConStr.Text);
        }

        private void TxtConStr_TextChanged(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Start();
        }

        private void btRestore_Click(object sender, EventArgs e)
        {
            string sql = txtSql.Text;

            MySqlBackup mb = null;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(txtConStr.Text))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        mb = new MySqlBackup(cmd);
                        conn.Open();
                        cmd.Connection = conn;

                        mb.ImportFromString(sql);

                        conn.Close();
                    }
                }

                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                var er = mb.LastError;
                MessageBox.Show(ex.ToString() + "\r\n\r\n" + er.ToString());
            }
        }

        private void btBackup_Click(object sender, EventArgs e)
        {
            string sql = "";

            MySqlBackup mb = null;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(txtConStr.Text))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        mb = new MySqlBackup(cmd);
                        conn.Open();
                        cmd.Connection = conn;

                        sql = mb.ExportToString();

                        conn.Close();
                    }
                }

                txtSql.Text = sql;

                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                var er = mb.LastError;
                MessageBox.Show(ex.ToString() + "\r\n\r\n" + er.ToString());
            }
        }

        private void btExecute_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Result");
                int rowsAffected = 0;

                using (MySqlConnection conn = new MySqlConnection(txtConStr.Text))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;

                        cmd.CommandText = txtSql.Text;
                        rowsAffected = cmd.ExecuteNonQuery();

                        conn.Close();
                    }
                }

                dt.Rows.Add(rowsAffected + " row(s) affected by last command. No result set return.");

                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Width = 600;
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btSelect_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection conn = new MySqlConnection(txtConStr.Text))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;

                        cmd.CommandText = txtSql.Text;

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);

                        conn.Close();
                    }
                }

                dataGridView1.DataSource = dt;
                dataGridView1.ClearSelection();
                foreach (DataGridViewColumn dc in dataGridView1.Columns)
                {
                    dc.Width = 180;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btLoadSample_Click(object sender, EventArgs e)
        {
            txtSql.Text = @"/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 
-- Definition of tablea
-- 

DROP TABLE IF EXISTS `tablea`;
CREATE TABLE IF NOT EXISTS `tablea` (
  `int` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `varchar` varchar(45) DEFAULT NULL,
  `text` text,
  `datetime` datetime DEFAULT NULL,
  `date` date DEFAULT NULL,
  `time` time DEFAULT NULL,
  `decimal` decimal(10,5) DEFAULT NULL,
  `bool` tinyint(1) DEFAULT NULL,
  `tinyint` tinyint(3) unsigned DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `char36` char(36) DEFAULT NULL,
  `binary16` binary(16) DEFAULT NULL,
  `float` float DEFAULT NULL,
  `double` double DEFAULT NULL,
  `blob` blob,
  PRIMARY KEY (`int`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table tablea
-- 

/*!40000 ALTER TABLE `tablea` DISABLE KEYS */;
INSERT INTO `tablea`(`int`,`varchar`,`text`,`datetime`,`date`,`time`,`decimal`,`bool`,`tinyint`,`timestamp`,`char36`,`binary16`,`float`,`double`,`blob`) VALUES
(1,'zYM4wzDEE8QQjJpZrSizdAkINYFojEM5JlTKpOzykIxv7','X8bQ6iB5Q2bDnTkOiBhuJw9koeXJf2YDjfkZ3id3yJ7jR','2019-04-26 10:42:15','2019-04-26 00:00:00','10:42:15',3487.23980,1,1,'2019-04-26 10:42:15','00000000-0000-0000-0000-000000000000',0x00000000000000000000000000000000,243.234,456.456,0x00000000000000000000000000000000),
(2,'zYM4wzDEE8QQjJpZrSizdAkINYFojEM5JlTKpOzykIxv7','X8bQ6iB5Q2bDnTkOiBhuJw9koeXJf2YDjfkZ3id3yJ7jR','2019-04-26 10:42:15','2019-04-26 00:00:00','10:42:15',3487.23980,1,1,'2019-04-26 10:42:15','00000000-0000-0000-0000-000000000000',0x00000000000000000000000000000000,243.234,456.456,0x00000000000000000000000000000000),
(3,'zYM4wzDEE8QQjJpZrSizdAkINYFojEM5JlTKpOzykIxv7','X8bQ6iB5Q2bDnTkOiBhuJw9koeXJf2YDjfkZ3id3yJ7jR','2019-04-26 10:42:15','2019-04-26 00:00:00','10:42:15',3487.23980,1,1,'2019-04-26 10:42:15','00000000-0000-0000-0000-000000000000',0x00000000000000000000000000000000,243.234,456.456,0x00000000000000000000000000000000);
/*!40000 ALTER TABLE `tablea` ENABLE KEYS */;

-- 
-- Dumping functions
-- 

DROP FUNCTION IF EXISTS `functionsample1`;
DELIMITER |
CREATE FUNCTION `functionsample1`() RETURNS int(11)
    DETERMINISTIC
BEGIN
DECLARE b INT;
SET b = 1;
RETURN b;
END |
DELIMITER ;

-- 
-- Dumping procedures
-- 

DROP PROCEDURE IF EXISTS `proceduresample1`;
DELIMITER |
CREATE PROCEDURE `proceduresample1`()
    DETERMINISTIC
    COMMENT 'A procedure'
BEGIN
SELECT 'Hello World !';
END |
DELIMITER ;

-- 
-- Dumping events
-- 

DROP EVENT IF EXISTS `eventA`;
DELIMITER |
CREATE EVENT `eventA` ON SCHEDULE EVERY 1 WEEK STARTS '2014-01-01 00:00:00' ON COMPLETION NOT PRESERVE ENABLE DO BEGIN
END |
DELIMITER ;

DROP EVENT IF EXISTS `eventsample1`;
DELIMITER |
CREATE EVENT `eventsample1` ON SCHEDULE EVERY 1 WEEK STARTS '2014-01-01 00:00:00' ON COMPLETION NOT PRESERVE ENABLE DO BEGIN
END |
DELIMITER ;

-- 
-- Dumping views
-- 

DROP TABLE IF EXISTS `viewa`;
DROP VIEW IF EXISTS `viewa`;
CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `viewa` AS select 'Hello View' AS `View Sample`;

DROP TABLE IF EXISTS `viewsample1`;
DROP VIEW IF EXISTS `viewsample1`;
CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `viewsample1` AS select 'Hello View' AS `View Sample`;


-- 
-- Dumping triggers
-- 

DROP TRIGGER /*!50030 IF EXISTS */ `triggerA`;
DELIMITER |
CREATE TRIGGER `triggerA` 
BEFORE INSERT ON `tableA` 
FOR EACH ROW BEGIN
Update `tableA` SET `bool` = 1 WHERE 1 = 2;
END |
DELIMITER ;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;";
        }
    }
}
