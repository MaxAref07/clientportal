using ClientPortal.Application.DTOs;
using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using MediatR;

namespace ClientPortal.Application.Features.Commands.DeleteFeature;

public class DeleteFeatureCommandHandler(IFeatureReadRepository featureReadRepository, IFeatureRepository featureRepository) : IRequestHandler<DeleteFeatureCommand, FeatureDto>
{
    public async Task<FeatureDto> Handle(DeleteFeatureCommand request, CancellationToken cancellationToken)
    {
        var feature = await featureReadRepository.GetFeatureById(request.Id);

        if (feature is null)
            throw new FeatureNotFoundException($"Feature with id {request.Id} was not found");
        
        await featureRepository.Delete(request.Id);

        return new FeatureDto(feature.Id, feature.Name, feature.Priority, feature.Status, feature.Description, feature.ProjectId);
    }
}