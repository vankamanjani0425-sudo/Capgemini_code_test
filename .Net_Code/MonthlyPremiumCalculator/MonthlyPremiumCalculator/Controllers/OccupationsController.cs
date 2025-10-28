using Microsoft.AspNetCore.Mvc;
using MonthlyPremiumCalculator.Interfaces;
using MonthlyPremiumCalculator.Models;

namespace MonthlyPremiumCalculator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OccupationsController : ControllerBase
    {
        private readonly IOccupationService _occupationService;

        public OccupationsController(IOccupationService occupationService)
        {
            _occupationService = occupationService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Occupation>>> GetOccupations()
        {
            try
            {
                var occupations = await _occupationService.GetOccupationsAsync();
                return Ok(occupations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}
