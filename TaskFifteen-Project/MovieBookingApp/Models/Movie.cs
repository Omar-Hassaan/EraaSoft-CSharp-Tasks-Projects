using System.ComponentModel.DataAnnotations;

namespace MovieBookingApp.Models
{
    // SRP: Each model only holds its own data
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Movie name is required")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Range(0, 10000)]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime ShowDateTime { get; set; }

        [StringLength(500)]
        public string? MainImageUrl { get; set; }

        [StringLength(2000)]
        public string? SubImagesUrls { get; set; }

        // Foreign Keys
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int CinemaId { get; set; }
        public Cinema? Cinema { get; set; }

        // Many-to-Many
        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();

        // Booking relation
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
