using System.Net;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Exceptions;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Mappings;
using TaskManagement.Application.Validators;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services;

public class UserService(
    IUserRepository userRepository,
    ILogger<UserService> logger) : IUserService
{
    public async Task<IReadOnlyCollection<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);
        return users.Select(x => x.ToDto()).ToList();
    }

    public async Task<UserDto> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        UserValidator.Validate(request);

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
            Role = request.Role,
            CreatedAt = DateTime.UtcNow
        };

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("User created by admin: {Email}", user.Email);

        return user.ToDto();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new AppException("User not found.", (int)HttpStatusCode.NotFound);

        userRepository.Remove(user);
        await userRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("User deleted: {UserId}", id);
    }
}

