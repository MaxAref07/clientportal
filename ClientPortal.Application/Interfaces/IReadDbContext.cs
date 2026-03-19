using ClientPortal.Application.Projects.Queries.GetProjectById;
using ClientPortal.Domain.Entities;

namespace ClientPortal.Application.Interfaces;

public interface IReadDbContext
{
    public Task<Project?> GetProjectById(Guid id);
}