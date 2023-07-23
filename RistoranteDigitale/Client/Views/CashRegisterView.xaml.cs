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
    /// Logica di interazione per CashRegisterView.xaml
    /// </summary>
    public partial class CashRegisterView : UserControl
    {
        readonly HubConnection itemsHub;

        public CashRegisterView()
        {
            InitializeComponent();

            itemsHub = new HubConnectionBuilder()
                .WithUrl($"{Settings.Default.server_url}/items")
                .Build();

            itemsHub.On<List<Item>, List<Item>>("ReceiveItems", (drinks, foods) =>
            {
                Dispatcher.Invoke(async () =>
                {
                    await ((CashRegisterViewModel)DataContext).RefreshItems(drinks, foods);
                });
            });

            itemsHub.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await itemsHub.StartAsync();
            };
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            await ((CashRegisterViewModel)DataContext).LoadItems();
            await ((CashRegisterViewModel)DataContext).ComputeGrandTotal();

            if (itemsHub.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await itemsHub.StartAsync();
                }
                catch (Exception ex)
                {
                    AutoClosingMessageBox.Show($"{ex.Message}", "Fallita connessione SignalR");
                }
            }
        }
    }
}
