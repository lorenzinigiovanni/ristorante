using System;

namespace RistoranteDigitaleClient.Models
{
    public class OrderItem
    {
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        public long Count { get; set; }
    }
}
