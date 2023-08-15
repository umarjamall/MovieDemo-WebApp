using System.ComponentModel.DataAnnotations;

namespace MovieDemo.Models
{
    public class Genre
    {
        public byte GenreID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
