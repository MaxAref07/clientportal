using ClientPortal.Application.DTOs;
using MediatR;

namespace ClientPortal.Application.Features.Queries.GetFeaturesByProjectIdQuery;

public class GetFeaturesByProjectIdQuery(Guid id) : IRequest<List<FeatureDto>>
{
    public Guid ProjectId { get; set; } = id;
}