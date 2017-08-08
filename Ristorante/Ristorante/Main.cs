using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ristorante.Properties;

namespace Ristorante
{
    public partial class Main : Form
    {
        private bool _exit;
        
        private readonly int[] _ordersToDo;
        private readonly int[] _remainingPlates;

        private int _remainingOrders;

        private string[] _plateDescriptions;
        private string[] _printerDescriptions;

        private string _keypressed = "";

        private readonly Printer _printer;
        private readonly DataBase _database;

        /// <summary>
        /// Main initialization
        /// </summary>
        /// <param name="database">Orders database</param>
        public Main(DataBase database)
        {
            InitializeComponent();

            KeyPreview = true;

            Fullscreen(Settings.Default.fullScreen);
            
            if (Settings.Default.hideCursor)
                Cursor.Hide();

            _ordersToDo = new int[18];
            _remainingPlates = new int[18];

            _printer = new Printer(Settings.Default.printerName);

            _database = database;

            var clients = new SocketServer[Settings.Default.clientsNumber];

            for (var i = 0; i < Settings.Default.clientsNumber; i++)
            {
                clients[i] = new SocketServer(50200 + i, _database);
            }

            foreach (var client in clients)
            {
                client.Run();
                client.NewData += NewOrder;
            }

            KeyUp += Main_KeyUp;
            Resize += Main_Resize;

            OnResize(new EventArgs());
        }

        protected sealed override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var getRemainingOrders = _database.GetRemainingOrdersNumberAsync();
            var getPlateDescriptions = _database.GetDescriptionsAsync();
            var getPrinterDescriptions = _database.GetPrinterDescriptionsAsync();

            var tasks = new Task<int>[18];
            for (var i = 0; i < 18; i++)
                tasks[i] = _database.GetOrdersToDoAsync(i);

            await getRemainingOrders;
            await getPlateDescriptions;
            await getPrinterDescriptions;

            await Task.WhenAll(tasks);

            _remainingOrders = getRemainingOrders.Result;
            _plateDescriptions = getPlateDescriptions.Result;
            _printerDescriptions = getPrinterDescriptions.Result;

            for (var i = 0; i < 18; i++)
                _ordersToDo[i] = tasks[i].Result;

            ResetLabels();
            ResetRemainingPlates();
            RefreshOrdersToDo();
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_exit) return;
                Cursor.Show();
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    _exit = await SaveAsync(saveFile.FileName);
                    e.Cancel = !_exit;
                }
                else
                {
                    var reply = MessageBox.Show(@"Sicuro di voler chiudere senza salvare?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    _exit = reply == DialogResult.Yes;
                    e.Cancel = !_exit;
                }
                if (Settings.Default.hideCursor)
                    Cursor.Hide();
                if (_exit)
                    Application.Exit();
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Save all the orders in a CSV file
        /// </summary>
        /// <param name="path">The OS path of the CSV file</param>
        /// <returns>Return true if the operation is successful or false if it has failed</returns>
        private async Task<bool> SaveAsync(string path)
        {
            try
            {
                var swOut = new StreamWriter(path);

                swOut.Write("Numero Ordine");

                for (var i = 0; i < 18; i++)
                {
                    swOut.Write(";");
                    swOut.Write(_plateDescriptions[i]);
                }

                swOut.Write(";");
                swOut.Write("Eseguito");

                var dataSet = await _database.GetAllOrdersAsync();

                await Task.Run(() =>
                {
                    for (var i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        swOut.WriteLine();

                        for (var j = 0; j < dataSet.Tables[0].Columns.Count; j++)
                        {
                            if (j > 0)
                            {
                                swOut.Write(";");
                            }

                            var value = dataSet.Tables[0].Rows[i].ItemArray[j].ToString();
                            value = value.Replace(Environment.NewLine, " ");

                            swOut.Write(value);
                        }
                    }

                    swOut.Close();
                });
                
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Event handler for a new order
        /// </summary>
        /// <param name="sender">Senser object</param>
        /// <param name="e">Order Event Args</param>
        public async void NewOrder(object sender, OrderEventArgs e)
        {
            try
            {
                const string pattern = @"(\d+),";

                var result = new int[18];
                var response = "";

                var matches = Regex.Matches(e.Value, pattern);

                for (var i = 0; i < 18; i++)
                {
                    var value = int.Parse(matches[i].Groups[1].Value);

                    if (_remainingPlates[i] == -1)
                    {
                        result[i] = value;
                    }
                    else
                    {
                        if (value <= _remainingPlates[i])
                        {
                            result[i] = value;
                        }
                        else
                        {
                            result[i] = _remainingPlates[i];
                        }
                        _remainingPlates[i] -= result[i];
                    }

                    _ordersToDo[i] += result[i];
                    response += result[i] + ",";
                }

                var orderNumber = await _database.GetLastOrderNumberAsync() + 1;

                response += orderNumber + ",";
                e.Response = response;

                await _database.AddOrderAsync(result, orderNumber);

                _remainingOrders++;

                RefreshOrdersToDo();
                RefreshRemainingPlates();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Display and print the selected order using keyboard
        /// </summary>
        private async Task SelectOrderAsync()
        {
            if (_keypressed == "") return;

            try
            {
                var order = Convert.ToInt32(_keypressed);
                _keypressed = "";

                if (await _database.IsOrderToDoAsync(order))
                {
                    _remainingOrders--;

                    var actualOrder = await _database.GetOrderAsync(order);

                    for (var i = 0; i < 18; i++)
                    {
                        _ordersToDo[i] -= actualOrder[i];
                    }

                    await _database.SetOrderDoAsync(order);
                    await PrintOrderAsync(order, actualOrder);

                    RefreshTiles(actualOrder);
                    RefreshOrdersToDo();
                    orderNumberLabel.Text = @"Ordine Numero " + order;
                }
                else
                {
                    AutoClosingMessageBox.Show("L'ordine " + order + " non è valido", "Ordine Invalido", 1000);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Print a order
        /// </summary>
        /// <param name="order">Order number</param>
        /// <param name="actualOrder">Number of order plates</param>
        private async Task PrintOrderAsync(int order, int[] actualOrder)
        {
            var str = "";

            for (var i = 0; i < 18; i++)
            {
                if (actualOrder[i] > 0)
                    str += actualOrder[i] + " " + _printerDescriptions[i] + "\n";
            }

            await _printer.PrintAsync("N°" + order, str);
        }

        /// <summary>
        /// Set the tiles with the order plates
        /// </summary>
        /// <param name="actualOrder">Number of order plates</param>
        private void RefreshTiles(int[] actualOrder)
        {
            tile1.Set(_plateDescriptions[0], actualOrder[0]);
            tile2.Set(_plateDescriptions[1], actualOrder[1]);
            tile3.Set(_plateDescriptions[2], actualOrder[2]);
            tile4.Set(_plateDescriptions[3], actualOrder[3]);
            tile5.Set(_plateDescriptions[4], actualOrder[4]);
            tile6.Set(_plateDescriptions[5], actualOrder[5]);
            tile7.Set(_plateDescriptions[6], actualOrder[6]);
            tile8.Set(_plateDescriptions[7], actualOrder[7]);
            tile9.Set(_plateDescriptions[8], actualOrder[8]);
            tile10.Set(_plateDescriptions[9], actualOrder[9]);
            tile11.Set(_plateDescriptions[10], actualOrder[10]);
            tile12.Set(_plateDescriptions[11], actualOrder[11]);
            tile13.Set(_plateDescriptions[12], actualOrder[12]);
            tile14.Set(_plateDescriptions[13], actualOrder[13]);
            tile15.Set(_plateDescriptions[14], actualOrder[14]);
            tile16.Set(_plateDescriptions[15], actualOrder[15]);
            tile17.Set(_plateDescriptions[16], actualOrder[16]);
            tile18.Set(_plateDescriptions[17], actualOrder[17]);
        }

        private delegate void RefreshOrdersToDoCallback();
        /// <summary>
        /// Update the label of the remaining orders
        /// </summary>
        private void RefreshOrdersToDo()
        {
            if (ordersToDoLabel.InvokeRequired)
            {
                var callback = new RefreshOrdersToDoCallback(RefreshOrdersToDo);
                Invoke(callback);
            }
            else
            {
                ordersToDoLabel.Text = @"Ancora " + Convert.ToString(_remainingOrders) + @" Ordini";
                ordersToDoListBox.Items.Clear();
                for (var i = 0; i < _ordersToDo.Length; i++)
                    ordersToDoListBox.Items.Add(_ordersToDo[i] + " " + _printerDescriptions[i]);
            }
        }

        private delegate void RefreshRemainingPlatesCallback();
        /// <summary>
        /// Update the numeric up down with the remaining plates
        /// </summary>
        private void RefreshRemainingPlates()
        {
            if (plateNumericUpDown1.InvokeRequired)
            {
                var callback = new RefreshRemainingPlatesCallback(RefreshRemainingPlates);
                Invoke(callback);
            }
            else
            {
                plateNumericUpDown1.Value = _remainingPlates[0];
                plateNumericUpDown2.Value = _remainingPlates[1];
                plateNumericUpDown3.Value = _remainingPlates[2];
                plateNumericUpDown4.Value = _remainingPlates[3];
                plateNumericUpDown5.Value = _remainingPlates[4];
                plateNumericUpDown6.Value = _remainingPlates[5];
                plateNumericUpDown7.Value = _remainingPlates[6];
                plateNumericUpDown8.Value = _remainingPlates[7];
                plateNumericUpDown9.Value = _remainingPlates[8];
                plateNumericUpDown10.Value = _remainingPlates[9];
                plateNumericUpDown11.Value = _remainingPlates[10];
                plateNumericUpDown12.Value = _remainingPlates[11];
                plateNumericUpDown13.Value = _remainingPlates[12];
                plateNumericUpDown14.Value = _remainingPlates[13];
                plateNumericUpDown15.Value = _remainingPlates[14];
                plateNumericUpDown16.Value = _remainingPlates[15];
                plateNumericUpDown17.Value = _remainingPlates[16];
                plateNumericUpDown18.Value = _remainingPlates[17];
            }
        }

        /// <summary>
        /// Reset the label of the form
        /// </summary>
        private void ResetLabels()
        {
            orderNumberLabel.Text = @"Ordine Numero ---";
            ordersToDoLabel.Text = @"Ancora " + _remainingOrders + @" Ordini";

            tile1.Set(_plateDescriptions[0], 0);
            tile2.Set(_plateDescriptions[1], 0);
            tile3.Set(_plateDescriptions[2], 0);
            tile4.Set(_plateDescriptions[3], 0);
            tile5.Set(_plateDescriptions[4], 0);
            tile6.Set(_plateDescriptions[5], 0);
            tile7.Set(_plateDescriptions[6], 0);
            tile8.Set(_plateDescriptions[7], 0);
            tile9.Set(_plateDescriptions[8], 0);
            tile10.Set(_plateDescriptions[9], 0);
            tile11.Set(_plateDescriptions[10], 0);
            tile12.Set(_plateDescriptions[11], 0);
            tile13.Set(_plateDescriptions[12], 0);
            tile14.Set(_plateDescriptions[13], 0);
            tile15.Set(_plateDescriptions[14], 0);
            tile16.Set(_plateDescriptions[15], 0);
            tile17.Set(_plateDescriptions[16], 0);
            tile18.Set(_plateDescriptions[17], 0);

            plateLabel1.Text = _plateDescriptions[0];
            plateLabel2.Text = _plateDescriptions[1];
            plateLabel3.Text = _plateDescriptions[2];
            plateLabel4.Text = _plateDescriptions[3];
            plateLabel5.Text = _plateDescriptions[4];
            plateLabel6.Text = _plateDescriptions[5];
            plateLabel7.Text = _plateDescriptions[6];
            plateLabel8.Text = _plateDescriptions[7];
            plateLabel9.Text = _plateDescriptions[8];
            plateLabel10.Text = _plateDescriptions[9];
            plateLabel11.Text = _plateDescriptions[10];
            plateLabel12.Text = _plateDescriptions[11];
            plateLabel13.Text = _plateDescriptions[12];
            plateLabel14.Text = _plateDescriptions[13];
            plateLabel15.Text = _plateDescriptions[14];
            plateLabel16.Text = _plateDescriptions[15];
            plateLabel17.Text = _plateDescriptions[16];
            plateLabel18.Text = _plateDescriptions[17];
        }
        
        /// <summary>
        /// Set the remaining plates array reading the numeric up down
        /// </summary>
        private void ResetRemainingPlates()
        {
            _remainingPlates[0] = Convert.ToInt32(plateNumericUpDown1.Value);
            _remainingPlates[1] = Convert.ToInt32(plateNumericUpDown2.Value);
            _remainingPlates[2] = Convert.ToInt32(plateNumericUpDown3.Value);
            _remainingPlates[3] = Convert.ToInt32(plateNumericUpDown4.Value);
            _remainingPlates[4] = Convert.ToInt32(plateNumericUpDown5.Value);
            _remainingPlates[5] = Convert.ToInt32(plateNumericUpDown6.Value);
            _remainingPlates[6] = Convert.ToInt32(plateNumericUpDown7.Value);
            _remainingPlates[7] = Convert.ToInt32(plateNumericUpDown8.Value);
            _remainingPlates[8] = Convert.ToInt32(plateNumericUpDown9.Value);
            _remainingPlates[9] = Convert.ToInt32(plateNumericUpDown10.Value);
            _remainingPlates[10] = Convert.ToInt32(plateNumericUpDown11.Value);
            _remainingPlates[11] = Convert.ToInt32(plateNumericUpDown12.Value);
            _remainingPlates[12] = Convert.ToInt32(plateNumericUpDown13.Value);
            _remainingPlates[13] = Convert.ToInt32(plateNumericUpDown14.Value);
            _remainingPlates[14] = Convert.ToInt32(plateNumericUpDown15.Value);
            _remainingPlates[15] = Convert.ToInt32(plateNumericUpDown16.Value);
            _remainingPlates[16] = Convert.ToInt32(plateNumericUpDown17.Value);
            _remainingPlates[17] = Convert.ToInt32(plateNumericUpDown18.Value);
        }

        /// <summary>
        /// Modify the window state
        /// </summary>
        /// <param name="fullscreen">True for fullscreen or false for in window</param>
        private void Fullscreen(bool fullscreen)
        {
            if (fullscreen)
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Maximized;
            }
        }

        private async void Main_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await SelectOrderAsync();
            }
            else
            {
                if ((e.KeyCode >= Keys.D0) && (e.KeyCode <= Keys.D9))
                    _keypressed += Convert.ToChar(e.KeyCode);
            }
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (ClientSize.Height == 0) return;

            var font = new Font("Microsoft Sans Serif", Convert.ToInt32(ClientSize.Height / 40));
            orderNumberLabel.Font = font;
            ordersToDoLabel.Font = font;

            font = new Font("Microsoft Sans Serif", Convert.ToInt32(ClientSize.Height / 50));
            ordersToDoListBox.Font = font;

            font = new Font("Microsoft Sans Serif", Convert.ToInt32(ClientSize.Height / 60));
            plateLabel1.Font = font;
            plateLabel2.Font = font;
            plateLabel3.Font = font;
            plateLabel4.Font = font;
            plateLabel5.Font = font;
            plateLabel6.Font = font;
            plateLabel7.Font = font;
            plateLabel8.Font = font;
            plateLabel9.Font = font;
            plateLabel10.Font = font;
            plateLabel11.Font = font;
            plateLabel12.Font = font;
            plateLabel13.Font = font;
            plateLabel14.Font = font;
            plateLabel15.Font = font;
            plateLabel16.Font = font;
            plateLabel17.Font = font;
            plateLabel18.Font = font;
            plateNumericUpDown1.Font = font;
            plateNumericUpDown2.Font = font;
            plateNumericUpDown3.Font = font;
            plateNumericUpDown4.Font = font;
            plateNumericUpDown5.Font = font;
            plateNumericUpDown6.Font = font;
            plateNumericUpDown7.Font = font;
            plateNumericUpDown8.Font = font;
            plateNumericUpDown9.Font = font;
            plateNumericUpDown10.Font = font;
            plateNumericUpDown11.Font = font;
            plateNumericUpDown12.Font = font;
            plateNumericUpDown13.Font = font;
            plateNumericUpDown14.Font = font;
            plateNumericUpDown15.Font = font;
            plateNumericUpDown16.Font = font;
            plateNumericUpDown17.Font = font;
            plateNumericUpDown18.Font = font;
        }

        private void plateNumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[0] = Convert.ToInt32(plateNumericUpDown1.Value);
        }

        private void plateNumericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[1] = Convert.ToInt32(plateNumericUpDown2.Value);
        }

        private void plateNumericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[2] = Convert.ToInt32(plateNumericUpDown3.Value);
        }

        private void plateNumericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[3] = Convert.ToInt32(plateNumericUpDown4.Value);
        }

        private void plateNumericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[4] = Convert.ToInt32(plateNumericUpDown5.Value);
        }

        private void plateNumericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[5] = Convert.ToInt32(plateNumericUpDown6.Value);
        }

        private void plateNumericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[6] = Convert.ToInt32(plateNumericUpDown7.Value);
        }

        private void plateNumericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[7] = Convert.ToInt32(plateNumericUpDown8.Value);
        }

        private void plateNumericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[8] = Convert.ToInt32(plateNumericUpDown9.Value);
        }

        private void plateNumericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[9] = Convert.ToInt32(plateNumericUpDown10.Value);
        }

        private void plateNumericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[10] = Convert.ToInt32(plateNumericUpDown11.Value);
        }

        private void plateNumericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[11] = Convert.ToInt32(plateNumericUpDown12.Value);
        }

        private void plateNumericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[12] = Convert.ToInt32(plateNumericUpDown13.Value);
        }

        private void plateNumericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[13] = Convert.ToInt32(plateNumericUpDown14.Value);
        }

        private void plateNumericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[14] = Convert.ToInt32(plateNumericUpDown15.Value);
        }

        private void plateNumericUpDown16_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[15] = Convert.ToInt32(plateNumericUpDown16.Value);
        }

        private void plateNumericUpDown17_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[16] = Convert.ToInt32(plateNumericUpDown17.Value);
        }

        private void plateNumericUpDown18_ValueChanged(object sender, EventArgs e)
        {
            _remainingPlates[17] = Convert.ToInt32(plateNumericUpDown18.Value);
        }
    }
}