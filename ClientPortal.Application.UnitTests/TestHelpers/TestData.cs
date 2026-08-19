using ClientPortal.Application.DTOs;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;

namespace ClientPortal.Application.UnitTests.TestHelpers;

public static class TestData
{
    public static Project Project(
        Guid? id = null,
        string name = "ValidName",
        string description = "ValidDescription",
        int scopeFeatures = 10)
        => new(id ?? Guid.NewGuid(), name, description, scopeFeatures);

    public static Feature Feature(
        Guid? id = null,
        string name = "ValidName",
        string description = "ValidDescription",
        FeaturePriority priority = FeaturePriority.Low,
        FeatureStatus status = FeatureStatus.ToDo,
        Guid? projectId = null)
        => new(id ?? Guid.NewGuid(), name, description, priority, status, projectId ?? Guid.NewGuid());

    public static User User(
        Guid? id = null,
        string email = "validemail@gmail.com",
        UserRole role = UserRole.Member)
        => new(id ?? Guid.NewGuid(), email, role);

    public static MagicLink MagicLink(
        Guid? id = null,
        string email = "validemail@gmail.com",
        string tokenHash = "validtokenhash",
        DateTime? expiresAt = null)
        => new(id ?? Guid.NewGuid(), email, tokenHash, expiresAt ?? DateTime.UtcNow.AddMinutes(30));

    public static ProjectDto ProjectDto(
        Guid? id = null,
        string name = "ValidName",
        string description = "ValidDescription",
        int scopeFeatures = 10,
        int currentFeaturesCount = 0,
        int completedFeaturesCount = 0)
        => new(id ?? Guid.NewGuid(), name, description, scopeFeatures, currentFeaturesCount, completedFeaturesCount);
}
