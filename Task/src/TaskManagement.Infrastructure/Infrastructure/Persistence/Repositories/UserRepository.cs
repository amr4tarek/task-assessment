using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<User>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.Users.OrderBy(x => x.CreatedAt).ToListAsync(cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        => await context.Users.AddAsync(user, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);

    public void Remove(User user)
        => context.Users.Remove(user);
}

