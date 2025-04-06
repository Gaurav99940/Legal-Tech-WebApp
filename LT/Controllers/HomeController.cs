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
        public IActionResult Dashboard()
        {
            return View();
        }

        
        [Route("Index")]
        public IActionResult Index()
        {
            string successmessage = TempData["successmessage"] as string;
            ViewBag.Message = successmessage;
            TempData["successmessage"] = null;

            SetSetting();
            //SignIn();
            //return RedirectToAction("SignIn", "Home");
            return View();
        }


        [HttpPost]
        [Route("Index")]
        public IActionResult Index(UserLogin model)
        {
            SetSetting();
            if (_configuration["Setting:LoginWith:PlainText"] != "1")
            {
                ViewBag.Message = "This feature has been disabled. Please contact the administrator for assistance.";
                return View(model);
            }

            if (model.emailid == "" || model.emailid == null)
            {
                ViewBag.Message = "Please provide an email address.";
                return View(model);
            }
            else if (model.password == "" || model.password == null)
            {
                ViewBag.Message = "Please provide a password!";
                return View(model);
            }
            else
            {
                var obj = _context.Users.Where(s => s.email.ToLower() == model.emailid.ToLower() && s.password == _utilities.MD5Hash(model.password)).FirstOrDefault();
                if (obj != null)
                {
                    if (obj.status != 1)
                    {
                        ViewBag.Message = "User is not active. Please contact the administrator for assistance.";
                        return View(model);
                    }

                    //obj.lastlogin = System.DateTime.Now;
                    //_context.Users.Update(obj);
                    //_context.SaveChanges();

                    HttpContext.Session.SetString("copyRightContent", _configuration["MasterContent:copyRightContent"]);
                    HttpContext.Session.SetString("id", Convert.ToString(obj.id));
                    if (obj.profilepicture == null)
                    {
                        HttpContext.Session.SetString("profilepicture", "avtar.png");
                    }
                    else
                    {
                        HttpContext.Session.SetString("profilepicture", Convert.ToString(obj.profilepicture));
                    }
                    HttpContext.Session.SetString("roleid", Convert.ToString(obj.roleId));
                    HttpContext.Session.SetString("name", Convert.ToString(obj.firstName) + " " + Convert.ToString(obj.lastName));
                    HttpContext.Session.SetString("email", obj.email);
                    HttpContext.Session.Remove("sso");

                  

                    var sessionId = Guid.NewGuid().ToString();
                    sessionManager.AddSession(sessionId, obj.email);
                    HttpContext.Session.SetString("sessionId", sessionId);
                    if(obj.roleId == -3)
                    {
                        return RedirectToAction("Home", "UserDashboard");
                    }
                    else if(obj.roleId == -2)
                    {
                        return RedirectToAction("Home", "Dashboard");
                    }
                    else if(obj.roleId == -4)
                    {
                        return RedirectToAction("Home", "AdvocateDashboard");
                    }
                    return  RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Email Id and/or Password is not correct.";
                    return View(model);
                }
            }
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
                    sessionManager.RemoveSession(HttpContext.Session.GetString("email"));
                    var sessionId = Guid.NewGuid().ToString();
                    sessionManager.AddSession(sessionId, HttpContext.Session.GetString("email"));
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
        public IActionResult logout()
        {
            string sso = HttpContext.Session.GetString("sso");
            if (sso == null)
            {
                sessionManager.RemoveSession(HttpContext.Session.GetString("email"));
            }
            HttpContext.Session.Clear();
            if (User.Identity.Name == null)
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
        public IActionResult SignUp()
        {
            string successmessage = TempData["successmessage"] as string;
            ViewBag.Message = successmessage;
            TempData["successmessage"] = null;

            SetSetting();
            //SignIn();
            //return RedirectToAction("SignIn", "Home");
            return View();
        }

        [HttpPost]
        [Route("SignUp")]
        public IActionResult SignUp(UserSignUp model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Please provide all fields";
                SetSetting();
                return View(model);
            }
            var _isExist = _context.Users.Where(x => x.email == model.email).Any();
            if (_isExist)
            {
                ViewBag.Message = "Emaild ID is already Registered! Use Different Email ID";
                SetSetting();
                return View(model);
            }
            Users newUser = new Users
            {
                firstName = model.firstName,
                lastName = model.lastName,
                email = model.email,
                password = _utilities.MD5Hash(model.password!),
                roleId = -3,
                status = 1,
                createdBy = -3,
                createdDate = DateTime.UtcNow
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            ViewBag.Message = "Register Successfully!";
            RedirectToAction("Index");
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Route("SignIn")]
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
