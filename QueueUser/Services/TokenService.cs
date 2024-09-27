using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.JSInterop;
using Microsoft.Extensions.Configuration;

namespace QMS.Services
{
    public class TokenService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IConfiguration _configuration;

        public TokenService(IJSRuntime jsRuntime, IConfiguration configuration)
        {
            _jsRuntime = jsRuntime;
            _configuration = configuration;
        }

        public async Task<string> GetTokenAsync()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwtToken");
            return token;
        }

        public bool IsTokenExpired(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            if (jwtToken == null)
                return true;

            var expiryDateUnix = long.Parse(jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDate = DateTimeOffset.FromUnixTimeSeconds(expiryDateUnix).UtcDateTime;

            return expiryDate <= DateTime.UtcNow;
        }

        public int? GetUserIdFromToken(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            if (jwtToken == null)
                return null;

            // ดึง Claim "nameid"
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");
            if (userIdClaim == null)
                return null;

            return int.Parse(userIdClaim.Value);
        }


        public async Task RemoveTokenAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "jwtToken");
        }
    }
}
