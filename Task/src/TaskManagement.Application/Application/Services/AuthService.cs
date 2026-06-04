using System.Net;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Exceptions;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Mappings;
using TaskManagement.Application.Validators;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services;

public class AuthService(
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        AuthValidator.Validate(request);

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var existingUser = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
        if (existingUser is not null)
        {
            throw new AppException("Email already exists.", (int)HttpStatusCode.Conflict);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Email = normalizedEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.User,
            CreatedAt = DateTime.UtcNow
        };

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("User registered: {Email}", user.Email);

        return new AuthResponse
        {
            User = user.ToDto(),
            Token = jwtTokenGenerator.GenerateToken(user)
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        AuthValidator.Validate(request);

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var user = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new AppException("Invalid credentials.", (int)HttpStatusCode.Unauthorized);
        }

        logger.LogInformation("User login: {Email}", user.Email);

        return new AuthResponse
        {
            User = user.ToDto(),
            Token = jwtTokenGenerator.GenerateToken(user)
        };
    }

    public async Task<UserDto> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new AppException("User not found.", (int)HttpStatusCode.NotFound);

        return user.ToDto();
    }
}

