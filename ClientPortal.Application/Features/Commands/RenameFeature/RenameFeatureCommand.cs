using ClientPortal.Application.DTOs;
using MediatR;

namespace ClientPortal.Application.Features.Commands.RenameFeature;

public class RenameFeatureCommand : IRequest<FeatureDto>
{
    public Guid Id { get; set; }
    public string NewName { get; set; } = string.Empty;
}