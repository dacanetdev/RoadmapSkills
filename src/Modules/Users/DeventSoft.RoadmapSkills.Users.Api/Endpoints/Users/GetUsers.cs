using FastEndpoints;
using DeventSoft.RoadmapSkills.Users.Domain.Repositories;
using Mapster;

namespace DeventSoft.RoadmapSkills.Users.Api.Endpoints.Users;

public class GetUsersResponse
{
    public IEnumerable<UserDto> Users { get; init; } = Enumerable.Empty<UserDto>();

    public class UserDto
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
}

public class GetUsersEndpoint : EndpointWithoutRequest<GetUsersResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUsersEndpoint(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override void Configure()
    {
        Get("/api/users");
        Description(d => d
            .WithName("GetUsers")
            .WithTags("Users")
            .WithSummary("Gets all users")
            .WithDescription("Retrieves a list of all active users"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var users = await _userRepository.GetAllAsync();
        var response = new GetUsersResponse
        {
            Users = users.Adapt<IEnumerable<GetUsersResponse.UserDto>>()
        };

        await SendOkAsync(response, ct);
    }
} 