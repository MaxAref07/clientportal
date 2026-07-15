using ClientPortal.Application.Interfaces;
using ClientPortal.Domain.Entities;
using ClientPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClientPortal.Infrastructure.Repositories;

public class ProjectRepository(AppDbContext context) : IProjectRepository, IProjectReadRepository
{
    public Task<Project> Add(Project project)
    {
        context.Projects.Add(project);
        return Task.FromResult(project);
    }

    public async Task<Project?> GetProjectById(Guid id)
    {
        return await context.Projects.SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Project>> GetProjects()
    {
        return await context.Projects.ToListAsync();
    }

    public async Task<Project> Delete(Guid projectId)
    {
        var project = await context.Projects.SingleOrDefaultAsync(p => p.Id == projectId);
        context.Projects.Remove(project!);
        return project!;
    }
}