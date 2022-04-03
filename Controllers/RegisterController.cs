using Microsoft.AspNetCore.Mvc;
using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;

namespace CSI5112BackEndApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegisterController: ControllerBase
{
    private readonly CustomersService _CustomersService;
    private readonly MerchantsService _MerchantsService;

    public RegisterController(CustomersService customersService, MerchantsService merchantsService)
    {
        _CustomersService = customersService;
        _MerchantsService = merchantsService;
    }

    [HttpGet]
    public async Task<Boolean> checkValid([FromQuery] String username, [FromQuery] String role)
    {
        var res = role == "Merchant" ? await _MerchantsService.GetMerchantByUsername(username) as dynamic : await _CustomersService.GetCustomerByUsername(username) as dynamic;
        return res.Count > 0;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
         
        if (!request.isMerchant) {
            List<Customer> customers = _CustomersService.GetCustomerByUsername(request.username).Result;
            if (customers.Count != 0) {
                return BadRequest(new
                {
                    error = "Username has been used by other user"
                });
            }
            Customer c = new Customer{
                customer_id = request.id,
                username = request.username,
                password = request.password,
                phone = request.phone,
                first_name = request.first_name,
                last_name = request.last_name,
                email = request.email
            };

            await _CustomersService.CreateNewCustomer(c);
            return Ok(new 
                {
                    success = "You have registered in our application as customer"
                });


        } else {
            List<Merchant> merchants = _MerchantsService.GetMerchantByUsername(request.username).Result;
            if (merchants.Count != 0) {
                return BadRequest(new
                {
                    error = "Username has been used by other user"
                });
            }
            Merchant m = new Merchant{
                merchant_id = request.id,
                username = request.username,
                password = request.password,
                phone = request.phone,
                first_name = request.first_name,
                last_name = request.last_name,
                email = request.email
            };

            await _MerchantsService.CreateNewMerchant(m);
            return Ok(new 
                {
                    success = "You have registered in our application as merchant"
                });
        }
    }
}