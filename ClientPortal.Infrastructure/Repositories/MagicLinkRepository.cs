using ClientPortal.Application.Interfaces;
using ClientPortal.Domain.Entities;
using ClientPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClientPortal.Infrastructure.Repositories;

public class MagicLinkRepository(AppDbContext context) : IMagicLinkReadRepository, IMagicLinkRepository
{
    public async Task<MagicLink?> GetByTokenHash(string hash)
    {
        return await context.MagicLinks.FirstOrDefaultAsync(f => f.TokenHash == hash);
    }

    public Task<MagicLink> Add(MagicLink magicLink)
    {
        context.MagicLinks.Add(magicLink);
        return Task.FromResult(magicLink);
    }
}