using DeventSoft.RoadmapSkills.Shared.Domain.Common;
using DeventSoft.RoadmapSkills.Users.Domain.Entities;
using DeventSoft.RoadmapSkills.Users.Domain.Repositories;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;

namespace DeventSoft.RoadmapSkills.Users.Api.Endpoints.Users;

public class CreateUserRequest
{
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class CreateUserResponse
{
    public Guid Id { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

public class CreateUserValidator : Validator<CreateUserRequest>
{
    public CreateUserValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters")
            .MustAsync(async (username, ct) => !await userRepository.ExistsByUsernameAsync(username))
            .WithMessage("Username already exists");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MustAsync(async (email, ct) => !await userRepository.ExistsByEmailAsync(email))
            .WithMessage("Email already exists");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one number")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
    }
}

public class CreateUserEndpoint : Endpoint<CreateUserRequest, CreateUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher;

    public CreateUserEndpoint(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public override void Configure()
    {
        Post("/api/users");
        AllowAnonymous();
        Description(d => d
            .WithName("CreateUser")
            .WithTags("Users")
            .WithSummary("Creates a new user")
            .WithDescription("Creates a new user with the provided information"));
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {    
        var user = new User(
            req.Username,
            req.Email,
            req.FirstName,
            req.LastName);

        var passwordHash = _passwordHasher.HashPassword(user, req.Password);
        user.UpdatePassword(passwordHash);

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(ct);

        var response = user.Adapt<CreateUserResponse>();
        await SendCreatedAtAsync<GetUserEndpoint>(
            new { id = user.Id },
            response,
            generateAbsoluteUrl: true,
            cancellation: ct);
    }
} 