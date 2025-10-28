using MonthlyPremiumCalculator.DAL;
using MonthlyPremiumCalculator.Interfaces;
using MonthlyPremiumCalculator.Models;

namespace MonthlyPremiumCalculator.BAL
{
    public class OccupationService :IOccupationService
    {
        public async Task<List<Occupation>> GetOccupationsAsync()
        {
            return await Task.FromResult(DataContext.Occupations);
        }

        public async Task<OccupationRating> GetOccupationRatingAsync(string rating)
        {
            var ratingObj = DataContext.OccupationRatings.FirstOrDefault(r => r.Rating == rating);
            return await Task.FromResult(ratingObj);
        }
    }
}
