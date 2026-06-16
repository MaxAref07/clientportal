using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.DTOs;
using MediatR;

namespace ClientPortal.Application.Projects.Commands.ChangeProjectScopeFeatures;

public class ChangeProjectScopeFeaturesCommandHandler(IProjectReadRepository projectReadRepository) : IRequestHandler<ChangeProjectScopeFeaturesCommand, ProjectDto>
{
    public async Task<ProjectDto> Handle(ChangeProjectScopeFeaturesCommand request, CancellationToken cancellationToken)
    {
        var project = await projectReadRepository.GetProjectById(request.Id);
        
        if (project == null)
            throw new ProjectNotFoundException($"Project with id {request.Id} was not found");
        
        project.ChangeScope(request.NewScopeFeatures);
        
        return new ProjectDto(project.Id, project.Name, project.Description, project.ScopeFeatures);
    }
}