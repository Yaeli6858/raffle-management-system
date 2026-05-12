using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using server.Services.Interfaces;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [DisableRateLimiting]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost("send-mail")]
        public async Task<IActionResult> SendWinningMail([FromQuery] int giftId, [FromQuery] int winnerId)
        {
            await _emailService.SendWinningEmailAsync(giftId, winnerId);
            return NoContent();

        }
    }
}
