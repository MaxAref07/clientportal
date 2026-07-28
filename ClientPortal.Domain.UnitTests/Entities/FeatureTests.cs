using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
using FluentAssertions;

namespace ClientPortal.Domain.UnitTests.Entities;

public class FeatureTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesFeatureWithFields()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validPriority = FeaturePriority.Low;
        var validStatus = FeatureStatus.ToDo;
        var validProjectId = Guid.NewGuid();
        
        //Act
        var feature = new Feature(validId, validName, validDescription, validPriority, validStatus, validProjectId);
        
        //Assert
        feature.Id.Should().Be(validId);
        feature.Name.Should().Be(validName);
        feature.Description.Should().Be(validDescription);
        feature.Priority.Should().Be(validPriority);
        feature.Status.Should().Be(validStatus);
        feature.ProjectId.Should().Be(validProjectId);
    }
    
    [Fact]
    public void Constructor_WithInvalidId_ThrowsException()
    {
        //Arrange
        var invalidId = Guid.Empty;
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validPriority = FeaturePriority.Low;
        var validStatus = FeatureStatus.ToDo;
        var validProjectId = Guid.NewGuid();
        
        //Act
        var act = () => new Feature(
            invalidId,
            validName,
            validDescription,
            validPriority,
            validStatus,
            validProjectId);
        
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
        var validPriority = FeaturePriority.Low;
        var validStatus = FeatureStatus.ToDo;
        var validProjectId = Guid.NewGuid();
        
        //Act
        var act = () => new Feature(
            validId,
            invalidName!,
            validDescription,
            validPriority,
            validStatus,
            validProjectId);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Name*");
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidDescription_ThrowsException(string? invalidDescription)
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validPriority = FeaturePriority.Low;
        var validStatus = FeatureStatus.ToDo;
        var validProjectId = Guid.NewGuid();
        
        //Act
        var act = () => new Feature(
            validId,
            validName,
            invalidDescription!,
            validPriority,
            validStatus,
            validProjectId);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Description*");
    }

    [Fact]
    public void Constructor_WithInvalidPriority_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var invalidPriority = (FeaturePriority)9999;
        var validStatus = FeatureStatus.ToDo;
        var validProjectId = Guid.NewGuid();
        
        //Act
        var act = () => new Feature(
            validId,
            validName,
            validDescription,
            invalidPriority,
            validStatus,
            validProjectId);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Priority*");
    }
    
    [Fact]
    public void Constructor_WithInvalidStatus_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validPriority = FeaturePriority.Low;
        var invalidStatus = (FeatureStatus)9999;
        var validProjectId = Guid.NewGuid();
        
        //Act
        var act = () => new Feature(
            validId,
            validName,
            validDescription,
            validPriority,
            invalidStatus,
            validProjectId);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Status*");
    }
    
    [Fact]
    public void Constructor_WithInvalidProjectId_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validPriority = FeaturePriority.Low;
        var validStatus = FeatureStatus.ToDo;
        var invalidProjectId = Guid.Empty;
        
        //Act
        var act = () => new Feature(
            validId,
            validName,
            validDescription,
            validPriority,
            validStatus,
            invalidProjectId);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*ProjectId*");
    }
    
    [Fact]
    public void Rename_WithValidName_UpdatesName()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validPriority = FeaturePriority.Low;
        var validStatus = FeatureStatus.ToDo;
        var validProjectId = Guid.NewGuid();
        var feature =  new Feature(
            validId,
            validName,
            validDescription,
            validPriority,
            validStatus,
            validProjectId);
        var newName = "NewName";
        
        //Act
        var act = () => feature.Rename(newName);
        
        //Assert
        act.Should().NotThrow();
        feature.Name.Should().Be(newName);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Rename_WithInvalidName_ThrowsException(string? invalidNewName)
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validPriority = FeaturePriority.Low;
        var validStatus = FeatureStatus.ToDo;
        var validProjectId = Guid.NewGuid();
        var feature =  new Feature(
            validId,
            validName,
            validDescription,
            validPriority,
            validStatus,
            validProjectId);
        
        //Act
        var act = () => feature.Rename(invalidNewName!);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Name*");
        feature.Name.Should().Be(validName);
    }
    
    [Fact]
    public void ChangePriority_WithValidPriority_UpdatesPriority()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validPriority = FeaturePriority.Low;
        var validStatus = FeatureStatus.ToDo;
        var validProjectId = Guid.NewGuid();
        var feature =  new Feature(
            validId,
            validName,
            validDescription,
            validPriority,
            validStatus,
            validProjectId);
        var newPriority = FeaturePriority.Medium;
        
        //Act
        var act = () => feature.ChangePriority(newPriority);
        
        //Assert
        act.Should().NotThrow();
        feature.Priority.Should().Be(newPriority);
    }
    
    [Fact]
    public void ChangePriority_WithInvalidPriority_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validPriority = FeaturePriority.Low;
        var validStatus = FeatureStatus.ToDo;
        var validProjectId = Guid.NewGuid();
        var feature =  new Feature(
            validId,
            validName,
            validDescription,
            validPriority,
            validStatus,
            validProjectId);
        var newPriority = (FeaturePriority)9999;
        
        //Act
        var act = () => feature.ChangePriority(newPriority);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Priority*");
        feature.Priority.Should().Be(validPriority);
    }

    [Theory]
    [InlineData(FeatureStatus.ToDo, FeatureStatus.InProgress)]
    [InlineData(FeatureStatus.InProgress, FeatureStatus.OnReview)]
    [InlineData(FeatureStatus.OnReview, FeatureStatus.Done)]
    [InlineData(FeatureStatus.OnReview, FeatureStatus.InProgress)]
    [InlineData(FeatureStatus.Done, FeatureStatus.ToDo)]
    [InlineData(FeatureStatus.Done, FeatureStatus.InProgress)]
    [InlineData(FeatureStatus.Done, FeatureStatus.OnReview)]
    public void ChangeStatus_WithValidStatus_UpdatesStatus(FeatureStatus validStatus, FeatureStatus newStatus)
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validPriority = FeaturePriority.Low;
        var validProjectId = Guid.NewGuid();
        var feature =  new Feature(
            validId,
            validName,
            validDescription,
            validPriority,
            validStatus,
            validProjectId);
        
        //Act
        var act = () => feature.ChangeStatus(newStatus);
        
        //Assert
        act.Should().NotThrow();
        feature.Status.Should().Be(newStatus);
    }

    [Theory]
    [InlineData(FeatureStatus.ToDo, FeatureStatus.ToDo)]
    [InlineData(FeatureStatus.InProgress, FeatureStatus.InProgress)]
    [InlineData(FeatureStatus.OnReview, FeatureStatus.OnReview)]
    [InlineData(FeatureStatus.Done, FeatureStatus.Done)]
    public void ChangeStatus_ToSameStatus_DoesNotThrowAndKeepsStatus(FeatureStatus initialStatus, FeatureStatus newStatus)
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validPriority = FeaturePriority.Low;
        var validProjectId = Guid.NewGuid();
        var feature =  new Feature(
            validId,
            validName,
            validDescription,
            validPriority,
            initialStatus,
            validProjectId);
        
        //Act
        var act = () => feature.ChangeStatus(newStatus);
        
        //Assert
        act.Should().NotThrow();
        feature.Status.Should().Be(newStatus);
    }

    [Theory]
    [InlineData(FeatureStatus.ToDo, FeatureStatus.OnReview)]
    [InlineData(FeatureStatus.ToDo, FeatureStatus.Done)]
    [InlineData(FeatureStatus.InProgress, FeatureStatus.ToDo)]
    [InlineData(FeatureStatus.InProgress, FeatureStatus.Done)]
    [InlineData(FeatureStatus.OnReview, FeatureStatus.ToDo)]
    public void ChangeStatus_WithInvalidStatus_ThrowsException(FeatureStatus initialStatus, FeatureStatus newInvalidStatus)
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validPriority = FeaturePriority.Low;
        var validProjectId = Guid.NewGuid();
        var feature =  new Feature(
            validId,
            validName,
            validDescription,
            validPriority,
            initialStatus,
            validProjectId);
        
        //Act
        var act = () => feature.ChangeStatus(newInvalidStatus);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Status*");
        feature.Status.Should().Be(initialStatus);
    }
    
    [Fact]
    public void ChangeDescription_WithValidDescription_UpdatesDescription()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validStatus = FeatureStatus.ToDo;
        var validPriority = FeaturePriority.Low;
        var validProjectId = Guid.NewGuid();
        var feature =  new Feature(
            validId,
            validName,
            validDescription,
            validPriority,
            validStatus,
            validProjectId);
        var newDescription = "NewDescription";
        
        //Act
        var act = () => feature.ChangeDescription(newDescription);
        
        //Assert
        act.Should().NotThrow();
        feature.Description.Should().Be(newDescription);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ChangeDescription_WithInvalidDescription_ThrowsException(string? newDescription)
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validStatus = FeatureStatus.ToDo;
        var validPriority = FeaturePriority.Low;
        var validProjectId = Guid.NewGuid();
        var feature =  new Feature(
            validId,
            validName,
            validDescription,
            validPriority,
            validStatus,
            validProjectId);
        
        //Act
        var act = () => feature.ChangeDescription(newDescription!);
        
        //Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Description*");
        feature.Description.Should().Be(validDescription);
    }
}