using DeventSoft.RoadmapSkills.Users.Domain.Entities;
using DeventSoft.RoadmapSkills.Users.Domain.Repositories;
using DeventSoft.RoadmapSkills.Users.Tests.Integration.Infrastructure;
using DeventSoft.RoadmapSkills.Shared.Infrastructure.Common;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace DeventSoft.RoadmapSkills.Users.Tests.Integration.Repositories;

public class UserRepositoryTests : IClassFixture<UsersModuleFixture>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string _username = "johndoe";
    private readonly string _email = "john@example.com";

    public UserRepositoryTests(UsersModuleFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }

    [Fact]
    public async Task AddAsync_ShouldPersistUser_WhenUserIsValid()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var user = new User(_username, _email, "John", "Doe", "hash");

        // Act
        await repository.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        // Assert
        var savedUser = await repository.GetByIdAsync(user.Id);
        savedUser.Should().NotBeNull();
        savedUser!.Username.Should().Be(_username);
        savedUser.Email.Should().Be(_email);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistMultipleUsers_WhenUsersAreValid()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var users = new[]
        {
            new User(_username, _email, "John", "Doe", "hash"),
            new User("janedoe", "jane@example.com", "Jane", "Doe", "hash"),
            new User("bobsmith", "bob@example.com", "Bob", "Smith", "hash")
        };

        // Act
        foreach (var user in users)
        {
            await repository.AddAsync(user);
        }
        await unitOfWork.SaveChangesAsync();

        // Assert
        var allUsers = await repository.GetAllAsync();
        allUsers.Should().HaveCount(3);
        allUsers.Should().Contain(u => u.Username == _username);
        allUsers.Should().Contain(u => u.Username == "janedoe");
        allUsers.Should().Contain(u => u.Username == "bobsmith");
    }

    [Fact]
    public async Task GetByUsernameAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var user = new User(_username, _email, "John", "Doe", "hash");
        await repository.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        // Act
        var result = await repository.GetByUsernameAsync(_username);

        // Assert
        result.Should().NotBeNull();
        result!.Username.Should().Be(_username);
    }

    [Fact]
    public async Task GetByUsernameAsync_ShouldReturnNull_WhenUserIsDeleted()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var user = new User(_username, _email, "John", "Doe", "hash");
        await repository.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        await repository.DeleteAsync(user.Id);
        await unitOfWork.SaveChangesAsync();

        // Act
        var result = await repository.GetByUsernameAsync(_username);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldMarkUserAsDeleted()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var user = new User(_username, _email, "John", "Doe", "hash");
        await repository.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(user.Id);
        await unitOfWork.SaveChangesAsync();

        // Assert
        var deletedUser = await repository.GetByIdAsync(user.Id);
        deletedUser.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser_WhenUserExists()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var user = new User(_username, _email, "John", "Doe", "hash");
        await repository.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        // Act
        user.UpdateProfile("Jonathan", "Smith");
        await repository.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync();

        // Assert
        var updatedUser = await repository.GetByIdAsync(user.Id);
        updatedUser.Should().NotBeNull();
        updatedUser!.FirstName.Should().Be("Jonathan");
        updatedUser.LastName.Should().Be("Smith");
    }

    [Fact]
    public async Task TransactionRollback_ShouldNotPersistChanges()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var user = new User(_username, _email, "John", "Doe", "hash");
        await repository.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        // Act
        await unitOfWork.BeginTransactionAsync();
        user.UpdateProfile("Jonathan", "Smith");
        await repository.UpdateAsync(user);
        await unitOfWork.RollbackTransactionAsync();

        // Assert
        var unchangedUser = await repository.GetByIdAsync(user.Id);
        unchangedUser.Should().NotBeNull();
        unchangedUser!.FirstName.Should().Be("John");
        unchangedUser.LastName.Should().Be("Doe");
    }

    [Fact]
    public async Task TransactionCommit_ShouldPersistChanges()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var user = new User(_username, _email, "John", "Doe", "hash");
        await repository.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        // Act
        await unitOfWork.BeginTransactionAsync();
        user.UpdateProfile("Jonathan", "Smith");
        await repository.UpdateAsync(user);
        await unitOfWork.CommitTransactionAsync();

        // Assert
        var changedUser = await repository.GetByIdAsync(user.Id);
        changedUser.Should().NotBeNull();
        changedUser!.FirstName.Should().Be("Jonathan");
        changedUser.LastName.Should().Be("Smith");
    }

    [Fact]
    public async Task ConcurrentUpdates_ShouldHandleOptimisticConcurrency()
    {
        // Arrange
        using var scope1 = _serviceProvider.CreateScope();
        using var scope2 = _serviceProvider.CreateScope();
        var repository1 = scope1.ServiceProvider.GetRequiredService<IUserRepository>();
        var repository2 = scope2.ServiceProvider.GetRequiredService<IUserRepository>();
        var unitOfWork1 = scope1.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var unitOfWork2 = scope2.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var user = new User(_username, _email, "John", "Doe", "hash");
        await repository1.AddAsync(user);
        await unitOfWork1.SaveChangesAsync();

        var user1 = await repository1.GetByIdAsync(user.Id);
        var user2 = await repository2.GetByIdAsync(user.Id);

        // Act & Assert
        user1!.UpdateProfile("Jonathan", "Smith");
        await repository1.UpdateAsync(user1);
        await unitOfWork1.SaveChangesAsync();

        user2!.UpdateProfile("Johnny", "Johnson");
        await repository2.UpdateAsync(user2);
        
        var act = async () => await unitOfWork2.SaveChangesAsync();
        await act.Should().ThrowAsync<DbUpdateConcurrencyException>();
    }
} 