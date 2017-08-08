using System;
using System.Drawing.Printing;
using System.Windows.Forms;
using Ristorante.Properties;

namespace Ristorante
{
    public partial class Impostazioni : Form
    {
        private bool _dbConnected;
        private bool _okay;

        private DataBase _database;

        public Impostazioni()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get parameters from saved settings and put the values on the windows form controls
        /// </summary>
        private void GetFromSettings()
        {
            try
            {
                cashRegistersNumber.Value = Settings.Default.clientsNumber;
                fullscreenChk.Checked = Settings.Default.fullScreen;
                cursorChk.Checked = Settings.Default.hideCursor;
                openFile.FileName = Settings.Default.dbPath;
                pathLbl.Text = Settings.Default.dbPath;
                printerComboBox.SelectedItem = Settings.Default.printerName;
                printerHeading.Text = Settings.Default.printerHeading;
                printerFooter.Text = Settings.Default.printerFooter;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Impostazioni_Load(object sender, EventArgs e)
        {
            for (var i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                var printer = PrinterSettings.InstalledPrinters[i];
                printerComboBox.Items.Add(printer);
            }

            GetFromSettings();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            _okay = true;

            try
            {
                if (_dbConnected == false)
                {
                    MessageBox.Show(@"Connettersi ad un DataBase", Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _okay = false;
                }

                if (_okay)
                {
                    Settings.Default.Save();
                    Close();
                    new Main(_database).Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Impostazioni_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();
            if(!_okay)
                Application.Exit();
        }

        private void selectDB_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    pathLbl.Text = openFile.FileName;
                    Settings.Default.dbPath = openFile.FileName;
                    connectDB.Enabled = true;
                    _dbConnected = false;
                }
            }
            catch (Exception ex)
            {
                connectDB.Enabled = false;
                _dbConnected = false;
                MessageBox.Show(ex.ToString());
            }
        }

        private async void connectDB_Click(object sender, EventArgs e)
        {
            try
            {
                resetDB.Enabled = true;
                connectDB.Enabled = false;
                _dbConnected = true;
                _database = new DataBase();
                await _database.DataBaseConnectionAsync(openFile.FileName);
            }
            catch (Exception ex)
            {
                resetDB.Enabled = false;
                connectDB.Enabled = true;
                _dbConnected = false;
                MessageBox.Show(ex.ToString());
            }
        }

        private async void resetDB_Click(object sender, EventArgs e)
        {
            try
            {
                resetDB.Enabled = false;
                await _database.DataBaseResetAsync();
            }
            catch (Exception ex)
            {
                resetDB.Enabled = true;
                MessageBox.Show(ex.ToString());
            }
        }

        private void fullscreenChk_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.fullScreen = fullscreenChk.Checked;
        }

        private void curChk_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.hideCursor = cursorChk.Checked;
        }

        private void cashRegistersNumber_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.clientsNumber = Convert.ToInt32(cashRegistersNumber.Value);
        }

        private void printerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.printerName = printerComboBox.Text;
        }

        private void printerHeading_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.printerHeading = printerHeading.Text;
        }

        private void printerFooter_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.printerFooter = printerFooter.Text;
        }
    }
}