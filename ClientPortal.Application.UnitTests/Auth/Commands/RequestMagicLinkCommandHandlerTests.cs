using ClientPortal.Application.Auth.Commands.RequestMagicLink;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.UnitTests.TestHelpers;
using ClientPortal.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace ClientPortal.Application.UnitTests.Auth.Commands;

public class RequestMagicLinkCommandHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IUserReadRepository _userReadRepository = Substitute.For<IUserReadRepository>();
    private readonly IMagicLinkRepository _magicLinkRepository = Substitute.For<IMagicLinkRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly RequestMagicLinkCommandHandler _handler;

    public RequestMagicLinkCommandHandlerTests()
    {
        _handler = new RequestMagicLinkCommandHandler(_userRepository, _userReadRepository, _magicLinkRepository, _unitOfWork);
    }

    [Fact]
    public async Task RequestMagicLinkCommand_WithExistingUser_DoesNotCreateUser()
    {
        //Arrange
        var email = "validemail@gmail.com";
        var command = new RequestMagicLinkCommand
        {
            Email = email
        };
        _userReadRepository
            .GetByEmail(email)
            .Returns(TestData.User(email: email));

        //Act
        await _handler.Handle(command, CancellationToken.None);

        //Assert
        await _userRepository.DidNotReceive().Add(Arg.Any<User>());
        await _magicLinkRepository.Received(1).Add(Arg.Any<MagicLink>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RequestMagicLinkCommand_WithNonExistentUser_CreatesUser()
    {
        //Arrange
        var email = "validemail@gmail.com";
        var command = new RequestMagicLinkCommand
        {
            Email = email
        };
        _userReadRepository
            .GetByEmail(email)
            .Returns((User?)null);

        //Act
        await _handler.Handle(command, CancellationToken.None);

        //Assert
        await _userRepository.Received(1).Add(Arg.Is<User>(u => u != null && u.Email == email));
        await _magicLinkRepository.Received(1).Add(Arg.Any<MagicLink>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RequestMagicLinkCommand_WithUnnormalizedEmail_NormalizesEmailBeforeLookup()
    {
        //Arrange
        var command = new RequestMagicLinkCommand
        {
            Email = "  VALIDEMAIL@GMAIL.COM  "
        };
        _userReadRepository
            .GetByEmail("validemail@gmail.com")
            .Returns(TestData.User(email: "validemail@gmail.com"));

        //Act
        await _handler.Handle(command, CancellationToken.None);

        //Assert
        await _userReadRepository.Received(1).GetByEmail("validemail@gmail.com");
    }

    [Fact]
    public async Task RequestMagicLinkCommand_WithValidData_ReturnsUrlSafeTokenAndStoresItsHash()
    {
        //Arrange
        var email = "validemail@gmail.com";
        var command = new RequestMagicLinkCommand
        {
            Email = email
        };
        _userReadRepository
            .GetByEmail(email)
            .Returns(TestData.User(email: email));

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.Token.Should().NotBeNullOrWhiteSpace();
        result.Token.Should().NotContain("+").And.NotContain("/").And.NotContain("=");
        await _magicLinkRepository
            .Received(1)
            .Add(Arg.Is<MagicLink>(ml => ml != null && ml.Email == email && ml.TokenHash != result.Token));
    }
}
