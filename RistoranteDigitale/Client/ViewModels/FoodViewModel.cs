using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    internal class FoodViewModel : ObservableRecipient
    {
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private ObservableCollection<Item> oldFoods = new();

        private ObservableCollection<Item> foods = new();
        public ObservableCollection<Item> Foods
        {
            get { return foods; }
            set
            {
                SetProperty(ref foods, value);
            }
        }

        public FoodViewModel()
        {
            SaveCommand = new AsyncRelayCommand(Save, CanSave);
            CancelCommand = new AsyncRelayCommand(Cancel, CanCancel);
        }

        public async Task LoadFoods()
        {
            try
            {
                HttpResponseMessage response = await HttpClientManager.Client.GetAsync($"{Settings.Default.server_url}/api/Items?type={ItemType.Food}");

                Foods = await response.Content.ReadAsAsync<ObservableCollection<Item>>();
                oldFoods = new(foods);
            }
            catch (Exception e)
            {
                AutoClosingMessageBox.Show(e.Message, "Errore caricamento piatti");
            }
        }

        public async Task SaveFoods()
        {
            try
            {
                List<Task<HttpResponseMessage>> tasks = new();

                foreach (Item food in foods)
                {
                    if (food.Id == Guid.Empty)
                    {
                        tasks.Add(HttpClientManager.Client.PostAsJsonAsync($"{Settings.Default.server_url}/api/Items", food));
                    }
                    else
                    {
                        tasks.Add(HttpClientManager.Client.PutAsJsonAsync($"{Settings.Default.server_url}/api/Items/{food.Id}", food));
                    }
                }

                foreach (Item food in oldFoods)
                {
                    if (!foods.Contains(food))
                    {
                        tasks.Add(HttpClientManager.Client.DeleteAsync($"{Settings.Default.server_url}/api/Items/{food.Id}"));
                    }
                }

                await Task.WhenAll(tasks);
            }
            catch (Exception e)
            {
                AutoClosingMessageBox.Show(e.Message, "Errore salvataggio piatti");
            }
        }

        public async Task Save()
        {
            foreach (Item food in Foods)
            {
                food.Type = ItemType.Food;
            }

            await SaveFoods();
        }

        public bool CanSave()
        {
            return true;
        }

        public async Task Cancel()
        {
            await LoadFoods();
        }

        public bool CanCancel()
        {
            return true;
        }
    }
}
