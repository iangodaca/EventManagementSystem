namespace EventManagementSystem.Models.Entities
{
    public class Report
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<EventReportItem> Events { get; set; } = new();
        public int TotalEvents => Events.Count;
    }

    public class EventReportItem
    {
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int AttendeeCount { get; set; }
    }
}