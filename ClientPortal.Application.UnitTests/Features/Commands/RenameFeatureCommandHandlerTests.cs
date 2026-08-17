using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Features.Commands.RenameFeature;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.UnitTests.TestHelpers;
using ClientPortal.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace ClientPortal.Application.UnitTests.Features.Commands;

public class RenameFeatureCommandHandlerTests
{
    private readonly IFeatureReadRepository _featureReadRepository = Substitute.For<IFeatureReadRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly RenameFeatureCommandHandler _handler;

    public RenameFeatureCommandHandlerTests()
    {
        _handler = new RenameFeatureCommandHandler(_featureReadRepository, _unitOfWork);
    }

    [Fact]
    public async Task RenameFeatureCommand_WithValidData_RenamesFeature()
    {
        //Arrange
        var newName = "NewName";
        var feature = TestData.Feature(name: "OldName");
        var command = new RenameFeatureCommand
        {
            Id = feature.Id,
            NewName = newName
        };
        _featureReadRepository
            .GetFeatureById(feature.Id)
            .Returns(feature);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.Name.Should().Be(newName);
        feature.Name.Should().Be(newName);
        await _featureReadRepository.Received(1).GetFeatureById(feature.Id);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RenameFeatureCommand_WithNonExistentFeature_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var command = new RenameFeatureCommand
        {
            Id = validId,
            NewName = "NewName"
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
