using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Ristorante
{
    internal class ProgressBarEx : ProgressBar
    {
        public ProgressBarEx()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush brush = null;
            var rec = new Rectangle(0, 0, Width, Height);

            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, rec);

            rec.Width = (int) (rec.Width*((double) Value/Maximum)) - 4;
            rec.Height -= 4;
            /*
            base.Refresh();
            base
                .CreateGraphics()
                .DrawString(
                    base.Value.ToString() + "%",
                    new Font("Arial", (float)8.25, FontStyle.Bold),
                    Brushes.Black,
                    new PointF(this.Width / 2 - 10,
                    this.Height / 2 - 7)
                );*/

            brush = new LinearGradientBrush(rec, ForeColor, BackColor, LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(brush, 2, 2, rec.Width, rec.Height);
        }
    }
}