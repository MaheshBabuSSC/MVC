using Microsoft.EntityFrameworkCore;

namespace MvcWebApiSwaggerApp.Models
{
    public class Admin
    {
    }


    [Keyless]

    public class UsersList
    {
        public int UserId { get; set; }
        public string EmployeeId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public int? DepartmentId { get; set; }
        public string Site { get; set; }
        public string Shift { get; set; }
        public string Location { get; set; }
        public int? RoleId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
