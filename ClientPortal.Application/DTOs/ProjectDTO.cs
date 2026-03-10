using Ardalis.GuardClauses;

namespace ClientPortal.Application.Projects.DTOs;

public class ProjectDTO
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int ScopeFeatures { get; private set; }

    public ProjectDTO(Guid id, string name, string description, int scopeFeatures)
    {
        Guard.Against.Default(id, nameof(id));
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.NullOrWhiteSpace(description, nameof(description));
        Guard.Against.NegativeOrZero(scopeFeatures, nameof(scopeFeatures));
    
        Id = id;
        Name = name;
        Description = description;
        ScopeFeatures = scopeFeatures;
    }
}