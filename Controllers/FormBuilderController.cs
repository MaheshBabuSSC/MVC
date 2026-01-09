using Microsoft.AspNetCore.Mvc;
using MvcWebApiSwaggerApp.Models;


[Area("Admin")] // 🔥 THIS FIXES 405

public class FormBuilderController : Controller
{
    private readonly FormService _service;

    public FormBuilderController(FormService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var forms = _service.GetForms();
        return View("~/Views/Admin/FormBuilder/Index.cshtml", forms);
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View("~/Views/Admin/FormBuilder/Create.cshtml");
    }

    [HttpPost]
    public IActionResult Create(CreateFormRequest request)
    {
        _service.CreateForm(
            request.FormTitle,
            request.FormDescription,
            "MVC-User",
            request.Fields
        );

        ViewBag.Message = "Form created successfully!";
        return View("~/Views/Admin/FormBuilder/Create.cshtml");
    }

    [HttpPost]
    public IActionResult Update(int id, CreateFormRequest request)
    {
        _service.UpdateForm(
            id,
            request.FormTitle,
            request.FormDescription,
            "MVC-User",
            request.Fields
        );

        TempData["Message"] = "Form updated successfully";
        return RedirectToAction("Create");
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        _service.DeleteForm(id);
        TempData["Message"] = "Form deleted successfully";
        return RedirectToAction("Create");
    }
    [HttpPost]
    public IActionResult ToggleStatus(int id, bool isActive)
    {
        _service.ToggleFormStatus(id, isActive);

        TempData["Message"] = isActive
            ? "Form activated successfully"
            : "Form deactivated successfully";

        return RedirectToAction("Index");
    }

}
