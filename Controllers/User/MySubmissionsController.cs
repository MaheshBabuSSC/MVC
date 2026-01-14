using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public class MySubmissionsController : Controller
{
    private readonly SubmissionService _submissionService;

    public MySubmissionsController(SubmissionService submissionService)
    {
        _submissionService = submissionService;
    }

    public IActionResult Index()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var submissions = _submissionService.GetUserSubmissions(userId);

        return View("~/Views/User/MySubmissions.cshtml", submissions);
    }

    [HttpPost]
    public IActionResult Delete(string tableName, int submissionId)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        _submissionService.DeleteSubmission(tableName, submissionId, userId);

        return RedirectToAction("Index");
    }
}
