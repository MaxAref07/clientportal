using ClientPortal.Application.DTOs;
using MediatR;

namespace ClientPortal.Application.Auth.Commands.RequestMagicLink;

public class RequestMagicLinkCommand : IRequest<RequestMagicLinkResult>
{
    public required string Email { get; set; }
}