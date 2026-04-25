using System.ComponentModel.DataAnnotations;

namespace MovieBookingApp.Models
{
    public class Cinema
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Img { get; set; }

        public List<Movie> Movies { get; set; } = new();
    }
}
