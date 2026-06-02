using ClientPortal.Application.DTOs;
using ClientPortal.Domain.Enums;
using MediatR;

namespace ClientPortal.Application.Features.Commands.ChangeFeaturePriority;

public class ChangeFeaturePriorityCommand : IRequest<FeatureDto>
{
    public Guid Id { get; set; }
    public FeaturePriority NewPriority { get; set; }
}