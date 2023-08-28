using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ESC_POS_USB_NET.Enums;
using RistoranteDigitaleClient.Models;
using RistoranteDigitaleClient.Properties;
using ESC_POS_Printer = ESC_POS_USB_NET.Printer;


namespace RistoranteDigitaleClient.Utils
{
    internal enum ReceiptType
    {
        CashRegister,
        Kitchen,
    }

    internal class Printer
    {
        public static async Task PrintReceiptAsync(ReceiptType receiptType, Order order)
        {
            if (Settings.Default.printer.Length > 0)
            {
                var isFood = false;
                var isDrink = false;

                if (receiptType == ReceiptType.CashRegister)
                {
                    isFood = order.ItemCounts.Any(ic => ic.Item.Type == ItemType.Food);
                    isDrink = order.ItemCounts.Any(ic => ic.Item.Type == ItemType.Drink);
                }
                else if (receiptType == ReceiptType.Kitchen)
                {
                    isFood = order.ItemCounts.Any(ic => ic.Item.Type == ItemType.Food);
                }

                if (isFood)
                {
                    var printGroups = order.ItemCounts
                        .Where(ic => ic.Item.Type == ItemType.Food)
                        .Select(ic => ic.Item.PrintGroup)
                        .Distinct()
                        .ToList();

                    foreach (var printGroup in printGroups)
                    {
                        var orderFood = new Order
                        {
                            Id = order.Id,
                            Index = order.Index,
                            ItemCounts = order.ItemCounts.Where(ic => ic.Item.Type == ItemType.Food && ic.Item.PrintGroup == printGroup).ToList(),
                        };
                        await PrintOrderAsync(receiptType, orderFood);
                    }
                }

                if (isDrink)
                {
                    var printGroups = order.ItemCounts
                        .Where(ic => ic.Item.Type == ItemType.Drink)
                        .Select(ic => ic.Item.PrintGroup)
                        .Distinct()
                        .ToList();

                    foreach (var printGroup in printGroups)
                    {
                        var orderDrink = new Order
                        {
                            Id = order.Id,
                            Index = order.Index,
                            ItemCounts = order.ItemCounts.Where(ic => ic.Item.Type == ItemType.Drink).ToList(),
                        };
                        await PrintOrderAsync(receiptType, orderDrink);
                    }
                }
            }
        }

        private static async Task PrintOrderAsync(ReceiptType receiptType, Order order)
        {
            await Task.Run(() =>
            {
                var isFood = order.ItemCounts.Any(ic => ic.Item.Type == ItemType.Food);
                var isDrink = order.ItemCounts.Any(ic => ic.Item.Type == ItemType.Drink);

                var printer = new ESC_POS_Printer.Printer(Settings.Default.printer, "IBM00858");

                printer.AlignCenter();

                // If there is a logo, print it
                if (Settings.Default.logo.Length != 0)
                {
                    var image = new Bitmap(Image.FromFile(Settings.Default.logo));
                    printer.Image(image);
                    printer.NewLines(2);
                }

                printer.AlignCenter();

                // If there is a header, print it
                if (Settings.Default.header.Length != 0)
                {
                    printer.Append(FontSize(1, 2));
                    printer.Append(Settings.Default.header);
                    printer.Append(FontSize(1, 1));
                    printer.NewLines(2);
                }

                // If there is food in the order, print the order number
                if (Settings.Default.orderNumber && isFood)
                {
                    printer.Append(FontSize(4, 4));
                    printer.Append($"#{order.Index}");
                    printer.Append(FontSize(1, 1));
                    printer.NewLines(2);
                }

                printer.AlignLeft();

                // Print the order items header
                printer.BoldMode(PrinterModeState.On);
                if (receiptType == ReceiptType.CashRegister)
                {
                    printer.Append(AlignLeftRight("QTA  DESCRIZIONE", "PREZZO (€)", 48));
                }
                else if (receiptType == ReceiptType.Kitchen)
                {
                    printer.Append("QTA  DESCRIZIONE");
                }
                printer.BoldMode(PrinterModeState.Off);

                printer.NewLines(1);

                // Print the order items
                foreach (var itemCount in order.ItemCounts.OrderBy(ic => ic.Item.Index))
                {
                    var item = itemCount.Item;
                    var count = itemCount.Count;

                    var countString = String.Format("{0,3:###}", count);

                    if (receiptType == ReceiptType.CashRegister)
                    {
                        printer.Append(AlignLeftRight($"{countString}  {item.Name}", $"{(item.Price * count):F2}", 48));
                    }
                    else if (receiptType == ReceiptType.Kitchen)
                    {
                        printer.Append($"{countString}  {item.Name}");
                    }
                }

                // Print the order total
                if (receiptType == ReceiptType.CashRegister)
                {
                    var grandTotal = order.ItemCounts.Sum(ic => ic.Item.Price * ic.Count);
                    printer.NewLines(2);
                    printer.Append(FontSize(1, 2));
                    printer.Append(AlignLeftRight("TOTALE COMPLESSIVO (€)", $"{grandTotal:F2}", 48));
                    printer.Append(FontSize(1, 1));
                }

                // Print operator, cash register number and date and time
                if (receiptType == ReceiptType.CashRegister)
                {
                    var dateTime = DateTime.Now;
                    printer.NewLines(2);
                    printer.Append(AlignLeftRight($"OP: {Settings.Default.operatorName} CASSA: {Settings.Default.cashNumber}", $"{dateTime:dd/MM/yyyy HH:mm}", 48));
                }

                printer.AlignCenter();

                // Print the qrcode
                if (Settings.Default.qrCode && isFood)
                {
                    printer.NewLines(2);
                    if (receiptType == ReceiptType.CashRegister)
                    {
                        printer.QrCode("cr-" + order.Id.ToString(), (QrCodeSize)3);
                    }
                    else if (receiptType == ReceiptType.Kitchen)
                    {
                        printer.QrCode("kc-" + order.Id.ToString(), (QrCodeSize)3);
                    }
                }

                // If there is a footer, print it
                if (Settings.Default.footer.Length != 0)
                {
                    printer.NewLines(2);
                    printer.Append(FontSize(1, 2));
                    printer.Append(Settings.Default.footer);
                    printer.Append(FontSize(1, 1));
                }

                // If there is a sponsor logo, print it
                if (Settings.Default.sponsor.Length != 0)
                {
                    printer.NewLines(2);
                    var image = new Bitmap(Image.FromFile(Settings.Default.logo));
                    printer.Image(image);
                }

                printer.AlignCenter();

                printer.NewLines(2);
                printer.Append("www.lorenzinigiovanni.com");

                printer.NewLines(2);
                printer.FullPaperCut();
                printer.PrintDocument();
            });
        }

        /// <summary>
        /// From two string returns a string with the first string aligned left and the second aligned right
        /// </summary>
        /// <param name="left">Sring to be aligned on the left</param>
        /// <param name="right">String to be aligned on the right</param>
        /// <param name="lineWidth">Number of characters composing a line</param>
        /// <returns>Padded strings</returns>
        static string AlignLeftRight(string left, string right, int lineWidth)
        {
            return left + new string(' ', lineWidth - left.Length - right.Length) + right;
        }

        /// <summary>
        /// Returns the command to set the font size
        /// </summary>
        /// <param name="width">Width, from 1 to 4 (8 potentially)</param>
        /// <param name="height">Height, from 1 to 4 (8 potentially)</param>
        /// <returns>The command to set the font size</returns>
        private static byte[] FontSize(byte width, byte height)
        {
            byte n = 0;
            n |= (byte)(height - 1);
            n |= (byte)((width - 1) << 4);
            return new byte[] { 29, 33, n };
        }

    }
}
