using System.ComponentModel.DataAnnotations;

namespace InputValidationDemo.Models
{
    public class InputModel
    {
        [Required]
        [Range(1,5)]
        public int EgySzam { get; set; }

        [Required]
        [MaxLength(10)]
        [MinLength(3)]
        public string EgySzoveg { get; set; }
    }
}