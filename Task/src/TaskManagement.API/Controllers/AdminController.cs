using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/users")]
public class AdminController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<UserDto>>>> GetUsers(CancellationToken cancellationToken)
    {
        var users = await userService.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<UserDto>>.Ok(users));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<UserDto>>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userService.CreateAsync(request, cancellationToken);
        return Ok(ApiResponse<UserDto>.Ok(user, "User created successfully."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        await userService.DeleteAsync(id, cancellationToken);
        return Ok(ApiResponse<object>.Ok(null, "User deleted successfully."));
    }
}

