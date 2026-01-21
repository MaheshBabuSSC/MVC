using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcWebApiSwaggerApp.Models;
using MvcWebApiSwaggerApp.Services;
using System.Security.Claims;
using System.Text.Json;

namespace MvcWebApiSwaggerApp.Controllers
{
    public class KSEB : Controller
    {
        private readonly FormService _formService;
        private readonly AdminService _adminService;
        private readonly AuthService _authService;

        public KSEB(FormService formService, AdminService adminService, AuthService authService)
        {
            _formService = formService;
            _adminService = adminService;
            _authService = authService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            var userId = _authService.ValidateLogin(model.Email, model.Password);

            if (userId == 0)
            {
                ViewBag.Error = "Invalid email or password";
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, model.Email)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity)
            );

            // Store userId in session for easy access
            HttpContext.Session.SetInt32("UserId", userId);

            // Get forms for sidebar
            var forms = _formService.GetForms();

            // ✅ CRITICAL: Use TempData instead of ViewBag (survives redirect)
            TempData["SidebarForms"] = JsonSerializer.Serialize(forms);
            TempData.Keep("SidebarForms"); // Keep it for the next request

            return RedirectToAction("Dashboard", "KSEB");
        }


        public IActionResult Dashboard()
        {
            return View();
        }



        [HttpGet]
        public IActionResult NewUser()
        {
            // Create a model with roles from database
            var model = new Register
            {
                Roles = _authService.GetActiveRoles() // Get roles from database
            };

            return View(model); // Pass model to view
        }

        [HttpPost]
        public IActionResult NewUser(Register model)
        {
            try
            {
                // DIRECT REGISTRATION WITHOUT OTP
                // Register user and get UserId
                model.UserId = _authService.RegisterUserWithoutOtp(model, "MVC");

                // Keep roles populated for the view
                model.Roles = _authService.GetActiveRoles();

                ViewBag.Message = "User created successfully!";
                return View(model);
            }
            catch (Exception ex)
            {
                // If error occurs, keep the form data and show error
                model.Roles = _authService.GetActiveRoles();
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }

        // Other actions remain the same...
        public IActionResult UserList()
        {
            var users = _adminService.GetUsers();
            return View(users);
        }

        public IActionResult FormBuilder()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FormBuilder(CreateFormRequest request)
        {
            try
            {
                _formService.CreateForm(
                    request.FormTitle,
                    request.FormDescription,
                    "MVC-User",
                    request.Fields
                );

                ViewBag.Message = "Form created successfully!";
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View();
        }

        public IActionResult FormsList()
        {
            var forms = _formService.GetForms();
            return View(forms);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to default route → KSEB / Index
            return RedirectToAction("Index", "KSEB");
        }
    }
}