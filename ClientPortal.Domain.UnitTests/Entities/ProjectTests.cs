using ClientPortal.Domain.Entities;
using FluentAssertions;

namespace ClientPortal.Domain.UnitTests.Entities;

public class ProjectTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesProjectWithFields()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validScopeFeatures = 10;

        //Act
        var project = new Project(
            validId,
            validName,
            validDescription,
            validScopeFeatures);

        //Assert
        project.Id.Should().Be(validId);
        project.Name.Should().Be(validName);
        project.Description.Should().Be(validDescription);
        project.ScopeFeatures.Should().Be(validScopeFeatures);
    }
    
    [Fact]
    public void Constructor_WithInvalidId_ThrowsException()
    {
        //Arrange
        var invalidId = Guid.Empty;
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validScopeFeatures = 10;
        
        //Act
        var act = () => new Project(
            invalidId,
            validName,
            validDescription,
            validScopeFeatures);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Id*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidName_ThrowsException(string? invalidName)
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validDescription = "ValidDescription";
        var validScopeFeatures = 10;

        //Act
        var act=  () => new Project(
            validId,
            invalidName!,
            validDescription,
            validScopeFeatures);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Name*");
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidDescription_ThrowsException(string? invaliDescription)
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validScopeFeatures = 10;

        //Act
        var act=  () => new Project(
            validId,
            validName,
            invaliDescription!,
            validScopeFeatures);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Description*");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Constructor_WithInvalidScopeFeatures_ThrowsException(int invalidScopeFeatures)
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";

        //Act
        var act=  () => new Project(
            validId,
            validName,
            validDescription,
            invalidScopeFeatures);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*ScopeFeatures*");
    }

    [Fact]
    public void Rename_WithValidName_UpdatesName()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validScopeFeatures = 10;
        var project = new Project(validId,
            validName,
            validDescription,
            validScopeFeatures);
        var newName = "NewName";
        
        //Act
        var act = () => project.Rename(newName);
        
        //Assert
        act.Should().NotThrow();
        project.Name.Should().Be(newName);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Rename_WithInvalidName_ThrowsException(string? newName)
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validScopeFeatures = 10;
        var project = new Project(validId,
            validName,
            validDescription,
            validScopeFeatures);
        
        //Act
        var act = () => project.Rename(newName!);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Name*");
        project.Name.Should().Be(validName);
    }
    
    [Fact]
    public void UpdateDescription_WithValidDescription_UpdatesDescription()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validScopeFeatures = 10;
        var project = new Project(validId,
            validName,
            validDescription,
            validScopeFeatures);
        var newDescription = "NewDescription";
        
        //Act
        var act = () => project.UpdateDescription(newDescription);
        
        //Assert
        act.Should().NotThrow();
        project.Description.Should().Be(newDescription);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateDescription_WithInvalidDescription_ThrowsException(string? newDescription)
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validScopeFeatures = 10;
        var project = new Project(validId,
            validName,
            validDescription,
            validScopeFeatures);
        
        //Act
        var act = () => project.UpdateDescription(newDescription!);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Description*");
        project.Description.Should().Be(validDescription);
    }
    
    [Fact]
    public void ChangeScope_WithValidScopeFeatures_UpdatesScopeFeatures()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validScopeFeatures = 10;
        var project = new Project(validId,
            validName,
            validDescription,
            validScopeFeatures);
        var newScopeFeatures = 11;
        
        //Act
        var act = () => project.ChangeScope(newScopeFeatures);
        
        //Assert
        act.Should().NotThrow();
        project.ScopeFeatures.Should().Be(newScopeFeatures);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void ChangeScope_WithInvalidScopeFeatures_ThrowsException(int newScopeFeatures)
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validScopeFeatures = 10;
        var project = new Project(validId,
            validName,
            validDescription,
            validScopeFeatures);

        //Act
        var act = () => project.ChangeScope(newScopeFeatures);

        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*ScopeFeatures*");
        project.ScopeFeatures.Should().Be(validScopeFeatures);
    }
}