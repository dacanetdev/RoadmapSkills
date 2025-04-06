using DeventSoft.RoadmapSkills.Users.Application.Services;
using DeventSoft.RoadmapSkills.Users.Domain.Entities;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace DeventSoft.RoadmapSkills.Users.Api.Endpoints;

public class CreateUserRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class CreateUserResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class CreateUserEndpoint : Endpoint<CreateUserRequest, CreateUserResponse>
{
    private readonly IUserService _userService;

    public CreateUserEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/users");
        AllowAnonymous();
        Description(b => b
            .WithName("CreateUser")
            .Produces<CreateUserResponse>(201)
            .ProducesProblem(400)
            .WithTags("Users"));
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        var user = new User
        {
            FirstName = req.FirstName,
            LastName = req.LastName,
            Email = req.Email,
            Password = req.Password
        };

        await _userService.CreateUserAsync(user);

        var response = new CreateUserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };

        await SendCreatedAtAsync<GetUserEndpoint>(
            new { id = user.Id },
            response,
            generateAbsoluteUrl: true,
            cancellation: ct);
    }
} 