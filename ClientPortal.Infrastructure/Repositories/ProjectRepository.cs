using ClientPortal.Application.Interfaces;
using ClientPortal.Domain.Entities;

namespace ClientPortal.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly List<Project> projects = new List<Project>();
    public Task<Project> Add(Project project)
    {
        projects.Add(project);
        return Task.FromResult(project);
    }
}