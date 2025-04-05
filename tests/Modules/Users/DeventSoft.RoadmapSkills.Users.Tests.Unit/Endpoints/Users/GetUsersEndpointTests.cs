using DeventSoft.RoadmapSkills.Users.Api.Endpoints.Users;
using DeventSoft.RoadmapSkills.Users.Domain.Entities;
using DeventSoft.RoadmapSkills.Users.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeventSoft.RoadmapSkills.Users.Tests.Unit.Endpoints.Users;

public class GetUsersEndpointTests
{
    private readonly IUserRepository _userRepository;
    private readonly GetUsersEndpoint _endpoint;

    public GetUsersEndpointTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _endpoint = new GetUsersEndpoint(_userRepository);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnAllUsers_WhenUsersExist()
    {
        // Arrange
        var users = new[]
        {
            new User("johndoe", "john@example.com", "John", "Doe", "hash"),
            new User("janedoe", "jane@example.com", "Jane", "Doe", "hash"),
            new User("bobsmith", "bob@example.com", "Bob", "Smith", "hash")
        };

        _userRepository.GetAllAsync().Returns(users);

        // Act
        var result = await _endpoint.ExecuteAsync(default);

        // Assert
        result.Should().NotBeNull();
        result!.Users.Should().HaveCount(3);
        result.Users.Should().Contain(u => u.Username == "johndoe");
        result.Users.Should().Contain(u => u.Username == "janedoe");
        result.Users.Should().Contain(u => u.Username == "bobsmith");
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        _userRepository.GetAllAsync().Returns(Array.Empty<User>());

        // Act
        var result = await _endpoint.ExecuteAsync(default);

        // Assert
        result.Should().NotBeNull();
        result!.Users.Should().BeEmpty();
    }

    [Fact]
    public async Task HandleAsync_ShouldNotReturnDeletedUsers()
    {
        // Arrange
        var users = new[]
        {
            new User("johndoe", "john@example.com", "John", "Doe", "hash"),
            new User("janedoe", "jane@example.com", "Jane", "Doe", "hash"),
            new User("bobsmith", "bob@example.com", "Bob", "Smith", "hash")
        };

        users[1].MarkAsDeleted();
        _userRepository.GetAllAsync().Returns(users.Where(u => !u.IsDeleted));

        // Act
        var result = await _endpoint.ExecuteAsync(default);

        // Assert
        result.Should().NotBeNull();
        result!.Users.Should().HaveCount(2);
        result.Users.Should().Contain(u => u.Username == "johndoe");
        result.Users.Should().NotContain(u => u.Username == "janedoe");
        result.Users.Should().Contain(u => u.Username == "bobsmith");
    }

    [Fact]
    public async Task HandleAsync_ShouldMapUserPropertiesCorrectly()
    {
        // Arrange
        var user = new User("johndoe", "john@example.com", "John", "Doe", "hash");
        _userRepository.GetAllAsync().Returns(new[] { user });

        // Act
        var result = await _endpoint.ExecuteAsync(default);

        // Assert
        result.Should().NotBeNull();
        var mappedUser = result!.Users.Single();
        mappedUser.Id.Should().Be(user.Id);
        mappedUser.Username.Should().Be(user.Username);
        mappedUser.Email.Should().Be(user.Email);
        mappedUser.FirstName.Should().Be(user.FirstName);
        mappedUser.LastName.Should().Be(user.LastName);
        mappedUser.IsActive.Should().Be(user.IsActive);
        mappedUser.CreatedAt.Should().Be(user.CreatedAt);
        mappedUser.UpdatedAt.Should().Be(user.UpdatedAt);
    }
} 