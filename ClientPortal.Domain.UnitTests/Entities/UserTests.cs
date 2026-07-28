using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
using FluentAssertions;

namespace ClientPortal.Domain.UnitTests.Entities;

public class UserTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesUserWithFields()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validEmail = "validemail@gmail.com";
        var role = UserRole.Member;
        
        //Act
        var user = new User(validId, validEmail, role);
        
        //Assert
        user.Id.Should().Be(validId);
        user.Email.Should().Be(validEmail);
        user.Role.Should().Be(role);
    }
    
    [Fact]
    public void Constructor_WithInvalidId_ThrowsException()
    {
        //Arrange
        var invalidId = Guid.Empty;
        var validEmail = "validemail@gmail.com";
        var validRole = UserRole.Member;
        
        //Act
        var act = () => new User(invalidId, validEmail, validRole);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Id*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidEmail_ThrowsException(string? invalidEmail)
    {
        //Arrange
        var validId = Guid.NewGuid();
        var role = UserRole.Member;
        
        //Act
        var act= () => new User(validId, invalidEmail!, role);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Email*");
    }

    [Fact]
    public void Constructor_WithInvalidRole_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validEmail = "validemail@gmail.com";
        var invalidRole = (UserRole)9999;
        
        //Act
        var act = () => new User(validId, validEmail, invalidRole);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Role*");
    }
}