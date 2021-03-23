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
    public class LoyaltyTransactionsController : ControllerBase
    {
        private readonly LoyaltyAppContext _context;

        public LoyaltyTransactionsController(LoyaltyAppContext context)
        {
            _context = context;
        }

        // GET: api/LoyaltyTransactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoyaltyTransaction>>> GetLoyaltyBalanceTransactions()
        {
            return await _context.LoyaltyBalanceTransactions.ToListAsync();
        }

        // GET
        [HttpGet("balance/{id}")]
        public async Task<ActionResult> GetBalanceByClientId(int id)
        {
            var relevantClient = await _context.Clients.SingleOrDefaultAsync(client => client.Id == id);
            if (relevantClient is null)
            {
                return BadRequest("Please provide an Id for an existing client");
            }

            var clientTransactions = await _context.LoyaltyBalanceTransactions.Where(tr => tr.ClientId == relevantClient.Id)
                                                                              .ToListAsync();
            if (!clientTransactions.Any())
            {
                return new JsonResult(new { Response = "Given client does not have any loyalty transactions" });
            }

            return new JsonResult(new { Balance = clientTransactions.Sum(t => t.LoyaltyPointsAmount) });
        }

        // GET: api/LoyaltyTransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoyaltyTransaction>> GetLoyaltyTransactionDetails(int id)
        {
            var loyaltyTransaction = await _context.LoyaltyBalanceTransactions.Include(tr => tr.Card)
                                                                              .Include(tr => tr.Client)
                                                                              .SingleOrDefaultAsync(tr => tr.Id == id);

            if (loyaltyTransaction == null)
            {
                return NotFound();
            }

            return loyaltyTransaction;
        }

        // GET
        [HttpGet("client/{id}")]
        public async Task<ActionResult<IEnumerable<LoyaltyTransaction>>> GetLoyaltyTransactionsForClient(int id)
        {
            var clientTransactions = await _context.LoyaltyBalanceTransactions.Where(tr => tr.ClientId == id)
                                                                              .ToListAsync();

            if (!clientTransactions.Any())
            {
                return NotFound("Please provide and Id for an existing client");
            }

            return clientTransactions;
        }

        // PUT: api/LoyaltyTransactions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoyaltyTransaction(int id, LoyaltyTransaction loyaltyTransaction)
        {
            if (id != loyaltyTransaction.Id)
            {
                return BadRequest();
            }

            _context.Entry(loyaltyTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoyaltyTransactionExists(id))
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

        // POST: api/LoyaltyTransactions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(Policy = "AbleToIssueTransactions")]
        public async Task<ActionResult<LoyaltyTransaction>> PostLoyaltyTransaction(LoyaltyTransaction loyaltyTransaction)
        {
            if (loyaltyTransaction.CardNumber == 0)
            {
                return BadRequest($"A card with the given number {loyaltyTransaction.CardNumber} does not exist");
            }

            var card = await _context.LoyaltyCards.SingleOrDefaultAsync(c => c.Number == loyaltyTransaction.CardNumber);

            if (card is null)
            {
                return BadRequest($"A card with the given number {loyaltyTransaction.CardNumber} does not exist");
            }

            loyaltyTransaction.ClientId = card.ClientId;
            card.LoyaltyTransactions.Add(loyaltyTransaction);
            _context.Entry(card).State = EntityState.Modified;

            _context.LoyaltyBalanceTransactions.Add(loyaltyTransaction);

            await _context.SaveChangesAsync();

            return CreatedAtAction("PostLoyaltyTransaction", new { id = loyaltyTransaction.Id }, loyaltyTransaction);
        }

        // DELETE: api/LoyaltyTransactions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LoyaltyTransaction>> DeleteLoyaltyTransaction(int id)
        {
            var loyaltyTransaction = await _context.LoyaltyBalanceTransactions.FindAsync(id);
            if (loyaltyTransaction == null)
            {
                return NotFound();
            }

            _context.LoyaltyBalanceTransactions.Remove(loyaltyTransaction);
            await _context.SaveChangesAsync();

            return loyaltyTransaction;
        }

        private bool LoyaltyTransactionExists(int id)
        {
            return _context.LoyaltyBalanceTransactions.Any(e => e.Id == id);
        }
    }
}
