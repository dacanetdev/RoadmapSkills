using DeventSoft.RoadmapSkills.Users.Api.Endpoints.Users;
using DeventSoft.RoadmapSkills.Users.Domain.Entities;
using DeventSoft.RoadmapSkills.Users.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeventSoft.RoadmapSkills.Users.Tests.Unit.Endpoints.Users;

public class GetUserEndpointTests
{
    private readonly IUserRepository _userRepository;
    private readonly GetUserEndpoint _endpoint;
    private readonly string _username = "johndoe";
    private readonly string _email = "john@example.com";

    public GetUserEndpointTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _endpoint = new GetUserEndpoint(_userRepository);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        _userRepository.GetByIdAsync(user.Id).Returns(user);

        // Act
        var result = await _endpoint.ExecuteAsync(user.Id, default);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(user.Id);
        result.Username.Should().Be(_username);
        result.Email.Should().Be(_email);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userRepository.GetByIdAsync(id).Returns((User?)null);

        // Act
        var result = await _endpoint.ExecuteAsync(id, default);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenUserIsDeleted()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        user.MarkAsDeleted();
        _userRepository.GetByIdAsync(user.Id).Returns((User?)null);

        // Act
        var result = await _endpoint.ExecuteAsync(user.Id, default);

        // Assert
        result.Should().BeNull();
    }
} 