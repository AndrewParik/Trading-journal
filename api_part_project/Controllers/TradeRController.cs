using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAllTraders()
        {
            _context.Traders.ForEachAsync( t => t.Trades = _context.Trades.Where(te => te.IdTrader == t.Id).ToList());
            return Ok(_context.Traders);
        }

        [HttpGet("{id}")]
        public IActionResult GetTraderById(int id)
        {
            var trader = _context.Traders.FirstOrDefault(t => t.Id == id);
            if ( trader == null)
            {
                return NotFound(new { message = "Obchodník nenalezen." });
            }
            trader.Trades = _context.Trades.Where(t => t.IdTrader == id).ToList();
            return Ok(trader);
        }
    }
}
