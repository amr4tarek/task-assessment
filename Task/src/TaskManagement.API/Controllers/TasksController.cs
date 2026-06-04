using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers;

[ApiController]
[Authorize]
[Route("api/tasks")]
public class TasksController(ITaskService taskService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiResponse<TaskItemDto>>> Create(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var task = await taskService.CreateAsync(GetUserId(), request, cancellationToken);
        return Ok(ApiResponse<TaskItemDto>.Ok(task, "Task created successfully."));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<TaskItemDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var task = await taskService.GetByIdAsync(GetUserId(), id, cancellationToken);
        return Ok(ApiResponse<TaskItemDto>.Ok(task));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<TaskItemDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var tasks = await taskService.GetAllAsync(GetUserId(), cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<TaskItemDto>>.Ok(tasks));
    }

    [HttpPut("{id:guid}/status")]
    public async Task<ActionResult<ApiResponse<TaskItemDto>>> UpdateStatus(Guid id, UpdateTaskStatusRequest request, CancellationToken cancellationToken)
    {
        var task = await taskService.UpdateStatusAsync(GetUserId(), id, request, cancellationToken);
        return Ok(ApiResponse<TaskItemDto>.Ok(task, "Task status updated successfully."));
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}

