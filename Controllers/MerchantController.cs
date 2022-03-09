using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSI5112BackEndApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MerchantsController : ControllerBase
{
    private readonly MerchantsService _MerchantsService;

    public MerchantsController(MerchantsService MerchantsService) =>
        _MerchantsService = MerchantsService;

    [HttpGet]
    public async Task<List<Merchant>> Get() =>
        await _MerchantsService.GetAllMerchant();

    [HttpGet("{id}")]
    public async Task<ActionResult<Merchant>> Get(string id)
    {
        var merchant = await _MerchantsService.GetMerchantByID(id);

        if (merchant is null)
        {
            return NotFound();
        }

        return merchant;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Post(Merchant newMerchant)
    {
        await _MerchantsService.CreateNewMerchant(newMerchant);

        return CreatedAtAction(nameof(Get), new { id = newMerchant.merchant_id }, newMerchant);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(string id, Merchant updatedMerchant)
    {
        var Merchant = await _MerchantsService.GetMerchantByID(id);

        if (Merchant is null)
        {
            return NotFound();
        }

        updatedMerchant.merchant_id = Merchant.merchant_id;

        await _MerchantsService.UpdateMerchant(id, updatedMerchant);

        return NoContent();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var Merchant = await _MerchantsService.GetMerchantByID(id);

        if (Merchant is null)
        {
            return NotFound();
        }

        await _MerchantsService.RemoveMerchant(id);

        return NoContent();
    }
}