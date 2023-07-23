using System;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RistoranteDigitaleClient.Properties;
using RistoranteDigitaleClient.Utils;

namespace RistoranteDigitaleClient.ViewModels
{
    public class SettingsViewModel : ObservableRecipient
    {
        public AsyncRelayCommand ResetOrdersIndexCommand { get; set; }
        public AsyncRelayCommand ResetOrdersCommand { get; set; }

        public SettingsViewModel()
        {
            ResetOrdersIndexCommand = new AsyncRelayCommand(ResetOrdersIndex);
            ResetOrdersCommand = new AsyncRelayCommand(ResetOrders);
        }

        private async Task ResetOrdersIndex()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Sei sicuro di voler resettare il contatore?", "Conferma cancellazione", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {

                try
                {
                    await HttpClientManager.Client.DeleteAsync($"{Settings.Default.server_url}/api/Orders/Index");
                }
                catch (Exception e)
                {
                    AutoClosingMessageBox.Show($"{e.Message}", "Errore di connessione al server");
                }
            }
        }

        private async Task ResetOrders()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Sei sicuro di voler resettare gli ordini?", "Conferma cancellazione", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                try
                {
                    await HttpClientManager.Client.DeleteAsync($"{Settings.Default.server_url}/api/Orders");
                }
                catch (Exception e)
                {
                    AutoClosingMessageBox.Show($"{e.Message}", "Errore di connessione al server");
                }
            }
        }
    }
}
