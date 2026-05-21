using ClientPortal.Application.DTOs;
using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using ClientPortal.Domain.Entities;
using MediatR;

namespace ClientPortal.Application.Features.Queries.GetFeatureById;

public class GetFeatureByIdHandler(IFeatureReadRepository featureReadRepository) : IRequestHandler<GetFeatureByIdQuery, FeatureDto>
{
    public async Task<FeatureDto> Handle(GetFeatureByIdQuery request, CancellationToken cancellationToken)
    {
        Feature? feature = await featureReadRepository.GetFeatureById(request.Id);

        if (feature == null)
            throw new FeatureNotFoundException($"Feature with id {request.Id} not found");
        
        return new FeatureDto(feature.Id, feature.Name, feature.Priority, feature.Status, feature.Description, feature.ProjectId);
    }
}