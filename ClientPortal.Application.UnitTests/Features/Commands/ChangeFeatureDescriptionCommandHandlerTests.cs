using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Features.Commands.ChangeFeatureDescription;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.UnitTests.TestHelpers;
using ClientPortal.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace ClientPortal.Application.UnitTests.Features.Commands;

public class ChangeFeatureDescriptionCommandHandlerTests
{
    private readonly IFeatureReadRepository _featureReadRepository = Substitute.For<IFeatureReadRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ChangeFeatureDescriptionCommandHandler _handler;

    public ChangeFeatureDescriptionCommandHandlerTests()
    {
        _handler = new ChangeFeatureDescriptionCommandHandler(_featureReadRepository, _unitOfWork);
    }

    [Fact]
    public async Task ChangeFeatureDescriptionCommand_WithValidData_UpdatesDescription()
    {
        //Arrange
        var newDescription = "NewDescription";
        var feature = TestData.Feature(description: "OldDescription");
        var command = new ChangeFeatureDescriptionCommand
        {
            Id = feature.Id,
            NewDescription = newDescription
        };
        _featureReadRepository
            .GetFeatureById(feature.Id)
            .Returns(feature);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.Description.Should().Be(newDescription);
        feature.Description.Should().Be(newDescription);
        await _featureReadRepository.Received(1).GetFeatureById(feature.Id);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ChangeFeatureDescriptionCommand_WithNonExistentFeature_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var command = new ChangeFeatureDescriptionCommand
        {
            Id = validId,
            NewDescription = "NewDescription"
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
