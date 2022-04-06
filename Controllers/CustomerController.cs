using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CSI5112BackEndApi.Controllers;

// [Authorize]
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly CustomersService _CustomersService;

    public CustomersController(CustomersService CustomersService) =>
        _CustomersService = CustomersService;

    [HttpGet("all")]
    public async Task<List<Customer>> Get() =>
        await _CustomersService.GetAllCustomers();

    [HttpGet]
    public async Task<List<Customer>> Get([FromQuery]string customer_id) =>
        await _CustomersService.GetCustomerByID(customer_id);

    [HttpPost("create")]
    public async Task<IActionResult> Post(Customer newCustomer)
    {
        await _CustomersService.CreateNewCustomer(newCustomer);

        return CreatedAtAction(nameof(Get), new { id = newCustomer.customer_id }, newCustomer);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromQuery]string customer_id, Customer updatedCustomer)
    {
        var Customer = await _CustomersService.GetCustomerByID(customer_id);

        if (Customer is null)
        {
            return NotFound();
        }

        updatedCustomer.customer_id = Customer[0].customer_id;

        await _CustomersService.UpdateCustomer(customer_id, updatedCustomer);

        return NoContent();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery]string customer_ids)
    {
        var Customer = await _CustomersService.GetCustomerByID(customer_ids);

        if (Customer is null)
        {
            return NotFound();
        }

        await _CustomersService.RemoveCustomer(customer_ids.Split('_'));

        return NoContent();
    }
}