using Microsoft.AspNetCore.Mvc;

namespace LT.Controllers
{
    public class HeroController : Controller
    {
        [Route("~/")]
        [Route("Hero")]
        [Route("Hero/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("AskChatBot")]
        [Route("Hero/AskChatBot")]
        public IActionResult AskChatBot()
        {
            return View();
        }

        [HttpGet]
        [Route("BookAdvocate")]
        [Route("Hero/BookAdvocate")]
        [Route("booking")]
        public IActionResult BookAdvocate()
        {
            return View();
        }

        [HttpGet]
        [Route("Features")]
        [Route("Hero/Features")]
        public IActionResult Features()
        {
            return View("Index");
        }

        [HttpGet]
        [Route("Pricing")]
        [Route("Hero/Pricing")]
        public IActionResult Pricing()
        {
            return View();
        }

        [HttpGet]
        [Route("FAQ")]
        [Route("Hero/FAQ")]
        public IActionResult FAQ()
        {
            return View();
        }

        [HttpGet]
        [Route("About")]
        [Route("Hero/About")]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        [Route("SearchLiveAdvocates")]
        [Route("Hero/SearchLiveAdvocates")]
        public IActionResult SearchLiveAdvocates(string query = "", string specialization = "", string court = "")
        {
            var advocates = LT.Services.CaseDataStore.SearchAdvocates(query, specialization, court);
            return Json(advocates);
        }
    }
}
