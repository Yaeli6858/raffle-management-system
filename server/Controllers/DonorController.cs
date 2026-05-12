using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using server.DTOs.Donors;
using server.Models;
using server.Services.Interfaces;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [DisableRateLimiting]
    [Authorize]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService _donorService;
        private readonly IUserService _userService;
        private readonly ILogger<DonorController> _logger;

        

        public DonorController(IDonorService donorService, IUserService userService, ILogger<DonorController> logger)
        {
            _donorService = donorService;
            _userService = userService;
            _logger = logger;
        }

        

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<DonorListItemDto>>> GetDonors(
                            [FromQuery] string? search,
                            [FromQuery] string? city)
                            => Ok(await _donorService.GetDonorsAsync(search, city));

        [Authorize(Roles = "Admin")]
        [HttpGet("with-gifts")]
        public async Task<ActionResult<IEnumerable<DonorWithGiftsDto>>> GetDonorsWithGifts()
        {
            var donorsWithGifts = await _donorService.GetDonorsWithGiftsAsync();
            return Ok(donorsWithGifts);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("role/{userId}")]
        public async Task<IActionResult> SetRole(int userId, [FromQuery] RoleEnum role)
        {
            await _donorService.SetUserRoleAsync(userId, role);
            return NoContent();
        }

        [Authorize(Roles = "Donor")]
        [HttpGet("dashboard")]
        public async Task<ActionResult<DonorDashboardResponseDto>> GetDonorDashboard()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim);

            return await _donorService.GetDonorDashboardAsync(userId);
        }
        

        [Authorize(Roles = "Donor")]
        [HttpGet("details")]
        public async Task<ActionResult<DonorListItemDto?>> GetDonorDetails()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim);

            return await _donorService.GetDonorDetailsAsync(userId);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddDonor([FromBody] addDonorDto donorDto)
        {
            await _donorService.AddDonorAsync(donorDto);
            return Ok(donorDto);

        }
    }
}