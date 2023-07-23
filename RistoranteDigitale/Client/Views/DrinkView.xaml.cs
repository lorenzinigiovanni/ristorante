using System.Windows;
using System.Windows.Controls;
using RistoranteDigitaleClient.ViewModels;

namespace RistoranteDigitaleClient.Views
{
    /// <summary>
    /// Logica di interazione per DrinkView.xaml
    /// </summary>
    public partial class DrinkView : UserControl
    {
        public DrinkView()
        {
            InitializeComponent();
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            await ((DrinkViewModel)DataContext).LoadDrinks();
        }
    }
}
