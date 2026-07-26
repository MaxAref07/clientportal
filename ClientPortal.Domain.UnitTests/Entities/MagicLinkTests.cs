using ClientPortal.Domain.Entities;
using FluentAssertions;

namespace ClientPortal.Domain.UnitTests.Entities;

public class MagicLinkTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesMagicLinkWithFields()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validEmail = "validemail@gmail.com";
        var validTokenHash = "ValidHash";
        var validExpiresAt = DateTime.UtcNow + TimeSpan.FromDays(1);

        //Act
        var magicLink = new MagicLink(validId,
            validEmail,
            validTokenHash,
            validExpiresAt);
        
        //Assert
        magicLink.Id.Should().Be(validId);
        magicLink.Email.Should().Be(validEmail);
        magicLink.TokenHash.Should().Be(validTokenHash);
        magicLink.ExpiresAt.Should().Be(validExpiresAt);
        magicLink.UsedAt.Should().Be(null);
        magicLink.IsUsed().Should().BeFalse();
    }
    
    [Fact]
    public void Constructor_WithInvalidId_ThrowsException()
    {
        //Arrange
        var invalidId = Guid.Empty;
        var validEmail = "validemail@gmail.com";
        var validTokenHash = "ValidHash";
        var validExpiresAt = DateTime.UtcNow + TimeSpan.FromDays(1);
        
        //Act
        var act = () => new MagicLink(
            invalidId,
            validEmail!,
            validTokenHash,
            validExpiresAt);
        
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
        var validTokenHash = "ValidHash";
        var validExpiresAt = DateTime.UtcNow + TimeSpan.FromDays(1);
        
        //Act
        var act = () => new MagicLink(
            validId,
            invalidEmail!,
            validTokenHash,
            validExpiresAt);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Email*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidTokenHash_ThrowsException(string? invalidTokenHash)
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validEmail = "validemail@gmail.com";
        var validExpiresAt = DateTime.UtcNow + TimeSpan.FromDays(1);
        
        //Act
        var act = () => new MagicLink(
            validId,
            validEmail,
            invalidTokenHash!,
            validExpiresAt);

        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*TokenHash*");
    }

    [Fact]
    public void Constructor_WithInvalidExpiresAt_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validEmail = "validemail@gmail.com";
        var validTokenHash = "ValidHash";
        var invalidExpiresAt = DateTime.MinValue;
        
        //Act
        var act = () => new MagicLink(
            validId,
            validEmail,
            validTokenHash,
            invalidExpiresAt);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*ExpiresAt*");
    }
    
    [Fact]
    public void IsExpired_WithExpiresAtInPast_ReturnsTrue()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validEmail = "validemail@gmail.com";
        var validTokenHash = "ValidHash";
        var validExpiresAt = DateTime.UtcNow - TimeSpan.FromDays(1);
        var magicLink = new MagicLink(
            validId,
            validEmail,
            validTokenHash,
            validExpiresAt);
        
        //Act
        var result = magicLink.IsExpired();
        
        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void IsExpired_WithExpiresAtInFuture_ReturnsFalse()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validEmail = "validemail@gmail.com";
        var validTokenHash = "ValidHash";
        var validExpiresAt = DateTime.UtcNow + TimeSpan.FromDays(1);
        var magicLink = new MagicLink(
            validId,
            validEmail,
            validTokenHash,
            validExpiresAt);
        
        //Act
        var result = magicLink.IsExpired();
        
        //Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void IsUsed_WhenNotUsed_ReturnsFalse()
    {
        // Arrange
        var validId = Guid.NewGuid();
        var validEmail = "validemail@gmail.com";
        var validTokenHash = "ValidHash";
        var validExpiresAt = DateTime.UtcNow + TimeSpan.FromDays(1);
        var magicLink = new MagicLink(
            validId,
            validEmail,
            validTokenHash,
            validExpiresAt);
    
        // Act
        var result = magicLink.IsUsed();
    
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void MarkAsUsed_WhenNotUsedAndNotExpired_UpdatesUsedAt()
    {
        // Arrange
        var validId = Guid.NewGuid();
        var validEmail = "validemail@gmail.com";
        var validTokenHash = "ValidHash";
        var validExpiresAt = DateTime.UtcNow + TimeSpan.FromDays(1);
        var magicLink = new MagicLink(
            validId,
            validEmail,
            validTokenHash,
            validExpiresAt);
        
        //Act
        magicLink.MarkAsUsed();
        
        //Assert
        magicLink.IsUsed().Should().BeTrue();
    }
    
    [Fact]
    public void MarkAsUsed_WhenLinkIsExpired_DoesNotChangeUsedAt()
    {
        // Arrange
        var validId = Guid.NewGuid();
        var validEmail = "validemail@gmail.com";
        var validTokenHash = "ValidHash";
        var validExpiresAt = DateTime.UtcNow - TimeSpan.FromDays(1);
        var magicLink = new MagicLink(
            validId,
            validEmail,
            validTokenHash,
            validExpiresAt);
    
        // Act
        magicLink.MarkAsUsed();
    
        // Assert
        magicLink.IsUsed().Should().BeFalse();
        magicLink.UsedAt.Should().BeNull();
    }
    
    [Fact]
    public void MarkAsUsed_WhenUsedAtIsNotNull_DoesNotChangeUsedAt()
    {
        // Arrange
        var validId = Guid.NewGuid();
        var validEmail = "validemail@gmail.com";
        var validTokenHash = "ValidHash";
        var validExpiresAt = DateTime.UtcNow + TimeSpan.FromDays(1);
        var magicLink = new MagicLink(
            validId,
            validEmail,
            validTokenHash,
            validExpiresAt);
    
        // Act
        magicLink.MarkAsUsed();
    
        // Assert
        var tempUsedAt = magicLink.UsedAt;
        magicLink.MarkAsUsed();
        magicLink.UsedAt.Should().Be(tempUsedAt);
    }
}