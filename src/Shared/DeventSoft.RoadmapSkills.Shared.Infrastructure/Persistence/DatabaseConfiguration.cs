using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeventSoft.RoadmapSkills.Shared.Infrastructure.Persistence;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddSqliteDatabase<TContext>(
        this IServiceCollection services,
        string connectionString) where TContext : DbContext
    {
        services.AddDbContext<TContext>(options =>
        {
            options.UseSqlite(connectionString, sqliteOptions =>
            {
                sqliteOptions.MigrationsAssembly(typeof(TContext).Assembly.FullName);
            });
        });

        return services;
    }
} 