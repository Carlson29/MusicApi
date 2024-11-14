using System.ComponentModel.DataAnnotations;
namespace MusicApi.Models
{
    public class Artists_Songs
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Song_Id { get; set; }
        [Required]
        public int Artist_Id { get; set; }
    }
}
