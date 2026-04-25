using System.ComponentModel.DataAnnotations;

namespace MovieBookingApp.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Des { get; set; }

        public decimal Price { get; set; }

        public bool Status { get; set; }

        public DateTime DateTime { get; set; }

        public string? MainImg { get; set; }

        public string? SubImages { get; set; }

        // Relations
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int CinemaId { get; set; }
        public Cinema? Cinema { get; set; }

        public List<Actor> Actors { get; set; } = new();
    }
}
