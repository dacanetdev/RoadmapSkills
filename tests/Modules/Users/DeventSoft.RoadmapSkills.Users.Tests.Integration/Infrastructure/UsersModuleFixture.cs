using DeventSoft.RoadmapSkills.Users.Infrastructure;
using DeventSoft.RoadmapSkills.Users.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeventSoft.RoadmapSkills.Users.Tests.Integration.Infrastructure;

public class UsersModuleFixture : IDisposable
{
    private readonly SqliteConnection _connection;
    public IServiceProvider ServiceProvider { get; }

    public UsersModuleFixture()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"ConnectionStrings:UsersDb", _connection.ConnectionString}
            })
            .Build();

        services.AddUsersModule(configuration);
        ServiceProvider = services.BuildServiceProvider();

        // Create database
        using var scope = ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _connection.Dispose();
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
} 