using Microsoft.EntityFrameworkCore;
using MvcWebApiSwaggerApp.Models;
using MvcWebApiSwaggerApp.Security;

namespace MvcWebApiSwaggerApp.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly SmsService _smsService;


        public AuthService(AppDbContext context, SmsService smsService  )
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

            // 1️⃣ Create user and get UserId
            var userId = _context.Database
                .SqlQueryRaw<int>(
                    "EXEC sp_CreateUser @p0, @p1, @p2, @p3, @p4, @p5",
                    request.Email,
                    hash,
                    salt,
                    request.Role,
                    createdBy,
                    request.MobileNumber
                )
                .AsEnumerable()
                .First();

            // 2️⃣ Generate OTP
            var otp = OtpHelper.GenerateOtp();
            var expiresAt = DateTime.Now.AddMinutes(5);

            // 3️⃣ Save OTP
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_GenerateOtp @p0, @p1, @p2",
                userId,
                otp,
                expiresAt
            );

            // 4️⃣ Send OTP to mobile
            _smsService.SendOtp(request.MobileNumber, otp);

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
                    "EXEC sp_GetUserForLogin @p0",
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
    }
}
