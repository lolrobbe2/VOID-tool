using Microsoft.AspNetCore.Mvc;

namespace FoxholeBot.src.Discord
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
