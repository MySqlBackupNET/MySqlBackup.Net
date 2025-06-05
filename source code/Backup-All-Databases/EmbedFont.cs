using System;
using System.Drawing.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Backup_All_Databases
{
    internal class EmbedFont
    {
        private static PrivateFontCollection privateFonts = null;
        private static Font _appFont = null;

        public static Font AppFont
        {
            get
            {
                if (_appFont == null)
                {
                    privateFonts = new PrivateFontCollection();
                    try
                    {
                        byte[] fontData = Properties.Resources.CascadiaCode;
                        if (fontData != null)
                        {
                             IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length);
                            Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
                            privateFonts.AddMemoryFont(fontPtr, fontData.Length);
                            Marshal.FreeCoTaskMem(fontPtr);
                        }

                        _appFont = new Font(privateFonts.Families[0], 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                    }
                    catch //(Exception ex)
                    {
                        //MessageBox.Show("Error loading font: " + ex.Message);
                        _appFont = new System.Drawing.Font("Cascadia Code", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    }
                }
                return _appFont;
            }
        }
    }
}