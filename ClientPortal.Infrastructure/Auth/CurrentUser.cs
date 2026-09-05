using ClientPortal.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ClientPortal.Infrastructure.Auth;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public Guid UserId
    {
        get
        {
            var userId = httpContextAccessor.HttpContext?.User
                .FindFirst("sub")?.Value;

            if (Guid.TryParse(userId, out Guid guid))
            {
                return guid;
            }
            
            throw new InvalidOperationException("Current user has no valid ID");
        }
    }

    public string? Email => httpContextAccessor.HttpContext?.User.FindFirst("email")?.Value;

    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}