using LoyaltyAppData;
using LoyaltyLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoyaltyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountCardsController : ControllerBase
    {
        private readonly LoyaltyAppContext _context;

        public DiscountCardsController(LoyaltyAppContext context)
        {
            _context = context;
        }

        // GET: api/DiscountCards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountCard>>> GetLoyaltyCards()
        {
            var result = await _context.LoyaltyCards.ToListAsync();
            return result;
        }

        // GET: api/DiscountCards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DiscountCard>> GetDiscountCard(int id)
        {
            var discountCard = await _context.LoyaltyCards.Include(card => card.Client)
                                                          .Include(card => card.LoyaltyTransactions)
                                                          .SingleOrDefaultAsync(card => card.Number == id);

            if (discountCard == null)
            {
                return NotFound();
            }

            return discountCard;
        }

        // POST: api/DiscountCards
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(Policy = "AbleToIssueACard")]
        public async Task<ActionResult<DiscountCard>> IssueLoyaltyCardForClient(DiscountCard discountCard)
        {
            if (discountCard.ClientId == 0)
            {
                return BadRequest("Please specify Id for an existing client");
            }

            var client = await _context.Clients.SingleOrDefaultAsync(c => c.Id == discountCard.ClientId);
            if (client is null)
            {
                return BadRequest("Please specify Id for an existing client");
            }

            client.DiscountCards.Add(discountCard);
            _context.Entry(client).State = EntityState.Modified;

            _context.LoyaltyCards.Add(discountCard);
            await _context.SaveChangesAsync();

            return CreatedAtAction("IssueLoyaltyCardForClient", new { id = discountCard.Number }, discountCard);
        }

        // DELETE: api/DiscountCards/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DiscountCard>> DeleteDiscountCard(int id)
        {
            var card = await _context.LoyaltyCards.FindAsync(id);
            if (card is null)
            {
                return NotFound();
            }

            _context.LoyaltyCards.Remove(card);
            await _context.SaveChangesAsync();

            return card;
        }

        // PUT: api/DiscountCards/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiscountCard(int id, DiscountCard discountCard)
        {
            if (id != discountCard.Number)
            {
                return BadRequest();
            }

            _context.Entry(discountCard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiscountCardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        private bool DiscountCardExists(int id)
        {
            return _context.LoyaltyCards.Any(e => e.Number == id);
        }
    }
}
