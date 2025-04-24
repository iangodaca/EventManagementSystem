namespace EventManagementSystem.Models
{
    public class ReportSummary
    {
        public int TotalEvents { get; set; }
        public int TotalUsers { get; set; }
        public int TotalCompletedEvents { get; set; }
        public int TotalUpcomingEvents { get; set; }
        public List<string>? EventCategoryLabels { get; set; }
        public List<int>? EventCategoryCounts { get; set; }
        public List<string>? Months { get; set; }
        public List<int>? EventsByMonth { get; set; }
        public List<dynamic>? MonthlyEvents { get; set; } // Dynamic type to hold the grouped events by month
    }

}
