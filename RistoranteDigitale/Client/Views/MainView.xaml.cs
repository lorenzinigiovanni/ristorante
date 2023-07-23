using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.AspNetCore.SignalR.Client;
using RistoranteDigitaleClient.Models;
using RistoranteDigitaleClient.Properties;
using RistoranteDigitaleClient.Utils;
using RistoranteDigitaleClient.ViewModels;

namespace RistoranteDigitaleClient.Views
{
    /// <summary>
    /// Logica di interazione per MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.mode == 0)
            {
                tabControl.Items.Insert(0, new TabItem()
                {
                    Header = "Home",
                    Content = new CashRegisterView()
                });
            }
            else
            {
                tabControl.Items.Insert(0, new TabItem()
                {
                    Header = "Home",
                    Content = new KitchenView()
                });
            }

            tabControl.SelectedIndex = 0;
        }
    }
}
