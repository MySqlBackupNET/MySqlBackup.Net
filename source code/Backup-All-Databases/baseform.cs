using System.Windows.Forms;
using System.Drawing.Text;

namespace Backup_All_Databases
{
    public partial class baseform : Form
    {
        private PrivateFontCollection privateFonts = new PrivateFontCollection();

        public baseform()
        {
            InitializeComponent();
#if !DEBUG
            this.Font = EmbedFont.AppFont;
#endif
        }

    }
}