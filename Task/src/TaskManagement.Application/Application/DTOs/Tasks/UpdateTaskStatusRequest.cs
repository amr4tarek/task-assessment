using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Tasks;

public class UpdateTaskStatusRequest
{
    public Domain.Enums.TaskStatus Status { get; set; }
}

