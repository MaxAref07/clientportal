using Ardalis.GuardClauses;

namespace ClientPortal.Domain.Entities;

public class MagicLink : Entity
{
    public string Email { get; private set; }
    public string TokenHash { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? UsedAt { get; private set; }
    
    public MagicLink(Guid id, string email, string tokenHash, DateTime expiresAt) : base(id)
    {
        Guard.Against.NullOrWhiteSpace(email, nameof(email));
        Guard.Against.NullOrWhiteSpace(tokenHash, nameof(tokenHash));
        Guard.Against.OutOfSQLDateRange(expiresAt, nameof(expiresAt));
        
        Email = email;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow >= ExpiresAt;
    }
    
    public bool IsUsed()
    {
        return UsedAt != null;
    }
    
    public void MarkAsUsed()
    {
        if (!IsUsed() && !IsExpired())
        {
            UsedAt = DateTime.UtcNow;
        }
    }
}