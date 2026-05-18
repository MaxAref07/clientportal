using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Domain.Entities;
using MediatR;

namespace ClientPortal.Application.Projects.Queries.GetProjectById;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto?>
{
    private readonly IProjectReadRepository _projectReadRepository;
    
    public GetProjectByIdQueryHandler(IProjectReadRepository projectReadRepository)
    {
        _projectReadRepository = projectReadRepository;
    }

    public async Task<ProjectDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        Project? project = await _projectReadRepository.GetProjectById(request.Id);

        if (project == null)
            return null;
        
        return new ProjectDto(project.Id, project.Name, project.Description, project.ScopeFeatures);
    }
}