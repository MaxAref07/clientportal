using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Domain.Entities;
using MediatR;

namespace ClientPortal.Application.Projects.Queries.GetProjectById;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto?>
{
    private readonly IReadDbContext _dbContext;
    
    public GetProjectByIdQueryHandler(IReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProjectDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        Project? project = await _dbContext.GetProjectById(request.Id);

        if (project == null)
            return null;
        
        return new ProjectDto(project.Id, project.Name, project.Description, project.ScopeFeatures);
    }
}