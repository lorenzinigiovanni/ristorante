using CommunityToolkit.Mvvm.ComponentModel;

namespace RistoranteDigitaleClient.Models
{
    public class ItemCount : ObservableObject
    {
        private Item item;
        public Item Item
        {
            get { return item; }
            set
            {
                SetProperty(ref item, value);
            }
        }

        private long count;
        public long Count
        {
            get { return count; }
            set
            {
                SetProperty(ref count, value);
            }
        }

        public ItemCount(Item item, long count)
        {
            this.item = item;
            this.count = count;
        }
    }
}
