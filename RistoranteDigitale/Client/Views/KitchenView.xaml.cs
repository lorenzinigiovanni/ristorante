using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RistoranteDigitaleClient.ViewModels;
using RistoranteDigitaleClient.Properties;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using RistoranteDigitaleClient.Models;
using System.Collections.Generic;
using RistoranteDigitaleClient.Utils;

namespace RistoranteDigitaleClient.Views
{
    /// <summary>
    /// Logica di interazione per KitchenView.xaml
    /// </summary>
    public partial class KitchenView : UserControl
    {
        readonly HubConnection ordersHub;

        private string qrCode = "";
        private bool isQrCode = false;

        public KitchenView()
        {
            InitializeComponent();

            ordersHub = new HubConnectionBuilder()
                .WithUrl($"{Settings.Default.server_url}/orders")
                .Build();

            ordersHub.On<List<ItemCount>, List<Order>>("ReceiveCreated", (items, orders) =>
            {
                Dispatcher.Invoke(async () =>
                {
                    await ((KitchenViewModel)DataContext).RefreshCreated(items, orders);
                });
            });

            ordersHub.On<List<ItemCount>, List<Order>>("ReceivePending", (items, orders) =>
            {
                Dispatcher.Invoke(async () =>
                {
                    await ((KitchenViewModel)DataContext).RefreshPending(items, orders);
                });
            });

            ordersHub.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await ordersHub.StartAsync();
            };
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            await ((KitchenViewModel)DataContext).LoadItems();

            if (Settings.Default.mode == 1)
            {
                var window = Window.GetWindow(this);
                window.PreviewTextInput += QrCodePreviewTextInput;
            }

            if (ordersHub.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await ordersHub.StartAsync();
                }
                catch (Exception ex)
                {
                    AutoClosingMessageBox.Show($"{ex.Message}", "Fallita connessione SignalR");
                }
            }
        }

        private async void QrCodePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "{")
            {
                e.Handled = true;
                isQrCode = true;
                qrCode = "";

                var timer = new System.Timers.Timer(1000);
                timer.Elapsed += (s, e) =>
                {
                    qrCode = "";
                    isQrCode = false;
                };
            }
            else if (e.Text == "}")
            {
                e.Handled = true;
                isQrCode = false;
                await ((KitchenViewModel)DataContext).ManageQrCode(qrCode);
                qrCode = "";
            }
            else if (isQrCode)
            {
                e.Handled = true;
                qrCode += e.Text;
            }
        }
    }
}
