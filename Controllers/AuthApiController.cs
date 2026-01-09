using Microsoft.AspNetCore.Mvc;
using MvcWebApiSwaggerApp.Models;
using MvcWebApiSwaggerApp.Services;

[ApiController]
[Route("api/auth")]
public class AuthApiController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthApiController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public IActionResult Register(Register request)
    {
        _authService.RegisterUser(request, "API");

        return Ok(new { message = "User registered successfully" });
    }
}
