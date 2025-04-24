using EventManagementSystem.Data;
using EventManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class ReportServices
{
    private readonly EventDbContext _context;

    public ReportServices(EventDbContext context)
    {
        _context = context;
    }

    public async Task<List<Report>> GetMonthlyEventReportAsync(string? search, string? category, DateTime? fromDate, DateTime? toDate)
    {
        var events = await _context.Events
            .Where(e =>
                (string.IsNullOrEmpty(search) || e.EventTitle!.Contains(search)) &&
                (string.IsNullOrEmpty(category) || e.EventCategory == category) &&
                (!fromDate.HasValue || e.EventDate >= fromDate.Value) &&
                (!toDate.HasValue || e.EventDate <= toDate.Value))
            .ToListAsync();

        var grouped = events
            .GroupBy(e => new { e.EventDate.Year, e.EventDate.Month })
            .Select(g => new Report
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Events = g.Select(e => new EventReportItem
                {
                    Title = e.EventTitle ?? "Untitled",
                    Date = e.EventDate,
                    AttendeeCount = e.AttendeeCount
                }).OrderBy(e => e.Date).ToList()
            })
            .OrderBy(r => r.Year).ThenBy(r => r.Month)
            .ToList();

        return grouped;
    }

}
