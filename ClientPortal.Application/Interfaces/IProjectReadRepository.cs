using ClientPortal.Domain.Entities;

namespace ClientPortal.Application.Interfaces;

public interface IProjectReadRepository
{
    public Task<Project?> GetProjectById(Guid id);
    public Task<List<Project>> GetProjects();
}
