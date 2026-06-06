using System.ComponentModel.DataAnnotations;

namespace MovieBookingApp.Models
{
    // Explicit join table for Movie <-> Actor (better than implicit for SOLID)
    public class MovieActor
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;

        public int ActorId { get; set; }
        public Actor Actor { get; set; } = null!;
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }

    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        public string CustomerPhone { get; set; } = string.Empty;

        public int NumberOfSeats { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        // FK
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}
