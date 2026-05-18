using ClientPortal.Application.DTOs;
using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
using MediatR;

namespace ClientPortal.Application.Features.Commands.CreateFeature;

public class CreateFeatureCommandHandler : IRequestHandler<CreateFeatureCommand, FeatureDto>
{
    private readonly IFeatureRepository _featureRepository;
    private readonly IFeatureReadRepository _readFeatureRepository;
    private readonly IProjectReadRepository _projectReadRepository;

    public CreateFeatureCommandHandler(IFeatureRepository featureRepository, IFeatureReadRepository readFeatureRepository, IProjectReadRepository projectReadRepository)
    {
        _featureRepository = featureRepository;
        _readFeatureRepository = readFeatureRepository;
        _projectReadRepository = projectReadRepository;
    }
    
    public async Task<FeatureDto> Handle(CreateFeatureCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectReadRepository.GetProjectById(request.ProjectId);

        if (project == null)
            throw new ProjectNotFoundException($"Project with id {request.ProjectId} not found");
        
        var features = await _readFeatureRepository.GetFeaturesByProjectId(project.Id);
        
        if (features.Count() >= project.ScopeFeatures)
            throw new FeaturesOutOfScopeException($"Feature scope for project {project.Id} has been exceeded");
        
        var feature = new Feature(Guid.NewGuid(), request.Name, request.Description, request.Priority, FeatureStatus.ToDo, request.ProjectId);
        
        var createdProject = await _featureRepository.Add(feature);

        return new FeatureDto(
            createdProject.Id,
            createdProject.Name, 
            createdProject.Priority,
            createdProject.Status,
            createdProject.Description,
            createdProject.ProjectId);
    }
}