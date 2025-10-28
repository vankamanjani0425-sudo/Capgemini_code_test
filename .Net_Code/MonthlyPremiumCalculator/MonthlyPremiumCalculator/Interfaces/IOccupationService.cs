using MonthlyPremiumCalculator.Models;

namespace MonthlyPremiumCalculator.Interfaces
{
    public interface IOccupationService
    {
        Task<List<Occupation>> GetOccupationsAsync();
        Task<OccupationRating> GetOccupationRatingAsync(string rating);
    }
}
