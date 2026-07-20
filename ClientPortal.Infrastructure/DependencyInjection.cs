using ClientPortal.Application.Interfaces;
using ClientPortal.Infrastructure.Auth;
using ClientPortal.Infrastructure.Persistence;
using ClientPortal.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClientPortal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<ProjectRepository>();

        services.AddScoped<IProjectRepository>(sp =>
            sp.GetRequiredService<ProjectRepository>());
        services.AddScoped<IProjectReadRepository>(sp =>
            sp.GetRequiredService<ProjectRepository>());
        
        services.AddScoped<FeatureRepository>();
        
        services.AddScoped<IFeatureRepository>(sp =>
            sp.GetRequiredService<FeatureRepository>());
        services.AddScoped<IFeatureReadRepository>(sp =>
            sp.GetRequiredService<FeatureRepository>());
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<UserRepository>();
        
        services.AddScoped<IUserRepository>(sp =>
            sp.GetRequiredService<UserRepository>());
        services.AddScoped<IUserReadRepository>(sp =>
            sp.GetRequiredService<UserRepository>());

        services.AddScoped<MagicLinkRepository>();
        
        services.AddScoped<IMagicLinkRepository>(sp =>
            sp.GetRequiredService<MagicLinkRepository>());
        services.AddScoped<IMagicLinkReadRepository>(sp => 
            sp.GetRequiredService<MagicLinkRepository>());
        
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        
        return services;
    }
}