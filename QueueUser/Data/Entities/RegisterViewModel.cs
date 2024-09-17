using System.ComponentModel.DataAnnotations;

namespace QMS.Data.Entities
{
    public class RegisterViewModel
    {
        [Key]
        public int user_id { get; set; }

        [Required(ErrorMessage = "กรุณากรอกรหัสบัตรประชาชน"), MaxLength(13)]
        [Display(Name = "รหัสบัตรประชาชน")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string citizen_id_number { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required(ErrorMessage = "กรุณากรอกชื่อจริง"), MaxLength(100)]
        [Display(Name = "ชื่อจริง")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string firstname { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required(ErrorMessage = "กรุณากรอกนามสกุล"), MaxLength(100)]
        [Display(Name = "นามสกุล")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string lastname { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required(ErrorMessage = "กรุณากรอกเบอร์โทรศัพท์"), MaxLength(10)]
        [Display(Name = "เบอร์โทรศัพท์")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string phone_number { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required(ErrorMessage = "กรุณากรอกอีเมล"), MaxLength(100)]
        [EmailAddress(ErrorMessage = "รูปแบบอีเมลไม่ถูกต้อง")]
        [Display(Name = "อีเมล")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string user_email { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required(ErrorMessage = "กรุณากรอกรหัสผ่าน"), MinLength(8)]
        [DataType(DataType.Password)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string password { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required(ErrorMessage = "กรุณากรอกยืนยันรหัสผ่าน")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "รหัสผ่านไม่ตรงกัน กรุณากรอกใหม่")]
        [Display(Name = "ยืนยันรหัสผ่าน")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string confirm_password { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }

}
