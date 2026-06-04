using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Exceptions;

namespace TaskManagement.Application.Validators;

public static class UserValidator
{
    public static void Validate(CreateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new AppException("Name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new AppException("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
        {
            throw new AppException("Password must be at least 6 characters.");
        }
    }
}

