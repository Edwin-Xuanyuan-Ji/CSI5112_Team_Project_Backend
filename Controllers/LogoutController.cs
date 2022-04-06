using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace CSI5112BackEndApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogoutController: ControllerBase
{
    // Supposed to delete session
    [HttpPost]
    public IActionResult Logout()
    {
        return Ok(new 
        {
            message = "You have logged out"
        });
    }
}