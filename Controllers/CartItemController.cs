using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CSI5112BackEndApi.Controllers;

// [Authorize]
[ApiController]
[Route("api/[controller]")]
public class CartItemsController : ControllerBase
{
    private readonly CartItemsService _CartItemsService;
    private readonly CartItemsServer _CartItemsServer;


    public CartItemsController(CartItemsService CartItemsService, CartItemsServer CartItemsServer)
    {
        _CartItemsService = CartItemsService;
        _CartItemsServer = CartItemsServer;
    }

    [HttpGet("all")]
    public async Task<List<CartItem>> Get() =>
        await _CartItemsService.GetAllCartItem();

    [HttpGet]
    public async Task<List<CartItem>> Get([FromQuery] string item_id) =>
        await _CartItemsService.GetCartItemByID(item_id);

    [HttpGet("by_customer")]
    public async Task<List<CartProduct>> Gets([FromQuery] string customer_id)
    {

        return await _CartItemsServer.GetCustomerCartProducts(customer_id);

    }




    [HttpPost("create")]
    public async Task<List<CartItem>> Post([FromBody] CartItem newCartItem)
    {
        await _CartItemsService.CreateNewCartItem(newCartItem);

        return await _CartItemsService.GetCartItemByCustomer(newCartItem.customer_id);
    }

    [HttpPut("update")]
    public async Task<List<CartItem>> Update([FromQuery] string item_id, [FromBody] CartItem updatedCartItem)
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