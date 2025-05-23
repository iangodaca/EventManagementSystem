﻿namespace EventManagementSystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using EventManagementSystem.Data;
    using EventManagementSystem.Models.Entities;
    using System;
    using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Create(Event evt, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    try
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/events");
                        Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        evt.ImagePath = "/images/events/" + uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the error here
                        ModelState.AddModelError("", "Image upload failed: " + ex.Message);
                        return View(evt);
                    }
                }

                _context.Events.Add(evt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(evt);
        }


        [Authorize]

        // GET: /Events
        public IActionResult Index(string search, string category, DateTime? fromDate, DateTime? toDate)
        {
            var events = _context.Events.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                events = events.Where(e => e.EventTitle.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                events = events.Where(e => e.EventCategory.Contains(category));
            }

            if (fromDate.HasValue)
            {
                events = events.Where(e => e.EventDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                events = events.Where(e => e.EventDate <= toDate.Value);
            }

            ViewBag.Search = search;
            ViewBag.Category = category;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(events.ToList());
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
        public async Task<IActionResult> Edit(Event evt, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/events");
                    Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    if (!string.IsNullOrEmpty(evt.ImagePath))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", evt.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    evt.ImagePath = "/images/events/" + uniqueFileName;
                }
                else
                {
                    var currentImagePath = _context.Events
                        .Where(e => e.Id == evt.Id)
                        .Select(e => e.ImagePath)
                        .FirstOrDefault();
                
                    evt.ImagePath = currentImagePath;
                }

                _context.Events.Update(evt);
                await _context.SaveChangesAsync();
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
