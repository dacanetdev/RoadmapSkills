using DeventSoft.RoadmapSkills.Users.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DeventSoft.RoadmapSkills.Users.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        return services;
    }
} 