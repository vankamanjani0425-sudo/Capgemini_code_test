using PremiumCalculator.Models;

namespace PremiumCalculator.Interfaces
{
    public interface IPremiumService
    {
        Task<PremiumResponse> CalculatePremiumAsync(PremiumRequest request);
    }
}
