using System.ComponentModel.DataAnnotations;
using ClientPortal.Application.Projects.DTOs;
using MediatR;

namespace ClientPortal.Application.Projects.Commands.CreateProject;

public class CreateProjectCommand : IRequest<ProjectDTO>
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int ScopeFeatures { get; set; }
}