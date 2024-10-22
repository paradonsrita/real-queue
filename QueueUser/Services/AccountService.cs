using Microsoft.AspNetCore.Components.Authorization;
using QMS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using QMS.Data;
using System.Net.Http;
using System.Net.Http.Json;

namespace QMS.Services
{
    public class AccountService
    {
        private readonly BookingContext _dbContext;
        private readonly ILogger<AccountService> _logger;
        private readonly HttpClient _httpClient;

        public AccountService(BookingContext dbContext, ILogger<AccountService> logger, IHttpClientFactory httpClientFactory)
        {
            _dbContext = dbContext;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("Queue");
        }

        public async Task RegisterAsync(RegisterViewModel model)
        {
            try
            {
                var existingUserById = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.citizen_id_number == model.citizen_id_number);

                if (existingUserById != null)
                {
                    throw new InvalidOperationException("รหัสบัตรประชาชนหมายเลขนี้ได้ถูกใช้งานไปแล้ว กรุณาใช้หมายเลขอื่น");
                }

                var user = new User
                {
                    citizen_id_number = model.citizen_id_number,
                    firstname = model.firstname,
                    lastname = model.lastname,
                    phone_number = model.phone_number,
                    user_email = model.user_email,
                    password = HashPassword(model.password)
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during registration.");
                throw;
            }
        }

        public async Task<User> LoginAsync(LoginViewModel loginModel)
        {
            try
            {
                var hashedPassword = HashPassword(loginModel.password);
                loginModel.password = hashedPassword;

                var response = await _httpClient.PostAsJsonAsync("/api/LogicRegister/login", loginModel);

                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<User>();
#pragma warning disable CS8603 // Possible null reference return.
                    return user;
#pragma warning restore CS8603 // Possible null reference return.
                }
                else
                {
                    throw new InvalidOperationException("การเข้าสู่ระบบล้มเหลว.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login.");
                throw;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"/api/User?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<User>();
#pragma warning disable CS8603 // Possible null reference return.
                return user;
#pragma warning restore CS8603 // Possible null reference return.
            }

#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }


        public async Task UpdateUserAsync(User updatedUser)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(updatedUser.user_id);
                if (user != null)
                {
                    user.firstname = updatedUser.firstname;
                    user.lastname = updatedUser.lastname;
                    user.phone_number = updatedUser.phone_number;

                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user.");
                throw;
            }
        }
    }
}
