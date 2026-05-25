using ClientPortal.Application.DTOs;
using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using ClientPortal.Domain.Entities;
using MediatR;
using System.Linq;

namespace ClientPortal.Application.Features.Queries.GetFeaturesByProjectIdQuery;

public class GetFeaturesByProjectIdQueryHandler(IFeatureReadRepository featureReadRepository, IProjectReadRepository projectReadRepository) : IRequestHandler<GetFeaturesByProjectIdQuery, List<FeatureDto>>
{
    public async Task<List<FeatureDto>> Handle(GetFeaturesByProjectIdQuery request, CancellationToken cancellationToken)
    {
        Project? project = await projectReadRepository.GetProjectById(request.ProjectId);

        if (project == null)
            throw new ProjectNotFoundException($"Project with id {request.ProjectId} not found");
        
        List<Feature> features = await featureReadRepository.GetFeaturesByProjectId(request.ProjectId);

        List<FeatureDto> responseFeatures = features
            .Select(f => new FeatureDto(
                f.Id,
                f.Name,
                f.Priority,
                f.Status,
                f.Description,
                f.ProjectId
            ))
            .ToList();

        return responseFeatures;
    }
}