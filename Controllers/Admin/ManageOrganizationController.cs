using Microsoft.AspNetCore.Mvc;

namespace MvcWebApiSwaggerApp.Controllers.Admin
{
    public class ManageOrganizationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
