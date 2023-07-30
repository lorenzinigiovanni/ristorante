using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using RistoranteDigitaleClient.Models;
using RistoranteDigitaleClient.Properties;
using RistoranteDigitaleClient.Utils;
using System.Net.Http;

namespace RistoranteDigitaleClient.ViewModels
{
    internal class StatisticsViewModel : ObservableRecipient
    {
        public AsyncRelayCommand SearchCommand { get; set; }

        private string toDate = "";
        public string ToDate
        {
            get { return toDate; }
            set
            {
                SetProperty(ref toDate, value);
            }
        }

        private string fromDate = "";
        public string FromDate
        {
            get { return fromDate; }
            set
            {
                SetProperty(ref fromDate, value);
            }
        }

        private string drinksText = "Bevande";
        public string DrinksText
        {
            get { return drinksText; }
            set
            {
                SetProperty(ref drinksText, value);
            }
        }

        private string foodsText = "Piatti";
        public string FoodsText
        {
            get { return foodsText; }
            set
            {
                SetProperty(ref foodsText, value);
            }
        }

        private ObservableCollection<ItemCount> drinksItems = new();
        public ObservableCollection<ItemCount> DrinksItems
        {
            get { return drinksItems; }
            set
            {
                SetProperty(ref drinksItems, value);
            }
        }

        private ObservableCollection<ItemCount> foodsItems = new();
        public ObservableCollection<ItemCount> FoodsItems
        {
            get { return foodsItems; }
            set
            {
                SetProperty(ref foodsItems, value);
            }
        }

        public StatisticsViewModel()
        {
            SearchCommand = new AsyncRelayCommand(Search);
        }

        private async Task Search()
        {
            try
            {
                var drinksItemsTask = HttpClientManager.Client.GetAsync($"{Settings.Default.server_url}/api/Orders/Items?type={ItemType.Drink}&fromDate={FromDate}&toDate={ToDate}");
                var foodsItemsTask = HttpClientManager.Client.GetAsync($"{Settings.Default.server_url}/api/Orders/Items?type={ItemType.Food}&fromDate={FromDate}&toDate={ToDate}");
                var drinksCountTask = HttpClientManager.Client.GetAsync($"{Settings.Default.server_url}/api/Orders/Count?type={ItemType.Drink}&fromDate={FromDate}&toDate={ToDate}");
                var foodsCountTask = HttpClientManager.Client.GetAsync($"{Settings.Default.server_url}/api/Orders/Count?type={ItemType.Food}&fromDate={FromDate}&toDate={ToDate}");

                var drinksItemsResponse = await drinksItemsTask;
                var foodsItemsResponse = await foodsItemsTask;
                var drinksCountResponse = await drinksCountTask;
                var foodsCountResponse = await foodsCountTask;

                DrinksItems = await drinksItemsResponse.Content.ReadAsAsync<ObservableCollection<ItemCount>>();
                FoodsItems = await foodsItemsResponse.Content.ReadAsAsync<ObservableCollection<ItemCount>>();

                DrinksText = $"Bevande ({await drinksCountResponse.Content.ReadAsAsync<int>()} ordini)";
                FoodsText = $"Piatti ({await foodsCountResponse.Content.ReadAsAsync<int>()} ordini)";
            }
            catch (Exception e)
            {
                AutoClosingMessageBox.Show($"{e.Message}", "Errore caricamento statistiche");
            }
        }
    }
}
 