using ClientPortal.Application.DTOs;
using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using MediatR;

namespace ClientPortal.Application.Features.Commands.ChangeFeatureStatus;

public class ChangeFeatureStatusCommandHandler(IFeatureReadRepository featureReadRepository) : IRequestHandler<ChangeFeatureStatusCommand, FeatureDto>
{
    public async Task<FeatureDto> Handle(ChangeFeatureStatusCommand request, CancellationToken cancellationToken)
    {
        var feature = await featureReadRepository.GetFeatureById(request.Id);
        
        if (feature == null)
            throw new FeatureNotFoundException($"Feature with id {request.Id} was not found");

        try
        {
            feature.ChangeStatus(request.NewStatus);
        }
        catch (ArgumentException ex)
        {
            throw new InvalidFeatureStatusTransitionException(ex.Message, ex);
        }

        return new FeatureDto(feature.Id, feature.Name, feature.Priority, feature.Status, feature.Description, feature.ProjectId);
    }
}