using ClientPortal.Domain.Enums;

namespace ClientPortal.WebAPI.Models;

public class ChangeFeaturePriorityRequest
{
    public FeaturePriority NewPriority { get; set; }
}