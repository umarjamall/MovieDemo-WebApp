using MovieDemo.Models;
using System.ComponentModel.DataAnnotations;

namespace MovieDemo.ViewModels
{
    public class MovieFormViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        public int Year { get; set; }

        [Range(0, 10)]
        public float Rate { get; set; }

        public byte[]? Poster { get; set; }
        [Display(Name = "Genre")]
        public byte? GenreID { get; set; }
        public IEnumerable<Genre>? Genres { get; set; }
    }
}
