using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ristorante
{
    public partial class Tile : UserControl
    {
        public Tile()
        {
            InitializeComponent();
            Resize += Tile_Resize;
        }

        private void Tile_Resize(object sender, EventArgs e)
        {
            descLbl.Font = new Font("Microsoft Sans Serif", Convert.ToInt32(ClientSize.Height / 8));
            numberLbl.Font = new Font("Microsoft Sans Serif", Convert.ToInt32(ClientSize.Height / 3.4));
        }

        public void Set(string desc, int quantity)
        {
            descLbl.Text = desc;
            numberLbl.Text = quantity.ToString();
        }
    }
}