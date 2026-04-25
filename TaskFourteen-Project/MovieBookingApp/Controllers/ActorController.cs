using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBookingApp.Data;
using MovieBookingApp.Models;

namespace MovieBookingApp.Controllers
{
    public class ActorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Actors.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Actor actor)
        {
            if (ModelState.IsValid)
            {
                _context.Actors.Add(actor);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actor);
        }

        public IActionResult Edit(int id)
        {
            var actor = _context.Actors.Find(id);
            if (actor == null) return NotFound();
            return View(actor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Actor actor)
        {
            if (ModelState.IsValid)
            {
                _context.Actors.Update(actor);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actor);
        }

        public IActionResult Delete(int id)
        {
            var actor = _context.Actors.Find(id);
            if (actor == null) return NotFound();
            return View(actor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var actor = _context.Actors.Find(id);
            if (actor != null)
            {
                _context.Actors.Remove(actor);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var actor = _context.Actors
                .Include(a => a.Movies)
                .FirstOrDefault(a => a.Id == id);
            if (actor == null) return NotFound();
            return View(actor);
        }
    }
}
