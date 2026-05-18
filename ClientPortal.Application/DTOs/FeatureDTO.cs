using Ardalis.GuardClauses;
using ClientPortal.Domain.Enums;

namespace ClientPortal.Application.Projects.DTOs;

public class FeatureDto
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public FeaturePriority Priority { get; private set; }
    public FeatureStatus Status { get; private set; }
    public string Description { get; private set; }
    public Guid ProjectId { get; private set; }

    public FeatureDto(Guid id, string name, FeaturePriority priority, FeatureStatus status, string description, Guid projectId)
    {
        Guard.Against.Default(id, nameof(id));
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.EnumOutOfRange(priority, nameof(priority));
        Guard.Against.EnumOutOfRange(status, nameof(status));
        Guard.Against.NullOrWhiteSpace(description, nameof(description));
        Guard.Against.Default(projectId, nameof(projectId));
        
        Id = id;
        Name = name;
        Priority = priority;
        Status = status;
        Description = description;
        ProjectId = projectId;
    }
}