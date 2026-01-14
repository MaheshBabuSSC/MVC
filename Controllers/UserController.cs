using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcWebApiSwaggerApp.Models;
using MvcWebApiSwaggerApp.Services;
using System.Security.Claims;
using System.Text.Json; // Add this

namespace MvcWebApiSwaggerApp.Controllers
{
    public class UserController : Controller
    {
        private readonly AuthService _authService;
        private readonly FormService _formService;

        public UserController(AuthService authService, FormService formService)
        {
            _authService = authService;
            _formService = formService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Views/User/Login.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
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

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}