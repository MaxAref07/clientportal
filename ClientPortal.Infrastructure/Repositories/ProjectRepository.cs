using ClientPortal.Application.Interfaces;
using ClientPortal.Domain.Entities;

namespace ClientPortal.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository, IReadDbContext
{
    private readonly List<Project> projects = new List<Project>();
    public Task<Project> Add(Project project)
    {
        projects.Add(project);
        return Task.FromResult(project);
    }

    public Task<Project?> GetProjectById(Guid id)
    {
        var project = projects.SingleOrDefault(p => p.Id == id);

        return Task.FromResult(project);
    }
}