using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Features.Commands.CreateFeature;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.UnitTests.TestHelpers;
using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
using FluentAssertions;
using NSubstitute;

namespace ClientPortal.Application.UnitTests.Features.Commands;

public class CreateFeatureCommandHandlerTests
{
    private readonly IFeatureRepository _featureRepository = Substitute.For<IFeatureRepository>();
    private readonly IFeatureReadRepository _featureReadRepository = Substitute.For<IFeatureReadRepository>();
    private readonly IProjectReadRepository _projectReadRepository = Substitute.For<IProjectReadRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CreateFeatureCommandHandler _handler;
    
    public CreateFeatureCommandHandlerTests()
    {
        _handler = new CreateFeatureCommandHandler(_featureRepository, _featureReadRepository, _projectReadRepository, _unitOfWork);
    }
    
    [Fact]
    public async Task CreateFeatureCommand_WithValidData_ShouldCreateFeature()
    {
        //Arrange
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validPriority = FeaturePriority.Low;
        var validProjectId = Guid.NewGuid();
        var command = new CreateFeatureCommand
        {
            Name = validName,
            Description = validDescription,
            Priority = validPriority,
            ProjectId = validProjectId
        };
        _projectReadRepository
            .GetProjectById(validProjectId)
            .Returns(TestData.Project(id: validProjectId));
        _featureReadRepository
            .CountByProjectId(validProjectId)
            .Returns(1);
        _featureRepository
            .Add(Arg.Any<Feature>())
            .Returns<Feature>(callInfo => callInfo.Arg<Feature>()!);
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        //Assert
        result.Name.Should().Be(validName);
        result.Description.Should().Be(validDescription);
        result.Priority.Should().Be(validPriority);
        result.Status.Should().Be(FeatureStatus.ToDo);
        result.ProjectId.Should().Be(validProjectId);
        await _projectReadRepository.Received(1).GetProjectById(validProjectId);
        await _featureReadRepository.Received(1).CountByProjectId(validProjectId);
        await _featureRepository.Received(1).Add(Arg.Any<Feature>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateFeatureCommand_WithNonExistentProject_ShouldThrowException()
    {
        //Arrange
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validPriority = FeaturePriority.Low;
        var validProjectId = Guid.NewGuid();
        var command = new CreateFeatureCommand{Name = validName,
            Description = validDescription,
            Priority = validPriority,
            ProjectId = validProjectId};
        _projectReadRepository
            .GetProjectById(validProjectId)
            .Returns((Project?)null);
        
        //Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        
        //Assert
        await act.Should().ThrowAsync<ProjectNotFoundException>()
            .WithMessage("*Project*");
        await _projectReadRepository.Received(1).GetProjectById(validProjectId);
        await _featureReadRepository.DidNotReceive().CountByProjectId(validProjectId);
        await _featureRepository.DidNotReceive().Add(Arg.Any<Feature>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}