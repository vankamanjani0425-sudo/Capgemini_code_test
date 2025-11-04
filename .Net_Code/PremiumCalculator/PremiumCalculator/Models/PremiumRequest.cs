using System.ComponentModel.DataAnnotations;

namespace PremiumCalculator.Models
{
    public class PremiumRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100")]
        public int AgeNextBirthday { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{4}$", ErrorMessage = "Date of Birth must be in MM/YYYY format")]
        public string DateOfBirth { get; set; }

        [Required(ErrorMessage = "Occupation is required")]
        public string Occupation { get; set; }

        [Required(ErrorMessage = "Death Sum Insured is required")]
        [Range(1000, 10000000, ErrorMessage = "Death Sum Insured must be between 1,000 and 10,000,000")]
        public decimal DeathSumInsured { get; set; }
    }
}