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
        private readonly bool _printerOK = false;
        private readonly Font _bigFont;
        private readonly Font _smallFont;

        /// <summary>
        /// Initialize a printer object
        /// </summary>
        /// <param name="printerName">Printer name</param>
        public Printer(string printerName)
        {
            if (printerName.Length != 0)
            {
                _printerOK = true;
                _printerName = printerName;
                _bigFont = new Font("Consolas", 40);
                _smallFont = new Font("Consolas", 9);
            }
        }
         
        /// <summary>
        /// Print a ticket with order number and plates name and quantities
        /// </summary>
        /// <param name="orderNumber">Order number</param>
        /// <param name="plateNumbers">Array of plate numbers</param>
        /// <param name="printerDescriptions">Array of printer descriptions</param>
        public async Task PrintOrderAsync(int orderNumber, int[] plateNumbers, string[] printerDescriptions)
        {
            if (_printerOK)
            {
                await Task.Run(() =>
                {
                    var printController = new StandardPrintController();
                    var printDocument = new PrintDocument { PrintController = printController, PrinterSettings = { PrinterName = _printerName } };

                    printDocument.PrintPage += delegate (object sender, PrintPageEventArgs e)
                    {
                        var y = 0;

                        var text = "#" + orderNumber;
                        var dimension = e.Graphics.MeasureString(text, _bigFont);
                        e.Graphics.DrawString(text, _bigFont, new SolidBrush(Color.Black), (printDocument.DefaultPageSettings.PrintableArea.Width - dimension.Width) / 2, y);

                        y += 60;

                        for (var i = 0; i < 18; i++)
                        {
                            if (plateNumbers[i] > 0)
                            {
                                y += 15;
                                text = plateNumbers[i].ToString().PadRight(3);
                                text += printerDescriptions[i];
                                e.Graphics.DrawString(text, _smallFont, new SolidBrush(Color.Black), 0, y);
                            }
                        }
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

        /// <summary>
        /// Print a ticket with total orders number and all plate sum and name
        /// </summary>
        /// <param name="totalOrdersNumber">Number of total orders</param>
        /// <param name="plateNumbers">Array of sum of all plates</param>
        /// <param name="printerDescriptions">Array of printer descriptions</param>
        public async Task PrintReport1Async(int totalOrdersNumber, int[] plateNumbers, string[] printerDescriptions)
        {
            if (_printerOK)
            {
                await Task.Run(() =>
                {
                    var printDocument = new PrintDocument { PrinterSettings = { PrinterName = _printerName } };

                    printDocument.PrintPage += delegate (object sender, PrintPageEventArgs e)
                    {
                        var y = 0;

                        var text = "Report";
                        var dimension = e.Graphics.MeasureString(text, _bigFont);
                        e.Graphics.DrawString(text, _bigFont, new SolidBrush(Color.Black), (printDocument.DefaultPageSettings.PrintableArea.Width - dimension.Width) / 2, y);

                        y += 80;

                        text = "Numero Ordini " + totalOrdersNumber;
                        dimension = e.Graphics.MeasureString(text, _smallFont);
                        e.Graphics.DrawString(text, _smallFont, new SolidBrush(Color.Black), (printDocument.DefaultPageSettings.PrintableArea.Width - dimension.Width) / 2, y);

                        y += 10;

                        for (var i = 0; i < 18; i++)
                        {
                            y += 15;
                            text = plateNumbers[i].ToString().PadRight(8);
                            text += printerDescriptions[i];
                            e.Graphics.DrawString(text, _smallFont, new SolidBrush(Color.Black), 0, y);
                        }
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

        /// <summary>
        /// Print a ticket with total orders number and recess
        /// </summary>
        /// <param name="totalOrdersNumber">Number of total orders</param>
        /// <param name="totalRecess">Total recess</param>
        /// <param name="recess">Array with plate recess</param>
        /// <param name="printerDescriptions">Array of printer descriptions</param>
        public async Task PrintReport2Async(int totalOrdersNumber, double totalRecess, double[] recess, string[] printerDescriptions)
        {
            if (_printerOK)
            {
                await Task.Run(() =>
                {
                    var printDocument = new PrintDocument { PrinterSettings = { PrinterName = _printerName } };

                    printDocument.PrintPage += delegate (object sender, PrintPageEventArgs e)
                    {
                        var y = 0;

                        var text = "Report";
                        var dimension = e.Graphics.MeasureString(text, _bigFont);
                        e.Graphics.DrawString(text, _bigFont, new SolidBrush(Color.Black), (printDocument.DefaultPageSettings.PrintableArea.Width - dimension.Width) / 2, y);

                        y += 80;

                        text = "Numero Ordini " + totalOrdersNumber;
                        dimension = e.Graphics.MeasureString(text, _smallFont);
                        e.Graphics.DrawString(text, _smallFont, new SolidBrush(Color.Black), (printDocument.DefaultPageSettings.PrintableArea.Width - dimension.Width) / 2, y);

                        y += 10;

                        for (var i = 0; i < 18; i++)
                        {
                            y += 15;
                            text = recess[i].ToString("0.00").PadRight(8);
                            text += printerDescriptions[i];
                            e.Graphics.DrawString(text, _smallFont, new SolidBrush(Color.Black), 0, y);
                        }

                        y += 25;
                        text = "Totale Incasso " + totalRecess.ToString("0.00");
                        e.Graphics.DrawString(text, _smallFont, new SolidBrush(Color.Black), 0, y);
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
}