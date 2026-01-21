using Microsoft.EntityFrameworkCore;
using MvcWebApiSwaggerApp.Models;
using MvcWebApiSwaggerApp.Security;

namespace MvcWebApiSwaggerApp.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly SmsService _smsService;


        public AuthService(AppDbContext context, SmsService smsService)
        {
            _context = context;
            _smsService = smsService;
        }



        public int RegisterUser(Register request, string createdBy)
        {
            PasswordHelper.CreatePasswordHash(
                request.Password,
                out byte[] hash,
                out byte[] salt
            );

            // 1️⃣ Create user (NEW TABLE)
            var userId = _context.Database
    .SqlQueryRaw<int>(
        @"EXEC sp_AddUser 
        @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12",
        request.EmployeeId,
        request.FullName,
        request.UserName,
        request.Email,
        hash,
        salt,                    // ✅ PASS SALT
        request.MobileNumber,
        request.DepartmentId,
        request.Site,
        request.Shift,
        request.Location,
        request.RoleId,
        createdBy
    )
    .AsEnumerable()
    .First();


            // 2️⃣ OTP
            var otp = OtpHelper.GenerateOtp();
            var expiresAt = DateTime.Now.AddMinutes(5);

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_GenerateOtp @p0, @p1, @p2",
                userId,
                otp,
                expiresAt
            );

            _smsService.SendOtp(request.MobileNumber, otp);

            return userId;
        }


        public int RegisterUserWithoutOtp(Register request, string createdBy)
        {
            PasswordHelper.CreatePasswordHash(
                request.Password,
                out byte[] hash,
                out byte[] salt
            );

            // Create user directly without OTP
            var userId = _context.Database
        .SqlQueryRaw<int>(
            @"EXEC sp_AddUsers 
        @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13",
            request.EmployeeId,   // @EmployeeId
            request.FullName,     // @FullName
            request.UserName,     // @UserName
            request.Email,        // @Email
            hash,                 // @PasswordHash
            salt,                 // @PasswordSalt
            request.MobileNumber, // @MobileNo
            request.DepartmentId, // @DepartmentId
            request.Site,         // @Site
            request.Shift,        // @Shift
            request.Location,     // @Location
            request.RoleId,       // @RoleId
            1,                    // @IsActive   <-- MISSING BEFORE
            "MVC"                 // @CreatedBy
        )
        .AsEnumerable()
        .First();


            // SKIP OTP GENERATION AND SMS SENDING
            // var otp = OtpHelper.GenerateOtp();
            // var expiresAt = DateTime.Now.AddMinutes(5);
            // _context.Database.ExecuteSqlRaw(
            //     "EXEC sp_GenerateOtp @p0, @p1, @p2",
            //     userId,
            //     otp,
            //     expiresAt
            // );
            // _smsService.SendOtp(request.MobileNumber, otp);

            return userId;
        }
        public bool VerifyOtp(int userId, string otp)
        {
            var result = _context.Database
                .SqlQueryRaw<int>(
                    "EXEC sp_VerifyOtp @p0, @p1",
                    userId,
                    otp
                )
                .AsEnumerable()
                .FirstOrDefault();

            return result == 1;
        }



        public int ValidateLogin(string email, string password)
        {
            // 1️⃣ Get hash + salt from DB via SP
            var loginResult = _context.Database
                .SqlQueryRaw<LoginResult>(
                    "EXEC sp_VerifyLogin @p0",
                    email
                )
                .AsEnumerable()
                .FirstOrDefault();

            if (loginResult == null)
                return 0;

            // 2️⃣ Verify password
            bool isValid = PasswordHelper.VerifyPassword(
                password,
                loginResult.PasswordHash,
                loginResult.PasswordSalt
            );

            return isValid ? loginResult.UserId : 0;
        }

        public List<RoleVM> GetActiveRoles()
        {
            return _context.Database
                .SqlQueryRaw<RoleVM>(
                    @"SELECT RoleId, RoleName 
              FROM tbl_Roles 
              WHERE IsActive = 1")
                .ToList();
        }

    }
}
