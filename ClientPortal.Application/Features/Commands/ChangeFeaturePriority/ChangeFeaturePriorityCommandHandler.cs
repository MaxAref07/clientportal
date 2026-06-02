using ClientPortal.Application.DTOs;
using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using MediatR;

namespace ClientPortal.Application.Features.Commands.ChangeFeaturePriority;

public class ChangeFeaturePriorityCommandHandler(IFeatureReadRepository featureReadRepository) : IRequestHandler<ChangeFeaturePriorityCommand, FeatureDto>
{
    public async Task<FeatureDto> Handle(ChangeFeaturePriorityCommand request, CancellationToken cancellationToken)
    {
        var feature = await featureReadRepository.GetFeatureById(request.Id);

        if (feature == null)
            throw new FeatureNotFoundException($"Feature with id {request.Id} was not found");

        feature.ChangePriority(request.NewPriority);
        
        return new FeatureDto(feature.Id, feature.Name, feature.Priority, feature.Status, feature.Description, feature.ProjectId);
    }
}