using DeventSoft.RoadmapSkills.Users.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeventSoft.RoadmapSkills.Bootstrap;

public static class DependencyInjection
{
    public static IServiceCollection AddRoadmapSkills(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddUsersInfrastructure(configuration);
        return services;
    }
} 