using LT.Data;
using LT.Model.Models.UserCaseModels;
using LT.Security;
using LT.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LT.Controllers
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class UserCaseController : Controller
    {
        private readonly ILogger<UserCaseController> _logger;
        private readonly IConfiguration _configuration;
        public readonly dbContext _context;
        public readonly ICommonService _utilities;

        public UserCaseController(ILogger<UserCaseController> logger, 
            IConfiguration configuration, dbContext context,
            ICommonService utilities)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;
            _utilities = utilities;
        }


        public IActionResult AddCase(int id, int pageIndex = 1, int pageSize = 5)
        {
            ViewBag.EditMode = "0";
            string successmessage = TempData["successmessage"] as string;
            ViewBag.successmessage = successmessage;
            TempData["successmessage"] = null;
            SetMasterData();
            GetCaseList(pageIndex, pageSize);
            if (id != 0)
            {
                var _obj = _context.UserCourtCaseDetails.AsNoTracking().Where(x => x.CaseID == id).FirstOrDefault();
                if(_obj != null)
                {
                    ViewBag.EditMode = "1";
                    return View(_obj);
                }
            }
                return View();
        }


        [HttpPost]
        public IActionResult AddCase(UserCourtCaseDetails obj)
        {
            if (obj.CaseTitle == null)
            {
                ViewBag.errormessage = "Please enter Case Title";
            }
            else if (obj.CaseType == null)
            {
                ViewBag.errormessage = "Please enter Case Title";
            }
            else if (obj.CaseStatus == null)
            {
                ViewBag.errormessage = "Please enter Case Title";
            }
            else if (obj.CourtName == null)
            {
                ViewBag.errormessage = "Please enter Case Title";
            }
            else if (obj.FilingDate == null)
            {
                ViewBag.errormessage = "Please enter Case Title";
            }
            else if (obj.HearingDate == null)
            {
                ViewBag.errormessage = "Please enter Case Title";
            }
            else if (obj.CaseStatus == null)
            {
                ViewBag.errormessage = "Please enter Case Title";
            }
            else if (obj.LawyerName == null)
            {
                ViewBag.errormessage = "Please enter Case Title";
            }
            else if (obj.OpponentName == null)
            {
                ViewBag.errormessage = "Please enter Case Title";
            }
            else if (obj.OpponentLawyerName == null)
            {
                ViewBag.errormessage = "Please enter Case Title";
            }
            else if (obj.CaseDescription == null)
            {
                ViewBag.errormessage = "Please enter Case Title";
            }
            else if (obj.VerdictDate == null)
            {
                ViewBag.errormessage = "Please enter Case Title";
            }
            else if (obj.CreatedDate == null)
            {
                ViewBag.errormessage = "Please enter Case Title";
            }
            else if (obj.IsActive == null)
            {
                ViewBag.errormessage = "Please enter Case Title";
            }
            else if (obj.Remarks == null)
            {
                ViewBag.errormessage = "Please enter Case Title";
            }
            else
            {
                obj.pdf = _utilities.SaveIamge(_configuration["FolderLocation:pdf"], obj.pdfFile);

                obj.CreatedDate = System.DateTime.Now;
                obj.UserID = Convert.ToInt32(HttpContext.Session.GetString("id"));
                _context.UserCourtCaseDetails.Add(obj);
                _context.SaveChanges();
                TempData["successmessage"] = "Case Save Successfully";
                return RedirectToAction("AddCase", "UserCase");
            }
            return View();
        }
        public void GetCaseList(int pageIndex, int pageSize)
        {
            var result = _context.UserCourtCaseDetails.ToList();

            ViewBag.caseList = result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var totalItems = result.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = pageIndex;
        }
        void SetMasterData()
        {
            ViewBag.roleId = _context.Roles.AsNoTracking().Select(r => new { id = r.id, name = r.name }).Where(s => s.id == -2).ToList();
        }
    }
}
