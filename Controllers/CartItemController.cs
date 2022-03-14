using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSI5112BackEndApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartItemsController : ControllerBase
{
    private readonly CartItemsService _CartItemsService;

    public CartItemsController(CartItemsService CartItemsService) =>
        _CartItemsService = CartItemsService;

    [HttpGet("all")]
    public async Task<List<CartItem>> Get() =>
        await _CartItemsService.GetAllCartItem();

    [HttpGet]
    public async Task<List<CartItem>> Get([FromQuery]string item_id) =>
        await _CartItemsService.GetCartItemByID(item_id);

    [HttpPost("create")]
    public async Task<IActionResult> Post([FromBody] CartItem newCartItem)
    {
        await _CartItemsService.CreateNewCartItem(newCartItem);

        return CreatedAtAction(nameof(Get), new { id = newCartItem.customer_id }, newCartItem);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromQuery]string item_id, [FromBody] CartItem updatedCartItem)
    {
        var cartItem = await _CartItemsService.GetCartItemByID(item_id);

        if (cartItem is null)
        {
            return NotFound();
        }

        updatedCartItem.customer_id = cartItem[0].customer_id;

        await _CartItemsService.UpdateCartItem(item_id, updatedCartItem);

        return NoContent();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] string[] ids)
    {
        await _CartItemsService.RemoveCartItem(ids);

        return NoContent();
    }
}