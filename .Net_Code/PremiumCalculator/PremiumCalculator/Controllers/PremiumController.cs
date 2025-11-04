using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PremiumCalculator.Interfaces;
using PremiumCalculator.Models;
using PremiumCalculator.Exceptions;

namespace PremiumCalculator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PremiumController : ControllerBase
    {
        private readonly ILogger<PremiumController> _logger;
        private readonly IPremiumService _premiumService;
        private readonly IOccupationService _occupationService;

        public PremiumController(
            ILogger<PremiumController> logger,
            IPremiumService premiumService,
            IOccupationService occupationService)
        {
            _logger = logger;
            _premiumService = premiumService;
            _occupationService = occupationService;
        }

        [HttpPost("calculate")]
        public async Task<ActionResult<PremiumResponse>> CalculatePremium([FromBody] PremiumRequest request)
        {
            try
            {
                _logger.LogInformation("Received premium calculation request for {Name}", request.Name);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid request model for premium calculation");
                    return BadRequest(ModelState);
                }

                var result = await _premiumService.CalculatePremiumAsync(request);
                return Ok(result);
            }
            catch (OccupationNotFoundException ex)
            {
                _logger.LogWarning(ex, "Occupation not found in calculation request");
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidCalculationException ex)
            {
                _logger.LogWarning(ex, "Invalid calculation request");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in premium calculation");
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [HttpGet("occupations")]
        public async Task<ActionResult<List<Occupation>>> GetOccupations()
        {
            try
            {
                _logger.LogInformation("Retrieving occupations list");
                var occupations = await _occupationService.GetOccupationsAsync();
                return Ok(occupations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving occupations");
                return StatusCode(500, new { error = "An error occurred while retrieving occupations." });
            }
        }
    }
}