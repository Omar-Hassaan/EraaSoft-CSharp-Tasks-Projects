using System.ComponentModel.DataAnnotations;

namespace MovieBookingApp.Models
{
    public class Cinema
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Cinema name is required")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [StringLength(500)]
        public string? Location { get; set; }

        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
