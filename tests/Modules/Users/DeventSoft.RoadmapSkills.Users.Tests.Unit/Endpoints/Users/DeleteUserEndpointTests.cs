using DeventSoft.RoadmapSkills.Users.Api.Endpoints.Users;
using DeventSoft.RoadmapSkills.Users.Domain.Entities;
using DeventSoft.RoadmapSkills.Users.Domain.Repositories;
using DeventSoft.RoadmapSkills.Shared.Infrastructure.Common;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeventSoft.RoadmapSkills.Users.Tests.Unit.Endpoints.Users;

public class DeleteUserEndpointTests
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteUserEndpoint _endpoint;
    private readonly string _username = "johndoe";
    private readonly string _email = "john@example.com";

    public DeleteUserEndpointTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _endpoint = new DeleteUserEndpoint(_userRepository, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        _userRepository.GetByIdAsync(user.Id).Returns(user);

        // Act
        await _endpoint.HandleAsync(user.Id, default);

        // Assert
        await _userRepository.Received(1).DeleteAsync(user.Id);
        await _unitOfWork.Received(1).SaveChangesAsync(default);
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
        await _userRepository.DidNotReceive().DeleteAsync(Arg.Any<Guid>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(default);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenUserIsAlreadyDeleted()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        user.MarkAsDeleted();
        _userRepository.GetByIdAsync(user.Id).Returns((User?)null);

        // Act
        var result = await _endpoint.ExecuteAsync(user.Id, default);

        // Assert
        result.Should().BeNull();
        await _userRepository.DidNotReceive().DeleteAsync(Arg.Any<Guid>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(default);
    }
} 