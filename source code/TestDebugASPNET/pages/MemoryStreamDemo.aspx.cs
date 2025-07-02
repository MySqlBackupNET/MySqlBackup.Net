using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using MySqlConnector;
using System.Text;
using System.IO.Compression;

namespace System.pages
{
    public partial class MemoryStreamDemo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btBackup_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();

            try
            {
                using (var conn = config.GetNewConnection())
                using (var cmd = conn.CreateCommand())
                using (var mb = new MySqlBackup(cmd))
                {
                    conn.Open();
                    mb.ExportInfo.ExportRows = cbExportRows.Checked;
                    mb.ExportToStream(ms);
                }

                if (cbCompress.Checked)
                {
                    byte[] ba = CompressorHelper.Compress(ms.ToArray());
                    ms.Dispose(); // recylce the memory before reuse the reference
                    ms = new MemoryStream(ba);
                }

                if (cbEncrypt.Checked)
                {
                    if (string.IsNullOrWhiteSpace(txtPwd.Text))
                    {
                        ((masterPage1)this.Master).ShowMessage("Cancelled", "", false);
                        return;
                    }

                    byte[] baPwd = Encoding.UTF8.GetBytes(txtPwd.Text);
                    byte[] ba = AesHelper.Encrypt(ms.ToArray(), baPwd);
                    ms.Dispose(); // recylce the memory before reuse the reference
                    ms = new MemoryStream(ba);
                }

                ms.Position = 0;

                string isEncrypted = cbEncrypt.Checked ? "-encrypted" : "";
                string isCompressed = cbCompress.Checked ? "-compressed" : "";

                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.Headers.Add("Content-Length", ms.Length.ToString());
                Response.Headers.Add("Content-Disposition", $"attachment; filename=\"backup{isEncrypted}{isCompressed}-{DateTime.Now:yyyy-MM-dd_HHmmss}\"");
                ms.CopyTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            finally
            {
                ms?.Dispose(); // recylce the memory
            }
        }

        protected void btRestore_Click(object sender, EventArgs e)
        {
            if (fileUploadRestore.FileBytes == null || fileUploadRestore.FileBytes.Length == 0)
            {
                throw new ArgumentException("Please select a backup file to restore");
            }

            MemoryStream ms = new MemoryStream(fileUploadRestore.FileBytes);

            try
            {
                if (cbEncrypt.Checked)
                {
                    if (string.IsNullOrWhiteSpace(txtPwd.Text))
                        throw new ArgumentException("Password required for decryption");

                    byte[] baPwd = Encoding.UTF8.GetBytes(txtPwd.Text);
                    byte[] decryptedData = AesHelper.Decrypt(ms.ToArray(), baPwd);
                    ms.Dispose(); // recylce the memory before reuse the reference
                    ms = new MemoryStream(decryptedData);
                }

                if (cbCompress.Checked)
                {
                    byte[] decompressedData = CompressorHelper.Decompress(ms.ToArray());
                    ms.Dispose(); // recylce the memory before reuse the reference
                    ms = new MemoryStream(decompressedData);
                }

                ms.Position = 0;

                using (var conn = config.GetNewConnection())
                using (var cmd = conn.CreateCommand())
                using (var mb = new MySqlBackup(cmd))
                {
                    conn.Open();
                    mb.ImportFromStream(ms);
                }

                ((masterPage1)this.Master).ShowMessage("Ok", "Restore success!", true);
                ((masterPage1)this.Master).WriteTopMessageBar("Restore success!", true);
            }
            catch (Exception ex)
            {
                ((masterPage1)this.Master).ShowMessage("Error", "Restore failed!", false);
                ((masterPage1)this.Master).WriteTopMessageBar($"Restore failed!<br />Error: {ex.Message}", false);
            }
            finally
            {
                ms?.Dispose(); // recylce the memory
            }
        }

        protected void btTest_Click(object sender, EventArgs e)
        {

        }
    }
}