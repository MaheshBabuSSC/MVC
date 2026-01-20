namespace MvcWebApiSwaggerApp.Models
{
    public class Register
    {
        // Existing
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public int RoleId { get; set; }
        public int IsOtpSent { get; set; }
        public string OtpCode { get; set; }

        // 🔹 NEW FIELDS (tbl_Users)
        public string EmployeeId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public int DepartmentId { get; set; }
        public string Site { get; set; }
        public string Shift { get; set; }
        public string Location { get; set; }

        // Dropdowns
        public List<RoleVM> Roles { get; set; }
    }


    public class RoleVM
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

}
