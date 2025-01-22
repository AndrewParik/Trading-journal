using Microsoft.AspNetCore.Mvc;

namespace api_part_project.Controllers
{
    public class TradersController : Controller
    {
        [HttpGet("/traders")]
        public IActionResult GetAllTraders()
        {
            List<Trade> trades = new();
            Trade t = new(1, "dog", 3, DateTime.Now);
            Trade t1 = new(2, "dog", 3, DateTime.Now);
            Trade t2 = new(3, "dog", 3, DateTime.Now);
            trades.Add(t);
            trades.Add(t1);
            trades.Add(t2);
            return Json(trades);
        }
    }
}
