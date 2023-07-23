using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using RistoranteDigitaleClient.Models;
using System.Linq;
using RistoranteDigitaleClient.Utils;
using RistoranteDigitaleClient.Properties;
using System.Collections.Generic;

namespace RistoranteDigitaleClient.ViewModels
{
    internal class KitchenViewModel : ObservableRecipient
    {
        public AsyncRelayCommand PendingOrderCommand { get; set; }
        public AsyncRelayCommand CompletedOrderCommand { get; set; }
        public AsyncRelayCommand ReprintCommand { get; set; }

        private ObservableCollection<ItemCount> createdItems = new();
        public ObservableCollection<ItemCount> CreatedItems
        {
            get { return createdItems; }
            set
            {
                SetProperty(ref createdItems, value);
            }
        }

        private ObservableCollection<Order> createdOrders = new();
        public ObservableCollection<Order> CreatedOrders
        {
            get { return createdOrders; }
            set
            {
                SetProperty(ref createdOrders, value);
            }
        }

        private ObservableCollection<ItemCount> pendingItems = new();
        public ObservableCollection<ItemCount> PendingItems
        {
            get { return pendingItems; }
            set
            {
                SetProperty(ref pendingItems, value);
            }
        }

        private ObservableCollection<Order> pendingOrders = new();
        public ObservableCollection<Order> PendingOrders
        {
            get { return pendingOrders; }
            set
            {
                SetProperty(ref pendingOrders, value);
            }
        }

        private string pendingOrdersString;
        public string PendingOrdersString
        {
            get { return pendingOrdersString; }
            set
            {
                SetProperty(ref pendingOrdersString, value);
            }
        }

        private long createdOrdersCount;
        public long CreatedOrdersCount
        {
            get { return createdOrdersCount; }
            set
            {
                SetProperty(ref createdOrdersCount, value);
            }
        }

        private string pendingOrderNumber = "";
        public string PendingOrderNumber
        {
            get { return pendingOrderNumber; }
            set
            {
                SetProperty(ref pendingOrderNumber, value);
            }
        }

        private string completedOrderNumber = "";
        public string CompletedOrderNumber
        {
            get { return completedOrderNumber; }
            set
            {
                SetProperty(ref completedOrderNumber, value);
            }
        }

        private Order? lastOrder = null;
        private Order? LastOrder
        {
            get { return lastOrder; }
            set
            {
                SetProperty(ref lastOrder, value);
                ReprintCommand.NotifyCanExecuteChanged();
            }
        }

        public KitchenViewModel()
        {
            PendingOrderCommand = new AsyncRelayCommand(PendingOrder);
            CompletedOrderCommand = new AsyncRelayCommand(CompletedOrder);
            ReprintCommand = new AsyncRelayCommand(Reprint, CanReprint);

            pendingOrdersString = "Ordini in attesa: 0";
        }

        public async Task LoadItems()
        {
            try
            {
                var createdItemsTask = HttpClientManager.Client.GetAsync($"{Settings.Default.server_url}/api/Orders/Items?status={OrderStatus.Created}&type={ItemType.Food}");
                var pendingItemsTask = HttpClientManager.Client.GetAsync($"{Settings.Default.server_url}/api/Orders/Items?status={OrderStatus.Pending}&type={ItemType.Food}");
                var createdOrdersTask = HttpClientManager.Client.GetAsync($"{Settings.Default.server_url}/api/Orders?status={OrderStatus.Created}&type={ItemType.Food}");
                var pendingOrdersTask = HttpClientManager.Client.GetAsync($"{Settings.Default.server_url}/api/Orders?status={OrderStatus.Pending}&type={ItemType.Food}");

                var createdItemsResponse = await createdItemsTask;
                var pendingItemsResponse = await pendingItemsTask;
                var createdOrdersResponse = await createdOrdersTask;
                var pendingOrdersResponse = await pendingOrdersTask;

                CreatedItems = await createdItemsResponse.Content.ReadAsAsync<ObservableCollection<ItemCount>>();
                PendingItems = await pendingItemsResponse.Content.ReadAsAsync<ObservableCollection<ItemCount>>();
                CreatedOrders = await createdOrdersResponse.Content.ReadAsAsync<ObservableCollection<Order>>();
                PendingOrders = await pendingOrdersResponse.Content.ReadAsAsync<ObservableCollection<Order>>();
                CreatedOrdersCount = CreatedOrders.Count;

                PendingOrdersString = "";
                foreach (Order order in PendingOrders)
                {
                    PendingOrdersString += $"#{order.Index} ";
                }
            }
            catch (Exception e)
            {
                AutoClosingMessageBox.Show($"{e.Message}", "Errore caricamento ordini");
            }
        }

        public async Task RefreshCreated(List<ItemCount> items, List<Order> orders)
        {
            CreatedItems = new(items);
            CreatedOrders = new(orders);
            CreatedOrdersCount = CreatedOrders.Count;
        }

        public async Task RefreshPending(List<ItemCount> items, List<Order> orders)
        {
            PendingItems = new(items);
            PendingOrders = new(orders);

            PendingOrdersString = "";
            foreach (Order order in PendingOrders)
            {
                PendingOrdersString += $"#{order.Index} ";
            }
        }

        public async Task ManageQrCode(string qrCode)
        {
            try
            {
                var type = qrCode.Split('-', 2)[0];
                var id = Guid.Parse(qrCode.Split('-', 2)[1]);

                if (type == "cr")
                {
                    var order = CreatedOrders.First(x => x.Id == id);
                    await UpdateOrderStatus(order, OrderStatus.Pending);
                }
                else if (type == "kc")
                {
                    var order = PendingOrders.First(x => x.Id == id);
                    await UpdateOrderStatus(order, OrderStatus.Completed);
                }
            }
            catch (Exception e)
            {
                AutoClosingMessageBox.Show("ID ordine non valido", $"Ordine {qrCode}");
            }
        }

        private async Task PendingOrder()
        {
            try
            {
                var orderNumber = long.Parse(PendingOrderNumber);
                var order = CreatedOrders.First(x => x.Index == orderNumber);

                await UpdateOrderStatus(order, OrderStatus.Pending);
            }
            catch (Exception e)
            {
                AutoClosingMessageBox.Show("Numero ordine non valido", $"Ordine {PendingOrderNumber}");
            }

            PendingOrderNumber = "";
        }

        private async Task CompletedOrder()
        {
            try
            {
                var orderNumber = long.Parse(CompletedOrderNumber);
                var order = PendingOrders.First(x => x.Index == orderNumber);

                await UpdateOrderStatus(order, OrderStatus.Completed);
            }
            catch (Exception e)
            {
                AutoClosingMessageBox.Show("Numero ordine non valido", $"Ordine {CompletedOrderNumber}");
            }

            CompletedOrderNumber = "";
        }

        private async Task UpdateOrderStatus(Order order, OrderStatus status)
        {
            order.Status = status;
            if (status == OrderStatus.Pending)
            {
                order.PendingAt = DateTime.Now.ToUniversalTime();
            }
            else if (status == OrderStatus.Completed)
            {
                order.CompletedAt = DateTime.Now.ToUniversalTime();
            }

            try
            {
                var response = await HttpClientManager.Client.PutAsJsonAsync($"{Settings.Default.server_url}/api/Orders/{order.Id}", order);

                if (response.IsSuccessStatusCode)
                {
                    if (status == OrderStatus.Pending)
                    {
                        LastOrder = order;
                        await Printer.PrintReceiptAsync(ReceiptType.Kitchen, order);
                    }
                    await LoadItems();
                }
                else
                {
                    AutoClosingMessageBox.Show($"{response.StatusCode}: {response.ReasonPhrase}", $"Errore aggiornamento ordine {order.Index}");
                }
            }
            catch (Exception e)
            {
                AutoClosingMessageBox.Show($"{e.Message}", $"Errore aggiornamento ordine {order.Index}");
            }
        }

        private async Task Reprint()
        {
            if (LastOrder != null)
            {
                await Printer.PrintReceiptAsync(ReceiptType.Kitchen, LastOrder);
            }
        }

        private bool CanReprint()
        {
            return LastOrder != null;
        }
    }
}
