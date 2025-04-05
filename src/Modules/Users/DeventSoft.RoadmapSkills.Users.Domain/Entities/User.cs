using DeventSoft.RoadmapSkills.Shared.Domain.Common;

namespace DeventSoft.RoadmapSkills.Users.Domain.Entities;

public class User : Entity
{
    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    private User() { } // For EF Core

    public User(string username, string email, string firstName, string lastName, string passwordHash)
    {
        Username = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PasswordHash = passwordHash;
        IsActive = true;
    }

    public void UpdateProfile(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        Update();
    }

    public void UpdateEmail(string email)
    {
        Email = email;
        Update();
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        Update();
    }

    public void Deactivate()
    {
        IsActive = false;
        Update();
    }

    public void Activate()
    {
        IsActive = true;
        Update();
    }
} 