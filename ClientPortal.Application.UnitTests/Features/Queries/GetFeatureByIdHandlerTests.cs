using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Features.Queries.GetFeatureById;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.UnitTests.TestHelpers;
using ClientPortal.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace ClientPortal.Application.UnitTests.Features.Queries;

public class GetFeatureByIdHandlerTests
{
    private readonly IFeatureReadRepository _featureReadRepository = Substitute.For<IFeatureReadRepository>();
    private readonly GetFeatureByIdHandler _handler;

    public GetFeatureByIdHandlerTests()
    {
        _handler = new GetFeatureByIdHandler(_featureReadRepository);
    }

    [Fact]
    public async Task GetFeatureByIdQuery_WithExistingFeature_ReturnsFeature()
    {
        //Arrange
        var feature = TestData.Feature();
        var query = new GetFeatureByIdQuery(feature.Id);
        _featureReadRepository
            .GetFeatureById(feature.Id)
            .Returns(feature);

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        result.Id.Should().Be(feature.Id);
        result.Name.Should().Be(feature.Name);
        result.Priority.Should().Be(feature.Priority);
        result.Status.Should().Be(feature.Status);
        result.Description.Should().Be(feature.Description);
        result.ProjectId.Should().Be(feature.ProjectId);
    }

    [Fact]
    public async Task GetFeatureByIdQuery_WithNonExistentFeature_ThrowsException()
    {
        //Arrange
        var validId = Guid.NewGuid();
        var query = new GetFeatureByIdQuery(validId);
        _featureReadRepository
            .GetFeatureById(validId)
            .Returns((Feature?)null);

        //Act
        var act = () => _handler.Handle(query, CancellationToken.None);

        //Assert
        await act.Should().ThrowAsync<FeatureNotFoundException>()
            .WithMessage("*Feature*");
    }
}
