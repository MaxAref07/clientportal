using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Domain.Entities;
using MediatR;

namespace ClientPortal.Application.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler :  IRequestHandler<CreateProjectCommand, ProjectDTO>
{
    private readonly IProjectRepository _projectRepository;
    
    public CreateProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<ProjectDTO> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project(Guid.NewGuid(), request.Name, request.Description, request.ScopeFeatures);
        
        var createdProject = await _projectRepository.Add(project);

        return new ProjectDTO(
            createdProject.Id,
            createdProject.Name, 
            createdProject.Description,
            createdProject.ScopeFeatures);
    }
}