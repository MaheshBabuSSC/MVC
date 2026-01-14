using Microsoft.EntityFrameworkCore;
using System;

namespace MvcWebApiSwaggerApp.Models
{
    [Keyless]   // 🔥 REQUIRED for FromSql
    public class UserSubmissionVM
    {
        public int FormId { get; set; }
        public string FormTitle { get; set; }
        public string TableName { get; set; }

        public int SubmissionId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    [Keyless]
    public class FormMetaVM
    {
        public int FormId { get; set; }
        public string FormTitle { get; set; }
        public string TableName { get; set; }
    }
}
