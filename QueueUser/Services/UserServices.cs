using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using QMS.Models;

namespace QMS.Services
{
    public class UserServices
    {
        private readonly HttpClient _httpClient;

        public UserServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserModel?> GetUserById(int userId)
        {
            try
            {
                // เรียก API เพื่อดึงข้อมูลผู้ใช้ตาม userId
                var response = await _httpClient.GetAsync($"https://localhost:44328/api/User?userId={userId}");
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
    }

}

