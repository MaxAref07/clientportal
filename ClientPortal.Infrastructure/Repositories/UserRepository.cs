using ClientPortal.Application.Interfaces;
using ClientPortal.Domain.Entities;
using ClientPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClientPortal.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserReadRepository, IUserRepository
{
    public async Task<User?> GetByEmail(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public Task<User> Add(User user)
    {
        context.Users.Add(user);
        return Task.FromResult(user);
    }
}