using System.ComponentModel.DataAnnotations;

namespace QMS.Data.Entities
{
    public class EmailModel
    {
        [Required(ErrorMessage = "กรุณากรอกอีเมล")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Email { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
