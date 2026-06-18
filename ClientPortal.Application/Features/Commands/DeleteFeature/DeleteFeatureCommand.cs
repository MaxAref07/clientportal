using ClientPortal.Application.DTOs;
using ClientPortal.Domain.Entities;
using MediatR;

namespace ClientPortal.Application.Features.Commands.DeleteFeature;

public class DeleteFeatureCommand : IRequest
{
    public required Guid Id { get; set; }
}