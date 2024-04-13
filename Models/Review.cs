using System.ComponentModel.DataAnnotations;

namespace EatEvals.Models
{
    public class Review
    {
        public int Id { get; set; }

        
        [Required]
        public string Restaurant { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        public string Food { get; set; }

        [RegularExpression(@"^\d{1,7}(\.\d{1,2})?$", ErrorMessage = "The price must be up to 7 whole digits and up to 2 decimal digits.")]
        [Range(0, 9999999.99, ErrorMessage = "The price must be in the range of 0 to 9999999.99")]
        public float Price { get; set; }
        public DateTime Date { get; set; }

        public byte[]? Image { get; set; }
    }
}
