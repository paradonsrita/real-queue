using System.ComponentModel.DataAnnotations;

namespace ApiIsocare2.Models.Booking
{
    public class AddBookingRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }
        [Required]
        public string AppointmentTime { get; set; }
    }
}
