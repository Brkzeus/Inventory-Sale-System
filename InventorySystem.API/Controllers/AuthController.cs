using InventorySystem_Core.DTOs;
using InventorySystem_Core.DTOs.AuthDTOs;
using InventorySystem_Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        var response = await _authService.RegisterAsync(dto);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var response = await _authService.LoginAsync(dto);

        if (!response.Success)
        {
            return Unauthorized(response);
        }

        return Ok(response);
    }
}