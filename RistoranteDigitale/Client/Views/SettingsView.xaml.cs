using System.Windows;
using System.Windows.Controls;
using RistoranteDigitaleClient.Properties;

namespace RistoranteDigitaleClient.Views
{
    /// <summary>
    /// Logica di interazione per SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Reload();
        }

        private void LogoButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                FileName = Settings.Default.logo,
                DefaultExt = ".bmp",
                Filter = "Bitmap image (.bmp)|*.bmp"
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                Settings.Default.logo = dialog.FileName;
            }
        }
        private void SponsorButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                FileName = Settings.Default.sponsor,
                DefaultExt = ".bmp",
                Filter = "Bitmap image (.bmp)|*.bmp"
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                Settings.Default.sponsor = dialog.FileName;
            }
        }
    }
}
