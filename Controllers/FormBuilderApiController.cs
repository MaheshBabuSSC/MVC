//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using MvcWebApiSwaggerApp.Models;

//[ApiController]
//[Route("api/forms")]
//public class FormBuilderApiController : ControllerBase
//{
//    private readonly FormService _service;

//    public FormBuilderApiController(FormService service)
//    {
//        _service = service;
//    }

//    [HttpPost("create")]
//    public IActionResult Create(CreateFormRequest request)
//    {
//        _service.CreateForm(
//            request.FormTitle,
//            request.FormDescription,
//            request.CreatedBy,
//            request.Fields);

//        return Ok("Form created");
//    }

//    [HttpGet]
//    public IActionResult GetAll()
//    {
//        return Ok(_service.GetForms());
//    }

//    [HttpPut("{id}")]
//    public IActionResult Update(int id, CreateFormRequest request)
//    {
//        _service.UpdateForm(
//            id,
//            request.FormTitle,
//            request.FormDescription,
//            request.CreatedBy,
//            request.Fields
//        );

//        return Ok("Form updated");
//    }

//    [HttpDelete("{id}")]
//    public IActionResult Delete(int id)
//    {
//        _service.DeleteForm(id);
//        return Ok("Form deleted");
//    }

//}
