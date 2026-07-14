using ClientPortal.Application.DTOs;
using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using MediatR;

namespace ClientPortal.Application.Features.Commands.RenameFeature;

public class RenameFeatureCommandHandler(IFeatureReadRepository featureReadRepository, IUnitOfWork unitOfWork) : IRequestHandler<RenameFeatureCommand, FeatureDto>
{
    public async Task<FeatureDto> Handle(RenameFeatureCommand request, CancellationToken cancellationToken)
    {
        var feature = await featureReadRepository.GetFeatureById(request.Id);
        
        if (feature == null)
            throw new FeatureNotFoundException($"Feature with id {request.Id} was not found");
        
        feature.Rename(request.NewName);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new FeatureDto(feature.Id, feature.Name, feature.Priority, feature.Status, feature.Description, feature.ProjectId);
    }
}