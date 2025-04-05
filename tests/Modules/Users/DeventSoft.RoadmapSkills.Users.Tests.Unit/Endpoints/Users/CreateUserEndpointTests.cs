using DeventSoft.RoadmapSkills.Users.Api.Endpoints.Users;
using DeventSoft.RoadmapSkills.Users.Domain.Entities;
using DeventSoft.RoadmapSkills.Users.Domain.Repositories;
using DeventSoft.RoadmapSkills.Shared.Infrastructure.Common;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeventSoft.RoadmapSkills.Users.Tests.Unit.Endpoints.Users;

public class CreateUserEndpointTests
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly CreateUserEndpoint _endpoint;
    private readonly string _username = "johndoe";
    private readonly string _email = "john@example.com";
    private readonly string _firstName = "John";
    private readonly string _lastName = "Doe";
    private readonly string _password = "Password123!";
    private readonly string _passwordHash = "hashedpassword";

    public CreateUserEndpointTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _endpoint = new CreateUserEndpoint(_userRepository, _unitOfWork, _passwordHasher);
    }

    [Fact]
    public async Task HandleAsync_ShouldCreateUser_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Username = _username,
            Email = _email,
            FirstName = _firstName,
            LastName = _lastName,
            Password = _password
        };

        _passwordHasher.HashPassword(_password).Returns(_passwordHash);
        User? capturedUser = null;
        await _userRepository.AddAsync(Arg.Do<User>(u => capturedUser = u));

        // Act
        await _endpoint.HandleAsync(request, default);

        // Assert
        await _userRepository.Received(1).AddAsync(Arg.Any<User>());
        await _unitOfWork.Received(1).SaveChangesAsync(default);

        capturedUser.Should().NotBeNull();
        capturedUser!.Username.Should().Be(_username);
        capturedUser.Email.Should().Be(_email);
        capturedUser.FirstName.Should().Be(_firstName);
        capturedUser.LastName.Should().Be(_lastName);
        capturedUser.PasswordHash.Should().Be(_passwordHash);
    }

    [Fact]
    public async Task HandleAsync_ShouldValidateRequest_WhenUsernameExists()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Username = _username,
            Email = _email,
            FirstName = _firstName,
            LastName = _lastName,
            Password = _password
        };

        _userRepository.ExistsByUsernameAsync(_username).Returns(true);

        var validator = new CreateUserValidator(_userRepository);

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Username));
    }

    [Fact]
    public async Task HandleAsync_ShouldValidateRequest_WhenEmailExists()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Username = _username,
            Email = _email,
            FirstName = _firstName,
            LastName = _lastName,
            Password = _password
        };

        _userRepository.ExistsByEmailAsync(_email).Returns(true);

        var validator = new CreateUserValidator(_userRepository);

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Email));
    }

    [Theory]
    [InlineData("", "john@example.com", "John", "Doe", "Password123!", "Username is required")]
    [InlineData("jo", "john@example.com", "John", "Doe", "Password123!", "Username must be at least 3 characters")]
    [InlineData("johndoe", "", "John", "Doe", "Password123!", "Email is required")]
    [InlineData("johndoe", "invalid-email", "John", "Doe", "Password123!", "Invalid email format")]
    [InlineData("johndoe", "john@example.com", "", "Doe", "Password123!", "First name is required")]
    [InlineData("johndoe", "john@example.com", "John", "", "Doe", "Last name is required")]
    [InlineData("johndoe", "john@example.com", "John", "Doe", "", "Password is required")]
    [InlineData("johndoe", "john@example.com", "John", "Doe", "weak", "Password must be at least 8 characters")]
    public async Task HandleAsync_ShouldValidateRequest_WithInvalidData(
        string username, string email, string firstName, string lastName, string password, string expectedError)
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Username = username,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Password = password
        };

        var validator = new CreateUserValidator(_userRepository);

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == expectedError);
    }
} 