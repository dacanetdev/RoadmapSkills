using DeventSoft.RoadmapSkills.Users.Application.Services;
using DeventSoft.RoadmapSkills.Users.Domain.Entities;
using DeventSoft.RoadmapSkills.Users.Domain.Repositories;
using NSubstitute;
using Xunit;

namespace DeventSoft.RoadmapSkills.Users.UnitTests.Services;

public class UserServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly UserService _sut;

    public UserServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _sut = new UserService(_userRepository);
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var expectedUsers = new List<User>
        {
            new("user1", "user1@test.com", "User", "One", "hash1"),
            new("user2", "user2@test.com", "User", "Two", "hash2")
        };
        _userRepository.GetAllAsync().Returns(expectedUsers);

        // Act
        var result = await _sut.GetAllUsersAsync();

        // Assert
        Assert.Equal(expectedUsers, result);
        await _userRepository.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task GetUserByIdAsync_WithExistingId_ShouldReturnUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedUser = new User("user1", "user1@test.com", "User", "One", "hash1");
        _userRepository.GetByIdAsync(userId).Returns(expectedUser);

        // Act
        var result = await _sut.GetUserByIdAsync(userId);

        // Assert
        Assert.Equal(expectedUser, result);
        await _userRepository.Received(1).GetByIdAsync(userId);
    }

    [Fact]
    public async Task GetUserByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepository.GetByIdAsync(userId).Returns((User?)null);

        // Act
        var result = await _sut.GetUserByIdAsync(userId);

        // Assert
        Assert.Null(result);
        await _userRepository.Received(1).GetByIdAsync(userId);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldAddUser()
    {
        // Arrange
        var user = new User("newuser", "newuser@test.com", "New", "User", "hash");

        // Act
        await _sut.CreateUserAsync(user);

        // Assert
        await _userRepository.Received(1).AddAsync(user);
    }

    [Fact]
    public async Task UpdateUserAsync_WithExistingUser_ShouldReturnTrue()
    {
        // Arrange
        var user = new User("user1", "user1@test.com", "User", "One", "hash1");
        _userRepository.GetByIdAsync(user.Id).Returns(user);

        // Act
        var result = await _sut.UpdateUserAsync(user);

        // Assert
        Assert.True(result);
        await _userRepository.Received(1).UpdateAsync(user);
    }

    [Fact]
    public async Task UpdateUserAsync_WithNonExistingUser_ShouldReturnFalse()
    {
        // Arrange
        var user = new User("user1", "user1@test.com", "User", "One", "hash1");
        _userRepository.GetByIdAsync(user.Id).Returns((User?)null);

        // Act
        var result = await _sut.UpdateUserAsync(user);

        // Assert
        Assert.False(result);
        await _userRepository.DidNotReceive().UpdateAsync(user);
    }

    [Fact]
    public async Task DeleteUserAsync_WithExistingUser_ShouldReturnTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User("user1", "user1@test.com", "User", "One", "hash1");
        _userRepository.GetByIdAsync(userId).Returns(user);

        // Act
        var result = await _sut.DeleteUserAsync(userId);

        // Assert
        Assert.True(result);
        await _userRepository.Received(1).DeleteAsync(user);
    }

    [Fact]
    public async Task DeleteUserAsync_WithNonExistingUser_ShouldReturnFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepository.GetByIdAsync(userId).Returns((User?)null);

        // Act
        var result = await _sut.DeleteUserAsync(userId);

        // Assert
        Assert.False(result);
        await _userRepository.DidNotReceive().DeleteAsync(Arg.Any<User>());
    }
} 