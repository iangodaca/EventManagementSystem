using Microsoft.AspNetCore.Mvc;
using EventManagementSystem.Data;
using EventManagementSystem.Models.Entities;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventDbContext _context;

        public EventsController(EventDbContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: /Events/Create
        public IActionResult Create()
        {
            return View();
        }
        

        // POST: /Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Event evt)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Add(evt);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(evt);
        }
        [Authorize]
        public async Task<IActionResult> Index(string? search, string? category, DateTime? fromDate, DateTime? toDate)
        {
            var categories = await _context.Events
                .Where(e => !string.IsNullOrEmpty(e.EventCategory))
                .Select(e => e.EventCategory)
                .Distinct()
                .ToListAsync();

            ViewBag.Categories = categories;

            var events = _context.Events.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                events = events.Where(e => e.EventTitle!.Contains(search));

            if (!string.IsNullOrEmpty(category))
                events = events.Where(e => e.EventCategory == category);

            if (fromDate.HasValue)
                events = events.Where(e => e.EventDate >= fromDate.Value);

            if (toDate.HasValue)
                events = events.Where(e => e.EventDate <= toDate.Value);

            var result = await events.OrderBy(e => e.EventDate).ToListAsync();

            ViewBag.Search = search;
            ViewBag.Category = category;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(result);
        }

        [Authorize]

        // GET: /Events/Edit/5
        public IActionResult Edit(Guid id)
        {
            var evt = _context.Events.Find(id);
            if (evt == null) return NotFound();
            return View(evt);
        }


        // POST: /Events/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Event evt)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Update(evt);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(evt);
        }
        [Authorize]
        // GET: /Events/Delete/5
        public IActionResult Delete(Guid id)
        {
            var evt = _context.Events.Find(id);
            if (evt == null) return NotFound();

            _context.Events.Remove(evt);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
