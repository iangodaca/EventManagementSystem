using EventManagementSystem.Data;
using EventManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ReportController : Controller
{
    private readonly EventDbContext _context;

    public ReportController(EventDbContext context)
    {
        _context = context;
    }

    [Authorize]
    public async Task<IActionResult> Index(string? search, string? category, DateTime? fromDate, DateTime? toDate)
    {
        // Get the monthly event report with additional data for charts
        var report = await GetMonthlyEventReportAsync(search, category, fromDate, toDate);
        var eventCategories = await GetEventCategoriesAsync(search, category, fromDate, toDate); // Fetch event categories data

        // Preparing the data for the charts
        ViewBag.Search = search;
        ViewBag.Category = category;
        ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
        ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

        // Pie chart data for event categories
        ViewBag.CategoryData = eventCategories.Select(c => c.Count).ToArray();
        ViewBag.CategoryLabels = eventCategories.Select(c => c.Category).ToArray();

        // Bar chart data for total events by month
        ViewBag.Months = report.Select(r => $"{r.Year}-{r.Month:D2}").ToArray();
        ViewBag.TotalEventsByMonth = report.Select(r => r.Events.Count).ToArray();

        return View(report);
    }

    private async Task<List<Report>> GetMonthlyEventReportAsync(string? search, string? category, DateTime? fromDate, DateTime? toDate)
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

    // Fetch event categories for the pie chart
    private async Task<List<EventCategoryCount>> GetEventCategoriesAsync(string? search, string? category, DateTime? fromDate, DateTime? toDate)
    {
        var events = await _context.Events
            .Where(e =>
                (string.IsNullOrEmpty(search) || e.EventTitle!.Contains(search)) &&
                (string.IsNullOrEmpty(category) || e.EventCategory == category) &&
                (!fromDate.HasValue || e.EventDate >= fromDate.Value) &&
                (!toDate.HasValue || e.EventDate <= toDate.Value))
            .ToListAsync();

        var categoryCounts = events
            .GroupBy(e => e.EventCategory)
            .Select(g => new EventCategoryCount
            {
                Category = g.Key,
                Count = g.Count()
            })
            .ToList();

        return categoryCounts;
    }

    public class EventCategoryCount
    {
        public string? Category { get; set; }
        public int Count { get; set; }
    }

}
