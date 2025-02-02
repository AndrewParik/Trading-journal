using Microsoft.AspNetCore.Mvc;

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
