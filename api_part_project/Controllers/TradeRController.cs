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

        [HttpGet("/trades")]
        public async Task<IActionResult> GetHisTrades([FromBody] int id)
        {
            var tr = await _context.Traders.FirstOrDefaultAsync(t => t.Id == id);
            tr!.Trades = await _context.Trades.Where(t => t.IdTrader == id).ToListAsync();
            return Ok(tr);
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
        public async Task<IActionResult> EditTrader([FromBody] dynamic data)
        {
            if (data is null || data.Id is null || string.IsNullOrWhiteSpace(data.FirstName) || string.IsNullOrWhiteSpace(data!.LastName) || string.IsNullOrWhiteSpace(data!.PassWord))
            {
                return BadRequest(new { msg = "Údaje sou empty" });
            } else
            {
                int id = data!.Id;
                var tr = await _context.Traders.FirstOrDefaultAsync(t => t.Id == id);
                tr!.FirstName = data.FirstName; tr!.LastName = data.LastName; tr!.PassWord = data.PassWord;
                await _context.SaveChangesAsync();
                return Ok(new { msg = "Data aktualizována." });
            }
        }
    }
}
