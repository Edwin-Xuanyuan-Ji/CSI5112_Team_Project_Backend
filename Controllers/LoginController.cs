using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace CSI5112BackEndApi.Controllers;

// [Authorize]
[ApiController]
[Route("api/token")]
public class LoginController: ControllerBase
{
    private readonly CustomersService _CustomersService;
    private readonly MerchantsService _MerchantsService;

    public LoginController(CustomersService customersService, MerchantsService merchantsService)
    {
        _CustomersService = customersService;
        _MerchantsService = merchantsService;
    }


    [HttpPost]
    public IActionResult GetToken([FromBody] TokenRequest request)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Username)
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("dd%88*377f6d&f£$$£$FdddFF33fssDG^!3"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "backend",
            audience: "backend",
            claims: claims,
            expires: DateTime.Now.AddDays(3),
            signingCredentials: creds
        );
        
        if (!request.isMerchant) {
            List<Customer> customers = _CustomersService.GetCustomerByUsername(request.Username).Result;
            if (customers.Count == 0) {
                return BadRequest(new
                {
                    error = "Username does not exist!"
                });
            } else {
                Customer customer = customers[0];
                return Ok(new 
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    customer_id = customer.customer_id,
                    first_name = customer.first_name,
                    last_name = customer.last_name,
                    phone = customer.phone,
                    email = customer.email,
                    username = customer.username,
                    password = customer.password
                });
            }
        } else {
            List<Merchant> merchants = _MerchantsService.GetMerchantByUsername(request.Username).Result;
            if (merchants.Count == 0) {
                return BadRequest(new
                {
                    error = "Username does not exist or password is invalid!"
                });
            } else {
                Merchant merchant = merchants[0];
                return Ok(new 
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    merchant_id = merchant.merchant_id,
                    first_name = merchant.first_name,
                    last_name = merchant.last_name,
                    phone = merchant.phone,
                    email = merchant.email,
                    username = merchant.username,
                    password = merchant.password
                });
            }
        }
    }
}