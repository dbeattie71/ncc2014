namespace DemoApp.Core.Models
{
    public class Session
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abstract { get; set; }
        public int SpeakerId { get; set; }
        public Speaker Speaker { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int TimeSlotId { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public string SessionLevel { get; set; }
        public bool GroupSession { get; set; }
        public object Evaluations { get; set; }
        public int? SubmissionId { get; set; }
        public string Tags { get; set; }
    }
}
