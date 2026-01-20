using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcWebApiSwaggerApp.Models
{
    public class user
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string IsActive { get; set; }

    }

    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResult
    {
        public int UserId { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }


    [Keyless]   // 🔥 REQUIRED
    public class DynamicFormField
    {
        public string Name { get; set; }        // Column alias MUST match SP
        public string Label { get; set; }
        public string Type { get; set; }
        public bool Required { get; set; }

        [NotMapped]
        public List<string>? Options { get; set; } // handled in SP / JSON
    }


    public class DynamicFormViewModel
    {
        public int FormId { get; set; }
        public string FormTitle { get; set; }
        public string TableName { get; set; }

        public List<DynamicFormField> Fields { get; set; } = new();
    }



}
