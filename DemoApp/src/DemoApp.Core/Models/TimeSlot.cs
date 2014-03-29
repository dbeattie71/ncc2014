namespace DemoApp.Core.Models
{
    public class TimeSlot
    {
        public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool SessionSlot { get; set; }
        public object Sessions { get; set; }
    }
}