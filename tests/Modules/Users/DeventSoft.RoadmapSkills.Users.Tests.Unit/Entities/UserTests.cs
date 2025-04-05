using DeventSoft.RoadmapSkills.Users.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace DeventSoft.RoadmapSkills.Users.Tests.Unit.Entities;

public class UserTests
{
    private readonly string _username = "johndoe";
    private readonly string _email = "john@example.com";
    private readonly string _firstName = "John";
    private readonly string _lastName = "Doe";
    private readonly string _passwordHash = "hashedpassword";

    [Fact]
    public void Constructor_ShouldCreateUser_WithValidProperties()
    {
        // Act
        var user = new User(_username, _email, _firstName, _lastName, _passwordHash);

        // Assert
        user.Username.Should().Be(_username);
        user.Email.Should().Be(_email);
        user.FirstName.Should().Be(_firstName);
        user.LastName.Should().Be(_lastName);
        user.PasswordHash.Should().Be(_passwordHash);
        user.IsActive.Should().BeTrue();
        user.Id.Should().NotBeEmpty();
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.UpdatedAt.Should().BeNull();
        user.IsDeleted.Should().BeFalse();
    }

    [Theory]
    [InlineData("", "Invalid username")]
    [InlineData(" ", "Invalid username")]
    [InlineData(null, "Invalid username")]
    public void Constructor_ShouldThrowException_WhenUsernameIsInvalid(string username, string expectedMessage)
    {
        // Act
        var act = () => new User(username, _email, _firstName, _lastName, _passwordHash);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage(expectedMessage);
    }

    [Theory]
    [InlineData("", "Invalid email")]
    [InlineData(" ", "Invalid email")]
    [InlineData(null, "Invalid email")]
    [InlineData("notanemail", "Invalid email format")]
    [InlineData("@example.com", "Invalid email format")]
    [InlineData("john@", "Invalid email format")]
    public void Constructor_ShouldThrowException_WhenEmailIsInvalid(string email, string expectedMessage)
    {
        // Act
        var act = () => new User(_username, email, _firstName, _lastName, _passwordHash);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage(expectedMessage);
    }

    [Theory]
    [InlineData("", "Invalid first name")]
    [InlineData(" ", "Invalid first name")]
    [InlineData(null, "Invalid first name")]
    public void Constructor_ShouldThrowException_WhenFirstNameIsInvalid(string firstName, string expectedMessage)
    {
        // Act
        var act = () => new User(_username, _email, firstName, _lastName, _passwordHash);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage(expectedMessage);
    }

    [Theory]
    [InlineData("", "Invalid last name")]
    [InlineData(" ", "Invalid last name")]
    [InlineData(null, "Invalid last name")]
    public void Constructor_ShouldThrowException_WhenLastNameIsInvalid(string lastName, string expectedMessage)
    {
        // Act
        var act = () => new User(_username, _email, _firstName, lastName, _passwordHash);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage(expectedMessage);
    }

    [Theory]
    [InlineData("", "Invalid password hash")]
    [InlineData(" ", "Invalid password hash")]
    [InlineData(null, "Invalid password hash")]
    public void Constructor_ShouldThrowException_WhenPasswordHashIsInvalid(string passwordHash, string expectedMessage)
    {
        // Act
        var act = () => new User(_username, _email, _firstName, _lastName, passwordHash);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage(expectedMessage);
    }

    [Fact]
    public void UpdateProfile_ShouldUpdateNameProperties()
    {
        // Arrange
        var user = new User(_username, _email, _firstName, _lastName, _passwordHash);
        var newFirstName = "Jonathan";
        var newLastName = "Smith";

        // Act
        user.UpdateProfile(newFirstName, newLastName);

        // Assert
        user.FirstName.Should().Be(newFirstName);
        user.LastName.Should().Be(newLastName);
        user.UpdatedAt.Should().NotBeNull();
        user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("", "Valid", "Invalid first name")]
    [InlineData(" ", "Valid", "Invalid first name")]
    [InlineData(null, "Valid", "Invalid first name")]
    [InlineData("Valid", "", "Invalid last name")]
    [InlineData("Valid", " ", "Invalid last name")]
    [InlineData("Valid", null, "Invalid last name")]
    public void UpdateProfile_ShouldThrowException_WhenNamesAreInvalid(string firstName, string lastName, string expectedMessage)
    {
        // Arrange
        var user = new User(_username, _email, _firstName, _lastName, _passwordHash);

        // Act
        var act = () => user.UpdateProfile(firstName, lastName);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage(expectedMessage);
    }

    [Fact]
    public void UpdateEmail_ShouldUpdateEmailProperty()
    {
        // Arrange
        var user = new User(_username, _email, _firstName, _lastName, _passwordHash);
        var newEmail = "jonathan@example.com";

        // Act
        user.UpdateEmail(newEmail);

        // Assert
        user.Email.Should().Be(newEmail);
        user.UpdatedAt.Should().NotBeNull();
        user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("", "Invalid email")]
    [InlineData(" ", "Invalid email")]
    [InlineData(null, "Invalid email")]
    [InlineData("notanemail", "Invalid email format")]
    [InlineData("@example.com", "Invalid email format")]
    [InlineData("john@", "Invalid email format")]
    public void UpdateEmail_ShouldThrowException_WhenEmailIsInvalid(string email, string expectedMessage)
    {
        // Arrange
        var user = new User(_username, _email, _firstName, _lastName, _passwordHash);

        // Act
        var act = () => user.UpdateEmail(email);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage(expectedMessage);
    }

    [Fact]
    public void UpdatePassword_ShouldUpdatePasswordHash()
    {
        // Arrange
        var user = new User(_username, _email, _firstName, _lastName, _passwordHash);
        var newPasswordHash = "newhash";

        // Act
        user.UpdatePassword(newPasswordHash);

        // Assert
        user.PasswordHash.Should().Be(newPasswordHash);
        user.UpdatedAt.Should().NotBeNull();
        user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("", "Invalid password hash")]
    [InlineData(" ", "Invalid password hash")]
    [InlineData(null, "Invalid password hash")]
    public void UpdatePassword_ShouldThrowException_WhenPasswordHashIsInvalid(string passwordHash, string expectedMessage)
    {
        // Arrange
        var user = new User(_username, _email, _firstName, _lastName, _passwordHash);

        // Act
        var act = () => user.UpdatePassword(passwordHash);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage(expectedMessage);
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var user = new User(_username, _email, _firstName, _lastName, _passwordHash);

        // Act
        user.Deactivate();

        // Assert
        user.IsActive.Should().BeFalse();
        user.UpdatedAt.Should().NotBeNull();
        user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Deactivate_ShouldNotChangeState_WhenUserIsAlreadyDeactivated()
    {
        // Arrange
        var user = new User(_username, _email, _firstName, _lastName, _passwordHash);
        user.Deactivate();
        var firstUpdateTime = user.UpdatedAt;

        // Act
        user.Deactivate();

        // Assert
        user.IsActive.Should().BeFalse();
        user.UpdatedAt.Should().Be(firstUpdateTime);
    }

    [Fact]
    public void Activate_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var user = new User(_username, _email, _firstName, _lastName, _passwordHash);
        user.Deactivate();

        // Act
        user.Activate();

        // Assert
        user.IsActive.Should().BeTrue();
        user.UpdatedAt.Should().NotBeNull();
        user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Activate_ShouldNotChangeState_WhenUserIsAlreadyActive()
    {
        // Arrange
        var user = new User(_username, _email, _firstName, _lastName, _passwordHash);
        var createdTime = user.CreatedAt;

        // Act
        user.Activate();

        // Assert
        user.IsActive.Should().BeTrue();
        user.UpdatedAt.Should().BeNull();
        user.CreatedAt.Should().Be(createdTime);
    }

    [Fact]
    public void MarkAsDeleted_ShouldSetIsDeletedToTrue()
    {
        // Arrange
        var user = new User(_username, _email, _firstName, _lastName, _passwordHash);

        // Act
        user.MarkAsDeleted();

        // Assert
        user.IsDeleted.Should().BeTrue();
        user.UpdatedAt.Should().NotBeNull();
        user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void MarkAsDeleted_ShouldNotChangeState_WhenUserIsAlreadyDeleted()
    {
        // Arrange
        var user = new User(_username, _email, _firstName, _lastName, _passwordHash);
        user.MarkAsDeleted();
        var firstUpdateTime = user.UpdatedAt;

        // Act
        user.MarkAsDeleted();

        // Assert
        user.IsDeleted.Should().BeTrue();
        user.UpdatedAt.Should().Be(firstUpdateTime);
    }
} 