using ClientPortal.Application.Projects.DTOs;
using MediatR;

namespace ClientPortal.Application.Projects.Commands.RenameProject;

public class RenameProjectCommand : IRequest<ProjectDto>
{
    public Guid Id { get; set; }
    public string NewName { get; set; } = string.Empty;
}