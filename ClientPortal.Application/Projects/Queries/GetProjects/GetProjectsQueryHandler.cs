using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Domain.Entities;
using MediatR;

namespace ClientPortal.Application.Projects.Queries.GetProjects;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, List<ProjectDto>>
{
    private readonly IReadDbContext _dbContext;
    
    public GetProjectsQueryHandler(IReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        List<Project> projects = await _dbContext.GetProjects();

        return new List<ProjectDto>(projects.Select(project => new ProjectDto(project.Id, project.Name, project.Description, project.ScopeFeatures)));
    }
}