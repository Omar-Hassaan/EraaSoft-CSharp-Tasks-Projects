using MovieBookingApp.Interfaces;
using MovieBookingApp.Models;

namespace MovieBookingApp.Services
{
    // SRP: Services handle business logic only
    // DIP: Depend on interfaces not concrete classes

    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepo;

        public MovieService(IMovieRepository movieRepo)
        {
            _movieRepo = movieRepo;
        }

        public IEnumerable<Movie> GetMoviesForHome() =>
            _movieRepo.GetAll().Where(m => m.IsAvailable).Take(12);

        public IEnumerable<Movie> GetFilteredMovies(int? categoryId, int? cinemaId, string? search)
        {
            if (!string.IsNullOrWhiteSpace(search))
                return _movieRepo.Search(search);

            return _movieRepo.GetAll(categoryId, cinemaId);
        }

        public Movie? GetMovieDetails(int id) =>
            _movieRepo.GetByIdWithDetails(id);
    }

    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IMovieRepository _movieRepo;

        public BookingService(IBookingRepository bookingRepo, IMovieRepository movieRepo)
        {
            _bookingRepo = bookingRepo;
            _movieRepo = movieRepo;
        }

        public bool CreateBooking(Booking booking)
        {
            var movie = _movieRepo.GetById(booking.MovieId);
            if (movie == null || !movie.IsAvailable)
                return false;

            booking.TotalPrice = movie.Price * booking.NumberOfSeats;
            booking.BookingDate = DateTime.Now;
            booking.Status = BookingStatus.Confirmed;

            _bookingRepo.Add(booking);
            return true;
        }

        public IEnumerable<Booking> GetCustomerBookings(string email) =>
            _bookingRepo.GetByEmail(email);

        public bool CancelBooking(int id, string email)
        {
            var booking = _bookingRepo.GetById(id);
            if (booking == null || booking.CustomerEmail != email)
                return false;

            if (booking.Status == BookingStatus.Cancelled)
                return false;

            _bookingRepo.UpdateStatus(id, BookingStatus.Cancelled);
            return true;
        }
    }
}
