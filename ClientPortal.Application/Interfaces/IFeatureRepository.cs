using ClientPortal.Domain.Entities;

namespace ClientPortal.Application.Interfaces;

public interface IFeatureRepository
{
    public Task<Feature> Add(Feature feature);
}