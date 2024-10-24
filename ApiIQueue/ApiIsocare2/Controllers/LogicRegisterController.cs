using ApiIsocare2.Data;
using ApiIsocare2.Models.UserModel;
using ApiIsocare2.Utilities;
using Microsoft.AspNetCore.Mvc;


namespace ApiIsocare2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogicRegisterController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;

        public LogicRegisterController(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;

        }

        // api/LogicRegister/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            
            if (loginModel == null)
            {
                return BadRequest("ข้อมูลล็อกอินไม่ถูกต้อง");
            }
            try
            {
                var hashedPassword = PasswordHasher.HashPassword(loginModel.password);
                var user = _db.Users
                    .Where(u => u.citizen_id_number == loginModel.citizenId)
                    .Select(u => new { u.user_id, u.citizen_id_number, u.password })
                    .SingleOrDefault();


                if (user != null)
                {
                    if (PasswordHasher.VerifyPassword(user.password, loginModel.password))
                    {
                        var key = _configuration["Jwt:Key"];
                        if (string.IsNullOrEmpty(key) || key.Length < 16)
                        {
                            return StatusCode(500, "ข้อผิดพลาด: คีย์ JWT สั้นเกินไป.");
                        }
                        var token = JwtHelper.GenerateJwtToken(
                           user.user_id.ToString(),
                            key,
                           _configuration["Jwt:Issuer"],
                           _configuration["Jwt:Audience"]
                        );
                        return Ok(new { Token = token });

                    }
                    else
                    {
                        return Unauthorized("รหัสผ่านไม่ถูกต้อง");
                    }



                }
                return Unauthorized("ไม่มีบัญชีผู้ใช้นี้");




            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }

        }

        // api/LogicRegister/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] User registerModel)
        {
            try
            {
                if (_db.Users.Any(u => u.citizen_id_number == registerModel.citizen_id_number))
                {
                    return BadRequest("หมายเลขบัตรประจำตัวประชาชนนี้ถูกใช้ไปแล้ว");
                }
                if (_db.Users.Any(u => u.user_email == registerModel.user_email))
                {
                    return BadRequest("อีเมลนี้ถูกใช้ไปแล้ว");
                }

                var hashedPassword = PasswordHasher.HashPassword(registerModel.password);

                var newUser = new User
                {
                    firstname = registerModel.firstname,
                    lastname = registerModel.lastname,
                    phone_number = registerModel.phone_number,
                    citizen_id_number = registerModel.citizen_id_number,
                    user_email = registerModel.user_email,
                    password = hashedPassword
                };

                _db.Users.Add(newUser);
                _db.SaveChanges();

                return Ok(new { Message = "ลงทะเบียนเสร็จสิ้น" });
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }

        }
    }
}
