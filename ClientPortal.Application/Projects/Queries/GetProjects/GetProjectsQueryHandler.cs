using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
using MediatR;

namespace ClientPortal.Application.Projects.Queries.GetProjects;

public class GetProjectsQueryHandler(IProjectReadRepository projectReadRepository, IFeatureReadRepository featureReadRepository) : IRequestHandler<GetProjectsQuery, List<ProjectDto>>
{
    public async Task<List<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        List<Project> projects = await projectReadRepository.GetProjects();

        var allFeatures = await featureReadRepository.GetFeatures();
        
        return new List<ProjectDto>(projects.Select(project => new ProjectDto(project.Id,
            project.Name,
            project.Description,
            project.ScopeFeatures,
            allFeatures.Count(x => x.ProjectId == project.Id),
            allFeatures.Count(x => x.ProjectId == project.Id && x.Status == FeatureStatus.Done))));
    }
}