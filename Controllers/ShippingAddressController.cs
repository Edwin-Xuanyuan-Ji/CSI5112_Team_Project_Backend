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

    [HttpGet]
    public async Task<List<ShippingAddress>> Get() =>
        await _ShippingAddressService.GetAllShippingAddress();

    [HttpGet("{id}")]
    public async Task<ActionResult<ShippingAddress>> Get(string id)
    {
        var shippingAddress = await _ShippingAddressService.GetShippingAddressByID(id);

        if (shippingAddress is null)
        {
            return NotFound();
        }

        return shippingAddress;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Post(ShippingAddress newShippingAddress)
    {
        await _ShippingAddressService.CreateNewShippingAddress(newShippingAddress);

        return CreatedAtAction(nameof(Get), new { id = newShippingAddress.shipping_address_id }, newShippingAddress);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(string id, ShippingAddress updatedShippingAddress)
    {
        var ShippingAddress = await _ShippingAddressService.GetShippingAddressByID(id);

        if (ShippingAddress is null)
        {
            return NotFound();
        }

        updatedShippingAddress.shipping_address_id = ShippingAddress.shipping_address_id;

        await _ShippingAddressService.UpdateShippingAddress(id, updatedShippingAddress);

        return NoContent();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var ShippingAddress = await _ShippingAddressService.GetShippingAddressByID(id);

        if (ShippingAddress is null)
        {
            return NotFound();
        }

        await _ShippingAddressService.RemoveShippingAddress(id);

        return NoContent();
    }
}