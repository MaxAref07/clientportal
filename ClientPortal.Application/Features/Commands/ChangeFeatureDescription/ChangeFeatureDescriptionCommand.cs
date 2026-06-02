using ClientPortal.Application.DTOs;
using MediatR;

namespace ClientPortal.Application.Features.Commands.ChangeFeatureDescription;

public class ChangeFeatureDescriptionCommand : IRequest<FeatureDto>
{
    public Guid Id { get; set; }
    public string NewDescription { get; set; } = string.Empty;
}