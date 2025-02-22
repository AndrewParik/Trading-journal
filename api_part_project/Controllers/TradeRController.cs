using api_part_project.Class;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;

namespace api_part_project.Controllers
{
    [Route("api/trader")]
    [ApiController]
    [EnableCors("AllowAll")]
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

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetTraderById(int id)
        {
            var trader = await _context.Traders.FirstOrDefaultAsync(t => t.Id == id);
            if (trader == null)
            {
                return NotFound(new { message = "Obchodník nenalezen." });
            } else
            {
                trader.Trades = await _context.Trades.Where(t => t.IdTrader == id).ToListAsync();
                return Ok(trader);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> GetTraderLogIn([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.UserName) || string.IsNullOrWhiteSpace(login.PassWord))
            {
                return BadRequest(new { msg = "Údaje pro přihlášení nesmí být prázdné!" });
            }
            else
            {
                var tr = await _context.Traders.FirstOrDefaultAsync(t => t.FullName == login.UserName && t.PassWord == login.PassWord);
                if (tr is null)
                {
                    return BadRequest(new { msg = "Špatné údaje pro přihlášení" });
                } else
                {
                    tr!.Trades = await _context.Trades.Where(t => t.IdTrader == tr.Id).ToListAsync();
                    return Ok(tr);
                }
            }
        }
    }
}
