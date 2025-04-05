using DeventSoft.RoadmapSkills.Users.Api.Endpoints.Users;
using DeventSoft.RoadmapSkills.Users.Domain.Entities;
using DeventSoft.RoadmapSkills.Users.Domain.Repositories;
using DeventSoft.RoadmapSkills.Shared.Infrastructure.Common;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeventSoft.RoadmapSkills.Users.Tests.Unit.Endpoints.Users;

public class UpdateUserEndpointTests
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateUserEndpoint _endpoint;
    private readonly string _username = "johndoe";
    private readonly string _email = "john@example.com";

    public UpdateUserEndpointTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _endpoint = new UpdateUserEndpoint(_userRepository, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_ShouldUpdateUser_WhenRequestIsValid()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        _userRepository.GetByIdAsync(user.Id).Returns(user);

        var request = new UpdateUserRequest
        {
            FirstName = "Jonathan",
            LastName = "Smith",
            Email = "jonathan@example.com"
        };

        // Act
        await _endpoint.HandleAsync(request, user.Id, default);

        // Assert
        await _userRepository.Received(1).UpdateAsync(Arg.Is<User>(u =>
            u.FirstName == "Jonathan" &&
            u.LastName == "Smith" &&
            u.Email == "jonathan@example.com"));
        await _unitOfWork.Received(1).SaveChangesAsync(default);
    }

    [Fact]
    public async Task HandleAsync_ShouldNotUpdateEmail_WhenEmailIsNotProvided()
    {
        // Arrange
        var user = new User(_username, _email, "John", "Doe", "hash");
        _userRepository.GetByIdAsync(user.Id).Returns(user);

        var request = new UpdateUserRequest
        {
            FirstName = "Jonathan",
            LastName = "Smith"
        };

        // Act
        await _endpoint.HandleAsync(request, user.Id, default);

        // Assert
        await _userRepository.Received(1).UpdateAsync(Arg.Is<User>(u =>
            u.FirstName == "Jonathan" &&
            u.LastName == "Smith" &&
            u.Email == _email));
        await _unitOfWork.Received(1).SaveChangesAsync(default);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userRepository.GetByIdAsync(id).Returns((User?)null);

        var request = new UpdateUserRequest
        {
            FirstName = "Jonathan",
            LastName = "Smith"
        };

        // Act
        var result = await _endpoint.ExecuteAsync(request, id, default);

        // Assert
        result.Should().BeNull();
        await _userRepository.DidNotReceive().UpdateAsync(Arg.Any<User>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(default);
    }

    [Theory]
    [InlineData("", "Smith", null, "First name is required")]
    [InlineData("Jonathan", "", null, "Last name is required")]
    [InlineData("Jonathan", "Smith", "invalid-email", "Invalid email format")]
    public async Task HandleAsync_ShouldValidateRequest_WithInvalidData(
        string firstName, string lastName, string? email, string expectedError)
    {
        // Arrange
        var request = new UpdateUserRequest
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email
        };

        var validator = new UpdateUserValidator(_userRepository);

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == expectedError);
    }

    [Fact]
    public async Task HandleAsync_ShouldValidateRequest_WhenEmailExists()
    {
        // Arrange
        var request = new UpdateUserRequest
        {
            FirstName = "Jonathan",
            LastName = "Smith",
            Email = "existing@example.com"
        };

        _userRepository.ExistsByEmailAsync("existing@example.com").Returns(true);

        var validator = new UpdateUserValidator(_userRepository);

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Email));
    }
} 