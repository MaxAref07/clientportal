using ClientPortal.Application.DTOs;
using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
using MediatR;

namespace ClientPortal.Application.Features.Commands.CreateFeature;

public class CreateFeatureCommandHandler(IFeatureRepository featureRepository, IFeatureReadRepository featureReadRepository, IProjectReadRepository projectReadRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateFeatureCommand, FeatureDto>
{
    public async Task<FeatureDto> Handle(CreateFeatureCommand request, CancellationToken cancellationToken)
    {
        var project = await projectReadRepository.GetProjectById(request.ProjectId);

        if (project == null)
            throw new ProjectNotFoundException($"Project with id {request.ProjectId} not found");
        
        var existingFeatureCount = await featureReadRepository.CountByProjectId(project.Id);

        var feature = project.AddFeature(Guid.NewGuid(), request.Name, request.Description, request.Priority,
            existingFeatureCount);
        var createdFeature = await featureRepository.Add(feature);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new FeatureDto(
            createdFeature.Id,
            createdFeature.Name, 
            createdFeature.Priority,
            createdFeature.Status,
            createdFeature.Description,
            createdFeature.ProjectId);
    }
}