using MediatR;

namespace ClientPortal.Application.Auth.Commands.VerifyMagicLink;

public class VerifyMagicLinkCommand : IRequest<VerifyMagicLinkResult>
{
    public required string Token { get; set; }
}