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

    [HttpGet]
    public async Task<List<CartItem>> Get() =>
        await _CartItemsService.GetAllCartItem();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<CartItem>> Get(string id)
    {
        var cartItem = await _CartItemsService.GetCartItemByID(id);

        if (cartItem is null)
        {
            return NotFound();
        }

        return cartItem;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CartItem newCartItem)
    {
        await _CartItemsService.CreateNewCartItem(newCartItem);

        return CreatedAtAction(nameof(Get), new { id = newCartItem.customer_id }, newCartItem);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, CartItem updatedCartItem)
    {
        var cartItem = await _CartItemsService.GetCartItemByID(id);

        if (cartItem is null)
        {
            return NotFound();
        }

        updatedCartItem.customer_id = cartItem.customer_id;

        await _CartItemsService.UpdateCartItem(id, updatedCartItem);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var CartItem = await _CartItemsService.GetCartItemByID(id);

        if (CartItem is null)
        {
            return NotFound();
        }

        await _CartItemsService.RemoveCartItem(id);

        return NoContent();
    }
}