namespace EventManagementSystem.Models
{
    public class EventViewModel
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
