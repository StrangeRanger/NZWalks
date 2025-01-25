using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using NZWalks.API.Dto.Auth;
using NZWalks.API.Services;

namespace NZWalks.API.Controllers;

/*
 * TODO: Add comments describing why the logic is in a service class instead of a repository class... (refer to chatGPT)
 */
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    // POST: /api/Auth/Register
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        IdentityResult result = await _authService.RegisterAsync(registerRequestDto);

        if (result.Succeeded)
        {
            return Ok("User registered successfully. Please login.");
        }

        return BadRequest(result.Errors.First().Description);
    }

}