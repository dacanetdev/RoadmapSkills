using DeventSoft.RoadmapSkills.Users.Application.Services;
using DeventSoft.RoadmapSkills.Users.Domain.Entities;
using FastEndpoints;

namespace DeventSoft.RoadmapSkills.Users.Api.Endpoints;

public class GetUsersEndpoint : EndpointWithoutRequest<IEnumerable<User>>
{
    private readonly IUserService _userService;

    public GetUsersEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/users");
        AllowAnonymous();
        Description(b => b
            .WithName("GetUsers")
            .Produces<IEnumerable<User>>(200)
            .WithTags("Users"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var users = await _userService.GetAllUsersAsync();
        await SendOkAsync(users, ct);
    }
} 