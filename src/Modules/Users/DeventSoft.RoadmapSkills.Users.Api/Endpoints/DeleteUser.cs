using DeventSoft.RoadmapSkills.Users.Application.Services;
using FastEndpoints;

namespace DeventSoft.RoadmapSkills.Users.Api.Endpoints;

public class DeleteUserRequest
{
    public Guid Id { get; set; }
}

public class DeleteUserEndpoint : Endpoint<DeleteUserRequest>
{
    private readonly IUserService _userService;

    public DeleteUserEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Delete("/users/{id}");
        AllowAnonymous();
        Description(b => b
            .WithName("DeleteUser")
            .Produces(204)
            .Produces(404)
            .WithTags("Users"));
    }

    public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
    {
        var deleted = await _userService.DeleteUserAsync(req.Id);
        if (!deleted)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendNoContentAsync(ct);
    }
} 