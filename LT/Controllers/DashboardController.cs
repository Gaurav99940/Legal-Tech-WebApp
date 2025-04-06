using LT.Data;
using LT.Security;
using LT.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace LT.Controllers
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IConfiguration _configuration;
        public readonly dbContext _context;
        public readonly ICommonService _utilities;

        public DashboardController(ILogger<DashboardController> logger,
            IConfiguration configuration, dbContext context, 
            ICommonService utilities)
        {
            _logger = logger;
            this._configuration = configuration;
            this._context = context;
            _utilities = utilities;
        }

        public IActionResult Home()//comment
        {
            return View();
        }
    }
}
