using ClientPortal.Application.Projects.DTOs;
using MediatR;

namespace ClientPortal.Application.Projects.Commands.ChangeProjectDescription;

public class ChangeProjectDescriptionCommand : IRequest<ProjectDto>
{
    public Guid Id { get; set; }
    public string NewDescription { get; set; } = string.Empty;
}