using Microsoft.AspNetCore.Mvc;
using MovieBookingApp.Interfaces;
using MovieBookingApp.Models;

namespace MovieBookingApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IActorRepository _actorRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ICinemaRepository _cinemaRepo;

        public HomeController(
            IMovieService movieService,
            IActorRepository actorRepo,
            ICategoryRepository categoryRepo,
            ICinemaRepository cinemaRepo)
        {
            _movieService = movieService;
            _actorRepo = actorRepo;
            _categoryRepo = categoryRepo;
            _cinemaRepo = cinemaRepo;
        }

        public IActionResult Index()
        {
            var movies = _movieService.GetMoviesForHome();
            ViewBag.Actors = _actorRepo.GetAll().Take(8).ToList();
            ViewBag.Categories = _categoryRepo.GetAll().ToList();
            ViewBag.Cinemas = _cinemaRepo.GetAll().ToList();
            return View(movies);
        }
    }

    [Area("Customer")]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ICinemaRepository _cinemaRepo;

        public MovieController(
            IMovieService movieService,
            ICategoryRepository categoryRepo,
            ICinemaRepository cinemaRepo)
        {
            _movieService = movieService;
            _categoryRepo = categoryRepo;
            _cinemaRepo = cinemaRepo;
        }

        public IActionResult Index(int? categoryId, int? cinemaId, string? search)
        {
            var movies = _movieService.GetFilteredMovies(categoryId, cinemaId, search);
            ViewBag.Categories = _categoryRepo.GetAll().ToList();
            ViewBag.Cinemas = _cinemaRepo.GetAll().ToList();
            ViewBag.SelectedCategory = categoryId;
            ViewBag.SelectedCinema = cinemaId;
            ViewBag.Search = search;
            return View(movies);
        }

        public IActionResult Details(int id)
        {
            var movie = _movieService.GetMovieDetails(id);
            if (movie == null) return NotFound();
            return View(movie);
        }
    }

    [Area("Customer")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IMovieService _movieService;

        public BookingController(IBookingService bookingService, IMovieService movieService)
        {
            _bookingService = bookingService;
            _movieService = movieService;
        }

        public IActionResult Create(int movieId)
        {
            var movie = _movieService.GetMovieDetails(movieId);
            if (movie == null || !movie.IsAvailable)
                return RedirectToAction("Index", "Movie");

            var booking = new Booking { MovieId = movieId };
            ViewBag.Movie = movie;
            return View(booking);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                var success = _bookingService.CreateBooking(booking);
                if (success)
                {
                    TempData["Success"] = "Booking confirmed! 🎬";
                    TempData["BookingEmail"] = booking.CustomerEmail;
                    return RedirectToAction(nameof(Confirmation));
                }
                ModelState.AddModelError("", "Movie is not available for booking.");
            }
            ViewBag.Movie = _movieService.GetMovieDetails(booking.MovieId);
            return View(booking);
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        public IActionResult MyBookings(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return View("LookupBookings");

            var bookings = _bookingService.GetCustomerBookings(email);
            ViewBag.Email = email;
            return View(bookings);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Cancel(int id, string email)
        {
            var success = _bookingService.CancelBooking(id, email);
            TempData[success ? "Success" : "Error"] = success
                ? "Booking cancelled successfully."
                : "Unable to cancel this booking.";
            return RedirectToAction(nameof(MyBookings), new { email });
        }
    }
}
