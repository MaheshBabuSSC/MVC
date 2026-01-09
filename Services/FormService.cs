using Microsoft.EntityFrameworkCore;
using MvcWebApiSwaggerApp.Models;
using System.Text.Json;

public class FormService
{
    private readonly AppDbContext _context;

    public FormService(AppDbContext context)
    {
        _context = context;
    }

    public void CreateForm(
        string title,
        string description,
        string createdBy,
        List<FormField> fields)
    {
        var fieldsJson = JsonSerializer.Serialize(fields);

        _context.Database.ExecuteSqlRaw(
            "EXEC sp_CreateForm @p0, @p1, @p2, @p3",
            title,
            description,
            createdBy,
            fieldsJson
        );
    }

    public void UpdateForm(
    int formId,
    string title,
    string description,
    string updatedBy,
    List<FormField> fields)
    {
        var fieldsJson = JsonSerializer.Serialize(fields);

        _context.Database.ExecuteSqlRaw(
            "EXEC sp_UpdateForm @p0, @p1, @p2, @p3, @p4",
            formId,
            title,
            description,
            updatedBy,
            fieldsJson
        );
    }

    public void DeleteForm(int formId)
    {
        _context.Database.ExecuteSqlRaw(
            "EXEC sp_DeleteForm @p0",
            formId
        );
    }

    public List<FormSummary> GetForms()
    {
        return _context.Set<FormSummary>()
            .FromSqlRaw("EXEC sp_GetForms")
            .AsNoTracking()
            .ToList();
    }

    public void ToggleFormStatus(int formId, bool isActive)
    {
        _context.Database.ExecuteSqlRaw(
            "EXEC sp_ToggleFormStatus @p0, @p1",
            formId,
            isActive
        );
    }



}
