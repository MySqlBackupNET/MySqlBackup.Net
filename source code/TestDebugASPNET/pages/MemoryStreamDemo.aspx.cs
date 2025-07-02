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
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();

                // Export database
                using (var conn = config.GetNewConnection())
                using (var cmd = conn.CreateCommand())
                using (var mb = new MySqlBackup(cmd))
                {
                    conn.Open();
                    mb.ExportInfo.ExportRows = cbExportRows.Checked;
                    mb.ExportToStream(ms);
                }

                // Compression
                if (cbCompress.Checked)
                {
                    byte[] compressed = CompressorHelper.Compress(ms.ToArray());

                    // important, recycle the memory before reuse the reference
                    ms.Dispose();

                    ms = new MemoryStream(compressed);
                }

                // Encryption
                if (cbEncrypt.Checked)
                {
                    if (string.IsNullOrWhiteSpace(txtPwd.Text))
                    {
                        ((masterPage1)this.Master).ShowMessage("Cancelled", "", false);
                        return;
                    }

                    byte[] password = Encoding.UTF8.GetBytes(txtPwd.Text);
                    byte[] encrypted = AesHelper.Encrypt(ms.ToArray(), password);

                    // important, recycle the memory before reuse the reference
                    ms.Dispose();

                    ms = new MemoryStream(encrypted);
                }

                // Prepare response
                string suffix = "";
                if (cbCompress.Checked) suffix += "-compressed";
                if (cbEncrypt.Checked) suffix += "-encrypted";

                string extension = "";

                if (cbEncrypt.Checked)
                {
                    extension = ".bin";
                }
                else if (cbCompress.Checked)
                {
                    extension = ".gz";
                }
                else
                {
                    extension = ".sql";
                }

                string filename = $"backup{suffix}-{DateTime.Now:yyyy-MM-dd_HHmmss}{extension}";

                ms.Position = 0;

                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.Headers.Add("Content-Length", ms.Length.ToString());
                Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{filename}\"");

                ms.CopyTo(Response.OutputStream);

                Response.Flush();
                Response.End();
            }
            finally
            {
                ms?.Dispose();
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
    }
}