using System;
using System.Windows.Forms;

namespace DBStructCourse
{
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void buttonAuth_Click(object sender, EventArgs e)
        {
            Program.Server = textBoxServer.Text;
            Program.Security = textBoxSecurity.Text;
            Program.Database = textBoxDB.Text;
            MainFrame mainFrame = new MainFrame();
            mainFrame.Show();
            this.Hide();
        }
    }
}
