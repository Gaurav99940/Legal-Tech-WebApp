using LT.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

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

            if (context.HttpContext.User.Identity.Name != null)
            {

                var _objUser = _context.Users.AsNoTracking().Where(s => s.email.ToLower() == context.HttpContext.User.Identity.Name.ToLower() && s.status == 1).FirstOrDefault();

                if (_objUser != null)
                {
                    if (session.GetString("id") == null)
                    {
                        _objUser.lastlogin = System.DateTime.Now;
                        _context.Users.Update(_objUser);
                        _context.SaveChanges();
                    }
                    session.SetString("copyRightContent", _configuration["MasterContent:copyRightContent"]);
                    session.SetString("id", Convert.ToString(_objUser.id));
                    if (_objUser.profilepicture == null)
                    {
                        session.SetString("profilepicture", "avtar.png");
                    }
                    else
                    {
                        session.SetString("profilepicture", Convert.ToString(_objUser.profilepicture));
                    }
                    session.SetString("roleid", Convert.ToString(_objUser.roleId));
                    session.SetString("name", Convert.ToString(_objUser.firstName) + " " + Convert.ToString(_objUser.lastName));

                    string sController = Convert.ToString(context.RouteData.Values["controller"]);
                    string sAction = Convert.ToString(context.RouteData.Values["action"]);
                    if (session != null && sController != null && sAction != null)
                    {

                        sController = sController.ToLower();
                        sAction = sAction.ToLower();
                        var sessionuser = session.GetString("id");
                        if (sessionuser != null)
                        {
                            int iUserId = Convert.ToInt32(sessionuser);
                            int iRoleId = Convert.ToInt32(session.GetString("roleid"));

                            session.SetString("sso", "1");

                            int icheck = 0;
                            string sMenu = GetMenuByRoleID(iRoleId, sAction, sController, out icheck);
                            if (icheck == 0)
                            {
                                context.Result = new RedirectToActionResult("Index", "Home", null);
                            }
                            else
                            {
                                session.SetString("menuitem", sMenu);
                                await next();
                            }
                        }
                        else
                        {
                            context.Result = new RedirectToActionResult("Index", "Home", null);
                        }
                    }
                    else
                    {
                        context.Result = new RedirectToActionResult("Index", "Home", null);
                    }
                }
                else
                {
                    context.Result = new RedirectToActionResult("StatusNotActive", "Home", null);
                }
            }
            else
            {
                string sController = Convert.ToString(context.RouteData.Values["controller"]);
                string sAction = Convert.ToString(context.RouteData.Values["action"]);

                if (session != null && sController != null && sAction != null)
                {

                    sController = sController.ToLower();
                    sAction = sAction.ToLower();
                    var sessionuser = session.GetString("id");
                    if (sessionuser != null)
                    {
                        int iUserId = Convert.ToInt32(sessionuser);
                        int iRoleId = Convert.ToInt32(session.GetString("roleid"));
                        int icheck = 0;
                        string sMenu = GetMenuByRoleID(iRoleId, sAction, sController, out icheck);
                        if (icheck == 0)
                        {
                            context.Result = new RedirectToActionResult("Index", "Home", null);
                        }
                        else
                        {
                            session.SetString("menuitem", sMenu);
                            await next();
                        }
                    }
                    else
                    {
                        context.Result = new RedirectToActionResult("Index", "Home", null);
                    }
                }
                else
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                }
            }
        }

        public string GetMenuByRoleID(int roleid, string sAction, string sController, out int iCheck)
        {
            iCheck = 0;
            string sMenu = "<ul class=\"navigation navigation-main\" id=\"main-menu-navigation\" data-menu=\"menu-navigation\">";
            var menu = (from menuVal in _context.Menu
                        join mapping in _context.RoleMenuMapping on menuVal.id equals mapping.menuid
                        where menuVal.status == 1 && mapping.status == 1 && mapping.roleid == roleid
                        select menuVal).Distinct().ToList();


            if (menu != null)
            {
                var menuCheck = menu.Where(s => s.action.ToLower() == sAction.ToLower() && s.controller.ToLower() == sController.ToLower()).Any();
                if (menuCheck)
                {
                    iCheck = 1;
                }
                menu = menu.Where(s => s.internalstatus == 0).OrderBy(s => s.order).ToList();
                foreach (var item in menu)
                {
                    if (item.parentid == null)
                    {

                        var subMenu = menu.Where(s => s.parentid == item.id).ToList();
                        if (subMenu.Count > 0)
                        {
                            sMenu += "<li class=\"nav-item\">";
                            sMenu += "<a href='#'><i class='" + item.icon + "'></i><span class='menu-title'>" + item.name + "</span></a>";
                        }
                        else
                        {
                            if (sAction == Convert.ToString(item.action).ToLower() && sController == Convert.ToString(item.controller).ToLower())
                            {
                                sMenu += "<li class='active'>";
                            }
                            else
                            {
                                sMenu += "<li class=''>";
                            }
                            sMenu += "<a href='/" + item.controller + "/" + item.action + "'><i class='" + item.icon + "'></i><span class='menu-title'>" + item.name + "</span></a>";
                        }
                        if (subMenu.Count > 0)
                        {
                            sMenu += "<ul class='menu-content'>";
                            foreach (var subitem in subMenu)
                            {
                                if (sAction == Convert.ToString(subitem.action).ToLower() && sController == Convert.ToString(subitem.controller).ToLower())
                                {
                                    sMenu += "<li class='active'>";
                                }
                                else
                                {
                                    sMenu += "<li>";
                                }
                                sMenu += "<a class='menu-item' href='/" + subitem.controller + "/" + subitem.action + "'>" + subitem.name + "</a>";
                                sMenu += "</li>";
                            }
                            sMenu += "</ul>";
                        }
                        sMenu += "</li>";
                    }
                }
            }
            sMenu += "</ul>";
            return sMenu;
        }
    }
}
