using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSI5112BackEndApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesOrdersController : ControllerBase
{
    private readonly SalesOrdersService _SalesOrdersService;

    public SalesOrdersController(SalesOrdersService SalesOrdersService) =>
        _SalesOrdersService = SalesOrdersService;

    [HttpGet]
    public async Task<List<SalesOrder>> Get() =>
        await _SalesOrdersService.GetAllSalesOrders();

    [HttpGet("{id:length(24)}/{role:length(24)}")]
    public async Task<ActionResult<List<SalesOrder>>> Get(string id, string role)
    {
        var salesOrder = new List<SalesOrder>();
        if (role == "Customer") salesOrder = await _SalesOrdersService.GetSalesOrdersByCustomer(id);
        else salesOrder = await _SalesOrdersService.GetSalesOrdersByMerchant(id);

        if (salesOrder is null)
        {
            return NotFound();
        }

        return salesOrder;
    }

    [HttpPost]
    public async Task<IActionResult> Post(SalesOrder newSalesOrder)
    {
        await _SalesOrdersService.CreateSalesOrder(newSalesOrder);

        return CreatedAtAction(nameof(Get), new { id = newSalesOrder.order_id }, newSalesOrder);
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var SalesOrder = await _SalesOrdersService.GetSalesOrdersByID(id);

        if (SalesOrder is null)
        {
            return NotFound();
        }

        await _SalesOrdersService.RemoveSalesOrder(id);

        return NoContent();
    }
}