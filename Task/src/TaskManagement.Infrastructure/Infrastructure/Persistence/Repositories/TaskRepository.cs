using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Infrastructure.Persistence.Repositories;

public class TaskRepository(ApplicationDbContext context) : ITaskRepository
{
    public async Task AddAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
        => await context.Tasks.AddAsync(taskItem, cancellationToken);

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Tasks.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<TaskItem>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => await context.Tasks
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Priority)
            .ThenByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<bool> ExistsByTitleForDateAsync(Guid userId, string title, DateTime date, CancellationToken cancellationToken = default)
    {
        var start = date.Date;
        var end = start.AddDays(1);

        return await context.Tasks.AnyAsync(
            x => x.UserId == userId && x.Title == title && x.CreatedAt >= start && x.CreatedAt < end,
            cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);
}

