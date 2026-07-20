using System.Security.Cryptography;
using System.Text;
using ClientPortal.Application.Interfaces;
using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
using MediatR;

namespace ClientPortal.Application.Auth.Commands.RequestMagicLink;

public record RequestMagicLinkResult(string Token);

public class RequestMagicLinkCommandHandler(
    IUserRepository userRepository, 
    IUserReadRepository userReadRepository, 
    IMagicLinkRepository magicLinkRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RequestMagicLinkCommand, RequestMagicLinkResult>
{
    public async Task<RequestMagicLinkResult> Handle(RequestMagicLinkCommand request, CancellationToken cancellationToken)
    {
        var formattedEmail = request.Email.Trim().ToLowerInvariant();
        
        var user = await userReadRepository.GetByEmail(formattedEmail);
        if (user == null)
        {
            user = new User(Guid.NewGuid(), formattedEmail, UserRole.Member);
            await userRepository.Add(user);
        }

        byte[] tokenBytes = RandomNumberGenerator.GetBytes(32);
        string rawBase64 = Convert.ToBase64String(tokenBytes);
        string rawToken = rawBase64
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');

        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
        string tokenHash = Convert.ToHexString(hashBytes).ToLowerInvariant();

        var expiresAt = DateTime.UtcNow.AddMinutes(15);
        var magicLink = new MagicLink(Guid.NewGuid(), formattedEmail, tokenHash, expiresAt);

        await magicLinkRepository.Add(magicLink);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new RequestMagicLinkResult(rawToken);
    }
}