using ClientPortal.Domain.Enums;

namespace ClientPortal.Application.Projects.Commands.CreateFeature;

public class CreateFeatureCommand
{
    public required string Name { get; set; }
    
    public required string Description { get; set; }
    
    public FeaturePriority Priority { get; set; }
}