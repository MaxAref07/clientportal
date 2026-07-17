using ClientPortal.Application.Interfaces;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
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

    public async Task<Project> Delete(Guid projectId)
    {
        var project = await context.Projects.SingleOrDefaultAsync(p => p.Id == projectId);
        context.Projects.Remove(project!);
        return project!;
    }

    public async Task<List<ProjectDto>> GetProjectsWithCounts()
    {
        return await context.Projects.Select(p => new ProjectDto(
            p.Id,
            p.Name,
            p.Description,
            p.ScopeFeatures,
            context.Features.Count(f => f.ProjectId == p.Id),
            context.Features.Count(f => f.ProjectId == p.Id && f.Status == FeatureStatus.Done)
        )).ToListAsync();
    }

    public async Task<ProjectDto?> GetProjectWithCountsById(Guid id)
    {
        return await context.Projects.Where(p => p.Id == id).Select(p => new ProjectDto(
            p.Id,
            p.Name,
            p.Description,
            p.ScopeFeatures,
            context.Features.Count(f => f.ProjectId == p.Id),
            context.Features.Count(f => f.ProjectId == p.Id && f.Status == FeatureStatus.Done)
        )).FirstOrDefaultAsync();
    }
}