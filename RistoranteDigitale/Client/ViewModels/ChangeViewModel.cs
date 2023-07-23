using CommunityToolkit.Mvvm.ComponentModel;

namespace RistoranteDigitaleClient.ViewModels
{
    public class ChangeViewModel : ObservableRecipient
    {
        private decimal cash;
        public decimal Cash
        {
            get { return cash; }
            set
            {
                SetProperty(ref cash, value);
                Change = Cash - Total;
            }
        }


        private decimal total;
        public decimal Total
        {
            get { return total; }
            set
            {
                SetProperty(ref total, value);
            }
        }

        private decimal change;
        public decimal Change
        {
            get { return change; }
            set
            {
                SetProperty(ref change, value);
            }
        }

        public ChangeViewModel(decimal total)
        {
            Total = total;
        }
    }
}
