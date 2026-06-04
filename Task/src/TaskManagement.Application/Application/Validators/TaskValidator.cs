using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Exceptions;

namespace TaskManagement.Application.Validators;

public static class TaskValidator
{
    public static void Validate(CreateTaskRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new AppException("Task title is required.");
        }
    }
}

