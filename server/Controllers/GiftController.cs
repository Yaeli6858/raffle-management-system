using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using server.Services.Interfaces;
using server.DTOs;
using server.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;


namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
[DisableRateLimiting]
public class GiftController : ControllerBase
{
    private readonly IGiftService _giftService;
    private readonly ILogger<GiftController> _logger;

    public GiftController(IGiftService giftService, ILogger<GiftController> logger)
    {
        _giftService = giftService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GiftResponseDto>>> GetAllGifts(
    [FromQuery] PriceSort sort = PriceSort.None,
    [FromQuery] int? categoryId = null,
    [FromQuery] int? donorId = null)
    {
        var gifts = await _giftService.GetAllGiftsAsync(sort, categoryId, donorId);
        return Ok(gifts);
    }


    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<GiftResponseDto>>> GetAll(PriceSort sort)
    {
        var gifts = await _giftService.GetAllAsync(sort);
        return Ok(gifts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GiftResponseDto>> GetById(int id)
    {
        var gift = await _giftService.GetGiftByIdAsync(id);
        if (gift == null)
            return NotFound();

        return Ok(gift);
    }

    [HttpGet("byCategory/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<GiftResponseDto>>> GetByCategory(int categoryId)
        => Ok(await _giftService.GetByGiftByCategoryAsync(categoryId));


    [HttpGet("byDonor/{donorId:int}")]
    public async Task<ActionResult<IEnumerable<GiftResponseDto>>> GetByDonor(int donorId)
        => Ok(await _giftService.GetByDonorAsync(donorId));


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<GiftResponseDto>> CreateWithImage([FromForm] GiftCreateWithImageDto dto)
    {
        var gift = await _giftService.AddGiftAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = gift.Id }, gift);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<GiftResponseDto>> Update(int id, [FromForm] GiftUpdateWithImageDto dto)
    {
        var gift = await _giftService.UpdateGiftAsync(id, dto);
        return Ok(gift);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _giftService.DeleteGiftAsync(id);
        return NoContent();
    }

    [HttpGet("purchaseCount")]
    public async Task<ActionResult<IEnumerable<GiftPurchaseCountDto>>> GetPurchaseCountByGift()
    {
        var purchaseCounts = await _giftService.GetPurchaseCountByGiftAsync();
        return Ok(purchaseCounts);
    }

}