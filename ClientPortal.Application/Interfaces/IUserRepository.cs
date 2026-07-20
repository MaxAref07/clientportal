using ClientPortal.Domain.Entities;

namespace ClientPortal.Application.Interfaces;

public interface IUserRepository
{
    Task<User> Add(User user);
}