using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces;

public interface ITaskRepository
{
    Task AddAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
    Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskItem>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByTitleForDateAsync(Guid userId, string title, DateTime date, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

