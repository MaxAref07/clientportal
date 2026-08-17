using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Features.Commands.DeleteFeature;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.UnitTests.TestHelpers;
using ClientPortal.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace ClientPortal.Application.UnitTests.Features.Commands;

public class DeleteFeatureCommandHandlerTests
{
    private readonly IFeatureReadRepository _featureReadRepository = Substitute.For<IFeatureReadRepository>();
    private readonly IFeatureRepository _featureRepository = Substitute.For<IFeatureRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly DeleteFeatureCommandHandler _handler;

    public DeleteFeatureCommandHandlerTests()
    {
        _handler = new DeleteFeatureCommandHandler(_featureReadRepository, _featureRepository, _unitOfWork);
    }

    [Fact]
    public async Task DeleteFeatureCommand_WithExistingFeature_DeletesFeature()
    {
        //Arrange
        var feature = TestData.Feature();
        var command = new DeleteFeatureCommand
        {
            Id = feature.Id
        };
        _featureReadRepository
            .GetFeatureById(feature.Id)
            .Returns(feature);

        //Act
        await _handler.Handle(command, CancellationToken.None);

        //Assert
        await _featureRepository.Received(1).Delete(feature.Id);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteFeatureCommand_WithNonExistentFeature_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var command = new DeleteFeatureCommand
        {
            Id = validId
        };
        _featureReadRepository
            .GetFeatureById(validId)
            .Returns((Feature?)null);

        //Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        //Assert
        await act.Should().ThrowAsync<FeatureNotFoundException>()
            .WithMessage("*Feature*");
        await _featureRepository.DidNotReceive().Delete(Arg.Any<Guid>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
