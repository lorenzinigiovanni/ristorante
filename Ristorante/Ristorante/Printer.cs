using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ristorante
{
    internal class Printer
    {
        private readonly string _printerName;

        /// <summary>
        /// Initialize a printer object
        /// </summary>
        /// <param name="printerName">Printer name</param>
        public Printer(string printerName)
        {
            _printerName = printerName;
        }

        /// <summary>
        /// Print a ticket with order number and plates name and quantities
        /// </summary>
        /// <param name="orderNumber">Order number</param>
        /// <param name="text">Plates name and quatities with \n between each row</param>
        public async Task PrintAsync(string orderNumber, string text)
        {
            await Task.Run(() =>
            {
                var printDocument = new PrintDocument {PrinterSettings = {PrinterName = _printerName}};

                printDocument.PrintPage += delegate(object sender, PrintPageEventArgs e)
                {
                    var dimension = e.Graphics.MeasureString(orderNumber, new Font("Arial", 40));
                    e.Graphics.DrawString(orderNumber, new Font("Arial", 40), new SolidBrush(Color.Black),
                        (printDocument.DefaultPageSettings.PrintableArea.Width - dimension.Width) / 2, 0);

                    e.Graphics.DrawString(text, new Font("Arial", 10), new SolidBrush(Color.Black), 0, 60);
                };

                try
                {
                    printDocument.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });
        }
    }
}