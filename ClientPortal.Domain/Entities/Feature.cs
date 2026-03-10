using Ardalis.GuardClauses;
using ClientPortal.Domain.Enums;

namespace ClientPortal.Domain.Entities;

public class Feature : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public FeaturePriority Priority { get; private set; }
    public FeatureStatus Status { get; private set; }
    public Guid ProjectId { get; private set; }

    public Feature(Guid id, string name, string description, FeaturePriority priority, FeatureStatus status, Guid projectId) : base(id)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));
        Guard.Against.NullOrEmpty(description, nameof(description));
        Guard.Against.EnumOutOfRange(status, nameof(status));
        Guard.Against.EnumOutOfRange(priority, nameof(priority));
        Guard.Against.Default(projectId, nameof(projectId));

        Name = name;
        Description = description;
        Priority = priority;
        Status = status;
        ProjectId = projectId;
    }

    public void Rename(string name)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));
        Name = name;
    }

    public void ChangePriority(FeaturePriority priority)
    {
        Guard.Against.EnumOutOfRange(priority, nameof(priority));
        Priority = priority;
    }

    public void ChangeStatus(FeatureStatus newStatus)
    {
        Guard.Against.EnumOutOfRange(newStatus, nameof(newStatus));
        
        if (Status == newStatus)
        {
            return;
        }

        if (IsValidStatus(Status, newStatus))
        {
            Status = newStatus;
        } else throw new ArgumentException($"Transition from {Status} to {newStatus} is not allowed.", nameof(newStatus));
    }
    
    public void ChangeDescription(string description)
    {
        Guard.Against.NullOrWhiteSpace(description, nameof(description));
        Description = description;
    }

    private bool IsValidStatus(FeatureStatus currentStatus, FeatureStatus newStatus)
    {
        switch (currentStatus)
        {
            case FeatureStatus.ToDo:
                return newStatus == FeatureStatus.InProgress;
            case FeatureStatus.InProgress:
                return newStatus == FeatureStatus.OnReview;
            case FeatureStatus.OnReview:
                return newStatus == FeatureStatus.Done || newStatus == FeatureStatus.InProgress;
            case FeatureStatus.Done:
                return true;
            default:
                return false;
        }
    }
}