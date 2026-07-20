using ClientPortal.Domain.Entities;

namespace ClientPortal.Application.Interfaces;

public interface IMagicLinkRepository
{
    Task<MagicLink> Add(MagicLink magicLink);
}