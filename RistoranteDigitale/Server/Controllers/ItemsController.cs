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
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationContext context;
        private readonly IHubContext<ItemsHub> itemsHub;

        public ItemsController(ApplicationContext context, IHubContext<ItemsHub> itemsHub)
        {
            this.context = context;
            this.itemsHub = itemsHub;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems([FromQuery] ItemType? type)
        {
            if (context.Items == null)
            {
                return NotFound();
            }

            return await GetItemsFunc(type);
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

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(Guid id)
        {
            if (context.Items == null)
            {
                return NotFound();
            }

            var item = await context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(Guid id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            context.Entry(item).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            await UpdateFrontendItems();

            return NoContent();
        }

        // POST: api/Items
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            if (context.Items == null)
            {
                return Problem("Entity set 'ApplicationContext.Items' is null.");
            }

            context.Items.Add(item);
            await context.SaveChangesAsync();

            await UpdateFrontendItems();

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            if (context.Items == null)
            {
                return NotFound();
            }

            var item = await context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            context.Items.Remove(item);
            await context.SaveChangesAsync();

            await UpdateFrontendItems();

            return NoContent();
        }

        private bool ItemExists(Guid id)
        {
            return (context.Items?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task UpdateFrontendItems()
        {
            var drinks = await GetItemsFunc(ItemType.Drink);
            var foods = await GetItemsFunc(ItemType.Food);

            await itemsHub.Clients.All.SendAsync("ReceiveItems", drinks, foods);
        }
    }
}
