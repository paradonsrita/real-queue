namespace QMS.Models
{
    public class QueueModel
    {
        public int queue_id { get; set; }
        public DateTime QueueDate { get; set; }
        public string QueueStatus { get; set; }
        public string QueueType { get; set; }
        public string QueueNumber { get; set; }
        public int? Counter { get; set; }
        public string Source { get; set; }
    }
}
