using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Web;

namespace MySqlBackupASPNET
{
    public class StoreFile
    {
        public static void StoreSqlText(string text)
        {
            byte[] ba = Encoding.UTF8.GetBytes(text);
            MemoryStream ms1 = new MemoryStream(ba);
            MemoryStream ms2 = new MemoryStream();
            ZipStorer zip = ZipStorer.Create(ms2, "MySQL Backup");
            zip.AddStream(ZipStorer.Compression.Deflate, "Backup.sql", ms1, DateTime.Now, "MySQL Backup");
            zip.Close();
            StoreZipFile(ms2.ToArray());
        }

        public static void StoreZipFile(byte[] ba)
        {
            HttpContext.Current.Session["ba"] = ba;
        }

        public static byte[] GetZipFile()
        {
            return (byte[])HttpContext.Current.Session["ba"];
        }

        public static string GetSqlText()
        {
            string text = "";
            byte[] ba = GetZipFile();
            MemoryStream ms1 = new MemoryStream(ba);
            ZipStorer zip = ZipStorer.Open(ms1, FileAccess.Read);
            List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();
            MemoryStream ms2 = new MemoryStream();
            zip.ExtractFile(dir[0], ms2);
            zip.Close();
            byte[] ba2 = ms2.ToArray();
            text = Encoding.UTF8.GetString(ba2);
            return text;
        }
    }
}