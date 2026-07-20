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
        
        var currentFeaturesCount = await featureReadRepository.CountByProjectId(project.Id);
        if (currentFeaturesCount > request.NewScopeFeatures)
            throw new MinimumFeatureScopeException(currentFeaturesCount, request.NewScopeFeatures);
        
        project.ChangeScope(request.NewScopeFeatures);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return (await projectReadRepository.GetProjectWithCountsById(project.Id))!;
    }
}