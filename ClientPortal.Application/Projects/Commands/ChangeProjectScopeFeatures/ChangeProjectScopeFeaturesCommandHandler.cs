using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Domain.Enums;
using MediatR;

namespace ClientPortal.Application.Projects.Commands.ChangeProjectScopeFeatures;

public class ChangeProjectScopeFeaturesCommandHandler(IProjectReadRepository projectReadRepository, IFeatureReadRepository featureReadRepository, IUnitOfWork unitOfWork) : IRequestHandler<ChangeProjectScopeFeaturesCommand, ProjectDto>
{
    public async Task<ProjectDto> Handle(ChangeProjectScopeFeaturesCommand request, CancellationToken cancellationToken)
    {
        var project = await projectReadRepository.GetProjectById(request.Id);
        
        if (project == null)
            throw new ProjectNotFoundException($"Project with id {request.Id} was not found");

        var features = await featureReadRepository.GetFeatures();
        var projectFeatures = features.Count(f => f.ProjectId == project.Id);
        
        if (features.Count(f => f.ProjectId == project.Id) > request.NewScopeFeatures)
            throw new MinimumFeatureScopeException(projectFeatures, request.NewScopeFeatures);
        
        project.ChangeScope(request.NewScopeFeatures);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var currentFeaturesCount = projectFeatures;
        var completedFeaturesCount = features.Count(x => x.Status == FeatureStatus.Done && x.ProjectId == project.Id);
        
        return new ProjectDto(project.Id,
            project.Name,
            project.Description,
            project.ScopeFeatures,
            currentFeaturesCount,
            completedFeaturesCount);
    }
}