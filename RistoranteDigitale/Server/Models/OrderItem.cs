using Microsoft.EntityFrameworkCore;

namespace RistoranteDigitaleServer.Models
{
    [PrimaryKey(nameof(OrderId), nameof(ItemId))]
    public class OrderItem
    {
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public long Count { get; set; }
    }
}
