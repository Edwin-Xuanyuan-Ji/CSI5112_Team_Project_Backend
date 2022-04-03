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

    [HttpGet("all")]
    public async Task<List<SalesOrder>> Get() =>
        await _SalesOrdersService.GetAllSalesOrders();

    [HttpGet]
    public async Task<ActionResult<List<SalesOrder>>> Get([FromQuery]string id, [FromQuery]string role)
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

    [HttpPost("create")]
    public async Task<IActionResult> Post(SalesOrder newSalesOrder)
    {
        await _SalesOrdersService.CreateSalesOrder(newSalesOrder);

        return CreatedAtAction(nameof(Get), new { id = newSalesOrder.order_id }, newSalesOrder);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] string[] ids)
    {
        await _SalesOrdersService.RemoveSalesOrder(ids);

        return NoContent();
    }

     [HttpDelete("post")]
    public async Task<IActionResult> PlaceOrder([FromBody] string[] ids)
    {
        await _SalesOrdersService.RemoveSalesOrder(ids);

        return NoContent();
    }
}