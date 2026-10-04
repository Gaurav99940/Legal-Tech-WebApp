using LT.Data;
using LT.Model.Models.UserCaseModels;
using LT.Security;
using LT.Services;
using LT.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.Controllers
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class UserDashboardController : Controller
    {
        private readonly ILogger<UserDashboardController> _logger;
        private readonly IConfiguration _configuration;
        public readonly dbContext _context;
        public readonly ICommonService _utilities;

        public UserDashboardController(ILogger<UserDashboardController> logger, 
            IConfiguration configuration, dbContext context, 
            ICommonService utilities)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;
            _utilities = utilities;
        }

        public IActionResult Home()
        {
            var cases = CaseDataStore.GetAll();
            
            ViewBag.TotalCases = cases.Count;
            ViewBag.ActiveCases = cases.Count(c => c.CaseStatus != "Disposed" && c.CaseStatus != "Closed");
            ViewBag.UpcomingHearings = cases.Count(c => c.HearingDate.HasValue && c.HearingDate.Value >= DateTime.Today);
            ViewBag.DisposedCases = cases.Count(c => c.CaseStatus == "Disposed" || c.CaseStatus == "Closed");
            ViewBag.TotalDocuments = cases.Count * 3 + 4;

            ViewBag.NextHearing = cases
                .Where(c => c.HearingDate.HasValue && c.HearingDate.Value >= DateTime.Today)
                .OrderBy(c => c.HearingDate)
                .FirstOrDefault();

            ViewBag.RecentCases = cases.OrderByDescending(c => c.FilingDate).Take(4).ToList();
            ViewBag.UpcomingList = cases.Where(c => c.HearingDate.HasValue).OrderBy(c => c.HearingDate).Take(3).ToList();

            return View(cases);
        }
    }
}
