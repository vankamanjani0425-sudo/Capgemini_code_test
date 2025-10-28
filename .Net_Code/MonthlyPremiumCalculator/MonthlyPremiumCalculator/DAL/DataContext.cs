using MonthlyPremiumCalculator.Models;

namespace MonthlyPremiumCalculator.DAL
{
    
        public static class DataContext
        {
            public static List<Occupation> Occupations = new()
        {
            new Occupation { Id = 1, Name = "Cleaner", Rating = "Light Manual" },
            new Occupation { Id = 2, Name = "Doctor", Rating = "Professional" },
            new Occupation { Id = 3, Name = "Author", Rating = "White Collar" },
            new Occupation { Id = 4, Name = "Farmer", Rating = "Heavy Manual" },
            new Occupation { Id = 5, Name = "Mechanic", Rating = "Heavy Manual" },
            new Occupation { Id = 6, Name = "Florist", Rating = "Light Manual" },
            new Occupation { Id = 7, Name = "Other", Rating = "Heavy Manual" }
        };

            public static List<OccupationRating> OccupationRatings = new()
        {
            new OccupationRating { Rating = "Professional", Factor = 1.5m },
            new OccupationRating { Rating = "White Collar", Factor = 2.25m },
            new OccupationRating { Rating = "Light Manual", Factor = 11.50m },
            new OccupationRating { Rating = "Heavy Manual", Factor = 31.75m }
        };

            public static List<PremiumCalculation> PremiumCalculations = new();
        }

        public class PremiumCalculation
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int AgeNextBirthday { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string Occupation { get; set; }
            public decimal DeathSumInsured { get; set; }
            public decimal MonthlyPremium { get; set; }
            public DateTime CalculatedDate { get; set; } = DateTime.UtcNow;
        }
    

}
