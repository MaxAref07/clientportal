using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.Commands.ChangeProjectScopeFeatures;
using ClientPortal.Application.UnitTests.TestHelpers;
using ClientPortal.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace ClientPortal.Application.UnitTests.Projects.Commands;

public class ChangeProjectScopeFeaturesCommandHandlerTests
{
    private readonly IProjectReadRepository _projectReadRepository = Substitute.For<IProjectReadRepository>();
    private readonly IFeatureReadRepository _featureReadRepository = Substitute.For<IFeatureReadRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ChangeProjectScopeFeaturesCommandHandler _handler;

    public ChangeProjectScopeFeaturesCommandHandlerTests()
    {
        _handler = new ChangeProjectScopeFeaturesCommandHandler(_projectReadRepository, _featureReadRepository, _unitOfWork);
    }

    [Theory]
    [InlineData(15)]
    [InlineData(10)]
    public async Task ChangeProjectScopeFeaturesCommand_WithValidData_UpdatesScopeFeatures(int validNewScopeFeatures)
    {
        //Arrange
        var validId = Guid.NewGuid();
        var traceProject = TestData.Project(id: validId);
        var command = new ChangeProjectScopeFeaturesCommand
        {
            Id = validId,
            NewScopeFeatures = validNewScopeFeatures
        };
        _projectReadRepository
            .GetProjectById(validId)
            .Returns(traceProject);
        _featureReadRepository
            .CountByProjectId(validId)
            .Returns(10);
        _projectReadRepository
            .GetProjectWithCountsById(validId)
            .Returns(TestData.ProjectDto(
                id: validId,
                scopeFeatures: validNewScopeFeatures,
                currentFeaturesCount: 9,
                completedFeaturesCount: 1));
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        //Assert
        result.Name.Should().Be("ValidName");
        result.Description.Should().Be("ValidDescription");
        result.ScopeFeatures.Should().Be(validNewScopeFeatures);
        result.CurrentFeaturesCount.Should().Be(9);
        result.CompletedFeaturesCount.Should().Be(1);
        traceProject.ScopeFeatures.Should().Be(validNewScopeFeatures);
        await _projectReadRepository
            .Received(1)
            .GetProjectById(validId);
        await _featureReadRepository
            .Received(1)
            .CountByProjectId(validId);
        await _projectReadRepository
            .Received(1)
            .GetProjectWithCountsById(validId);
        await _unitOfWork
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ChangeProjectScopeFeaturesCommand_WithNonExistentProject_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validNewScopeFeatures = 15;
        var command = new ChangeProjectScopeFeaturesCommand
        {
            Id = validId,
            NewScopeFeatures = validNewScopeFeatures
        };
        _projectReadRepository
            .GetProjectById(validId)
            .Returns((Project?)null);
        
        //Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        
        //Assert
        await act.Should().ThrowAsync<ProjectNotFoundException>()
            .WithMessage("*Project*");
        await _projectReadRepository
            .Received(1)
            .GetProjectById(validId);
        await _featureReadRepository
            .DidNotReceive()
            .CountByProjectId(validId);
        await _projectReadRepository
            .DidNotReceive()
            .GetProjectWithCountsById(validId);
        await _unitOfWork
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ChangeProjectScopeFeaturesCommand_WithScopeFeaturesExceeded_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var validNewScopeFeatures = 15;
        var traceProject = TestData.Project(id: validId);
        var command = new ChangeProjectScopeFeaturesCommand
        {
            Id = validId,
            NewScopeFeatures = validNewScopeFeatures
        };
        _projectReadRepository
            .GetProjectById(validId)
            .Returns(traceProject);
        _featureReadRepository
            .CountByProjectId(validId)
            .Returns(16);
        
        //Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        
        //Assert
        traceProject.ScopeFeatures.Should().Be(10);
        await act.Should().ThrowAsync<MinimumFeatureScopeException>()
            .WithMessage("*Feature*");
        await _projectReadRepository
            .Received(1)
            .GetProjectById(validId);
        await _featureReadRepository
            .Received(1)
            .CountByProjectId(validId);
        await _projectReadRepository
            .DidNotReceive()
            .GetProjectWithCountsById(validId);
        await _unitOfWork
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}