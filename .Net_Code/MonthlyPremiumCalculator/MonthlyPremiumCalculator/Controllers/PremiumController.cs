using Microsoft.AspNetCore.Mvc;
using MonthlyPremiumCalculator.DAL;
using MonthlyPremiumCalculator.Interfaces;
using MonthlyPremiumCalculator.Models;

namespace MonthlyPremiumCalculator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PremiumController : ControllerBase
    {
        private readonly IPremiumService _premiumService;

        public PremiumController(IPremiumService premiumService)
        {
            _premiumService = premiumService;
        }

        [HttpPost("calculate")]
        public async Task<ActionResult<PremiumResponse>> CalculatePremium([FromBody] Member member)
        {
            var result = await _premiumService.CalculatePremiumAsync(member);

            if (!string.IsNullOrEmpty(result.Error))
                return BadRequest(new { error = result.Error });

            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<ActionResult<List<PremiumCalculation>>> GetHistory()
        {
            var history = await _premiumService.GetCalculationHistoryAsync();
            return Ok(history);
        }
    }
}
