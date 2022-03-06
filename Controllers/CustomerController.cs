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

    [HttpGet]
    public async Task<List<Customer>> Get() =>
        await _CustomersService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Customer>> Get(string id)
    {
        var customer = await _CustomersService.GetAsync(id);

        if (customer is null)
        {
            return NotFound();
        }

        return customer;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Customer newCustomer)
    {
        await _CustomersService.CreateAsync(newCustomer);

        return CreatedAtAction(nameof(Get), new { id = newCustomer.customer_id }, newCustomer);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Customer updatedCustomer)
    {
        var Customer = await _CustomersService.GetAsync(id);

        if (Customer is null)
        {
            return NotFound();
        }

        updatedCustomer.customer_id = Customer.customer_id;

        await _CustomersService.UpdateAsync(id, updatedCustomer);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var Customer = await _CustomersService.GetAsync(id);

        if (Customer is null)
        {
            return NotFound();
        }

        await _CustomersService.RemoveAsync(id);

        return NoContent();
    }
}