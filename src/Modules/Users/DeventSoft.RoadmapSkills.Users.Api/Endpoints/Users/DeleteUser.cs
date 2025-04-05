using FastEndpoints;
using DeventSoft.RoadmapSkills.Users.Domain.Repositories;
using DeventSoft.RoadmapSkills.Shared.Infrastructure.Common;

namespace DeventSoft.RoadmapSkills.Users.Api.Endpoints.Users;

public class DeleteUserEndpoint : EndpointWithoutRequest
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserEndpoint(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Delete("/api/users/{id}");
        Description(d => d
            .WithName("DeleteUser")
            .WithTags("Users")
            .WithSummary("Deletes a user")
            .WithDescription("Soft deletes a user by their ID"));
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

        await _userRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
} 