using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Features.Commands.ChangeFeaturePriority;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.UnitTests.TestHelpers;
using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
using FluentAssertions;
using NSubstitute;

namespace ClientPortal.Application.UnitTests.Features.Commands;

public class ChangeFeaturePriorityCommandHandlerTests
{
    private readonly IFeatureReadRepository _featureReadRepository = Substitute.For<IFeatureReadRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ChangeFeaturePriorityCommandHandler _handler;

    public ChangeFeaturePriorityCommandHandlerTests()
    {
        _handler = new ChangeFeaturePriorityCommandHandler(_featureReadRepository, _unitOfWork);
    }

    [Fact]
    public async Task ChangeFeaturePriorityCommand_WithValidData_UpdatesPriority()
    {
        //Arrange
        var newPriority = FeaturePriority.High;
        var feature = TestData.Feature(priority: FeaturePriority.Low);
        var command = new ChangeFeaturePriorityCommand
        {
            Id = feature.Id,
            NewPriority = newPriority
        };
        _featureReadRepository
            .GetFeatureById(feature.Id)
            .Returns(feature);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.Priority.Should().Be(newPriority);
        feature.Priority.Should().Be(newPriority);
        await _featureReadRepository.Received(1).GetFeatureById(feature.Id);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ChangeFeaturePriorityCommand_WithNonExistentFeature_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var command = new ChangeFeaturePriorityCommand
        {
            Id = validId,
            NewPriority = FeaturePriority.High
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
