using LT.Data;
using LT.Model.Models.UserCaseModels;
using LT.Security;
using LT.Services;
using LT.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IActionResult AddCase(int id = 0, int pageIndex = 1, int pageSize = 8, string searchQuery = "", string filterStatus = "")
        {
            ViewBag.EditMode = "0";
            string successmessage = TempData["successmessage"] as string ?? "";
            ViewBag.successmessage = string.IsNullOrEmpty(successmessage) ? null : successmessage;
            TempData["successmessage"] = null;

            ViewBag.SearchQuery = searchQuery;
            ViewBag.FilterStatus = filterStatus;

            SetMasterData();
            GetCaseList(pageIndex, pageSize, searchQuery, filterStatus);

            if (id != 0)
            {
                var caseItem = CaseDataStore.GetById(id);
                if (caseItem != null)
                {
                    ViewBag.EditMode = "1";
                    return View(caseItem);
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddCase(UserCourtCaseDetails obj)
        {
            if (string.IsNullOrWhiteSpace(obj.CaseTitle))
            {
                ViewBag.errormessage = "Please enter Case Title (e.g., XYZ Enterprises vs ABC Corp).";
            }
            else if (string.IsNullOrWhiteSpace(obj.CaseType))
            {
                ViewBag.errormessage = "Please select or enter Case Classification / Section.";
            }
            else if (string.IsNullOrWhiteSpace(obj.CourtName))
            {
                ViewBag.errormessage = "Please enter Court or Bench Name.";
            }
            else
            {
                try
                {
                    if (obj.pdfFile != null && _configuration["FolderLocation:pdf"] != null)
                    {
                        obj.pdf = _utilities.SaveIamge(_configuration["FolderLocation:pdf"], obj.pdfFile);
                    }

                    int currentUserId = 1;
                    var sessionUserId = HttpContext.Session.GetString("id");
                    if (!string.IsNullOrEmpty(sessionUserId) && int.TryParse(sessionUserId, out int parsedId))
                    {
                        currentUserId = parsedId;
                    }
                    obj.UserID = currentUserId;

                    CaseDataStore.Save(obj);

                    try
                    {
                        if (obj.CaseID == 0)
                        {
                            _context.UserCourtCaseDetails.Add(obj);
                        }
                        else
                        {
                            var existing = _context.UserCourtCaseDetails.FirstOrDefault(x => x.CaseID == obj.CaseID);
                            if (existing != null)
                            {
                                existing.CaseTitle = obj.CaseTitle;
                                existing.CaseType = obj.CaseType;
                                existing.CourtName = obj.CourtName;
                                existing.FilingDate = obj.FilingDate;
                                existing.HearingDate = obj.HearingDate;
                                existing.CaseStatus = obj.CaseStatus;
                                existing.LawyerName = obj.LawyerName;
                                existing.OpponentName = obj.OpponentName;
                                existing.OpponentLawyerName = obj.OpponentLawyerName;
                                existing.CaseDescription = obj.CaseDescription;
                                existing.VerdictDate = obj.VerdictDate;
                                existing.VerdictDetails = obj.VerdictDetails;
                                existing.Remarks = obj.Remarks;
                                existing.ModifiedDate = DateTime.Now;
                                if (!string.IsNullOrEmpty(obj.pdf)) existing.pdf = obj.pdf;
                                _context.UserCourtCaseDetails.Update(existing);
                            }
                        }
                        _context.SaveChanges();
                    }
                    catch (Exception dbEx)
                    {
                        _logger.LogWarning(dbEx, "Database sync unavailable; case persisted in dynamic data store.");
                    }

                    TempData["successmessage"] = obj.CaseID == 0 ? "Case filed and indexed successfully!" : "Case record updated successfully!";
                    return RedirectToAction("AddCase", "UserCase");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving case details");
                    ViewBag.errormessage = "Error while saving case: " + ex.Message;
                }
            }

            SetMasterData();
            GetCaseList(1, 8, "", "");
            return View(obj);
        }

        [HttpPost]
        public IActionResult DeleteCase(int id)
        {
            CaseDataStore.Delete(id);
            try
            {
                var existing = _context.UserCourtCaseDetails.FirstOrDefault(x => x.CaseID == id);
                if (existing != null)
                {
                    _context.UserCourtCaseDetails.Remove(existing);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "DB delete fallback to in-memory store.");
            }

            TempData["successmessage"] = "Case removed successfully from active registry.";
            return RedirectToAction("AddCase", "UserCase");
        }

        [HttpGet]
        public IActionResult SuggestAdvocate(string query)
        {
            var suggestions = CaseDataStore.GetSuggestedAdvocates(query);
            return Json(suggestions);
        }

        [HttpGet]
        public IActionResult HearingCalendar()
        {
            var cases = CaseDataStore.GetAll();
            var upcomingHearings = cases
                .Where(c => c.HearingDate.HasValue)
                .OrderBy(c => c.HearingDate)
                .ToList();

            return View(upcomingHearings);
        }

        [HttpGet]
        public IActionResult DocumentVault()
        {
            var cases = CaseDataStore.GetAll();
            return View(cases);
        }

        [HttpGet]
        public IActionResult LegalDrafts()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AdvocateBooking()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SearchLiveAdvocates(string query = "", string specialization = "", string court = "")
        {
            var results = CaseDataStore.SearchAdvocates(query, specialization, court);
            return Json(results);
        }

        public void GetCaseList(int pageIndex, int pageSize, string searchQuery = "", string filterStatus = "")
        {
            var allCases = CaseDataStore.GetAll();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                allCases = allCases.Where(c => 
                    (c.CaseTitle != null && c.CaseTitle.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)) ||
                    (c.CourtName != null && c.CourtName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)) ||
                    (c.LawyerName != null && c.LawyerName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)) ||
                    (c.CaseType != null && c.CaseType.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            if (!string.IsNullOrWhiteSpace(filterStatus))
            {
                allCases = allCases.Where(c => c.CaseStatus != null && c.CaseStatus.Equals(filterStatus, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var totalItems = allCases.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if (totalPages < 1) totalPages = 1;

            ViewBag.caseList = allCases.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = pageIndex;
        }

        private void SetMasterData()
        {
            try
            {
                ViewBag.roleId = _context.Roles.AsNoTracking().Select(r => new { id = r.id, name = r.name }).Where(s => s.id == -2).ToList();
            }
            catch
            {
                ViewBag.roleId = new List<object>();
            }
        }
    }
}
