using ClientPortal.Application.Projects.DTOs;
using MediatR;

namespace ClientPortal.Application.Projects.Queries.GetProjectById;

public class GetProjectByIdQuery : IRequest<ProjectDTO?>
{
    public Guid Id { get; set; }
    
    public GetProjectByIdQuery(Guid id)
    {
        Id = id;
    }
}