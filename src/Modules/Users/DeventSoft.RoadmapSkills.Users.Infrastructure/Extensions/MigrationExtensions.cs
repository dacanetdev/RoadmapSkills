using DeventSoft.RoadmapSkills.Users.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeventSoft.RoadmapSkills.Users.Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static async Task MigrateUsersDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        
        if (dbContext.Database.IsSqlite())
        {
            await dbContext.Database.MigrateAsync();
        }
    }
} 