using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class ReportController : Controller
{
    private readonly ReportServices _reportService;

    public ReportController(ReportServices reportService)
    {
        _reportService = reportService;
    }
    [Authorize]
    public async Task<IActionResult> Index(string? search, string? category, DateTime? fromDate, DateTime? toDate)
    {
        var report = await _reportService.GetMonthlyEventReportAsync(search, category, fromDate, toDate);
        ViewBag.Search = search;
        ViewBag.Category = category;
        ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
        ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");
        return View(report);
    }
}
