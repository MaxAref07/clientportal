using Ardalis.GuardClauses;
using ClientPortal.Domain.Enums;
using ClientPortal.Domain.Exceptions;

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

    public Feature AddFeature(Guid featureId,
        string featureName,
        string featureDescription,
        FeaturePriority featurePriority,
        int existingFeatureCount)
    {
        if (!CanAccommodate(existingFeatureCount + 1, ScopeFeatures)) throw new FeaturesOutOfScopeException($"Feature scope for project {Id} has been exceeded");
        
        return new Feature(featureId,
            featureName,
            featureDescription,
            featurePriority,
            FeatureStatus.ToDo,
            Id);
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

    public void ChangeScope(int newScopeFeatures, int existingFeatureCount)
    {
        Guard.Against.NegativeOrZero(newScopeFeatures, nameof(newScopeFeatures));
        if (!CanAccommodate(existingFeatureCount, newScopeFeatures)) throw new MinimumFeatureScopeException(existingFeatureCount, newScopeFeatures);
        ScopeFeatures = newScopeFeatures;
    }

    private bool CanAccommodate(int resultingFeatureCount, int scopeLimit) =>
        resultingFeatureCount <= scopeLimit;
}