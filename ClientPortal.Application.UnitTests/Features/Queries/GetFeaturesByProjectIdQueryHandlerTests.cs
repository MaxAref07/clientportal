using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Features.Queries.GetFeaturesByProjectIdQuery;
using ClientPortal.Application.Interfaces;
using ClientPortal.Application.UnitTests.TestHelpers;
using ClientPortal.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace ClientPortal.Application.UnitTests.Features.Queries;

public class GetFeaturesByProjectIdQueryHandlerTests
{
    private readonly IFeatureReadRepository _featureReadRepository = Substitute.For<IFeatureReadRepository>();
    private readonly IProjectReadRepository _projectReadRepository = Substitute.For<IProjectReadRepository>();
    private readonly GetFeaturesByProjectIdQueryHandler _handler;

    public GetFeaturesByProjectIdQueryHandlerTests()
    {
        _handler = new GetFeaturesByProjectIdQueryHandler(_featureReadRepository, _projectReadRepository);
    }

    [Fact]
    public async Task GetFeaturesByProjectIdQuery_WithExistingProject_ReturnsMappedFeatures()
    {
        //Arrange
        var projectId = Guid.NewGuid();
        var project = TestData.Project(id: projectId);
        var firstFeature = TestData.Feature(projectId: projectId);
        var secondFeature = TestData.Feature(projectId: projectId);
        var query = new GetFeaturesByProjectIdQuery(projectId);
        _projectReadRepository
            .GetProjectById(projectId)
            .Returns(project);
        _featureReadRepository
            .GetFeaturesByProjectId(projectId)
            .Returns(new List<Feature> { firstFeature, secondFeature });

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        result.Should().HaveCount(2);
        result[0].Id.Should().Be(firstFeature.Id);
        result[1].Id.Should().Be(secondFeature.Id);
        await _featureReadRepository.Received(1).GetFeaturesByProjectId(projectId);
    }

    [Fact]
    public async Task GetFeaturesByProjectIdQuery_WithNonExistentProject_ThrowsException()
    {
        //Arrange
        var projectId = Guid.NewGuid();
        var query = new GetFeaturesByProjectIdQuery(projectId);
        _projectReadRepository
            .GetProjectById(projectId)
            .Returns((Project?)null);

        //Act
        var act = () => _handler.Handle(query, CancellationToken.None);

        //Assert
        await act.Should().ThrowAsync<ProjectNotFoundException>()
            .WithMessage("*Project*");
        await _featureReadRepository.DidNotReceive().GetFeaturesByProjectId(Arg.Any<Guid>());
    }
}
