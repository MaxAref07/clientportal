using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
using MediatR;

namespace ClientPortal.Application.Projects.Queries.GetProjectById;

public class GetProjectByIdQueryHandler(IProjectReadRepository projectReadRepository, IFeatureReadRepository featureReadRepository) : IRequestHandler<GetProjectByIdQuery, ProjectDto?>
{
    public async Task<ProjectDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        Project? project = await projectReadRepository.GetProjectById(request.Id);

        if (project == null)
            return null;

        var currentFeatures = await featureReadRepository.GetFeaturesByProjectId(project.Id);
        var currentFeaturesCount = currentFeatures.Count;
        var completedFeaturesCount = currentFeatures.Count(x => x.Status == FeatureStatus.Done);
        
        return new ProjectDto(project.Id,
            project.Name,
            project.Description,
            project.ScopeFeatures,
            currentFeaturesCount,
            completedFeaturesCount);
    }
}