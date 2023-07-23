using System.Windows;
using RistoranteDigitaleClient.ViewModels;

namespace RistoranteDigitaleClient.Views
{
    /// <summary>
    /// Logica di interazione per ChangeView.xaml
    /// </summary>
    public partial class ChangeView : Window
    {
        public ChangeView(decimal total)
        {
            InitializeComponent();
            //((ChangeViewModel)DataContext).Total = total;
            DataContext = new ChangeViewModel(total);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
