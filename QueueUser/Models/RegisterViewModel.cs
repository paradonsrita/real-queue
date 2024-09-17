using System.ComponentModel.DataAnnotations;

namespace QMS.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "กรุณากรอกรหัสบัตรประชาชน")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string citizen_id_number { get; set; }
 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required(ErrorMessage = "กรุณากรอกชื่อจริง")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string firstname { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required(ErrorMessage = "กรุณากรอกนามสกุล")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string lastname { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required(ErrorMessage = "กรุณากรอกอีเมล")]
        [EmailAddress(ErrorMessage = "รูปแบบอีเมลไม่ถูกต้อง")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string user_email { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required(ErrorMessage = "กรุณากรอกเบอร์โทรศัพท์")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string phone_number { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required(ErrorMessage = "กรุณากรอกรหัสผ่าน")]
        [MinLength(6, ErrorMessage = "รหัสผ่านต้องมีความยาวอย่างน้อย 6 ตัวอักษร")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string password { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required(ErrorMessage = "กรุณายืนยันรหัสผ่าน")]
        [Compare("password", ErrorMessage = "รหัสผ่านไม่ตรงกัน")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string confirm_password { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }

}
