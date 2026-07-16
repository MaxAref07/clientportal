using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
using MediatR;

namespace ClientPortal.Application.Projects.Queries.GetProjectById;

public class GetProjectByIdQueryHandler(IProjectReadRepository projectReadRepository)
    : IRequestHandler<GetProjectByIdQuery, ProjectDto?>
{
    public async Task<ProjectDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        return await projectReadRepository.GetProjectWithCountsById(request.Id);
    }
}