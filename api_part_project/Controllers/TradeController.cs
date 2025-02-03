using api_part_project.Class;
using Microsoft.AspNetCore.Mvc;

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
    }
}

