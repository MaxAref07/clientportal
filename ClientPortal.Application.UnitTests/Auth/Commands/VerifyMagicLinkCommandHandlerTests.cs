using ClientPortal.Application.Auth.Commands.VerifyMagicLink;
using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.UnitTests.TestHelpers;
using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
using FluentAssertions;
using NSubstitute;

namespace ClientPortal.Application.UnitTests.Auth.Commands;

public class VerifyMagicLinkCommandHandlerTests
{
    private readonly IMagicLinkReadRepository _magicLinkReadRepository = Substitute.For<IMagicLinkReadRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserReadRepository _userReadRepository = Substitute.For<IUserReadRepository>();
    private readonly VerifyMagicLinkCommandHandler _handler;

    public VerifyMagicLinkCommandHandlerTests()
    {
        _handler = new VerifyMagicLinkCommandHandler(_magicLinkReadRepository, _unitOfWork, _userReadRepository);
    }

    [Fact]
    public async Task VerifyMagicLinkCommand_WithValidData_ShouldVerifyMagicLink()
    {
        //Arrange
        var validEmail = "validemail@gmail.com";
        var validUserId = Guid.NewGuid();
        var validUserRole = UserRole.Member;

        var magicLink = TestData.MagicLink(email: validEmail);
        var user = TestData.User(id: validUserId, email: validEmail, role: validUserRole);

        var command = new VerifyMagicLinkCommand
        {
            Token = "token"
        };

        _magicLinkReadRepository
            .GetByTokenHash(Arg.Any<string>())
            .Returns(magicLink);
        _userReadRepository
            .GetByEmail(validEmail)
            .Returns(user);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        magicLink.IsUsed().Should().BeTrue();
        await _unitOfWork
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
        await _userReadRepository
            .Received(1)
            .GetByEmail(validEmail);
        result.UserId.Should().Be(validUserId);
        result.Email.Should().Be(validEmail);
        result.Role.Should().Be(validUserRole);
    }

    [Fact]
    public async Task VerifyMagicLinkCommand_WithNonExistentLink_ShouldThrowException()
    {
        //Arrange
        var command = new VerifyMagicLinkCommand
        {
            Token = "token"
        };

        _magicLinkReadRepository
            .GetByTokenHash(Arg.Any<string>())
            .Returns((MagicLink?)null);

        //Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        //Assert
        await act.Should().ThrowAsync<InvalidMagicLinkException>()
            .WithMessage("*exist*");
        await _unitOfWork
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task VerifyMagicLinkCommand_WithMagicLinkExpired_ShouldThrowException()
    {
        //Arrange
        var magicLink = TestData.MagicLink(expiresAt: DateTime.UtcNow - TimeSpan.FromDays(1));

        var command = new VerifyMagicLinkCommand
        {
            Token = "token"
        };

        _magicLinkReadRepository
            .GetByTokenHash(Arg.Any<string>())
            .Returns(magicLink);

        //Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        //Assert
        await act.Should().ThrowAsync<InvalidMagicLinkException>()
            .WithMessage("*expired*");
        await _unitOfWork
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task VerifyMagicLinkCommand_WithMagicLinkUsed_ShouldThrowException()
    {
        //Arrange
        var magicLink = TestData.MagicLink();
        magicLink.MarkAsUsed();

        var command = new VerifyMagicLinkCommand
        {
            Token = "token"
        };

        _magicLinkReadRepository
            .GetByTokenHash(Arg.Any<string>())
            .Returns(magicLink);

        //Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        //Assert
        await act.Should().ThrowAsync<InvalidMagicLinkException>()
            .WithMessage("*used*");
        await _unitOfWork
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task VerifyMagicLinkCommand_WithNonExistentUser_ShouldThrowException()
    {
        //Arrange
        var magicLink = TestData.MagicLink();

        var command = new VerifyMagicLinkCommand
        {
            Token = "token"
        };

        _magicLinkReadRepository
            .GetByTokenHash(Arg.Any<string>())
            .Returns(magicLink);
        _userReadRepository
            .GetByEmail(Arg.Any<string>())
            .Returns((User?)null);

        //Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        //Assert
        await act.Should().ThrowAsync<InvalidMagicLinkException>()
            .WithMessage("*User*");
        await _unitOfWork
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
