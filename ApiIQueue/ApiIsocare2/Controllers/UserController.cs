using System.Security.Claims;
using ApiIsocare2.Data;
using ApiIsocare2.Models.UserModel;
using ApiIsocare2.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiIsocare2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;
        public UserController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserData(int userId)
        {
            try
            {

                /*
                                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                                var userId = JwtHelper.GetUserIdFromToken(token, _configuration);

                                if (!userId.HasValue)
                                {
                                    return Unauthorized("Invalid token.");
                                }
                */


                var result = await _db.Users
                    .Where(u => u.user_id == userId)
                    .Select(u => new
                    {
                        u.user_id,
                        u.citizen_id_number,
                        u.firstname,
                        u.lastname,
                        u.phone_number,
                        u.user_email
                    })
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return NotFound("ไม่มีบัญชีผู้ใช้นี้");
                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }
        }

        ///api/User/citizen-id?citizenId=1500701309052
        [HttpGet("citizen-id")]
        public async Task<IActionResult> GetUserNameByCitizenId(string citizenId)
        {
            var user = await _db.Users
                            .Where(u => u.citizen_id_number == citizenId)
                            .Select(u => new
                            {
                                u.user_id,
                                citizen_id = u.citizen_id_number,
                                name = $"{u.firstname} {u.lastname}"
                            })
                            .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("ไม่มีบัญชีผู้ใช้นี้");
            }
            return Ok(user);
        }


        [HttpPut]
        public async Task<IActionResult> EditProfile([FromBody] EditProfile newProfile)
        {
            try
            {
                var oldUser = _db.Users
                                .Where(u => u.user_id == newProfile.user_id)
                                .FirstOrDefault();


                if (oldUser == null)
                {
                    return NotFound();
                }

                oldUser.phone_number = newProfile.phone_number;
                oldUser.user_email = newProfile.user_email;
                await _db.SaveChangesAsync();

                return Ok("แก้ไขเรียบร้อย");
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }

        }

    }
}
