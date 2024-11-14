using System.ComponentModel.DataAnnotations;

namespace MusicApi.Models
{
    public class Artists
    {
        [Key]
        public int Id { get; set ; }
       [Required]
        public string Artist_Name { get; set ; }
        public string? Bio { get; set ; }
        public DateTime? DateOfBirth { get; set ; }

    }
}
