using Microsoft.Extensions.Logging;
using PremiumCalculator.Interfaces;
using PremiumCalculator.Models;
using PremiumCalculator.Exceptions;

namespace PremiumCalculator.Services
{
    public class OccupationService : IOccupationService
    {
        private readonly ILogger<OccupationService> _logger;
        private readonly List<Occupation> _occupations;

        public OccupationService(ILogger<OccupationService> logger)
        {
            _logger = logger;
            _occupations = InitializeOccupations();
        }

        public Task<List<Occupation>> GetOccupationsAsync()
        {
            _logger.LogInformation("Retrieving all occupations");
            return Task.FromResult(_occupations);
        }

        public Task<Occupation> GetOccupationByNameAsync(string occupationName)
        {
            _logger.LogInformation("Retrieving occupation by name: {OccupationName}", occupationName);

            var occupation = _occupations.FirstOrDefault(o =>
                o.Name.Equals(occupationName, StringComparison.OrdinalIgnoreCase));

            if (occupation == null)
            {
                _logger.LogWarning("Occupation not found: {OccupationName}", occupationName);
                throw new OccupationNotFoundException($"Occupation '{occupationName}' not found.");
            }

            return Task.FromResult(occupation);
        }

        public async Task<decimal> GetOccupationFactorAsync(string occupationName)
        {
            _logger.LogInformation("Getting occupation factor for: {OccupationName}", occupationName);
            var occupation = await GetOccupationByNameAsync(occupationName);
            return occupation.Factor;
        }

        private List<Occupation> InitializeOccupations()
        {
            var ratingFactors = new Dictionary<string, decimal>
            {
                { "Professional", 1.5m },
                { "White Collar", 2.25m },
                { "Light Manual", 11.50m },
                { "Heavy Manual", 31.75m }
            };

            return new List<Occupation>
            {
                new Occupation { Name = "Cleaner", Rating = "Light Manual", Factor = ratingFactors["Light Manual"] },
                new Occupation { Name = "Doctor", Rating = "Professional", Factor = ratingFactors["Professional"] },
                new Occupation { Name = "Author", Rating = "White Collar", Factor = ratingFactors["White Collar"] },
                new Occupation { Name = "Farmer", Rating = "Heavy Manual", Factor = ratingFactors["Heavy Manual"] },
                new Occupation { Name = "Mechanic", Rating = "Heavy Manual", Factor = ratingFactors["Heavy Manual"] },
                new Occupation { Name = "Florist", Rating = "Light Manual", Factor = ratingFactors["Light Manual"] },
                new Occupation { Name = "Other", Rating = "Heavy Manual", Factor = ratingFactors["Heavy Manual"] }
            };
        }
    }
}