using Microsoft.AspNetCore.Mvc;

namespace LT.Controllers
{
    public class FeaturesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
