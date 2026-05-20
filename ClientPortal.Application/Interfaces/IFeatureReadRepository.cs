using ClientPortal.Domain.Entities;

namespace ClientPortal.Application.Interfaces;

public interface IFeatureReadRepository
{
    public Task<Feature?> Get(Guid id);
    public Task<List<Feature>> GetFeatures();
    public Task<List<Feature>> GetFeaturesByProjectId(Guid id);
}