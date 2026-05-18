using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Application.Projects.Queries.GetProjectById;
using ClientPortal.Application.Projects.Queries.GetProjects;
using ClientPortal.Domain.Entities;

namespace ClientPortal.Application.Interfaces;

public interface IReadDbContext
{
    public Task<Project?> GetProjectById(Guid id);
    public Task<List<Project>> GetProjects();
}