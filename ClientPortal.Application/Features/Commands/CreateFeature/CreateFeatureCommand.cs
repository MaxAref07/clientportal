using ClientPortal.Application.DTOs;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
using MediatR;

namespace ClientPortal.Application.Features.Commands.CreateFeature;

public class CreateFeatureCommand : IRequest<FeatureDto>
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public FeaturePriority Priority { get; set; }
    public Guid ProjectId { get; set; }
}