using ClientPortal.Application.Interfaces;
using ClientPortal.Infrastructure.Persistence;
using ClientPortal.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClientPortal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<ProjectRepository>();

        services.AddSingleton<IProjectRepository>(sp =>
            sp.GetRequiredService<ProjectRepository>());
        services.AddSingleton<IProjectReadRepository>(sp =>
            sp.GetRequiredService<ProjectRepository>());
        
        services.AddSingleton<FeatureRepository>();
        
        services.AddSingleton<IFeatureRepository>(sp =>
            sp.GetRequiredService<FeatureRepository>());
        services.AddSingleton<IFeatureReadRepository>(sp =>
            sp.GetRequiredService<FeatureRepository>());
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}