using ClientPortal.Domain.Entities;

namespace ClientPortal.Application.Interfaces;

public interface IFeatureRepository
{
    public Task<Feature> Add(Feature feature);
    public Task<Feature> Delete(Guid id);
}