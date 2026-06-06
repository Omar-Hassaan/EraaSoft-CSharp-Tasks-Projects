using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieBookingApp.Interfaces;
using MovieBookingApp.Models;

namespace MovieBookingApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ICinemaRepository _cinemaRepo;

        // DIP: Injecting interfaces, not concrete classes
        public MovieController(
            IMovieRepository movieRepo,
            ICategoryRepository categoryRepo,
            ICinemaRepository cinemaRepo)
        {
            _movieRepo = movieRepo;
            _categoryRepo = categoryRepo;
            _cinemaRepo = cinemaRepo;
        }

        public IActionResult Index()
        {
            var movies = _movieRepo.GetAll();
            return View(movies);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _movieRepo.Add(movie);
                TempData["Success"] = "Movie added successfully!";
                return RedirectToAction(nameof(Index));
            }
            LoadDropdowns();
            return View(movie);
        }

        public IActionResult Edit(int id)
        {
            var movie = _movieRepo.GetById(id);
            if (movie == null) return NotFound();
            LoadDropdowns(movie.CategoryId, movie.CinemaId);
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _movieRepo.Update(movie);
                TempData["Success"] = "Movie updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            LoadDropdowns(movie.CategoryId, movie.CinemaId);
            return View(movie);
        }

        public IActionResult Details(int id)
        {
            var movie = _movieRepo.GetByIdWithDetails(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        public IActionResult Delete(int id)
        {
            var movie = _movieRepo.GetByIdWithDetails(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _movieRepo.Delete(id);
            TempData["Success"] = "Movie deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns(int? selectedCategory = null, int? selectedCinema = null)
        {
            ViewBag.Categories = new SelectList(_categoryRepo.GetAll(), "Id", "Name", selectedCategory);
            ViewBag.Cinemas = new SelectList(_cinemaRepo.GetAll(), "Id", "Name", selectedCinema);
        }
    }
}
