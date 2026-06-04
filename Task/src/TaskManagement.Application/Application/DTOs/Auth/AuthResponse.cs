using TaskManagement.Application.DTOs.Users;

namespace TaskManagement.Application.DTOs.Auth;

public class AuthResponse
{
    public UserDto User { get; set; } = new();
    public string Token { get; set; } = string.Empty;
}

