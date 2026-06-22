using Ardalis.GuardClauses;

namespace ClientPortal.Application.Projects.DTOs;

public class ProjectDto
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int ScopeFeatures { get; private set; }
    
    public int CurrentFeaturesCount { get; private set; }
    
    public int CompletedFeaturesCount { get; private set; }

    public ProjectDto(Guid id, string name, string description, int scopeFeatures, int currentFeaturesCount, int completedFeaturesCount)
    {
        Guard.Against.Default(id, nameof(id));
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.NullOrWhiteSpace(description, nameof(description));
        Guard.Against.NegativeOrZero(scopeFeatures, nameof(scopeFeatures));
        Guard.Against.Negative(currentFeaturesCount, nameof(currentFeaturesCount));
        Guard.Against.Negative(completedFeaturesCount, nameof(completedFeaturesCount));
    
        Id = id;
        Name = name;
        Description = description;
        ScopeFeatures = scopeFeatures;
        CurrentFeaturesCount = currentFeaturesCount;
        CompletedFeaturesCount = completedFeaturesCount;
    }
}