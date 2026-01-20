namespace MvcWebApiSwaggerApp.Models
{
    public class Register
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public int RoleId { get; set; }
        public List<RoleVM> Roles { get; set; } = new();

        public string MobileNumber { get; set; }

        // OTP
        public int UserId { get; set; }
        public string OtpCode { get; set; }
        public int IsOtpSent { get; set; }
    }
    public class RoleVM
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

}
