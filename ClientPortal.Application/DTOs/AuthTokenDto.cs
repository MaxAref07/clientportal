using Ardalis.GuardClauses;

namespace ClientPortal.Application.DTOs;

public class AuthTokenDto
{
    public string AccessToken { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    
    public AuthTokenDto(string accessToken, DateTime expiresAt) {
        Guard.Against.NullOrWhiteSpace(accessToken, nameof(accessToken));
        Guard.Against.OutOfSQLDateRange(expiresAt, nameof(expiresAt));

        AccessToken = accessToken;
        ExpiresAt = expiresAt;
    }
}