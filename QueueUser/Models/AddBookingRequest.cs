namespace QMS.Models
{
    public class AddBookingRequest
    {
        public int UserId { get; set; }
        public string Type { get; set; } // ใช้ `queue_type_id`
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; } // เวลาในรูปแบบ "08:00" หรือ "13:00"
    }
}
