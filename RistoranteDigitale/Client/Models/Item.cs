using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RistoranteDigitaleClient.Models
{
    public enum ItemType
    {
        Food,
        Drink,
    }

    public class Item : ObservableObject
    {
        private Guid id;
        
        public Guid Id {
            get { return id; }
            set
            {
                SetProperty(ref id, value);
            }
        }

        private long index;
        public long Index {
            get { return index; }
            set
            {
                SetProperty(ref index, value);
            }
        }

        private string? name;
        public string? Name
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value);
            }
        }

        private decimal price;
        public decimal Price
        {
            get { return price; }
            set
            {
                SetProperty(ref price, value);
            }
        }

        private long availability;
        public long Availability
        {
            get { return availability; }
            set
            {
                SetProperty(ref availability, value);
            }
        }

        private ItemType type;
        public ItemType Type
        {
            get { return type; }
            set
            {
                SetProperty(ref type, value);
            }
        }
    }
}
