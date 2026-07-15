using ClientPortal.Application.DTOs;
using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using MediatR;

namespace ClientPortal.Application.Features.Commands.DeleteFeature;

public class DeleteFeatureCommandHandler(IFeatureReadRepository featureReadRepository, IFeatureRepository featureRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteFeatureCommand>
{
    public async Task Handle(DeleteFeatureCommand request, CancellationToken cancellationToken)
    {
        var feature = await featureReadRepository.GetFeatureById(request.Id);

        if (feature is null)
            throw new FeatureNotFoundException($"Feature with id {request.Id} was not found");
        
        await featureRepository.Delete(request.Id);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}