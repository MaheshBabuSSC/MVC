using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using MvcWebApiSwaggerApp.Models;
using MvcWebApiSwaggerApp.Services;
using System.Linq;

namespace MvcWebApiSwaggerApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly FormService _formService;
        private readonly AdminService _adminService;

        public DashboardController(FormService formService,AdminService adminService )
        {
            _formService = formService;
            _adminService = adminService;
        }

        // GET: /Dashboard/Index
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "User");

            var forms = _formService.GetForms();

            // ✅ CRITICAL: Pass forms to Layout via ViewBag
            ViewBag.SidebarForms = forms;
            // OR use ViewData:
            // ViewData["SidebarForms"] = forms;

            return View("~/Views/User/Dashboard.cshtml", forms);
        }

        // GET: /Dashboard/Submit/{id}
        [HttpGet]
        public IActionResult Submit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "User");

            var form = _formService.GetForms()
                .FirstOrDefault(f => f.FormId == id);

            if (form == null)
                return NotFound();

            // ✅ Also pass forms to Layout for Submit page
            var allForms = _formService.GetForms();
            ViewBag.SidebarForms = allForms;

            // Build dynamic table name
            //var tableName = "Form_" + form.FormTitle.Replace(" ", "");
            var tableName = form.TableName;
            var fields = _formService.GetFields(tableName);

            var vm = new DynamicFormViewModel
            {
                FormId = form.FormId,
                FormTitle = form.FormTitle,
                TableName = tableName,
                Fields = fields
            };

            return View("~/Views/User/FormSubmit.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(int FormId, string TableName)
        {
            var values = new Dictionary<string, string>();

            foreach (var key in Request.Form.Keys)
            {
                if (key == "__RequestVerificationToken"
                    || key == "FormId"
                    || key == "TableName")
                {
                    continue;
                }

                values[key] = Request.Form[key];
            }

            // 🔐 Get logged-in user
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "User");

            _formService.SaveFormData(TableName, values, userId.Value);

            TempData["Message"] = "Form submitted successfully";
            return RedirectToAction("Index");
        }

        [Route("UserList")]

        public IActionResult UserList()
        {
            var users = _adminService.GetUsers();
            return View("~/Views/Admin/UserList.cshtml", users);
        }
    }
}
