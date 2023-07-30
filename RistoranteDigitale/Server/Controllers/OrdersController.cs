using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RistoranteDigitaleServer.Hubs;
using RistoranteDigitaleServer.Models;
using RistoranteDigitaleServer.Services;

namespace RistoranteDigitaleServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationContext context;
        private readonly IHubContext<OrdersHub> ordersHub;
        private readonly IHubContext<ItemsHub> itemsHub;

        public OrdersController(ApplicationContext context, IHubContext<OrdersHub> ordersHub, IHubContext<ItemsHub> itemsHub)
        {
            this.context = context;
            this.ordersHub = ordersHub;
            this.itemsHub = itemsHub;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders([FromQuery] OrderStatus? status, [FromQuery] ItemType? type)
        {
            if (context.Orders == null)
            {
                return NotFound();
            }

            return await GetOrdersFunc(status, type);
        }

        private async Task<List<OrderDto>> GetOrdersFunc(OrderStatus? status, ItemType? type)
        {
            var orders = await context.Orders
                .Include(o => o.Items)
                .Include(o => o.OrderItems)
                .Where(o => status == null || o.Status == status)
                .Where(o => type == null || o.Items.Any(i => i.Type == type))
                .OrderBy(o => o.Index)
                .ToListAsync();

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var itemCounts = new List<ItemCount>();
                foreach (var orderItem in order.OrderItems)
                {
                    itemCounts.Add(new ItemCount(orderItem.Item, orderItem.Count));
                }

                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    Index = order.Index,
                    CreatedAt = order.CreatedAt,
                    PendingAt = order.PendingAt,
                    CompletedAt = order.CompletedAt,
                    Status = order.Status,
                    ItemCounts = itemCounts,
                };

                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            if (context.Orders == null)
            {
                return NotFound();
            }

            var order = await context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // GET: api/Orders/5/Items
        [HttpGet("{id}/Items")]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems(Guid id)
        {
            if (context.Orders == null)
            {
                return NotFound();
            }

            var order = await context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            context.Entry(order)
                .Collection(o => o.OrderItems)
                .Load();

            return order.OrderItems;
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(Guid id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            context.Entry(order).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            await UpdateFrontendOrders();
            await UpdateFrontendItems();

            return NoContent();
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(JsonElement json)
        {
            if (context.Orders == null)
            {
                return Problem("Entity set 'ApplicationContext.Orders' is null.");
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                Index = -1,
                CreatedAt = DateTime.Now.ToUniversalTime(),
                Status = OrderStatus.Created,
            };

            await context.Orders.AddAsync(order);

            var itemCounts = JsonSerializer.Deserialize<List<ItemCount>>(json);

            if (itemCounts == null)
            {
                return BadRequest();
            }

            foreach (var itemCount in itemCounts)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ItemId = itemCount.Item.Id,
                    Count = itemCount.Count,
                };

                var item = await context.Items.FirstAsync(i => i.Id == itemCount.Item.Id);
                item.Availability -= itemCount.Count;

                await context.OrderItem.AddAsync(orderItem);
            }

            if (itemCounts.Any(i => i.Item.Type == ItemType.Food))
            {
                var index = await context.Orders.MaxAsync(o => (long?)o.Index);
                if (index == null || index < 0)
                {
                    index = 0;
                }
                order.Index = (long)(index + 1);
            }

            await context.SaveChangesAsync();

            var orderDto = new OrderDto
            {
                Id = order.Id,
                Index = order.Index,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                ItemCounts = itemCounts,
            };

            var response = CreatedAtAction("GetOrder", new { id = order.Id }, orderDto);

            await UpdateFrontendOrders();
            await UpdateFrontendItems();

            return response;
        }

        // GET: api/Orders/Items
        [HttpGet("Items")]
        public async Task<ActionResult<IEnumerable<ItemCount>>> GetOrdersItems([FromQuery] OrderStatus? status, [FromQuery] ItemType? type, [FromQuery] string? fromDate, [FromQuery] string? toDate)
        {
            if (context.Orders == null)
            {
                return NotFound();
            }

            DateTime? fromDateTime = fromDate == null ? null : DateTime.Parse(fromDate).ToUniversalTime();
            DateTime? toDateTime = toDate == null ? null : DateTime.Parse(toDate).ToUniversalTime();

            return await GetOrdersItemsFunc(status, type, fromDateTime, toDateTime);
        }

        private async Task<List<ItemCount>> GetOrdersItemsFunc(OrderStatus? status, ItemType? type, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var items = await context.Items
               .Where(i => type == null || i.Type == type)
               .Join(context.OrderItem, i => i.Id, oi => oi.ItemId, (i, oi) => new { Item = i, OrderItem = oi })
               .Join(context.Orders, i => i.OrderItem.OrderId, o => o.Id, (i, o) => new { Item = i.Item, OrderItem = i.OrderItem, Order = o })
               .Where(i => status == null || i.Order.Status == status)
               .Where(i => fromDate == null || i.Order.CreatedAt >= fromDate)
               .Where(i => toDate == null || i.Order.CreatedAt <= toDate)
               .OrderBy(i => i.Item.Index)
               .ToListAsync();

            var itemCounts = new List<ItemCount>();
            foreach (var item in items)
            {
                var itemCount = new ItemCount(item.Item, item.OrderItem.Count);

                if (itemCounts.Any(i => i.Item.Id == item.Item.Id))
                {
                    var existingItemCount = itemCounts.First(i => i.Item.Id == item.Item.Id);
                    existingItemCount.Count += itemCount.Count;
                }
                else
                {
                    itemCounts.Add(itemCount);
                }
            }

            return itemCounts;
        }

        // GET: api/Orders/Count
        [HttpGet("Count")]
        public async Task<ActionResult<long>> GetOrdersCount([FromQuery] OrderStatus? status, [FromQuery] ItemType? type, [FromQuery] string? fromDate, [FromQuery] string? toDate)
        {
            if (context.Orders == null)
            {
                return NotFound();
            }

            DateTime? fromDateTime = fromDate == null ? null : DateTime.Parse(fromDate).ToUniversalTime();
            DateTime? toDateTime = toDate == null ? null : DateTime.Parse(toDate).ToUniversalTime();

            var orders = await context.Orders
                .Include(o => o.Items)
                .Where(o => status == null || o.Status == status)
                .Where(o => type == null || o.Items.Any(i => i.Type == type))
                .Where(o => fromDate == null || o.CreatedAt >= fromDateTime)
                .Where(o => toDate == null || o.CreatedAt <= toDateTime)
                .ToListAsync();

            return orders.Count;
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            if (context.Orders == null)
            {
                return NotFound();
            }

            var order = await context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            context.Orders.Remove(order);
            await context.SaveChangesAsync();

            await UpdateFrontendOrders();

            return NoContent();
        }

        // DELETE: api/Orders
        [HttpDelete("")]
        public async Task<IActionResult> DeleteOrders()
        {
            context.Orders.RemoveRange(context.Orders);
            await context.SaveChangesAsync();

            await UpdateFrontendOrders();

            return NoContent();
        }

        // DELETE: api/Orders/Index
        [HttpDelete("Index")]
        public async Task<IActionResult> DeleteOrdersIndex()
        {
            foreach (var order in context.Orders)
            {
                order.Index = -1;
            }

            await context.SaveChangesAsync();

            await UpdateFrontendOrders();

            return NoContent();
        }

        private bool OrderExists(Guid id)
        {
            return (context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task UpdateFrontendOrders()
        {
            var createdItems = await GetOrdersItemsFunc(OrderStatus.Created, ItemType.Food);
            var createdOrders = await GetOrdersFunc(OrderStatus.Created, ItemType.Food);

            var pendingItems = await GetOrdersItemsFunc(OrderStatus.Pending, ItemType.Food);
            var pendingOrders = await GetOrdersFunc(OrderStatus.Pending, ItemType.Food);

            await ordersHub.Clients.All.SendAsync("ReceiveCreated", createdItems, createdOrders);
            await ordersHub.Clients.All.SendAsync("ReceivePending", pendingItems, pendingOrders);
        }

        public async Task<List<Item>> GetItemsFunc(ItemType? type)
        {
            List<Item> items;

            if (type != null)
            {
                items = await context.Items.Where(i => i.Type == type).OrderBy(x => x.Index).ToListAsync();
            }
            else
            {
                items = await context.Items.OrderBy(x => x.Index).ToListAsync();
            }

            return items;
        }

        public async Task UpdateFrontendItems()
        {
            var drinks = await GetItemsFunc(ItemType.Drink);
            var foods = await GetItemsFunc(ItemType.Food);

            await itemsHub.Clients.All.SendAsync("ReceiveItems", drinks, foods);
        }
    }
}
