using System.ComponentModel.DataAnnotations;

namespace MovieBookingApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Movie> Movies { get; set; } = new();
    }
}
