using System.ComponentModel.DataAnnotations;

namespace MusicApi.Models
{
    public class Users
    {
         [Key]
        public int Id { get; set; }
         [Required]
        public string User_Name { get; set; }
         [Required]
         [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
