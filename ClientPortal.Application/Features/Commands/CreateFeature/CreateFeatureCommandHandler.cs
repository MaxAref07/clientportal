using ClientPortal.Application.DTOs;
using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
using MediatR;

namespace ClientPortal.Application.Features.Commands.CreateFeature;

public class CreateFeatureCommandHandler(IFeatureRepository featureRepository, IFeatureReadRepository readFeatureRepository, IProjectReadRepository projectReadRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateFeatureCommand, FeatureDto>
{
    public async Task<FeatureDto> Handle(CreateFeatureCommand request, CancellationToken cancellationToken)
    {
        var project = await projectReadRepository.GetProjectById(request.ProjectId);

        if (project == null)
            throw new ProjectNotFoundException($"Project with id {request.ProjectId} not found");
        
        var features = await readFeatureRepository.GetFeaturesByProjectId(project.Id);
        
        if (features.Count() >= project.ScopeFeatures)
            throw new FeaturesOutOfScopeException($"Feature scope for project {project.Id} has been exceeded");
        
        var feature = new Feature(Guid.NewGuid(), request.Name, request.Description, request.Priority, FeatureStatus.ToDo, request.ProjectId);
        
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