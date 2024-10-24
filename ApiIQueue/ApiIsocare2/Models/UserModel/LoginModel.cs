using System.ComponentModel.DataAnnotations;

namespace ApiIsocare2.Models.UserModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "โปรดใส่หมายเลขบัตรบัตรประจำตัวประชาชน")]
        public string citizenId { get; set; }
        [Required(ErrorMessage = "โปรดใส่รหัสผ่าน")]
        public string password { get; set; }
    }
}
