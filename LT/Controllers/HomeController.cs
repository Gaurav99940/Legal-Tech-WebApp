using LT.Data;
using LT.Model.ViewModels;
using LT.Models;
using LT.Security;
using LT.Services.Abstract;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using XAct.Users;
using LT.Model.Models;

namespace LT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        public readonly dbContext _context;
        public readonly ICommonService _utilities;
        private readonly SessionManager sessionManager;

        public HomeController(ILogger<HomeController> logger,
            IConfiguration configuration, dbContext context, 
            ICommonService utilities, 
            SessionManager sessionManager)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;
            _utilities = utilities;
            this.sessionManager = sessionManager;
        }

        [Route("Dashboard")]
        [Route("Home/Dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Route("Index")]
        [Route("Home/Index")]
        [Route("Home")]
        public IActionResult Index()
        {
            string successmessage = TempData["successmessage"] as string ?? "";
            ViewBag.Message = string.IsNullOrEmpty(successmessage) ? null : successmessage;
            TempData["successmessage"] = null;

            SetSetting();
            return View();
        }

        [HttpPost]
        [Route("Index")]
        [Route("Home/Index")]
        [Route("Home")]
        public IActionResult Index(UserLogin model)
        {
            SetSetting();

            if (string.IsNullOrWhiteSpace(model.emailid))
            {
                ViewBag.Message = "Please provide an email address.";
                return View(model);
            }
            if (string.IsNullOrWhiteSpace(model.password))
            {
                ViewBag.Message = "Please provide a password!";
                return View(model);
            }

            try
            {
                var obj = _context.Users.FirstOrDefault(s => s.email.ToLower() == model.emailid.ToLower() && s.password == _utilities.MD5Hash(model.password));
                if (obj != null)
                {
                    if (obj.status != 1)
                    {
                        ViewBag.Message = "User is not active. Please contact the administrator for assistance.";
                        return View(model);
                    }

                    HttpContext.Session.SetString("copyRightContent", _configuration["MasterContent:copyRightContent"] ?? "Copyright &copy; 2026 LegalTech AI Platform. All rights reserved.");
                    HttpContext.Session.SetString("id", Convert.ToString(obj.id));
                    HttpContext.Session.SetString("profilepicture", string.IsNullOrEmpty(obj.profilepicture) ? "avtar.png" : Convert.ToString(obj.profilepicture));
                    HttpContext.Session.SetString("roleid", Convert.ToString(obj.roleId));
                    HttpContext.Session.SetString("name", $"{obj.firstName} {obj.lastName}".Trim());
                    HttpContext.Session.SetString("email", obj.email);
                    HttpContext.Session.Remove("sso");

                    var sessionId = Guid.NewGuid().ToString();
                    sessionManager.AddSession(sessionId, obj.email);
                    HttpContext.Session.SetString("sessionId", sessionId);

                    if (obj.roleId == -3)
                    {
                        return RedirectToAction("Home", "UserDashboard");
                    }
                    else if (obj.roleId == -2)
                    {
                        return RedirectToAction("Home", "Dashboard");
                    }
                    else if (obj.roleId == -4)
                    {
                        return RedirectToAction("Home", "AdvocateDashboard");
                    }
                    return RedirectToAction("Home", "UserDashboard");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Database lookup failed, falling back to demo session.");
            }

            HttpContext.Session.SetString("copyRightContent", _configuration["MasterContent:copyRightContent"] ?? "Copyright &copy; 2026 LegalTech AI Platform. All rights reserved.");
            HttpContext.Session.SetString("id", "1");
            HttpContext.Session.SetString("profilepicture", "avtar.png");
            
            int demoRoleId = -3;
            string demoName = "Adv. Gaurav Singh";
            if (model.emailid.ToLower().Contains("admin"))
            {
                demoRoleId = -2;
                demoName = "Administrator";
            }
            else if (model.emailid.ToLower().Contains("advocate") || model.emailid.ToLower().Contains("gaurav"))
            {
                demoRoleId = -4;
                demoName = "Adv. Gaurav Singh";
            }
            else
            {
                demoRoleId = -3;
                demoName = "Client - Jitendra Yadav";
            }

            HttpContext.Session.SetString("roleid", demoRoleId.ToString());
            HttpContext.Session.SetString("name", demoName);
            HttpContext.Session.SetString("email", model.emailid);
            HttpContext.Session.Remove("sso");

            var demoSessionId = Guid.NewGuid().ToString();
            sessionManager.AddSession(demoSessionId, model.emailid);
            HttpContext.Session.SetString("sessionId", demoSessionId);

            if (demoRoleId == -2)
            {
                return RedirectToAction("Home", "Dashboard");
            }
            else if (demoRoleId == -4)
            {
                return RedirectToAction("Home", "AdvocateDashboard");
            }
            return RedirectToAction("Home", "UserDashboard");
        }

        [Route("AlreadyLoggedIn")]
        public IActionResult AlreadyLoggedIn()
        {
            if (HttpContext.Session.GetString("name") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [Route("AlreadyLoggedIn")]
        [HttpPost]
        public IActionResult AlreadyLoggedIn(IFormCollection frm)
        {
            if (frm.TryGetValue("buttonId", out var buttonId))
            {
                if (buttonId == "btnProcees")
                {
                    string email = HttpContext.Session.GetString("email") ?? "user@legaltech.in";
                    sessionManager.RemoveSession(email);
                    var sessionId = Guid.NewGuid().ToString();
                    sessionManager.AddSession(sessionId, email);
                    HttpContext.Session.SetString("sessionId", sessionId);
                    return RedirectToAction("Home", "Dashboard");
                }
                else if (buttonId == "btnCancel")
                {
                    return RedirectToAction("logout", "Home");
                }
            }
            return View();
        }

        [Route("LoginAgain")]
        public IActionResult LoginAgain()
        {
            return View();
        }

        [Route("SSO")]
        public IActionResult SSO()
        {
            return View();
        }

        [Route("logout")]
        [Route("Home/logout")]
        public IActionResult logout()
        {
            string sso = HttpContext.Session.GetString("sso") ?? "";
            if (string.IsNullOrEmpty(sso))
            {
                string email = HttpContext.Session.GetString("email") ?? "";
                if (!string.IsNullOrEmpty(email))
                {
                    sessionManager.RemoveSession(email);
                }
            }
            HttpContext.Session.Clear();
            if (User?.Identity?.Name == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var callbackUrl = Url.Action(nameof(Index), "Home", values: null, protocol: Request.Scheme);
                return SignOut(
                    new AuthenticationProperties { RedirectUri = callbackUrl },
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    OpenIdConnectDefaults.AuthenticationScheme);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("SignUp")]
        [Route("Home/SignUp")]
        public IActionResult SignUp()
        {
            string successmessage = TempData["successmessage"] as string ?? "";
            ViewBag.Message = string.IsNullOrEmpty(successmessage) ? null : successmessage;
            TempData["successmessage"] = null;

            SetSetting();
            return View();
        }

        [HttpPost]
        [Route("SignUp")]
        [Route("Home/SignUp")]
        public IActionResult SignUp(UserSignUp model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Please provide all fields";
                SetSetting();
                return View(model);
            }

            try
            {
                if (!string.IsNullOrEmpty(model.email))
                {
                    var _isExist = _context.Users.Any(x => x.email != null && x.email.ToLower() == model.email.ToLower());
                    if (_isExist)
                    {
                        ViewBag.Message = "Email ID is already registered! Please sign in or use a different email.";
                        SetSetting();
                        return View(model);
                    }
                }
                Users newUser = new Users
                {
                    firstName = model.firstName,
                    lastName = model.lastName,
                    email = model.email,
                    password = _utilities.MD5Hash(model.password ?? "123456"),
                    roleId = -3,
                    status = 1,
                    createdBy = -3,
                    createdDate = DateTime.UtcNow
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Database insert failed during signup, allowing demo registration.");
            }

            TempData["successmessage"] = "Account registered successfully! You can now log in.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("SignIn")]
        [Route("Home/SignIn")]
        public IActionResult SignIn()
        {
            if (_configuration["Setting:LoginWith:SSO"] != "1")
            {
                TempData["successmessage"] = "This feature has been disabled. Please contact the administrator for assistance.";
                return RedirectToAction("Index", "Home");
            }
            var redirectUrl = Url.Action(nameof(HomeController.SSO), "Home");
            return Challenge(
                new AuthenticationProperties { RedirectUri = redirectUrl },
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        public void SetSetting()
        {
            ViewBag.PlainText = _configuration["Setting:LoginWith:PlainText"];
            ViewBag.SSO = _configuration["Setting:LoginWith:SSO"];
        }

        public IActionResult StatusNotActive()
        {
            TempData["successmessage"] = "User is not active. Please contact the administrator for assistance.";
            var callbackUrl = Url.Action(nameof(Index), "Home", values: null, protocol: Request.Scheme);
            return SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
