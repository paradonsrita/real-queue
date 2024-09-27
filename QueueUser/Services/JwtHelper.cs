using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QMS.Services
{
    public class JwtHelper
    {

        // ฟังก์ชันที่ใช้ตรวจสอบว่า token หมดอายุหรือไม่
        public static bool IsTokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var expiration = jwtToken.ValidTo;
            return expiration < DateTime.UtcNow; // คืนค่า true ถ้าหมดอายุ
        }

        public static int? GetUserIdFromToken(string token, IConfiguration configuration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                {
                    Console.WriteLine("JWT Token is null.");
                    return null;
                }

#pragma warning disable CS8604 // Possible null reference argument.
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Key"]))
                };
#pragma warning restore CS8604 // Possible null reference argument.

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                {
                    Console.WriteLine("UserId claim not found in token.");
                    return null;
                }

                Console.WriteLine($"UserId from token: {userIdClaim.Value}");
                return int.Parse(userIdClaim.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while getting UserId from token: {ex.Message}");
                return null;
            }
        }
    }
}
