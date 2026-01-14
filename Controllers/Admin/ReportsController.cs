using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcWebApiSwaggerApp.Models;

[Area("Admin")]
public class ReportsController : Controller
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
    {
        _context = context;
    }

    // 1️⃣ Reports list
    public IActionResult Index()
    {
        var forms = _context.Set<FormSummary>()
            .FromSqlRaw("EXEC sp_GetForms")
            .ToList();

        return View("~/Views/Admin/Reports.cshtml", forms);
    }

    // 2️⃣ View submitted data for a form
    public IActionResult Details(string tableName)
    {
        var data = new List<Dictionary<string, object>>();

        using var conn = _context.Database.GetDbConnection();
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT * FROM {tableName}";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var row = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                row[reader.GetName(i)] = reader[i];
            }
            data.Add(row);
        }

        ViewBag.TableName = tableName;
        return View("~/Views/Admin/ReportView.cshtml", data);
    }
}
