using api_part_project.Class;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;

namespace api_part_project.Controllers
{
    [Route("api/trader")]
    public class TraderController : Controller
    {
        private readonly AppDbContext _context;

        public TraderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTraders()
        {
            var traders = await _context.Traders.ToListAsync();
            foreach (Trader t in traders)
            {
                t.Trades = await _context.Trades.Where(te => te.IdTrader ==  t.Id).ToListAsync();
            }
            return Ok(_context.Traders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTraderById(int id)
        {
            var trader = await _context.Traders.FirstOrDefaultAsync(t => t.Id == id);
            if ( trader == null)
            {
                return NotFound(new { message = "Obchodník nenalezen." });
            }
            trader.Trades = await _context.Trades.Where(t => t.IdTrader == id).ToListAsync();
            return Ok(trader);
        }

        [HttpGet("login/{us}+{ps}")]
        public IActionResult GetTraderLogIn(string us, string ps)
        {
            var tr = _context.Traders.FirstOrDefault(t => t.FullName == us || t.PassWord == ps);
            tr!.Trades = _context.Trades.Where(t => t.IdTrader == tr.Id).ToList();
            if (tr == null)
            {
                return NotFound(new { message = "Špatné jméno nebo heslo" });
            } else { return Ok(tr); }
        }
    }
}
