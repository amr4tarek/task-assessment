using System.Net;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Exceptions;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Mappings;
using TaskManagement.Application.Validators;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services;

public class TaskService(
    ITaskRepository taskRepository,
    ICacheService cacheService,
    ILogger<TaskService> logger) : ITaskService
{
    public async Task<TaskItemDto> CreateAsync(Guid userId, CreateTaskRequest request, CancellationToken cancellationToken = default)
    {
        TaskValidator.Validate(request);

        var normalizedTitle = request.Title.Trim();
        var exists = await taskRepository.ExistsByTitleForDateAsync(userId, normalizedTitle, DateTime.UtcNow, cancellationToken);
        if (exists)
        {
            throw new AppException("A user cannot create two tasks with the same title on the same day.");
        }

        var taskItem = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = normalizedTitle,
            Description = request.Description?.Trim(),
            Status = Domain.Enums.TaskStatus.Pending,
            Priority = request.Priority,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };

        await taskRepository.AddAsync(taskItem, cancellationToken);
        await taskRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Task created: {TaskId} by user: {UserId}", taskItem.Id, userId);

        return taskItem.ToDto();
    }

    public async Task<TaskItemDto> GetByIdAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default)
    {
        var cacheKey = GetCacheKey(taskId);
        var cached = await cacheService.GetAsync<TaskItemDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            if (cached.UserId != userId)
            {
                throw new AppException("Task not found.", (int)HttpStatusCode.NotFound);
            }

            return cached;
        }

        var taskItem = await taskRepository.GetByIdAsync(taskId, cancellationToken)
            ?? throw new AppException("Task not found.", (int)HttpStatusCode.NotFound);

        if (taskItem.UserId != userId)
        {
            throw new AppException("Task not found.", (int)HttpStatusCode.NotFound);
        }

        var dto = taskItem.ToDto();
        await cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(30), cancellationToken);
        return dto;
    }

    public async Task<IReadOnlyCollection<TaskItemDto>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tasks = await taskRepository.GetByUserIdAsync(userId, cancellationToken);
        return tasks.Select(x => x.ToDto()).ToList();
    }

    public async Task<TaskItemDto> UpdateStatusAsync(Guid userId, Guid taskId, UpdateTaskStatusRequest request, CancellationToken cancellationToken = default)
    {
        var taskItem = await taskRepository.GetByIdAsync(taskId, cancellationToken)
            ?? throw new AppException("Task not found.", (int)HttpStatusCode.NotFound);

        if (taskItem.UserId != userId)
        {
            throw new AppException("Task not found.", (int)HttpStatusCode.NotFound);
        }

        taskItem.Status = request.Status;
        await taskRepository.SaveChangesAsync(cancellationToken);
        await cacheService.RemoveAsync(GetCacheKey(taskId), cancellationToken);

        return taskItem.ToDto();
    }

    private static string GetCacheKey(Guid taskId) => $"task:{taskId}";
}

