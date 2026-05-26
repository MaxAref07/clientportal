using ClientPortal.Application.DTOs;
using ClientPortal.Domain.Enums;
using MediatR;

namespace ClientPortal.Application.Features.Commands.ChangeFeatureStatus;

public class ChangeFeatureStatusCommand : IRequest<FeatureDto>
{
    public required Guid Id { get; set; }
    public required FeatureStatus NewStatus { get; set; }
}