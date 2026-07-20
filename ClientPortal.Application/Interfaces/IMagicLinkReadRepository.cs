using ClientPortal.Domain.Entities;

namespace ClientPortal.Application.Interfaces;

public interface IMagicLinkReadRepository
{
    Task<MagicLink?> GetByTokenHash(string tokenHash);
}