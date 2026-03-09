using Ardalis.GuardClauses;

namespace ClientPortal.Domain.Entities;

public class Project : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int ScopeFeatures { get; private set; }

    public Project(Guid id, string name, string description, int scopeFeatures) : base(id)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.NullOrWhiteSpace(description, nameof(description));
        Guard.Against.NegativeOrZero(scopeFeatures, nameof(scopeFeatures));

        Name = name;
        Description = description;
        ScopeFeatures = scopeFeatures;
    }

    public void Rename(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Name = name;
    }

    public void UpdateDescription(string description)
    {
        Guard.Against.NullOrWhiteSpace(description, nameof(description));
        Description = description;
    }

    public void ChangeScope(int scopeFeatures)
    {
        Guard.Against.NegativeOrZero(scopeFeatures, nameof(scopeFeatures));
        ScopeFeatures = scopeFeatures;
    }
}