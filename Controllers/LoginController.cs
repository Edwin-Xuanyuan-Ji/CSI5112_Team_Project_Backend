using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CSI5112BackEndApi.Controllers;

[ApiController]
[Route("api/token")]
public class LoginController: ControllerBase
{

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

        return Ok(new 
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }
}