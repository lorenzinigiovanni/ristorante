using System;
using System.Windows.Forms;

namespace Ristorante
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void taimer_Tick(object sender, EventArgs e)
        {
            progressBar1.Increment(+1);
            valLbl.Text = progressBar1.Value + "%";

            if (progressBar1.Value >= progressBar1.Maximum)
            {
                Close();
                new Impostazioni().Show();
            }
        }
    }
}