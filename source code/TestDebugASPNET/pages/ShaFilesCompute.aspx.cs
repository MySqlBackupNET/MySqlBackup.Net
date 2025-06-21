using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.pages
{
    public partial class ShaFilesCompute : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btCompute_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            List<FileUpload> lstFileUpload = new List<FileUpload>();
            lstFileUpload.Add(fileUpload1);
            lstFileUpload.Add(fileUpload2);
            lstFileUpload.Add(fileUpload3);

            string folder = Server.MapPath("~/temp");
            Directory.CreateDirectory(folder);

            foreach (FileUpload file in lstFileUpload)
            {
                if (file.HasFile)
                {
                    string filePath = Path.Combine(folder, file.FileName);
                    file.SaveAs(filePath);

                    string sha256checksum = CalculateSHA256(filePath);

                    if (sb.Length == 0)
                    {
                        sb.AppendLine("Results:");
                    }

                    sb.AppendLine();
                    sb.AppendLine($"File: {file.FileName}");
                    sb.AppendLine($"SHA256: {sha256checksum}");

                    try
                    {
                        File.Delete(filePath);
                    }
                    catch { }
                }
            }

            ph1.Controls.Add(new LiteralControl(sb.ToString()));
        }

        private string CalculateSHA256(string filePath)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    byte[] hash = sha256.ComputeHash(stream);
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hash)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
        }

    }
}