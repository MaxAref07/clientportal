using ClientPortal.Application.Projects.DTOs;
using MediatR;

namespace ClientPortal.Application.Projects.Queries.GetProjects;

public class GetProjectsQuery : IRequest<List<ProjectDTO>>
{
    
}