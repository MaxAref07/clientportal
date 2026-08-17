using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.Commands.RenameProject;
using ClientPortal.Application.UnitTests.TestHelpers;
using ClientPortal.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace ClientPortal.Application.UnitTests.Projects.Commands;

public class RenameProjectCommandHandlerTests
{
    private readonly IProjectReadRepository _projectReadRepository = Substitute.For<IProjectReadRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly RenameProjectCommandHandler _handler;

    public RenameProjectCommandHandlerTests()
    {
        _handler = new RenameProjectCommandHandler(_projectReadRepository, _unitOfWork);
    }

    [Fact]
    public async Task RenameProjectCommand_WithValidData_RenamesProject()
    {
        //Arrange
        var newName = "NewName";
        var traceProject = TestData.Project(name: "OldName");
        var command = new RenameProjectCommand
        {
            Id = traceProject.Id,
            NewName = newName
        };
        _projectReadRepository
            .GetProjectById(traceProject.Id)
            .Returns(traceProject);
        _projectReadRepository
            .GetProjectWithCountsById(traceProject.Id)
            .Returns(TestData.ProjectDto(id: traceProject.Id, name: newName));

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.Name.Should().Be(newName);
        traceProject.Name.Should().Be(newName);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        await _projectReadRepository.Received(1).GetProjectWithCountsById(traceProject.Id);
    }

    [Fact]
    public async Task RenameProjectCommand_WithNonExistentProject_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var command = new RenameProjectCommand
        {
            Id = validId,
            NewName = "NewName"
        };
        _projectReadRepository
            .GetProjectById(validId)
            .Returns((Project?)null);

        //Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        //Assert
        await act.Should().ThrowAsync<ProjectNotFoundException>()
            .WithMessage("*Project*");
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        await _projectReadRepository.DidNotReceive().GetProjectWithCountsById(Arg.Any<Guid>());
    }
}
