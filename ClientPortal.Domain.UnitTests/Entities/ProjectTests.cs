using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
using ClientPortal.Domain.Exceptions;
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
        var act = () => project.ChangeScope(newScopeFeatures, 7);
        
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
        var act = () => project.ChangeScope(newScopeFeatures, 7);

        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*ScopeFeatures*");
        project.ScopeFeatures.Should().Be(validScopeFeatures);
    }

    [Fact]
    public void ChangeScope_WithInvalidExistingFeatureCount_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validScopeFeatures = 10;
        var validNewScopeFeatures = 6;
        var project = new Project(validId,
            validName,
            validDescription,
            validScopeFeatures);

        //Act
        var act = () => project.ChangeScope(validNewScopeFeatures, 7);

        //Assert
        act.Should().Throw<MinimumFeatureScopeException>()
            .WithMessage("*scope*");
        project.ScopeFeatures.Should().Be(validScopeFeatures);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(9)]
    public void AddFeature_WithValidExistingFeatureCount_ReturnsFeature(int existingFeatureCount)
    {
        //Arrange
        var validScopeFeatures = 10;
        var project = new Project(Guid.NewGuid(), "ValidName", "ValidDescription", validScopeFeatures);
        var featureId = Guid.NewGuid();
        var featureName = "FeatureName";
        var featureDescription = "FeatureDescription";
        var featurePriority = FeaturePriority.High;

        //Act
        Feature? feature = null;
        var act = () => feature = project.AddFeature(featureId, featureName, featureDescription, featurePriority, existingFeatureCount);

        //Assert
        act.Should().NotThrow();
        feature!.Id.Should().Be(featureId);
        feature.Name.Should().Be(featureName);
        feature.Description.Should().Be(featureDescription);
        feature.Priority.Should().Be(featurePriority);
        feature.Status.Should().Be(FeatureStatus.ToDo);
        feature.ProjectId.Should().Be(project.Id);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(11)]
    public void AddFeature_WithScopeFeaturesExceeded_ThrowsException(int existingFeatureCount)
    {
        //Arrange
        var validScopeFeatures = 10;
        var project = new Project(Guid.NewGuid(), "ValidName", "ValidDescription", validScopeFeatures);

        //Act
        var act = () => project.AddFeature(Guid.NewGuid(), "FeatureName", "FeatureDescription", FeaturePriority.High, existingFeatureCount);

        //Assert
        act.Should().Throw<FeaturesOutOfScopeException>()
            .WithMessage("*scope*");
    }
}