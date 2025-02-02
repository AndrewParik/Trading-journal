using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace api_part_project.Controllers
{
    [Route("api/trades")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private static readonly List<Trade> trades = new()
        {
            new Trade(1, "Bitcoin", 5000, DateTime.UtcNow) { IdTrader = 1 },
            new Trade(2, "Ethereum", 2000, DateTime.UtcNow) { IdTrader = 1 },
            new Trade(3, "Dogecoin", 300, DateTime.UtcNow) { IdTrader = 2 }
        };

        // 🔹 Získání všech obchodů
        [HttpGet]
        public IActionResult GetAllTrades()
        {
            return Ok(trades);
        }

        // 🔹 Získání detailu obchodu podle ID
        [HttpGet("{id}")]
        public IActionResult GetTradeById(int id)
        {
            var trade = trades.FirstOrDefault(t => t.Id == id);
            if (trade == null)
            {
                return NotFound(new { message = "Obchod nenalezen." });
            }
            return Ok(trade);
        }
    }
}
