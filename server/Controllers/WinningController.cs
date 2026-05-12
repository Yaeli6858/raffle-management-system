using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using server.DTOs;
using server.Services.Implementations;
using server.Services.Interfaces;


namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
[DisableRateLimiting]
[Authorize]
public class WinningController : ControllerBase
{
    private readonly IWinningService _winningService;
    private readonly IRaffleStateService _raffleState;

    public WinningController(IWinningService winningService, IRaffleStateService raffleState)
    {
        _raffleState = raffleState;
        _winningService = winningService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IEnumerable<WinningResponseDto>> GetAllWinningsAsync()
    {
        return await _winningService.GetAllWinningsAsync();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<WinningResponseDto> GetWinningByIdAsync(int id)
    {
        return await _winningService.GetWinningByIdAsync(id);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<WinningResponseDto> AddWinningAsync([FromBody] WinningCreateDto dto)
         => await _winningService.AddWinningAsync(dto);

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<WinningResponseDto> UpdateWinningAsync(int id, [FromBody] WinningCreateDto dto)
            => await _winningService.UpdateWinningAsync(id, dto);

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWinningAsync(int id)
    {
        await _winningService.DeleteWinningAsync(id);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("total-income")]
    public async Task<decimal> GetTotalIncome()
    {
        return await _winningService.GetTotalIncome();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("doRaffle")]
    public async Task<IEnumerable<WinningResponseDto>> DoRaffle()
    {
        return await _winningService.RaffleAsync();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("raffle-single/{giftId}")]
    public async Task<WinningResponseDto?> RaffleSingleGift(int giftId)
    {
        return await _winningService.RaffleSingleGiftAsync(giftId);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("statusIsFinished")]
    public IActionResult GetRaffleStatus()
    {
        return Ok(_raffleState.isFinished());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("finishRaffle")]
    public IActionResult FinishRaffle()
    {
        _raffleState.FinishRaffle();
        return Ok(_raffleState.isFinished());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("resetStatus")]
    public IActionResult ResetRaffleStatus()
    {
        _raffleState.Reset();
        return Ok(_raffleState.Status == RaffleStatus.Open);

    }
    
    [HttpGet("sorted-by-most-purchased")]
    public async Task<ActionResult<IEnumerable<WinningResponseDto>>> GetWinningsSortedByMostPurchasedGift()
    {
        var winnings = await _winningService.GetWinningsSortedByMostPurchasedGiftAsync();
        return Ok(winnings);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<WinningResponseDto>>> SearchWinnings(
        [FromQuery] string? giftName,
        [FromQuery] string? donorName,
        [FromQuery] int? minPurchases)
    {
        var results = await _winningService.SearchWinningsAsync(giftName, donorName, minPurchases);
        return Ok(results);
    }


}