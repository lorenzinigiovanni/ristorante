using System.ComponentModel.DataAnnotations;

namespace RistoranteDigitaleServer.Models
{
    public enum OrderStatus
    {
        Created,
        Pending,
        Completed,
    }

    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public long Index { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PendingAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public OrderStatus Status { get; set; }
        public string? Operator { get; set; }
        public List<Item> Items { get; } = new();
        public List<OrderItem> OrderItems { get; } = new();
    }
}
