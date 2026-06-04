using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Mappings;

public static class MappingExtensions
{
    public static UserDto ToDto(this User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Role = user.Role,
        CreatedAt = user.CreatedAt
    };

    public static TaskItemDto ToDto(this TaskItem taskItem) => new()
    {
        Id = taskItem.Id,
        Title = taskItem.Title,
        Description = taskItem.Description,
        Status = taskItem.Status,
        Priority = taskItem.Priority,
        CreatedAt = taskItem.CreatedAt,
        UserId = taskItem.UserId
    };
}

