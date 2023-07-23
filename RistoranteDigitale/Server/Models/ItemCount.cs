namespace RistoranteDigitaleServer.Models
{
    public class ItemCount
    {
        private Item item;
        public Item Item
        {
            get { return item; }
            set { item = value; }
        }

        private long count;
        public long Count
        {
            get { return count; }
            set { count = value; }
        }

        public ItemCount(Item item, long count)
        {
            this.item = item;
            this.count = count;
        }
    }
}
