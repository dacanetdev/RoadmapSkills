using FastEndpoints;
using DeventSoft.RoadmapSkills.Users.Domain.Repositories;
using Mapster;

namespace DeventSoft.RoadmapSkills.Users.Api.Endpoints.Users;

public class GetUserResponse
{
    public Guid Id { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

public class GetUserEndpoint : EndpointWithoutRequest<GetUserResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserEndpoint(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override void Configure()
    {
        Get("/api/users/{id}");
        Summary(s => {
            s.Summary = "Gets a user by ID";
            s.Description = "Retrieves a user's information by their unique identifier";
        });
        Tags("Users");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var response = user.Adapt<GetUserResponse>();
        await SendOkAsync(response, ct);
    }
} 