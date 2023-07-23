using System.ComponentModel.DataAnnotations;

namespace RistoranteDigitaleServer.Models
{
    public class OrderDto
    {
        [Key]
        public Guid Id { get; set; }
        public long Index { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PendingAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public OrderStatus Status { get; set; }
        public string? Operator { get; set; }
        public List<ItemCount> ItemCounts { get; set; } = new();
    }
}
