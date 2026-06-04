using TaskManagement.Application.DTOs.Users;

namespace TaskManagement.Application.Interfaces;

public interface IUserService
{
    Task<IReadOnlyCollection<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserDto> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

