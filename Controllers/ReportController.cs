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
        var report = await GetMonthlyEventReportAsync(search, category, fromDate, toDate);

        ViewBag.Search = search;
        ViewBag.Category = category;
        ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
        ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

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
}
