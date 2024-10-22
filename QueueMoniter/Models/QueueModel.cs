namespace QMS.Models
{
    public class QueueModel
    {
        public string QueueType { get; set; }
        public string QueueNumber { get; set; }
        public int? Counter { get; set; }
        public DateTime? CallQueueTime { get; set; }
        public string Source { get; set; }
    }
}
