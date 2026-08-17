using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Features.Commands.ChangeFeatureStatus;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.UnitTests.TestHelpers;
using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
using FluentAssertions;
using NSubstitute;

namespace ClientPortal.Application.UnitTests.Features.Commands;

public class ChangeFeatureStatusCommandHandlerTests
{
    private readonly IFeatureReadRepository _featureReadRepository = Substitute.For<IFeatureReadRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ChangeFeatureStatusCommandHandler _handler;

    public ChangeFeatureStatusCommandHandlerTests()
    {
        _handler = new ChangeFeatureStatusCommandHandler(_featureReadRepository, _unitOfWork);
    }

    [Fact]
    public async Task ChangeFeatureStatusCommand_WithValidTransition_UpdatesStatus()
    {
        //Arrange
        var feature = TestData.Feature(status: FeatureStatus.ToDo);
        var command = new ChangeFeatureStatusCommand
        {
            Id = feature.Id,
            NewStatus = FeatureStatus.InProgress
        };
        _featureReadRepository
            .GetFeatureById(feature.Id)
            .Returns(feature);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.Status.Should().Be(FeatureStatus.InProgress);
        feature.Status.Should().Be(FeatureStatus.InProgress);
        await _featureReadRepository.Received(1).GetFeatureById(feature.Id);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ChangeFeatureStatusCommand_WithInvalidTransition_ThrowsException()
    {
        //Arrange
        var feature = TestData.Feature(status: FeatureStatus.ToDo);
        var command = new ChangeFeatureStatusCommand
        {
            Id = feature.Id,
            NewStatus = FeatureStatus.Done
        };
        _featureReadRepository
            .GetFeatureById(feature.Id)
            .Returns(feature);

        //Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        //Assert
        await act.Should().ThrowAsync<InvalidFeatureStatusTransitionException>();
        feature.Status.Should().Be(FeatureStatus.ToDo);
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ChangeFeatureStatusCommand_WithNonExistentFeature_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var command = new ChangeFeatureStatusCommand
        {
            Id = validId,
            NewStatus = FeatureStatus.InProgress
        };
        _featureReadRepository
            .GetFeatureById(validId)
            .Returns((Feature?)null);

        //Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        //Assert
        await act.Should().ThrowAsync<FeatureNotFoundException>()
            .WithMessage("*Feature*");
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
