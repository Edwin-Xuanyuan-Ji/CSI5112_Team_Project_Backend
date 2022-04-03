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

    [HttpGet("search_salesOrder_by_userId")]
    public async Task<List<SalesOrderEcho>> SearchSalesOrderByUserId([FromQuery] string customer_id, [FromQuery] string merchant_id, [FromQuery] string role)
    {
        return await _SalesOrdersService.SearchSalesOrderByUserId(customer_id, merchant_id, role);
    }

    [HttpPut("deliver_product")]
    public async Task<IActionResult> DeliverProduct([FromQuery] string order_id, [FromQuery] string merchant_shipping_address_id)
    {
        await _SalesOrdersService.DeliverProduct(order_id, merchant_shipping_address_id);
        return NoContent();
    }

    [HttpPut("recieve_product")]
    public async Task<IActionResult> RecieveProduct([FromQuery] string order_id)
    {
        await _SalesOrdersService.RecieveProduct(order_id);
        return NoContent();
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

    [HttpPost("placeOrder")]
    public async Task<IActionResult> PlaceOrder([FromBody] List<PlaceOrdersFrontendRequire> placeOrdersFrontendRequires)
    {
        await _SalesOrdersService.PlaceOrder(placeOrdersFrontendRequires);

        return NoContent();
    }
}