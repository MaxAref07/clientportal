using ClientPortal.Application.DTOs;
using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using MediatR;

namespace ClientPortal.Application.Features.Commands.ChangeFeatureDescription;

public class ChangeFeatureDescriptionCommandHandler(IFeatureReadRepository featureReadRepository) : IRequestHandler<ChangeFeatureDescriptionCommand, FeatureDto>
{
    public async Task<FeatureDto> Handle(ChangeFeatureDescriptionCommand request, CancellationToken cancellationToken)
    {
        var feature = await featureReadRepository.GetFeatureById(request.Id);

        if (feature == null)
            throw new FeatureNotFoundException($"Feature with id {request.Id} was not found");
        
        feature.ChangeDescription(request.NewDescription);
        
        return new FeatureDto(feature.Id, feature.Name, feature.Priority, feature.Status, feature.Description, feature.ProjectId);
    }
}