using api_part_project.Class;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace api_part_project.Controllers
{
    [Route("api/trade")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TradeController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }


        [HttpGet]
        public IActionResult GetAllTrades()
        {
            return Ok(_context.Trades);
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetTradeById(int id)
        {
            var te = await _context.Trades.FirstOrDefaultAsync(t => t.Id == id);
            if (te == null)
            {
                return NotFound(new { message = "Obchod nenalezen." });
            }
            return Ok(te);
        }
        [HttpPost("/add")]
        public async Task<IActionResult> AddTrade([FromBody] Trade te)
        {
            _context.Trades.Add(te);
            var tr = await _context.Traders.FindAsync(te.IdTrader);
            await _context.SaveChangesAsync();
            return Ok(tr!.Trades);
        }
        [HttpDelete("/del/{id}")]
        public async Task<IActionResult> RemoveTrade(int id)
        {
            var te = await _context.Trades.FindAsync(id);
            var tr = await _context.Traders.FindAsync(te!.IdTrader);
            _context.Trades.Remove(te!);
            await _context.SaveChangesAsync();
            return Ok(tr!.Trades);
        }
    }
}

