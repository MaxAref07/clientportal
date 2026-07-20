using System.Security.Cryptography;
using System.Text;
using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using ClientPortal.Domain.Enums;
using MediatR;

namespace ClientPortal.Application.Auth.Commands.VerifyMagicLink;

public record VerifyMagicLinkResult(Guid UserId, string Email, UserRole Role);

public class VerifyMagicLinkCommandHandler(IMagicLinkReadRepository magicLinkReadRepository, IUnitOfWork unitOfWork, IUserReadRepository userReadRepository) : IRequestHandler<VerifyMagicLinkCommand, VerifyMagicLinkResult>
{
    public async Task<VerifyMagicLinkResult> Handle(VerifyMagicLinkCommand request, CancellationToken cancellationToken)
    {
        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(request.Token));
        string tokenHash = Convert.ToHexString(hashBytes).ToLowerInvariant();
        
        var magicLink = await magicLinkReadRepository.GetByTokenHash(tokenHash);
        
        if (magicLink == null) throw new InvalidMagicLinkException("Magic link does not exist");
        if (magicLink.IsExpired()) throw new InvalidMagicLinkException("Magic link expired");
        if (magicLink.IsUsed()) throw new InvalidMagicLinkException("Magic link has already been used");
        
        magicLink.MarkAsUsed();
        
        var user = await userReadRepository.GetByEmail(magicLink.Email);

        if (user == null) throw new InvalidMagicLinkException("User associated with this link was not found");

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new VerifyMagicLinkResult(user.Id, user.Email, user.Role);
    }
}