using System.Windows.Forms;
using System.Drawing.Text;
using System.IO;
using System.Drawing;

namespace Backup_All_Databases
{
    public partial class baseform : Form
    {
        PrivateFontCollection myFontCollection = new PrivateFontCollection();

        public baseform()
        {
            InitializeComponent();
            myFontCollection.AddFontFile(Path.Combine(Application.StartupPath, "CascadiaCode.ttf"));
            this.Font = new Font(myFontCollection.Families[0], 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        }

    }
}