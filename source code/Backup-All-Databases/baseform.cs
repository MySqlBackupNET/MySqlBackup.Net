using System.Windows.Forms;
using System.Drawing;

namespace Backup_All_Databases
{
    public partial class baseform : Form
    {

        public baseform()
        {
            InitializeComponent();
#if !DEBUG
            this.Font = new Font(Program.myFontCollection.Families[0], 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
#endif
        }

    }
}