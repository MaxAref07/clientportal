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
        var updatedProject = await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var project = await projectReadRepository.GetProjectByIdForUpdate(request.Id);

            if (project == null)
                throw new ProjectNotFoundException($"Project with id {request.Id} was not found");

            var existingFeatureCount = await featureReadRepository.CountByProjectId(project.Id);

            project.ChangeScope(request.NewScopeFeatures, existingFeatureCount);

            await unitOfWork.SaveChangesAsync(ct);
            
            return project;

        }, cancellationToken);
        
        return (await projectReadRepository.GetProjectWithCountsById(updatedProject.Id))!;
    }
}