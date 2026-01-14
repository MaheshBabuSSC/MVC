using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcWebApiSwaggerApp.Models;
using System.Collections.Generic;

public class SubmissionService
{
    private readonly AppDbContext _context;

    public SubmissionService(AppDbContext context)
    {
        _context = context;
    }

    public List<UserSubmissionVM> GetUserSubmissions(int userId)
    {
        var result = new List<UserSubmissionVM>();

        // 1️⃣ Get all active forms using RAW SQL
        var forms = _context.Set<FormMetaVM>()
            .FromSqlRaw(@"
            SELECT FormId, FormTitle, TableName
            FROM Forms
            WHERE IsActive = 1
        ")
            .AsNoTracking()
            .ToList();

        // 2️⃣ Loop each dynamic table
        foreach (var form in forms)
        {
            var sql = $@"
            SELECT 
                {form.FormId} AS FormId,
                '{form.FormTitle}' AS FormTitle,
                '{form.TableName}' AS TableName,
                Id AS SubmissionId,
                CreatedAt
            FROM [{form.TableName}]
            WHERE SubmittedByUserId = @UserId
        ";

            var submissions = _context.Set<UserSubmissionVM>()
                .FromSqlRaw(sql, new SqlParameter("@UserId", userId))
                .AsNoTracking()
                .ToList();

            result.AddRange(submissions);
        }

        return result;
    }

    public void DeleteSubmission(string tableName, int submissionId, int userId)
    {
        var sql = $@"
            DELETE FROM [{tableName}]
            WHERE Id = @Id AND SubmittedByUserId = @UserId
        ";

        _context.Database.ExecuteSqlRaw(
            sql,
            new SqlParameter("@Id", submissionId),
            new SqlParameter("@UserId", userId)
        );
    }
}
