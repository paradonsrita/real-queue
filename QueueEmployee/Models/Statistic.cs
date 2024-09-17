namespace QMS.Models
{
    public class Statistic
    {
        public string type_name { get; set; }
        public string? Source { get; set; }
        public DateTime Date { get; set; }
        public int Total { get; set; }
    }
}
