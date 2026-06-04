using System.Net;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Exceptions;

namespace TaskManagement.Application.Validators;

public static class AuthValidator
{
    public static void Validate(RegisterRequest request)
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
            throw new AppException("Password must be at least 6 characters.", (int)HttpStatusCode.BadRequest);
        }
    }

    public static void Validate(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new AppException("Email and password are required.", (int)HttpStatusCode.BadRequest);
        }
    }
}

