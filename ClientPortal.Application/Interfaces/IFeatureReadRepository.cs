using ClientPortal.Domain.Entities;

namespace ClientPortal.Application.Interfaces;

public interface IFeatureReadRepository
{
    public Task<Feature?> GetFeatureById(Guid id);
    public Task<List<Feature>> GetFeatures();
    public Task<List<Feature>> GetFeaturesByProjectId(Guid id);
}