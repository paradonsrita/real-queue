using System.Security.Cryptography;
using System.Text;

namespace ApiIsocare2.Utilities
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
        public static bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            // แฮชรหัสผ่านที่ผู้ใช้ส่งมา
            var hashedInputPassword = HashPassword(inputPassword);

            // ตรวจสอบว่าแฮชตรงกันหรือไม่
            return hashedPassword == hashedInputPassword;
        }
    }
}
