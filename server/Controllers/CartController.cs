using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Services.Interfaces;
using System.Security.Claims;

namespace server.Controllers;

[ApiController]
[DisableRateLimiting]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _service;

    public CartController(ICartService service)
    {
        _service = service;
    }

    [HttpGet("cart")]
    public async Task<ActionResult<IEnumerable<CartItemResponseDto>>> GetCart()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);
        return Ok(await _service.GetCartAsync(userId));
    }


    [HttpPost]
    public async Task<ActionResult<IEnumerable<CartItemResponseDto>>> Add([FromBody] CartAddDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);

        var created = await _service.AddToCartAsync(dto, userId);
        return Ok(created);
    }


    [HttpPut]
    public async Task<ActionResult<IEnumerable<CartItemResponseDto>>> UpdateQty([FromBody] CartAddDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);
        var updated = await _service.UpdateQtyAsync(dto, userId);
        return Ok(updated);
    }


    [HttpDelete("{purchaseId:int}")]
    public async Task<IActionResult> Remove(int purchaseId)
    {
        await _service.RemoveAsync(purchaseId);
        return NoContent();
    }

    [HttpPost("checkout")]
    [ProducesResponseType(typeof(CartCheckoutResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Checkout()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);
        return Ok(await _service.CheckoutAsync(userId));
    }

}