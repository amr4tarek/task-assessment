using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var response = await authService.RegisterAsync(request, cancellationToken);
        return Ok(ApiResponse<AuthResponse>.Ok(response, "User registered successfully."));
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await authService.LoginAsync(request, cancellationToken);
        return Ok(ApiResponse<AuthResponse>.Ok(response, "Login successful."));
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Me(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await authService.GetCurrentUserAsync(userId, cancellationToken);
        return Ok(ApiResponse<UserDto>.Ok(response));
    }
}

