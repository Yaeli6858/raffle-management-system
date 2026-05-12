using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Services.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
[DisableRateLimiting]
[Authorize]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseService _service;

    public PurchaseController(IPurchaseService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PurchaseResponseDto>>> GetAll()
            => Ok(await _service.GetAllAsync());



    [HttpGet("{id:int}")]
    public async Task<ActionResult<PurchaseResponseDto?>> GetById(int id)
    {
        var purchase = await _service.GetByIdAsync(id);
        return Ok(purchase);
    }


    [HttpPost]
    public async Task<ActionResult<IEnumerable<PurchaseResponseDto>>> Create([FromBody] PurchaseCreateDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var created = await _service.AddAsync(userId, dto);
        return Ok(created);
    }


    [HttpPut]
    public async Task<ActionResult<IEnumerable<PurchaseResponseDto?>>> Update([FromBody] PurchaseUpdateDto dto)
    {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _service.UpdateAsync(userId, dto);
        return NoContent();
    }



    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        return ok ? NoContent() : NotFound(new { message = $"Purchase with ID {id} not found." });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("byGift/{giftId:int}")]
    public async Task<ActionResult<IEnumerable<PurchaseResponseDto>>> GetByGift(int giftId)
        => Ok(await _service.GetByGiftAsync(giftId));


    [Authorize(Roles = "Admin")]
    [HttpGet("count-by-gift")]
    public async Task<ActionResult<IEnumerable<GiftPurchaseCountDto>>> GetPurchaseCountByGift()
    => Ok(await _service.GetPurchaseCountByGiftAsync());

}

