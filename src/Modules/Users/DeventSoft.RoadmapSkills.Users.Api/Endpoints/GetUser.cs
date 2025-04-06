using DeventSoft.RoadmapSkills.Users.Application.Services;
using DeventSoft.RoadmapSkills.Users.Domain.Entities;
using FastEndpoints;

namespace DeventSoft.RoadmapSkills.Users.Api.Endpoints;

public class GetUserRequest
{
    public Guid Id { get; set; }
}

public class GetUserEndpoint : Endpoint<GetUserRequest, User>
{
    private readonly IUserService _userService;

    public GetUserEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/users/{id}");
        AllowAnonymous();
        Description(b => b
            .WithName("GetUserById")
            .Produces<User>(200)
            .Produces(404)
            .WithTags("Users"));
    }

    public override async Task HandleAsync(GetUserRequest req, CancellationToken ct)
    {
        var user = await _userService.GetUserByIdAsync(req.Id);
        if (user is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(user, ct);
    }
} 