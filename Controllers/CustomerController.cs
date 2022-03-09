using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSI5112BackEndApi.Controllers;

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
    public async Task<ActionResult<Customer>> Get([FromQuery]string customer_id)
    {
        var customer = await _CustomersService.GetCustomerByID(customer_id);

        if (customer is null)
        {
            return NotFound();
        }

        return customer;
    }

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

        updatedCustomer.customer_id = Customer.customer_id;

        await _CustomersService.UpdateCustomer(customer_id, updatedCustomer);

        return NoContent();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery]string customer_id)
    {
        var Customer = await _CustomersService.GetCustomerByID(customer_id);

        if (Customer is null)
        {
            return NotFound();
        }

        await _CustomersService.RemoveCustomer(customer_id);

        return NoContent();
    }
}