using Therasim.Application.Common.Interfaces;
using Therasim.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Therasim.Infrastructure.Data.Interceptors;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultSqlConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString);
            
        }, ServiceLifetime.Transient);

        // services.AddDbContextFactory<ApplicationDbContext>((sp, options) =>
        // {
        //     options.UseSqlServer(connectionString);
        //
        // }, ServiceLifetime.Transient);

        services.AddTransient<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddSingleton(TimeProvider.System);
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        return services;
    }
}
