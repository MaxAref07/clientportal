using ClientPortal.Application.Interfaces;
using ClientPortal.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ClientPortal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IProjectRepository, ProjectRepository>();

        return services;
    }
}