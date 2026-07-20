using ClientPortal.Application.DTOs;
using ClientPortal.Domain.Enums;

namespace ClientPortal.Application.Interfaces;

public interface IJwtTokenService
{
    AuthTokenDto GenerateToken(Guid userId, string email, UserRole role);
}