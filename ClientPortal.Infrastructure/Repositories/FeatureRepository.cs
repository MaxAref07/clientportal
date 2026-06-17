using ClientPortal.Application.Exceptions;
using ClientPortal.Application.Interfaces;
using ClientPortal.Domain.Entities;

namespace ClientPortal.Infrastructure.Repositories;

public class FeatureRepository : IFeatureRepository, IFeatureReadRepository
{
    private readonly List<Feature> _features = new List<Feature>();
    public Task<Feature> Add(Feature feature)
    {
        _features.Add(feature);
        return Task.FromResult(feature);
    }

    public Task<Feature?> GetFeatureById(Guid id)
    {
        var feature = _features.SingleOrDefault(p => p.Id == id);

        return Task.FromResult(feature);
    }

    public Task<List<Feature>> GetFeatures()
    {
        var features = this._features;
        
        return Task.FromResult(features);
    }

    public Task<List<Feature>> GetFeaturesByProjectId(Guid id)
    {
        var features = this._features.Where(f => f.ProjectId == id).ToList();
        return Task.FromResult(features);
    }

    public Task<Feature> Delete(Guid featureId)
    {
        var feature = _features.SingleOrDefault(p => p.Id == featureId)!;
        _features.Remove(feature);
        return Task.FromResult(feature);
    }
}