using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Domain.Entities;

namespace ClientPortal.Application.Interfaces;

public interface IProjectReadRepository
{
    public Task<Project?> GetProjectById(Guid id);
    Task<Project?> GetProjectByIdForUpdate(Guid id);
    Task<List<ProjectDto>> GetProjectsWithCounts();
    Task<ProjectDto?> GetProjectWithCountsById(Guid id);
}
