using System.Security.Cryptography;
using System.Text;

namespace MvcWebApiSwaggerApp.Security
{
    public static class PasswordHelper
    {
        public static void CreatePasswordHash(
            string password,
            out byte[] hash,
            out byte[] salt)
        {
            using var hmac = new HMACSHA256();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
