using ClientPortal.Application.Interfaces;
using ClientPortal.Domain.Entities;
using ClientPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClientPortal.Infrastructure.Repositories;

public class FeatureRepository(AppDbContext context) : IFeatureRepository, IFeatureReadRepository
{
    public Task<Feature> Add(Feature feature)
    {
        context.Features.Add(feature);
        return Task.FromResult(feature);
    }

    public async Task<Feature?> GetFeatureById(Guid id)
    {
        return await context.Features.SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Feature>> GetFeaturesByProjectId(Guid id)
    {
        return await context.Features.AsNoTracking().Where(f => f.ProjectId == id).ToListAsync();
    }

    public async Task<Feature> Delete(Guid featureId)
    {
        var feature = await context.Features.SingleOrDefaultAsync(p => p.Id == featureId);
        context.Features.Remove(feature!);
        return feature!;
    }

    public async Task<int> CountByProjectId(Guid projectId)
    {
        return await context.Features.CountAsync(p => p.ProjectId == projectId);
    }
}