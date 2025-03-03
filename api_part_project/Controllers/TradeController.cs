using api_part_project.Class;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace api_part_project.Controllers
{
    [Route("api/trade")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TradeController()
        {
            _context = new AppDbContext();
        }

        [HttpGet]
        public IActionResult GetAllTrades()
        {
            return Ok(_context.Trades.ToList());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTradeById(int id)
        {
            var te = await _context.Trades.FirstOrDefaultAsync(t => t.Id == id);
            if (te == null)
            {
                return NotFound(new { message = "Obchod nenalezen." });
            }
            return Ok(te);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddTrade([FromBody] Trade te)
        {
            _context.Trades.Add(te);
            var tr = await _context.Traders.FindAsync(te.IdTrader);
            await _context.SaveChangesAsync();

            return Ok(tr!.Trades.ToList());
        }
        [HttpDelete("del/{id}")]
        public async Task<IActionResult> RemoveTrade(int id)
        {
            var te = await _context.Trades.FindAsync(id);
            if (te == null) return NotFound(new {msg = "Obchod neexistuje!"});
            var tr = await _context.Traders.FirstOrDefaultAsync(tr => tr.Id == te.IdTrader);

            _context.Trades.Remove(te);
            await _context.SaveChangesAsync();

            return Ok(tr!.Trades.ToList());
        }
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditTrade([FromBody] TradeEditDto dto, int id)
        {
            var te = await _context.Trades.FirstOrDefaultAsync(t => t.Id == id);
            if (te == null) return NotFound(new { m = "Obchod neexistuje!" });
            var tr = await _context.Traders.FirstOrDefaultAsync(tr => tr.Id == te!.IdTrader);
            if(string.IsNullOrWhiteSpace(dto.CoinType)) te!.CoinType = dto.CoinType;
            dto.DateCreated = te!.DateCreated; dto.Worth = te!.Worth;
            await _context.SaveChangesAsync(); return Ok(te);
        }
    }
}

