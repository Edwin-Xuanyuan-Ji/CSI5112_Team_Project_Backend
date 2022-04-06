using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CSI5112BackEndApi.Controllers;

// [Authorize]
[ApiController]
[Route("api/[controller]")]
public class MerchantsController : ControllerBase
{
    private readonly MerchantsService _MerchantsService;

    public MerchantsController(MerchantsService MerchantsService) =>
        _MerchantsService = MerchantsService;

    [HttpGet("all")]
    public async Task<List<Merchant>> Get() =>
        await _MerchantsService.GetAllMerchant();

    [HttpGet]
    public async Task<List<Merchant>> Get([FromQuery]string id) =>
        await _MerchantsService.GetMerchantByID(id);

    [HttpPost("create")]
    public async Task<IActionResult> Post(Merchant newMerchant)
    {
        await _MerchantsService.CreateNewMerchant(newMerchant);

        return CreatedAtAction(nameof(Get), new { id = newMerchant.merchant_id }, newMerchant);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromQuery]string merchant_id, Merchant updatedMerchant)
    {
        var Merchant = await _MerchantsService.GetMerchantByID(merchant_id);

        if (Merchant is null)
        {
            return NotFound();
        }

        updatedMerchant.merchant_id = Merchant[0].merchant_id;

        await _MerchantsService.UpdateMerchant(merchant_id, updatedMerchant);

        return NoContent();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(string merchant_id)
    {
        var Merchant = await _MerchantsService.GetMerchantByID(merchant_id);

        if (Merchant is null)
        {
            return NotFound();
        }

        await _MerchantsService.RemoveMerchant(merchant_id);

        return NoContent();
    }
}  