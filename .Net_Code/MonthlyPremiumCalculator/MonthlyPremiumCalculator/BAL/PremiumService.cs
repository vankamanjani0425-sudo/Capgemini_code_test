using MonthlyPremiumCalculator.DAL;
using MonthlyPremiumCalculator.Interfaces;
using MonthlyPremiumCalculator.Models;

namespace MonthlyPremiumCalculator.BAL
{
    public class PremiumService : IPremiumService
    {
        private readonly IOccupationService _occupationService;
        private readonly ILogger<PremiumService> _logger;

        public PremiumService(IOccupationService occupationService, ILogger<PremiumService> logger)
        {
            _occupationService = occupationService;
            _logger = logger;
        }

        public async Task<PremiumResponse> CalculatePremiumAsync(Member member)
        {
            try
            {
                if (member == null)
                    return new PremiumResponse { Error = "Member data is required" };

                if (string.IsNullOrEmpty(member.Name) || member.AgeNextBirthday <= 0 ||
                    member.DeathSumInsured <= 0 || string.IsNullOrEmpty(member.Occupation))
                    return new PremiumResponse { Error = "All fields are required and must be valid" };

                
                var occupations = await _occupationService.GetOccupationsAsync();
                var occupation = occupations.FirstOrDefault(o => o.Name == member.Occupation);

                if (occupation == null)
                    return new PremiumResponse { Error = "Invalid occupation" };

                
                var rating = await _occupationService.GetOccupationRatingAsync(occupation.Rating);
                if (rating == null)
                    return new PremiumResponse { Error = "Invalid occupation rating" };

                
                decimal monthlyPremium = (member.DeathSumInsured * rating.Factor * member.AgeNextBirthday) / (1000 * 12);

                
                var calculation = new PremiumCalculation
                {
                    Id = DataContext.PremiumCalculations.Count + 1,
                    Name = member.Name,
                    AgeNextBirthday = member.AgeNextBirthday,
                    DateOfBirth = member.DateOfBirth,
                    Occupation = member.Occupation,
                    DeathSumInsured = member.DeathSumInsured,
                    MonthlyPremium = monthlyPremium
                };

                DataContext.PremiumCalculations.Add(calculation);

                return new PremiumResponse
                {
                    MonthlyPremium = Math.Round(monthlyPremium, 2),
                    CalculationDetails = $"Premium calculated based on {occupation.Rating} rating with factor {rating.Factor}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating premium");
                return new PremiumResponse { Error = "An error occurred while calculating premium" };
            }
        }

        public async Task<List<PremiumCalculation>> GetCalculationHistoryAsync()
        {
            return await Task.FromResult(DataContext.PremiumCalculations);
        }
    }
}
