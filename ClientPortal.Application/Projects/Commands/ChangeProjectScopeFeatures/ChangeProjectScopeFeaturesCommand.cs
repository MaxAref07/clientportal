using ClientPortal.Application.Projects.DTOs;
using MediatR;

namespace ClientPortal.Application.Projects.Commands.ChangeProjectScopeFeatures;

public class ChangeProjectScopeFeaturesCommand : IRequest<ProjectDto>
{
    public Guid Id { get; set; }
    public int NewScopeFeatures { get; set; }
}