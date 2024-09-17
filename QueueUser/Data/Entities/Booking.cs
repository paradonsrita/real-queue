using System.ComponentModel.DataAnnotations;

namespace QMS.Data.Entities
{
    public class Booking
    {
        [Key]
        public int queue_id { get; set; }

        [Required]
        public required string queue_type_id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan appointment_date { get; set; }

        public const int MaxBookingsPerDay = 10; // กำหนดจำนวนคิวสูงสุดต่อวัน
    }

}
