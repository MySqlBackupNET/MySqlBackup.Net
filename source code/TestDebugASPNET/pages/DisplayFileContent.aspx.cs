using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO.Compression;
using System.Threading;

namespace System.pages
{
    public partial class DisplayFileContent : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"] + "";
            string filename = Request["filename"] + "";

            if (action == "download")
            {
                string filepath = "";
                if (int.TryParse(Request["id"], out int id))
                {
                    var dbFile = BackupFilesManager.GetRecord(id);
                    filepath = Path.Combine(BackupFilesManager.folder, dbFile.Filename);
                }
                else if (!string.IsNullOrWhiteSpace(filename))
                {
                    filepath = Path.Combine(BackupFilesManager.folder, filename);
                }
                else
                {
                    return;
                }

                if (File.Exists(filepath))
                {
                    try
                    {
                        int attemptCount = 0;

                        while (attemptCount < 10)
                        {
                            attemptCount++;

                            try
                            {
                                // Get or create the zip file using BackupFilesManager
                                string zipFilePath = BackupFilesManager.GetOrCreateZipFile(filepath);

                                // Get original filename for download
                                string orifilename = Path.GetFileNameWithoutExtension(filepath);

                                // Transmit the file
                                long fileSize = new FileInfo(zipFilePath).Length;
                                Response.Clear();
                                Response.ContentType = "application/octet-stream";
                                Response.AddHeader("Content-Disposition", $"attachment; filename=\"{orifilename}.zip\"");
                                Response.AddHeader("Content-Length", fileSize.ToString());

                                await Task.Run(() => Response.TransmitFile(zipFilePath));
                                Response.End();
                            }
                            catch (Exception ex)
                            {
                                if (attemptCount >= 10)
                                {
                                    throw ex;
                                }
                                else
                                {
                                    Thread.Sleep(1000); 
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //if (ex is IOException && ex.Message.Contains("being used by another process"))
                        //{
                        //    Response.Clear();
                        //    Response.ContentType = "text/html";
                        //    Response.Write("<!DOCTYPE html><html><head><script>alert('Hold on, the file is being prepared, please try again later.');</script></head><body></body></html>");
                        //    Response.Flush();
                        //    Response.End();
                        //}
                        //else
                        //{
                        //    Response.Clear();
                        //    Response.ContentType = "text/html";
                        //    string escapedMessage = System.Web.HttpUtility.HtmlEncode(ex.Message);
                        //    Response.Write($"<!DOCTYPE html><html><head><script>alert('{escapedMessage}');</script></head><body></body></html>");
                        //    Response.Flush();
                        //    Response.End();
                        //}
                    }
                    finally
                    {
                        // Async cleanup task for files older than 72 hours
                        _ = Task.Run(() => BackupFilesManager.CleanupOldTempFiles(720));
                    }

                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                string text = "";

                if (int.TryParse(Request["id"], out int id))
                {
                    text = BackupFilesManager.GetFileContent(id);
                }
                else if (!string.IsNullOrWhiteSpace(filename))
                {
                    text = BackupFilesManager.GetFileContent(filename);
                }
                else
                {
                    return;
                }

                Response.Clear();
                Response.ContentType = "text/plain; charset=utf-8";
                Response.Write(text);
            }
        }
    }
}