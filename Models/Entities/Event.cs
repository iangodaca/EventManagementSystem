namespace EventManagementSystem.Models.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string? EventTitle { get; set; }
        public string? EventDescription { get; set; }
        public string? EventLocation { get; set; }
        public DateTime EventDate { get; set; }
        public string? EventCategory { get; set; }
        public int AttendeeCount { get; set; }
    }
}
