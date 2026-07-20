using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.DTOs;
using MediatR;

namespace ClientPortal.Application.Projects.Commands.RenameProject;

public class RenameProjectCommandHandler(IProjectReadRepository projectReadRepository, IUnitOfWork unitOfWork) : IRequestHandler<RenameProjectCommand, ProjectDto>
{
    public async Task<ProjectDto> Handle(RenameProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projectReadRepository.GetProjectById(request.Id);

        if (project == null)
            throw new ProjectNotFoundException($"Project with id {request.Id} was not found");

        project.Rename(request.NewName);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return (await projectReadRepository.GetProjectWithCountsById(project.Id))!;
    }
}