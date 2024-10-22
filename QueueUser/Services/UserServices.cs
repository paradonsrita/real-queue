using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using QMS.Data.Entities;
using QMS.Models;
using static System.Net.WebRequestMethods;

namespace QMS.Services
{
    public class UserServices
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly IConfiguration _configuration;
        private readonly NavigationManager _navigation;

        public UserServices(IHttpClientFactory httpClientFactory, IJSRuntime jsRuntime, IConfiguration configuration, NavigationManager navigation)
        {
            _httpClient = httpClientFactory.CreateClient("Queue");
            _jsRuntime = jsRuntime;
            _configuration = configuration;
            _navigation = navigation;
        }

        public async Task<UserModel?> GetUserById(int userId)
        {
            try
            {
                // เรียก API เพื่อดึงข้อมูลผู้ใช้ตาม userId
                var response = await _httpClient.GetAsync($"/api/User?userId={userId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserModel>();
                }
                else
                {
                    throw new Exception("Unable to retrieve user data.");
                }

            }
            catch (Exception ex)
            {
                // จัดการข้อผิดพลาด เช่น แสดงข้อความหรือบันทึก log
                Console.WriteLine($"Error fetching user data: {ex.Message}");
                return null;
            }
        }

        public async Task<string> GetUserIdByToken()
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwtToken");

                if (!string.IsNullOrEmpty(token))
                {

                    // สร้าง JwtSecurityTokenHandler เพื่ออ่านข้อมูลจาก token
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);

                    // ดึงค่า userId จาก Claim ของ JWT token
                    var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");

                    if (userIdClaim != null)
                    {
                        return userIdClaim.Value; // คืนค่าที่ดึงได้จาก userId claim
                    }


                }
                else
                {
                    _navigation.NavigateTo("/login");
                }
                return string.Empty;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user profile: {ex.Message}");
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "jwtToken");
                _navigation.NavigateTo("/login");
                return string.Empty;

            }
        }
    }

}

