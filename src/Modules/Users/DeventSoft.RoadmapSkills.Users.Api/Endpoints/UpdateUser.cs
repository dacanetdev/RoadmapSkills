using DeventSoft.RoadmapSkills.Users.Application.Services;
using DeventSoft.RoadmapSkills.Users.Domain.Entities;
using FastEndpoints;

namespace DeventSoft.RoadmapSkills.Users.Api.Endpoints;

public class UpdateUserRequest
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class UpdateUserEndpoint : Endpoint<UpdateUserRequest>
{
    private readonly IUserService _userService;

    public UpdateUserEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Put("/users/{id}");
        AllowAnonymous();
        Description(b => b
            .WithName("UpdateUser")
            .Produces(204)
            .Produces(404)
            .ProducesProblem(400)
            .WithTags("Users"));
    }

    public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
    {
        var user = new User
        {
            Id = req.Id,
            FirstName = req.FirstName,
            LastName = req.LastName,
            Email = req.Email
        };

        var updated = await _userService.UpdateUserAsync(user);
        if (!updated)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendNoContentAsync(ct);
    }
} 