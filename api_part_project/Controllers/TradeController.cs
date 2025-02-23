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

        [HttpGet("{id}")]
        public IActionResult GetTradeById(int id)
        {
            var trade = _context.Trades.FirstOrDefault(t => t.Id == id);
            if (trade == null)
            {
                return NotFound(new { message = "Obchod nenalezen." });
            }
            return Ok(trade);
        }
        [HttpPost("/add")]
        public IActionResult AddTrade([FromBody] Trade trade)
        {
            _context.Trades.Add(trade);
            _context.SaveChanges();
            return Ok(new { msg = "Nový obchod úspěšně přidán." });
        }
        [HttpDelete("del/{id}")]
        public async Task<IActionResult> RemoveTrade(int id)
        {
            var tr = await _context.Trades.FindAsync(id);
            _context.Trades.Remove(tr!);
            await _context.SaveChangesAsync();
            return Ok(new { msg = "Záznam úspěšně odstraněn" });
        }
    }
}

