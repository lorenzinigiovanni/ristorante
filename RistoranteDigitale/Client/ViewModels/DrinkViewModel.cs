using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RistoranteDigitaleClient.Models;
using RistoranteDigitaleClient.Properties;
using RistoranteDigitaleClient.Utils;

namespace RistoranteDigitaleClient.ViewModels
{
    internal class DrinkViewModel : ObservableRecipient
    {
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private ObservableCollection<Item> oldDrinks = new();

        private ObservableCollection<Item> drinks = new();
        public ObservableCollection<Item> Drinks
        {
            get { return drinks; }
            set
            {
                SetProperty(ref drinks, value);
            }
        }

        public DrinkViewModel()
        {
            SaveCommand = new AsyncRelayCommand(Save, CanSave);
            CancelCommand = new AsyncRelayCommand(Cancel, CanCancel);
        }

        public async Task LoadDrinks()
        {
            try
            {
                HttpResponseMessage response = await HttpClientManager.Client.GetAsync($"{Settings.Default.server_url}/api/Items?type={ItemType.Drink}");

                Drinks = await response.Content.ReadAsAsync<ObservableCollection<Item>>();
                oldDrinks = new(drinks);
            }
            catch (Exception e)
            {
                AutoClosingMessageBox.Show(e.Message, "Errore caricamento bevande");
            }
        }

        public async Task SaveDrinks()
        {
            try
            {
                List<Task<HttpResponseMessage>> tasks = new();

                foreach (Item drink in drinks)
                {
                    if (drink.Id == Guid.Empty)
                    {
                        tasks.Add(HttpClientManager.Client.PostAsJsonAsync($"{Settings.Default.server_url}/api/Items", drink));
                    }
                    else
                    {
                        tasks.Add(HttpClientManager.Client.PutAsJsonAsync($"{Settings.Default.server_url}/api/Items/{drink.Id}", drink));
                    }
                }

                foreach (Item drink in oldDrinks)
                {
                    if (!drinks.Contains(drink))
                    {
                        tasks.Add(HttpClientManager.Client.DeleteAsync($"{Settings.Default.server_url}/api/Items/{drink.Id}"));
                    }
                }
            }
            catch (Exception e)
            {
                AutoClosingMessageBox.Show(e.Message, "Errore salvataggio bevande");
            }
        }

        public async Task Save()
        {
            foreach (Item drink in Drinks)
            {
                drink.Type = ItemType.Drink;
            }

            await SaveDrinks();
        }

        public bool CanSave()
        {
            return true;
        }

        public async Task Cancel()
        {
            await LoadDrinks();
        }

        public bool CanCancel()
        {
            return true;
        }
    }
}
