namespace MonthlyPremiumCalculator.Models
{
    public class PremiumResponse
    {
        public decimal MonthlyPremium { get; set; }
        public string CalculationDetails { get; set; }
        public string Error { get; set; }
    }
}
