using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CSI5112BackEndApi.Controllers;

// [Authorize]
[ApiController]
[Route("api/[controller]")]
public class ShippingAddressController : ControllerBase
{
    private readonly ShippingAddressService _ShippingAddressService;

    public ShippingAddressController(ShippingAddressService ShippingAddressService) =>
        _ShippingAddressService = ShippingAddressService;

    [HttpGet("by_user")]
    public async Task<List<ShippingAddress>> Get([FromQuery] string user_id) =>
        await _ShippingAddressService.GetShippingAddressByUser(user_id);

    [HttpGet]
    public async Task<List<ShippingAddress>> Gets([FromQuery]string shipping_address_id) =>
        await _ShippingAddressService.GetShippingAddressByID(shipping_address_id);

    [HttpPost("create")]
    public async Task<List<ShippingAddress>> Post(ShippingAddress newShippingAddress)
    {
        await _ShippingAddressService.CreateNewShippingAddress(newShippingAddress);

        return await _ShippingAddressService.GetShippingAddressByUser(newShippingAddress.user_id);
    }

    [HttpPut("update")]
    public async Task<List<ShippingAddress>> Update([FromQuery]string id, ShippingAddress updatedShippingAddress)
    {
        var ShippingAddress = await _ShippingAddressService.GetShippingAddressByID(id);

        updatedShippingAddress.shipping_address_id = ShippingAddress[0].shipping_address_id;

        await _ShippingAddressService.UpdateShippingAddress(id, updatedShippingAddress);

        return await _ShippingAddressService.GetShippingAddressByUser(updatedShippingAddress.user_id);
    }

    [HttpDelete("delete")]
    public async Task<List<ShippingAddress>> Delete([FromBody] string[] ids)
    {
        List<ShippingAddress> shippingAddress = await _ShippingAddressService.GetShippingAddressByID(ids[0]);
        
        await _ShippingAddressService.RemoveShippingAddress(ids);

        return await _ShippingAddressService.GetShippingAddressByUser(shippingAddress[0].user_id);
    }
}