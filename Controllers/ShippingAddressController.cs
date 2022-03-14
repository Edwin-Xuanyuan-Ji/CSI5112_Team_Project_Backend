using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSI5112BackEndApi.Controllers;

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
    public async Task<IActionResult> Post(ShippingAddress newShippingAddress)
    {
        await _ShippingAddressService.CreateNewShippingAddress(newShippingAddress);

        return CreatedAtAction(nameof(Get), new { id = newShippingAddress.shipping_address_id }, newShippingAddress);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromQuery]string id, ShippingAddress updatedShippingAddress)
    {
        var ShippingAddress = await _ShippingAddressService.GetShippingAddressByID(id);

        if (ShippingAddress is null)
        {
            return NotFound();
        }

        updatedShippingAddress.shipping_address_id = ShippingAddress[0].shipping_address_id;

        await _ShippingAddressService.UpdateShippingAddress(id, updatedShippingAddress);

        return NoContent();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] string[] ids)
    {
        await _ShippingAddressService.RemoveShippingAddress(ids);

        return NoContent();
    }
}