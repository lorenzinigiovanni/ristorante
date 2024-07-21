using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RistoranteDigitaleClient.Models;
using RistoranteDigitaleClient.Properties;
using RistoranteDigitaleClient.Utils;
using RistoranteDigitaleClient.Views;

namespace RistoranteDigitaleClient.ViewModels
{
    public class CashRegisterViewModel : ObservableRecipient
    {
        public AsyncRelayCommand SendCommand { get; set; }
        public AsyncRelayCommand CancelCommand { get; set; }
        public AsyncRelayCommand ReprintCommand { get; set; }
        public AsyncRelayCommand AddDrinkCommand { get; set; }
        public AsyncRelayCommand AddFoodCommand { get; set; }
        public AsyncRelayCommand RemoveItemCommand { get; set; }

        private ObservableCollection<Item> drinks = new();
        public ObservableCollection<Item> Drinks
        {
            get { return drinks; }
            set
            {
                SetProperty(ref drinks, value);
                SendCommand.NotifyCanExecuteChanged();
            }
        }

        private ObservableCollection<Item> foods = new();
        public ObservableCollection<Item> Foods
        {
            get { return foods; }
            set
            {
                SetProperty(ref foods, value);
                SendCommand.NotifyCanExecuteChanged();
            }
        }

        private ObservableCollection<ItemCount> items = new();
        public ObservableCollection<ItemCount> Items
        {
            get { return items; }
            set
            {
                SetProperty(ref items, value);
                SendCommand.NotifyCanExecuteChanged();
                CancelCommand.NotifyCanExecuteChanged();
            }
        }

        private Item? selectedDrink;
        public Item? SelectedDrink { get => selectedDrink; set => SetProperty(ref selectedDrink, value); }

        private Item? selectedFood;
        public Item? SelectedFood { get => selectedFood; set => SetProperty(ref selectedFood, value); }

        private ItemCount? selectedItem;
        public ItemCount? SelectedItem { get => selectedItem; set => SetProperty(ref selectedItem, value); }

        private string grandTotal = "";
        public string GrandTotal { get => grandTotal; set => SetProperty(ref grandTotal, value); }

        private bool isSending = false;
        public bool IsSending { get => isSending; set => SetProperty(ref isSending, value); }

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

        public CashRegisterViewModel()
        {
            SendCommand = new AsyncRelayCommand(Send, CanSend);
            CancelCommand = new AsyncRelayCommand(Cancel, CanCancel);
            ReprintCommand = new AsyncRelayCommand(Reprint, CanReprint);
            AddDrinkCommand = new AsyncRelayCommand(AddDrink);
            AddFoodCommand = new AsyncRelayCommand(AddFood);
            RemoveItemCommand = new AsyncRelayCommand(RemoveItem);

            Items.CollectionChanged += (sender, e) =>
            {
                SendCommand.NotifyCanExecuteChanged();
                CancelCommand.NotifyCanExecuteChanged();
            };

            Foods.CollectionChanged += (sender, e) =>
            {
                SendCommand.NotifyCanExecuteChanged();
            };

            Drinks.CollectionChanged += (sender, e) =>
            {
                SendCommand.NotifyCanExecuteChanged();
            };
        }

        public async Task LoadItems()
        {
            try
            {
                var drinksTask = HttpClientManager.Client.GetAsync($"{Settings.Default.server_url}/api/Items?type={ItemType.Drink}");
                var foodsTask = HttpClientManager.Client.GetAsync($"{Settings.Default.server_url}/api/Items?type={ItemType.Food}");

                var drinksResponse = await drinksTask;
                var foodsResponse = await foodsTask;

                Drinks = await drinksResponse.Content.ReadAsAsync<ObservableCollection<Item>>();
                Foods = await foodsResponse.Content.ReadAsAsync<ObservableCollection<Item>>();
            }
            catch (Exception e)
            {
                AutoClosingMessageBox.Show(e.Message, "Errore caricamento oggetti");
            }
        }

        public async Task RefreshItems(List<Item> drinks, List<Item> foods)
        {
            foreach (ItemCount item in Items)
            {
                if (item.Item.Type == ItemType.Drink)
                {
                    drinks.First(i => i.Id == item.Item.Id).Availability -= item.Count;
                }
                else if (item.Item.Type == ItemType.Food)
                {
                    foods.First(i => i.Id == item.Item.Id).Availability -= item.Count;
                }
            }

            Drinks = new(drinks);
            Foods = new(foods);
        }

        public async Task ComputeGrandTotal()
        {
            decimal total = 0;
            foreach (ItemCount item in Items)
            {
                total += item.Item.Price * item.Count;
            }
            GrandTotal = $"Totale: {total:C}";
        }

        private async Task Send()
        {
            IsSending = true;
            var sendItems = Items.ToList();

            decimal total = 0;
            foreach (ItemCount item in Items)
            {
                total += item.Item.Price * item.Count;
            }

            var changeView = new ChangeView(total);
            changeView.ShowDialog();
            if (changeView.DialogResult == false)
            {
                IsSending = false;
                return;
            }

            try
            {
                var toSend = new { items = sendItems, operatorName = Settings.Default.operatorName };
                var x = JsonSerializer.Serialize(toSend);
                var content = new StringContent(x, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientManager.Client.PostAsync($"{Settings.Default.server_url}/api/Orders", content);

                if (response.IsSuccessStatusCode)
                {
                    var stringData = await response.Content.ReadAsStringAsync();
                    var order = JsonSerializer.Deserialize<Order>(stringData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (order != null)
                    {
                        LastOrder = order;
                        await Printer.PrintReceiptAsync(ReceiptType.CashRegister, order);
                        await Cancel();
                    }
                }
                else
                {
                    AutoClosingMessageBox.Show($"{response.StatusCode}: {response.ReasonPhrase}", "Errore creazione ordine");
                }
            }
            catch (Exception e)
            {
                AutoClosingMessageBox.Show(e.Message, "Errore creazione ordine");
            }

            IsSending = false;
        }

        private bool CanSend()
        {
            if (IsSending)
            {
                return false;
            }

            if (Items.Count <= 0)
            {
                return false;
            }

            foreach (ItemCount item in Items)
            {
                if (item.Item.Type == ItemType.Drink)
                {
                    if (Drinks.First(i => i.Id == item.Item.Id).Availability < 0)
                    {
                        return false;
                    }
                }
                else if (item.Item.Type == ItemType.Food)
                {
                    if (Foods.First(i => i.Id == item.Item.Id).Availability < 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private async Task Cancel()
        {
            Items.Clear();
            await ComputeGrandTotal();
            await LoadItems();
        }

        private bool CanCancel()
        {
            return Items.Count > 0;
        }

        private async Task Reprint()
        {
            if (LastOrder != null)
            {
                await Printer.PrintReceiptAsync(ReceiptType.CashRegister, LastOrder);
            }
        }

        private bool CanReprint()
        {
            return LastOrder != null;
        }

        private async Task AddDrink()
        {
            if (SelectedDrink != null)
            {
                if (SelectedDrink.Availability > 0)
                {
                    Item selectedItem = Drinks.First(i => i.Id == SelectedDrink.Id);
                    selectedItem.Availability--;

                    ItemCount? item = Items.FirstOrDefault(i => i.Item.Id == SelectedDrink.Id);
                    if (item == null)
                    {
                        Items.Add(new ItemCount(SelectedDrink, 1));
                    }
                    else
                    {
                        item.Count++;
                    }

                    await ComputeGrandTotal();
                    SendCommand.NotifyCanExecuteChanged();
                }
                else
                {
                    AutoClosingMessageBox.Show($"Non ci sono più {SelectedDrink.Name} disponibili", "Bevanda terminata");
                }
            }
        }

        private async Task AddFood()
        {
            if (SelectedFood != null)
            {
                if (SelectedFood.Availability > 0)
                {
                    Item selectedItem = Foods.First(f => f.Id == SelectedFood.Id);
                    selectedItem.Availability--;

                    ItemCount? item = Items.FirstOrDefault(i => i.Item.Id == SelectedFood.Id);
                    if (item == null)
                    {
                        Items.Add(new ItemCount(SelectedFood, 1));
                    }
                    else
                    {
                        item.Count++;
                    }

                    await ComputeGrandTotal();
                    SendCommand.NotifyCanExecuteChanged();
                }
                else
                {
                    AutoClosingMessageBox.Show($"Non ci sono più {SelectedFood.Name} disponibili", "Piatto terminato");
                }
            }
        }

        private async Task RemoveItem()
        {
            if (SelectedItem != null)
            {
                if (SelectedItem.Item.Type == ItemType.Drink)
                {
                    Drinks.First(i => i.Id == SelectedItem.Item.Id).Availability++;
                }
                else if (SelectedItem.Item.Type == ItemType.Food)
                {
                    Foods.First(i => i.Id == SelectedItem.Item.Id).Availability++;
                }

                var item = Items.First(i => i.Item.Id == SelectedItem.Item.Id);
                if (item != null)
                {
                    item.Count--;
                    if (item.Count == 0)
                    {
                        Items.Remove(item);
                    }
                }

                await ComputeGrandTotal();
                SendCommand.NotifyCanExecuteChanged();
            }
        }
    }
}
