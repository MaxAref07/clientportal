using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.Commands.CreateProject;
using ClientPortal.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace ClientPortal.Application.UnitTests.Projects.Commands;

public class CreateProjectCommandHandlerTests
{
    private readonly IProjectRepository _projectRepository = Substitute.For<IProjectRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CreateProjectCommandHandler _handler;

    public CreateProjectCommandHandlerTests()
    {
        _handler = new CreateProjectCommandHandler(_projectRepository, _unitOfWork);
    }

    [Fact]
    public async Task CreateProjectCommand_WithValidData_CreatesProject()
    {
        //Arrange
        var validName = "ValidName";
        var validDescription = "ValidDescription";
        var validScopeFeatures = 10;
        var command = new CreateProjectCommand
        {
            Name = validName,
            Description = validDescription,
            ScopeFeatures = validScopeFeatures
        };
        _projectRepository
            .Add(Arg.Any<Project>())
            .Returns<Project>(callInfo => callInfo.Arg<Project>()!);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.Name.Should().Be(validName);
        result.Description.Should().Be(validDescription);
        result.ScopeFeatures.Should().Be(validScopeFeatures);
        result.CurrentFeaturesCount.Should().Be(0);
        result.CompletedFeaturesCount.Should().Be(0);
        await _projectRepository.Received(1).Add(Arg.Any<Project>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
