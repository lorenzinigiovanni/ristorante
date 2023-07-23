using System.Windows;
using System.Windows.Controls;
using RistoranteDigitaleClient.ViewModels;

namespace RistoranteDigitaleClient.Views
{
    /// <summary>
    /// Logica di interazione per FoodView.xaml
    /// </summary>
    public partial class FoodView : UserControl
    {
        public FoodView()
        {
            InitializeComponent();
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            await ((FoodViewModel)DataContext).LoadFoods();
        }
    }
}
