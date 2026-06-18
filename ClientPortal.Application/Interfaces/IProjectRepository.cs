using ClientPortal.Domain.Entities;

namespace ClientPortal.Application.Interfaces;

public interface IProjectRepository
{
    public Task<Project> Add(Project project);
    public Task<Project> Delete(Guid id);
}