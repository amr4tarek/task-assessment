using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.DTOs.Users;

namespace TaskManagement.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<UserDto> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default);
}

