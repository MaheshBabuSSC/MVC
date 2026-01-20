using Microsoft.AspNetCore.Mvc;
using MvcWebApiSwaggerApp.Models;
using MvcWebApiSwaggerApp.Services; // Add this namespace

namespace MvcWebApiSwaggerApp.Controllers
{
    public class KSEB : Controller
    {
        private readonly FormService _formService;
        private readonly AdminService _adminService; // Add this

        // Update constructor to inject both services
        public KSEB(FormService formService, AdminService adminService)
        {
            _formService = formService;
            _adminService = adminService; // Add this
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewUser()
        {
            return View();
        }

        public IActionResult UserList()
        {
            // Call the AdminService to get users
            var users = _adminService.GetUsers(); // This was missing!
            return View(users); // Pass users to the view
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
                // Add error handling
                ViewBag.Error = ex.Message;
            }

            return View("~/Views/KSEB/FormBuilder.cshtml");
        }

        public IActionResult FormsList()
        {
            var forms = _formService.GetForms();
            return View("~/Views/KSEB/FormsList.cshtml", forms);
        }
    }
}