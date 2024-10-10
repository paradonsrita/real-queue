using System.ComponentModel.DataAnnotations;

namespace ApiIsocare2.Models.Queue
{
    public class QueueStatus
    {
        [Key]
        public int queue_status_id { get; set; }
        public string queue_status_name { get; set; }
    }
}
