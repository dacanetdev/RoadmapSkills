using DeventSoft.RoadmapSkills.Users.Application;
using DeventSoft.RoadmapSkills.Users.Infrastructure;
using DeventSoft.RoadmapSkills.Users.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeventSoft.RoadmapSkills.Composition;

public static class DependencyInjection
{
    public static IServiceCollection AddRoadmapSkills(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Users module
        services.AddUsersApplication()
               .AddUsersInfrastructure(configuration);

        return services;
    }

    public static async Task MigrateRoadmapSkillsDatabasesAsync(this IServiceProvider serviceProvider)
    {
        // Migrate Users module database
        await serviceProvider.MigrateUsersDatabaseAsync();
    }
} 