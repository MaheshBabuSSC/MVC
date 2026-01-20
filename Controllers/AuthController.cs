using Microsoft.AspNetCore.Mvc;
using MvcWebApiSwaggerApp.Models;
using MvcWebApiSwaggerApp.Services;

namespace MvcWebApiSwaggerApp.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new Register
            {
                Roles = _authService.GetActiveRoles()
            };

            return View("~/Views/Admin/Register/Create.cshtml", model);
        }


        [HttpPost]
        public IActionResult Register(Register model)
        {
            // STEP 1: Register + send OTP
            if (model.IsOtpSent == 0)
            {
                model.UserId = _authService.RegisterUser(model, "MVC");
                model.IsOtpSent = 1;

                // ✅ FIX
                model.Roles = _authService.GetActiveRoles();

                ViewBag.Message = "OTP sent to your mobile number";
                return View("~/Views/Admin/Register/Create.cshtml", model);
            }

            // STEP 2: Verify OTP
            var isValid = _authService.VerifyOtp(model.UserId, model.OtpCode);

            if (!isValid)
            {
                // ✅ FIX
                model.Roles = _authService.GetActiveRoles();

                ViewBag.Error = "Invalid or expired OTP";
                return View("~/Views/Admin/Register/Create.cshtml", model);
            }

            // ✅ SUCCESS
            var newModel = new Register
            {
                Roles = _authService.GetActiveRoles()
            };

            ViewBag.Message = "User registered and verified successfully";
            return View("~/Views/Admin/Register/Create.cshtml", newModel);
        }


    }
}
