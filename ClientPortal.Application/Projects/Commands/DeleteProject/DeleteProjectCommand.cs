using MediatR;

namespace ClientPortal.Application.Projects.Commands.DeleteProject;

public class DeleteProjectCommand : IRequest
{
    public required Guid Id { get; set; }
}