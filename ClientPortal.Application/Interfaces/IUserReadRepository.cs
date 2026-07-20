using ClientPortal.Domain.Entities;

namespace ClientPortal.Application.Interfaces;

public interface IUserReadRepository
{
    Task<User?> GetByEmail(string email);
}