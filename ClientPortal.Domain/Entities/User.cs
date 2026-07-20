using Ardalis.GuardClauses;
using ClientPortal.Domain.Enums;

namespace ClientPortal.Domain.Entities;

public class User : Entity
{
    public string Email { get; private set; }
    public UserRole Role { get; private set; }

    public User(Guid id, string email, UserRole role) : base(id)
    {
        Guard.Against.NullOrWhiteSpace(email, nameof(email));
        Guard.Against.EnumOutOfRange(role, nameof(role));
        
        Email = email;
        Role = role;
    }
}