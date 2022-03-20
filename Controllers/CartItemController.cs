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

    [HttpGet("by_customer")]
    public async Task<List<CartItem>> Gets([FromQuery]string customer_id) =>
        await _CartItemsService.GetCartItemByCustomer(customer_id);

    [HttpPost("create")]
    public async Task<List<CartItem>> Post([FromBody] CartItem newCartItem)
    {
        await _CartItemsService.CreateNewCartItem(newCartItem);

        return await _CartItemsService.GetCartItemByCustomer(newCartItem.customer_id);
    }

    [HttpPut("update")]
    public async Task<List<CartItem>> Update([FromQuery]string item_id, [FromBody] CartItem updatedCartItem)
    {
        var cartItem = await _CartItemsService.GetCartItemByID(item_id);

        updatedCartItem.item_id = cartItem[0].item_id;

        await _CartItemsService.UpdateCartItem(item_id, updatedCartItem);

        return await _CartItemsService.GetCartItemByCustomer(updatedCartItem.customer_id);
    }

    [HttpDelete("delete")]
    public async Task<List<CartItem>> Delete([FromBody] string[] ids)
    {
        List<CartItem> cartItem = await _CartItemsService.GetCartItemByID(ids[0]);
        
        await _CartItemsService.RemoveCartItem(ids);

        return await _CartItemsService.GetCartItemByCustomer(cartItem[0].customer_id);
    }
}