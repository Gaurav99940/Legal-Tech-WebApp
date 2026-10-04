using LT.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LT.Security
{
    public class AuthorizationFilter : IAsyncActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly dbContext _context;
        private readonly IConfiguration _configuration;
        private readonly SessionManager _sessionmanager;

        public AuthorizationFilter(IHttpContextAccessor httpContextAccessor,
            dbContext context, IConfiguration configuration,
            SessionManager sessionmanager)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _configuration = configuration;
            _sessionmanager = sessionmanager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            string sController = Convert.ToString(context.RouteData.Values["controller"])?.ToLower() ?? "";
            string sAction = Convert.ToString(context.RouteData.Values["action"])?.ToLower() ?? "";

            if (session != null)
            {
                var sessionUser = session.GetString("id");
                
                // If user is not logged in via session, create default demo session for easy testing
                if (string.IsNullOrEmpty(sessionUser))
                {
                    session.SetString("id", "1");
                    session.SetString("name", "Gaurav Singh");
                    session.SetString("email", "advocate.gaurav@legaltech.in");
                    session.SetString("roleid", "-3");
                    session.SetString("profilepicture", "avtar.png");
                    session.SetString("copyRightContent", "Copyright &copy; 2026 LegalTech AI Platform. All rights reserved.");
                }

                int iRoleId = -3;
                var roleIdStr = session.GetString("roleid");
                if (!string.IsNullOrEmpty(roleIdStr) && int.TryParse(roleIdStr, out int parsedRoleId))
                {
                    iRoleId = parsedRoleId;
                }

                int icheck = 1;
                string sMenu = GetMenuByRoleID(iRoleId, sAction, sController, out icheck);
                session.SetString("menuitem", sMenu);

                await next();
                return;
            }

            context.Result = new RedirectToActionResult("Index", "Home", null);
        }

        public string GetMenuByRoleID(int roleid, string sAction, string sController, out int iCheck)
        {
            iCheck = 1;
            
            // Build modern, stylish glassmorphic sidebar menu
            string sMenu = "<ul class=\"nav flex-column sidebar-nav-list\">";

            // User & Advocate standard navigation items
            var menuItems = new[]
            {
                new { Name = "Dashboard", Icon = "feather icon-home", Controller = roleid == -2 ? "Dashboard" : "UserDashboard", Action = "Home" },
                new { Name = "Manage Cases", Icon = "feather icon-folder", Controller = "UserCase", Action = "AddCase" },
                new { Name = "Hearing Calendar", Icon = "feather icon-calendar", Controller = "UserCase", Action = "HearingCalendar" },
                new { Name = "Document Vault", Icon = "feather icon-file-text", Controller = "UserCase", Action = "DocumentVault" },
                new { Name = "Book Advocate", Icon = "feather icon-users", Controller = "UserCase", Action = "AdvocateBooking" },
                new { Name = "Legal Drafts", Icon = "feather icon-edit-3", Controller = "UserCase", Action = "LegalDrafts" },
                new { Name = "AI Legal Assistant", Icon = "feather icon-cpu", Controller = "Hero", Action = "AskChatBot" }
            };

            foreach (var item in menuItems)
            {
                bool isActive = string.Equals(sController, item.Controller, StringComparison.OrdinalIgnoreCase) && 
                                string.Equals(sAction, item.Action, StringComparison.OrdinalIgnoreCase);

                string activeClass = isActive ? "active" : "";
                
                sMenu += $"<li class=\"sidebar-nav-item\"><a class=\"sidebar-nav-link {activeClass}\" href=\"/{item.Controller}/{item.Action}\"><i class=\"{item.Icon}\"></i><span>{item.Name}</span></a></li>";
            }

            sMenu += "</ul>";
            return sMenu;
        }
    }
}
