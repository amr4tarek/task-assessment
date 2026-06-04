# Task Management Backend API

## Project Overview

This project is a production-style ASP.NET Core Web API built on .NET 8 with a clean, DDD-inspired structure. It demonstrates:

- RESTful API development
- JWT authentication and authorization
- Swagger / OpenAPI documentation with Bearer token support
- Redis caching
- Background processing with `BackgroundService`
- Database seeding
- Basic business rules
- Clean and maintainable code organization

## Architecture

The solution is implemented as a single .NET project organized by layers:

- `Domain`
  - Entities
  - Enums
  - Repository contracts
- `Application`
  - DTOs
  - Interfaces
  - Services
  - Validators
  - Mappings
  - Application exceptions
- `Infrastructure`
  - EF Core `DbContext`
  - Repository implementations
  - JWT token generation
  - Redis cache implementation
  - Background queue and worker
  - Database seeding
- `API`
  - Dependency injection and Swagger configuration
- `Controllers`
  - Authentication endpoints
  - Admin endpoints
  - Task endpoints
- `Middleware`
  - Global exception handling

## Functional Summary

### Authentication

- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /api/auth/me`

### Admin

Admin-only endpoints:

- `GET /api/admin/users`
- `POST /api/admin/users`
- `DELETE /api/admin/users/{id}`

### Tasks

Authenticated users can:

- `POST /api/tasks`
- `GET /api/tasks/{id}`
- `GET /api/tasks`
- `PUT /api/tasks/{id}/status`

## Business Rules

- A user cannot create two tasks with the same title on the same day.
- Task listings are sorted by:
  1. Priority descending
  2. CreatedAt descending
- Users can only access and update their own tasks.
- Admin endpoints are restricted to users with the `Admin` role.

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Redis
- JWT Authentication
- Swagger / OpenAPI

## Setup Instructions

### Prerequisites

- .NET 8 SDK
- SQL Server or SQL Server LocalDB
- Redis running locally or remotely

### Configuration

Configure the application through `appsettings.json` or environment variables.

Expected connection strings and JWT settings:

- `ConnectionStrings:DefaultConnection`
- `ConnectionStrings:Redis`
- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:SecretKey`

Example Redis value:

- `localhost:6379`

## Database Migration Commands

Run from the project directory:

```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Redis Setup

Make sure Redis is running and the Redis connection string matches `ConnectionStrings:Redis`.

The API uses Redis to cache task details for:

- `GET /api/tasks/{id}`

Cache behavior:

- cache key format: `task:{id}`
- expiration: 30 minutes
- invalidated when task status changes

## Running the Application

```powershell
dotnet run
```

## Swagger

Once the API is running, open:

- `https://localhost:<port>/swagger`
- or `http://localhost:<port>/swagger`

Swagger is configured with JWT Bearer authentication, so reviewers can:

- register
- log in
- copy the JWT token
- click **Authorize** in Swagger UI
- test protected endpoints directly

## Seeded Admin Credentials

The application seeds a default admin user on startup if it does not already exist.

- Email: `admin@example.com`
- Password: `Admin@123`

## Background Processing

When a task is created:

1. it is saved to the database
2. its task ID is queued
3. a background worker reads the queue
4. the worker waits 5 seconds to simulate processing
5. a log entry is written in the format:

`Processing task: {TaskId}`

## Logging

Structured logging is used for key operations such as:

- user registration
- user login
- task creation
- task processing
- user deletion

## Global Error Handling

The API uses global exception middleware to return consistent error responses such as:

```json
{
  "success": false,
  "message": "Task not found"
}
```

## Assumptions Made

- The solution uses a single project with a clean layered folder structure rather than separate class library projects.
- SQL Server is the primary database provider.
- Redis is available through the configured connection string.
- Database migrations are applied before or during startup as needed.
- Passwords are hashed before being stored.
- The seeded admin user is only created if it does not already exist.
