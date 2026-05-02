using MovieBookingApp.Models;

namespace MovieBookingApp.Interfaces
{
    // ISP: Separate small interfaces instead of one fat interface

    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAll(int? categoryId = null, int? cinemaId = null);
        Movie? GetById(int id);
        Movie? GetByIdWithDetails(int id);
        IEnumerable<Movie> Search(string term);
        void Add(Movie movie);
        void Update(Movie movie);
        void Delete(int id);
    }

    public interface IActorRepository
    {
        IEnumerable<Actor> GetAll();
        Actor? GetById(int id);
        Actor? GetByIdWithMovies(int id);
        void Add(Actor actor);
        void Update(Actor actor);
        void Delete(int id);
    }

    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        Category? GetById(int id);
        Category? GetByIdWithMovies(int id);
        void Add(Category category);
        void Update(Category category);
        void Delete(int id);
    }

    public interface ICinemaRepository
    {
        IEnumerable<Cinema> GetAll();
        Cinema? GetById(int id);
        Cinema? GetByIdWithMovies(int id);
        void Add(Cinema cinema);
        void Update(Cinema cinema);
        void Delete(int id);
    }

    public interface IBookingRepository
    {
        IEnumerable<Booking> GetAll();
        Booking? GetById(int id);
        IEnumerable<Booking> GetByEmail(string email);
        void Add(Booking booking);
        void UpdateStatus(int id, BookingStatus status);
    }

    // Service interfaces (SRP: business logic separate from data access)
    public interface IMovieService
    {
        IEnumerable<Movie> GetMoviesForHome();
        IEnumerable<Movie> GetFilteredMovies(int? categoryId, int? cinemaId, string? search);
        Movie? GetMovieDetails(int id);
    }

    public interface IBookingService
    {
        bool CreateBooking(Booking booking);
        IEnumerable<Booking> GetCustomerBookings(string email);
        bool CancelBooking(int id, string email);
    }
}
