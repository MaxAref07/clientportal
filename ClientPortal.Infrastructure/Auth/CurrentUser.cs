using ClientPortal.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ClientPortal.Infrastructure.Auth;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public Guid? UserId => Guid.TryParse(httpContextAccessor.HttpContext?.User
            .FindFirst("sub")?.Value, out var parsedGuid) ? parsedGuid : null;
    
    public string? Email => httpContextAccessor.HttpContext?.User.FindFirst("email")?.Value;

    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}