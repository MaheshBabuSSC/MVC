using Microsoft.AspNetCore.Mvc;

namespace MvcWebApiSwaggerApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("MVC is working");
        }
    }
}
