using ClientPortal.Application.DTOs;
using MediatR;

namespace ClientPortal.Application.Features.Queries.GetFeatureById;

public class GetFeatureByIdQuery(Guid id) : IRequest<FeatureDto>
{
    public Guid Id { get; set; } = id;
}