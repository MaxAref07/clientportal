using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Application.Projects.Queries.GetProjects;
using ClientPortal.Domain.Entities;

namespace ClientPortal.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository, IProjectReadRepository
{
    private readonly List<Project> _projects = new List<Project>();
    public Task<Project> Add(Project project)
    {
        _projects.Add(project);
        return Task.FromResult(project);
    }

    public Task<Project?> GetProjectById(Guid id)
    {
        var project = _projects.SingleOrDefault(p => p.Id == id);

        return Task.FromResult(project);
    }

    public Task<List<Project>> GetProjects()
    {
        var projects = this._projects;
        
        return Task.FromResult(projects);
    }
    
    public Task<Project> Delete(Guid projectId)
    {
        var project = _projects.SingleOrDefault(p => p.Id == projectId)!;
        _projects.Remove(project);
        return Task.FromResult(project);
    }
}