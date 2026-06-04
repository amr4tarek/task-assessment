using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Infrastructure.Persistence;

public class DatabaseSeeder(ApplicationDbContext context, ILogger<DatabaseSeeder> logger)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.MigrateAsync(cancellationToken);

        const string adminEmail = "admin@example.com";
        var adminExists = await context.Users.AnyAsync(x => x.Email == adminEmail, cancellationToken);
        if (adminExists)
        {
            return;
        }

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Name = "System Admin",
            Email = adminEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = UserRole.Admin,
            CreatedAt = DateTime.UtcNow
        };

        await context.Users.AddAsync(admin, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Default admin user seeded.");
    }
}

