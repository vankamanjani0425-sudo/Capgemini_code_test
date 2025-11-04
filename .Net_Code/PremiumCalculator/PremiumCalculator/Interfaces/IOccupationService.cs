using PremiumCalculator.Models;

namespace PremiumCalculator.Interfaces
{
    public interface IOccupationService
    {
        Task<List<Occupation>> GetOccupationsAsync();
        Task<Occupation> GetOccupationByNameAsync(string occupationName);
        Task<decimal> GetOccupationFactorAsync(string occupationName);
    }
}
