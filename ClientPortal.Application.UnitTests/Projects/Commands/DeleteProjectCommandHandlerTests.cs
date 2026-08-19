using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.Commands.DeleteProject;
using ClientPortal.Application.UnitTests.TestHelpers;
using ClientPortal.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace ClientPortal.Application.UnitTests.Projects.Commands;

public class DeleteProjectCommandHandlerTests
{
    private readonly IProjectReadRepository _projectReadRepository = Substitute.For<IProjectReadRepository>();
    private readonly IProjectRepository _projectRepository = Substitute.For<IProjectRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly DeleteProjectCommandHandler _handler;

    public DeleteProjectCommandHandlerTests()
    {
        _handler = new DeleteProjectCommandHandler(_projectReadRepository, _projectRepository, _unitOfWork);
    }

    [Fact]
    public async Task DeleteProjectCommand_WithExistingProject_DeletesProject()
    {
        //Arrange
        var project = TestData.Project();
        var command = new DeleteProjectCommand
        {
            Id = project.Id
        };
        _projectReadRepository
            .GetProjectById(project.Id)
            .Returns(project);

        //Act
        await _handler.Handle(command, CancellationToken.None);

        //Assert
        await _projectRepository.Received(1).Delete(project.Id);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteProjectCommand_WithNonExistentProject_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var command = new DeleteProjectCommand
        {
            Id = validId
        };
        _projectReadRepository
            .GetProjectById(validId)
            .Returns((Project?)null);

        //Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        //Assert
        await act.Should().ThrowAsync<ProjectNotFoundException>()
            .WithMessage("*Project*");
        await _projectRepository.DidNotReceive().Delete(Arg.Any<Guid>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
