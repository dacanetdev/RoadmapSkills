using DeventSoft.RoadmapSkills.Users.Domain.Entities;
using DeventSoft.RoadmapSkills.Users.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace DeventSoft.RoadmapSkills.Users.Tests.Unit.Repositories;

public class UserRepositoryTests : IDisposable
{
    private readonly UsersDbContext _context;
    private readonly UserRepository _repository;
    private readonly string _username = "johndoe";
    private readonly string _email = "john@example.com";

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<UsersDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new UsersDbContext(options);
        _repository = new UserRepository(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(user.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Act
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenUserIsDeleted()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        user.MarkAsDeleted();
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(user.Id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers_WhenUsersExist()
    {
        // Arrange
        var users = new[]
        {
            new User(_username, _email, "John", "Doe", "hash"),
            new User("janedoe", "jane@example.com", "Jane", "Doe", "hash"),
            new User("bobsmith", "bob@example.com", "Bob", "Smith", "hash")
        };

        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(u => u.Username == _username);
        result.Should().Contain(u => u.Username == "janedoe");
        result.Should().Contain(u => u.Username == "bobsmith");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ShouldNotReturnDeletedUsers()
    {
        // Arrange
        var users = new[]
        {
            new User(_username, _email, "John", "Doe", "hash"),
            new User("janedoe", "jane@example.com", "Jane", "Doe", "hash"),
            new User("bobsmith", "bob@example.com", "Bob", "Smith", "hash")
        };

        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();

        users[1].MarkAsDeleted();
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(u => u.Username == _username);
        result.Should().Contain(u => u.Username == "bobsmith");
        result.Should().NotContain(u => u.Username == "janedoe");
    }

    [Fact]
    public async Task GetByUsernameAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByUsernameAsync(_username);

        // Assert
        result.Should().NotBeNull();
        result!.Username.Should().Be(_username);
    }

    [Fact]
    public async Task GetByUsernameAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Act
        var result = await _repository.GetByUsernameAsync(_username);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByUsernameAsync_ShouldReturnNull_WhenUserIsDeleted()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        user.MarkAsDeleted();
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByUsernameAsync(_username);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByEmailAsync(_email);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be(_email);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Act
        var result = await _repository.GetByEmailAsync(_email);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenUserIsDeleted()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        user.MarkAsDeleted();
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByEmailAsync(_email);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ExistsByUsernameAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsByUsernameAsync(_username);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByUsernameAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        // Act
        var result = await _repository.ExistsByUsernameAsync(_username);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ExistsByUsernameAsync_ShouldReturnFalse_WhenUserIsDeleted()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        user.MarkAsDeleted();
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsByUsernameAsync(_username);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ExistsByEmailAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsByEmailAsync(_email);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByEmailAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        // Act
        var result = await _repository.ExistsByEmailAsync(_email);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ExistsByEmailAsync_ShouldReturnFalse_WhenUserIsDeleted()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        user.MarkAsDeleted();
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsByEmailAsync(_email);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task AddAsync_ShouldAddUser_WhenUserIsValid()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");

        // Act
        await _repository.AddAsync(user);
        await _context.SaveChangesAsync();

        // Assert
        var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        savedUser.Should().NotBeNull();
        savedUser!.Username.Should().Be(_username);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser_WhenUserExists()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        user.UpdateProfile("Jonathan", "Smith");

        // Act
        await _repository.UpdateAsync(user);
        await _context.SaveChangesAsync();

        // Assert
        var updatedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        updatedUser.Should().NotBeNull();
        updatedUser!.FirstName.Should().Be("Jonathan");
        updatedUser.LastName.Should().Be("Smith");
    }

    [Fact]
    public async Task DeleteAsync_ShouldMarkUserAsDeleted_WhenUserExists()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(user.Id);
        await _context.SaveChangesAsync();

        // Assert
        var deletedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        deletedUser.Should().NotBeNull();
        deletedUser!.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotThrowException_WhenUserDoesNotExist()
    {
        // Act
        var act = async () => await _repository.DeleteAsync(Guid.NewGuid());

        // Assert
        await act.Should().NotThrowAsync();
    }
} 