using System.ComponentModel.DataAnnotations;

namespace QMS.Data.Entities
{
    public class ResetPasswordModel
    {
        public string? otp { get; set; }  // เพิ่มฟิลด์สำหรับเก็บค่า OTP
        [Required(ErrorMessage = "กรุณากรอกรหัสผ่านใหม่"), MinLength(8)]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "กรุณายืนยันรหัสผ่าน")]
        [Compare("NewPassword", ErrorMessage = "รหัสผ่านไม่ตรงกัน")]
        public string? ConfirmPassword { get; set; }
    }
}
