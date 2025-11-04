using Microsoft.Extensions.Logging;
using PremiumCalculator.Exceptions;
using PremiumCalculator.Interfaces;
using PremiumCalculator.Models;

namespace PremiumCalculator.Services
{
    public class PremiumService : IPremiumService
    {
        private readonly ILogger<PremiumService> _logger;
        private readonly IOccupationService _occupationService;

        public PremiumService(ILogger<PremiumService> logger, IOccupationService occupationService)
        {
            _logger = logger;
            _occupationService = occupationService;
        }

        public async Task<PremiumResponse> CalculatePremiumAsync(PremiumRequest request)
        {
            try
            {
                _logger.LogInformation("Starting premium calculation for {Name}", request.Name);

                // Parse the string date to DateTime for validation
                if (!TryParseDateOfBirth(request.DateOfBirth, out DateTime dateOfBirth))
                {
                    _logger.LogWarning("Invalid Date of Birth format: {DateOfBirth}", request.DateOfBirth);
                    throw new InvalidCalculationException("Date of Birth must be in MM/YYYY format.");
                }

                // Validate age consistency
                var calculatedAge = CalculateAgeFromDOB(dateOfBirth);
                if (Math.Abs(calculatedAge - request.AgeNextBirthday) > 1)
                {
                    _logger.LogWarning("Age inconsistency detected for {Name}. DOB: {DOB}, Age: {Age}",
                        request.Name, request.DateOfBirth, request.AgeNextBirthday);
                    throw new InvalidCalculationException("Age and Date of Birth do not match.");
                }

                // Get occupation factor
                var occupationFactor = await _occupationService.GetOccupationFactorAsync(request.Occupation);

                // Calculate premium using formula: (Death Cover * Occupation Rating Factor * Age) / 1000 * 12
                var monthlyPremium = (request.DeathSumInsured * occupationFactor * request.AgeNextBirthday) / (1000 * 12);

                _logger.LogInformation("Premium calculated successfully for {Name}: {MonthlyPremium}",
                    request.Name, monthlyPremium);

                return new PremiumResponse
                {
                    MonthlyPremium = Math.Round(monthlyPremium, 2),
                    CalculationDetails = $"Premium calculated based on {request.Occupation} occupation with factor {occupationFactor}"
                };
            }
            catch (Exception ex) when (ex is not InvalidCalculationException)
            {
                _logger.LogError(ex, "Error calculating premium for {Name}", request.Name);
                throw new InvalidCalculationException("An error occurred while calculating premium.", ex);
            }
        }

        // ADD THIS MISSING METHOD
        private bool TryParseDateOfBirth(string dateOfBirthString, out DateTime dateOfBirth)
        {
            dateOfBirth = DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(dateOfBirthString))
                return false;

            try
            {
                var parts = dateOfBirthString.Split('/');
                if (parts.Length != 2)
                    return false;

                if (int.TryParse(parts[0], out int month) && int.TryParse(parts[1], out int year))
                {
                    // Validate month range
                    if (month < 1 || month > 12)
                        return false;

                    // Validate year range (reasonable bounds)
                    if (year < 1900 || year > DateTime.Now.Year)
                        return false;

                    // Create a date representing the first day of the month
                    dateOfBirth = new DateTime(year, month, 1);
                    return true;
                }
            }
            catch
            {
                // Log the error but don't throw, just return false
                return false;
            }

            return false;
        }

        private int CalculateAgeFromDOB(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;

            // If the birthday hasn't occurred this year yet, subtract one year
            if (dateOfBirth.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }
}