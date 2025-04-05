using DeventSoft.RoadmapSkills.Users.Domain.Repositories;
using DeventSoft.RoadmapSkills.Users.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeventSoft.RoadmapSkills.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Infrastructure dependencies
        services.AddDbContext<UsersDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection") ?? "Data Source=RoadmapSkills.db"));

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
} 