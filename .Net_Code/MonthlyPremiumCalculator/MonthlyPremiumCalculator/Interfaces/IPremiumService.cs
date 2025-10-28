using MonthlyPremiumCalculator.DAL;
using MonthlyPremiumCalculator.Models;

namespace MonthlyPremiumCalculator.Interfaces
{
    public interface IPremiumService
    {
        Task<PremiumResponse> CalculatePremiumAsync(Member member);
        Task<List<PremiumCalculation>> GetCalculationHistoryAsync();
    }
}
