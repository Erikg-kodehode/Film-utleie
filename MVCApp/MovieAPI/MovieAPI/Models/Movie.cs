using System.ComponentModel.DataAnnotations;
using MovieAPI.Models;

namespace MovieAPI.Models
{
    public class Movie
    {
        [Key] public int Id { get; set; }

        [Required] public string Title { get; set; } = string.Empty;
        public int Year { get; set; }

        [Required] public string Genre { get; set; } = string.Empty;
    }
}
