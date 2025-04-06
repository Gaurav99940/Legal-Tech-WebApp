using Microsoft.AspNetCore.Mvc;

namespace LT.Controllers
{
    public class AdvocateDashboardController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
