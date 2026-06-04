using TaskManagement.Application.DTOs.Tasks;

namespace TaskManagement.Application.Interfaces;

public interface ITaskService
{
    Task<TaskItemDto> CreateAsync(Guid userId, CreateTaskRequest request, CancellationToken cancellationToken = default);
    Task<TaskItemDto> GetByIdAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskItemDto>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<TaskItemDto> UpdateStatusAsync(Guid userId, Guid taskId, UpdateTaskStatusRequest request, CancellationToken cancellationToken = default);
}

