using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RistoranteDigitaleServer.Models
{
    public enum ItemType
    {
        Food,
        Drink,
    }

    public class Item
    {
        [Key]
        public Guid Id { get; set; }
        public long Index { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public long Availability { get; set; }
        public ItemType Type { get; set; }
        public long PrintGroup { get; set; }
        [JsonIgnore]
        public List<Order> Orders { get; } = new();
        [JsonIgnore]
        public List<OrderItem> OrderItems { get; } = new();
    }
}
