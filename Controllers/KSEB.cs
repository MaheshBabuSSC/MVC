using Microsoft.AspNetCore.Mvc;
using MvcWebApiSwaggerApp.Models;
using MvcWebApiSwaggerApp.Services;

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

        [HttpGet]
        public IActionResult NewUser()
        {
            // Create a model with roles from database
            var model = new Register
            {
                IsOtpSent = 0, // Default to registration step
                Roles = _authService.GetActiveRoles() // Get roles from database
            };

            return View(model); // Pass model to view
        }

        [HttpPost]
        public IActionResult NewUser(Register model)
        {
            // STEP 1: Register + send OTP
            if (model.IsOtpSent == 0)
            {
                // Register user and get UserId
                model.UserId = _authService.RegisterUser(model, "MVC");
                model.IsOtpSent = 1; // Move to OTP verification step

                // Keep roles populated for the view
                model.Roles = _authService.GetActiveRoles();

                ViewBag.Message = "OTP sent to your email";
                return View(model);
            }

            // STEP 2: Verify OTP
            var isValid = _authService.VerifyOtp(model.UserId, model.OtpCode);

            if (!isValid)
            {
                // Invalid OTP - stay on OTP page
                model.Roles = _authService.GetActiveRoles();
                ViewBag.Error = "Invalid or expired OTP";
                return View(model);
            }

            // ✅ SUCCESS - User verified
            // Reset form for new registration
            var newModel = new Register
            {
                Roles = _authService.GetActiveRoles()
            };

            ViewBag.Message = "User registered and verified successfully";
            return View(newModel);
        }

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
    }
}