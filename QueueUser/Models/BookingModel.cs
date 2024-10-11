namespace QMS.Models
{
    public class BookingModel
    {
        public int queue_id { get; set; }
        public DateTime booking_date { get; set; }
        public DateTime appointment_date { get; set; }
        public string QueueStatus { get; set; }
        public string QueueType { get; set; }
        public string QueueNumber { get; set; }
        public int counter { get; set; }
        public int user_id { get; set; }
        public string Name { get; set; }
        public string lastname { get; set; }
        public string phone_number { get; set; }
        public string citizen_id_number { get; set; }
    }
}
