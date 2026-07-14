using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Features.Commands.DeleteFeature;
using ClientPortal.Application.Interfaces;
using MediatR;

namespace ClientPortal.Application.Projects.Commands.DeleteProject;

public class DeleteProjectCommandHandler(IProjectReadRepository projectReadRepository,
    IProjectRepository projectRepository,
    IFeatureReadRepository featureReadRepository,
    IFeatureRepository featureRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteProjectCommand>
{
    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projectReadRepository.GetProjectById(request.Id);
        
        if (project == null)
            throw new ProjectNotFoundException($"Project with id {request.Id} was not found");
        
        var projectFeatures = await featureReadRepository.GetFeaturesByProjectId(project.Id);

        if (projectFeatures.Count != 0)
        {
            foreach (var feature in projectFeatures)
                await featureRepository.Delete(feature.Id);
        }
        
        await projectRepository.Delete(request.Id);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}