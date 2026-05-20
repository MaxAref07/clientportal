using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Domain.Entities;
using MediatR;

namespace ClientPortal.Application.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler(IProjectRepository projectRepository) : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project(Guid.NewGuid(), request.Name, request.Description, request.ScopeFeatures);
        
        var createdProject = await projectRepository.Add(project);

        return new ProjectDto(
            createdProject.Id,
            createdProject.Name, 
            createdProject.Description,
            createdProject.ScopeFeatures);
    }
}