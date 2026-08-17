using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.Commands.ChangeProjectDescription;
using ClientPortal.Application.UnitTests.TestHelpers;
using ClientPortal.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace ClientPortal.Application.UnitTests.Projects.Commands;

public class ChangeProjectDescriptionCommandHandlerTests
{
    private readonly IProjectReadRepository _projectReadRepository = Substitute.For<IProjectReadRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ChangeProjectDescriptionCommandHandler _handler;

    public ChangeProjectDescriptionCommandHandlerTests()
    {
        _handler = new ChangeProjectDescriptionCommandHandler(_projectReadRepository, _unitOfWork);
    }

    [Fact]
    public async Task ChangeProjectDescriptionCommand_WithValidData_UpdatesDescription()
    {
        //Arrange
        var newDescription = "NewDescription";
        var traceProject = TestData.Project(description: "OldDescription");
        var command = new ChangeProjectDescriptionCommand
        {
            Id = traceProject.Id,
            NewDescription = newDescription
        };
        _projectReadRepository
            .GetProjectById(traceProject.Id)
            .Returns(traceProject);
        _projectReadRepository
            .GetProjectWithCountsById(traceProject.Id)
            .Returns(TestData.ProjectDto(id: traceProject.Id, description: newDescription));

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.Description.Should().Be(newDescription);
        traceProject.Description.Should().Be(newDescription);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        await _projectReadRepository.Received(1).GetProjectWithCountsById(traceProject.Id);
    }

    [Fact]
    public async Task ChangeProjectDescriptionCommand_WithNonExistentProject_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var command = new ChangeProjectDescriptionCommand
        {
            Id = validId,
            NewDescription = "NewDescription"
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
