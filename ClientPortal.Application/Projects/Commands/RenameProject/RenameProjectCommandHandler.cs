using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Domain.Enums;
using MediatR;

namespace ClientPortal.Application.Projects.Commands.RenameProject;

public class RenameProjectCommandHandler(IProjectReadRepository projectReadRepository, IFeatureReadRepository featureReadRepository, IUnitOfWork unitOfWork) : IRequestHandler<RenameProjectCommand, ProjectDto>
{
    public async Task<ProjectDto> Handle(RenameProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projectReadRepository.GetProjectById(request.Id);

        if (project == null)
            throw new ProjectNotFoundException($"Project with id {request.Id} was not found");
        
        project.Rename(request.NewName);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        var currentFeatures = await featureReadRepository.GetFeaturesByProjectId(project.Id);
        var currentFeaturesCount = currentFeatures.Count;
        var completedFeaturesCount = currentFeatures.Count(x => x.Status == FeatureStatus.Done);
        
        return new ProjectDto(project.Id,
            project.Name,
            project.Description,
            project.ScopeFeatures,
            currentFeaturesCount,
            completedFeaturesCount);
    }
}