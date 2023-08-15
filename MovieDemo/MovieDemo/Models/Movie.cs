using System.ComponentModel.DataAnnotations;

namespace MovieDemo.Models
{
    public class Movie
    {
        public int MovieID { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        
        public int Year { get; set; }
        
        [Range(0, 10)]
        public float Rate { get; set; }
        [Required]
        public byte[] Poster { get; set; }
        public byte GenreID { get; set; }
        public Genre Genre { get; set; }
    }
}
