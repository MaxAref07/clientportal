using Ardalis.GuardClauses;

namespace ClientPortal.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; init; }
    
    protected Entity(Guid id)
    {
        Guard.Against.Default(id, nameof(id));
        Id = id;
    }
}