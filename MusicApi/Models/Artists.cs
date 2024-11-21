using System.ComponentModel.DataAnnotations;

namespace MusicApi.Models
{
    public class Artists : Interface
    {
        [Key]
        public int Id { get; set ; }
       [Required]
        public string Artist_Name { get; set ; }
        public string? Bio { get; set ; }
        public DateTime? DateOfBirth { get; set ; }

        public int getAge()
        {
            int years = 0;
            if (DateOfBirth!=null)
            {
              int days=  DateTime.Now.Subtract((DateTime)DateOfBirth).Days;
                 years = days / 365;
                
            }
            return years;
        }

    }
}
