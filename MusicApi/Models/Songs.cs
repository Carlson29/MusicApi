using System.ComponentModel.DataAnnotations;

namespace MusicApi.Models
{
    public class Songs
    {
         [Key]
        public int Id { get; set; }
         [Required]
        public string Title {  get; set; }
         [Required]
        public string Genre { get; set; }
        [Required]
        public TimeSpan Duration { get; set; }
        
        public DateTime? ReleaseDate { get; set; }

    }
}
