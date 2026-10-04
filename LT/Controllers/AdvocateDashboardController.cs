using Microsoft.AspNetCore.Mvc;
using LT.Services;

namespace LT.Controllers
{
    public class AdvocateDashboardController : Controller
    {
        public IActionResult Home()
        {
            ViewBag.ActiveBriefs = 18;
            ViewBag.TodayAppearances = 4;
            ViewBag.PendingConsultations = 6;
            ViewBag.MonthlyRetainers = "₹8,45,000";
            ViewBag.AdvocateName = "Senior Adv. Priya Sharma";
            ViewBag.BarNo = "D/1842/2012";
            ViewBag.Court = "High Court of Delhi & Supreme Court";
            ViewBag.AdvocatesList = CaseDataStore.GetAllAdvocates();
            ViewBag.RecentCases = CaseDataStore.GetAllCases();

            return View();
        }
    }
}
