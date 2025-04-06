using Microsoft.AspNetCore.Mvc;

namespace LT.Controllers
{
    public class HeroController : Controller
    {
        [Route("~/")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("AskChatBot")]
        public IActionResult AskChatBot()
        {
            return View(); // Ensure you're calling View method correctly
        }
    }
}
