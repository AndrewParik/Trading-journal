using api_part_project.Class;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace api_part_project.Controllers
{
    [Route("api/trader")]
    [ApiController]
    public class TraderController : Controller
    {
        private readonly AppDbContext _context;

        public TraderController()
        {
            _context = new AppDbContext();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTraders()
        {
            var traders = await _context.Traders.Include(t => t.Trades).ToListAsync();
            return Ok(traders);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTraderById(int id)
        {
            var trader = await _context.Traders.Include(t => t.Trades).FirstOrDefaultAsync(t => t.Id == id);

            if (trader == null)
            {
                return NotFound(new { message = "Obchodník nenalezen." });
            } else
            {
                return Ok(trader);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> GetTraderLogIn([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.UserName) || string.IsNullOrWhiteSpace(login.PassWord))
            {
                return BadRequest(new { msg = "Vyplňte všechny pole!" });
            }
            else
            {
                var tr = await _context.Traders.FirstOrDefaultAsync(t => t.FullName == login.UserName && t.PassWord == login.PassWord);
                if (tr is null)
                {
                    return BadRequest(new { msg = "Špatné přihlašovací údaje!" });
                } else
                {
                    tr!.Trades = await _context.Trades.Where(t => t.IdTrader == tr.Id).ToListAsync();
                    return Ok(tr);
                }
            }
        }
        [HttpPost("new")]
        public async Task<IActionResult> AddTrader([FromBody] Trader trader)
        {
            if (trader is null || string.IsNullOrWhiteSpace(trader.FirstName) || string.IsNullOrWhiteSpace(trader.LastName) || string.IsNullOrWhiteSpace(trader.PassWord))
            {
                return BadRequest(new { msg = "Informace pro vytvoření tradera nejsou kompletní." });
            } else
            {
                _context.Traders.Add(trader); await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetTraderById), new { id = trader.Id } ,trader);
            }
        }
        [HttpPut("edit")]
        public async Task<IActionResult> EditTrader([FromBody] Trader trader)
        {
            var tr = await _context.Traders.FirstOrDefaultAsync(t => t.Id == trader.Id);
            if (trader.FirstName != null) tr!.FirstName = trader.FirstName;
            if (trader.LastName != null) tr!.LastName = trader.LastName;
            if (trader.PassWord != null) tr!.PassWord = trader.PassWord;
            await _context.SaveChangesAsync();
            return Ok(tr);
        }
    }
}
