using Microsoft.AspNetCore.Mvc;
using EventManagementSystem.Data;
using EventManagementSystem.Models.Entities;
using System;
using Microsoft.AspNetCore.Authorization;

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

        // GET: /Events
        public IActionResult Index()
        {
            var events = _context.Events.ToList();
            return View(events);
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
