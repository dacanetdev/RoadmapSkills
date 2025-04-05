using FastEndpoints;
using DeventSoft.RoadmapSkills.Users.Domain.Repositories;
using DeventSoft.RoadmapSkills.Shared.Domain.Common;

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
        Summary(s => {
            s.Summary = "Deletes a user";
            s.Description = "Soft deletes a user by their ID";
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

        await _userRepository.DeleteAsync(user);
        await _unitOfWork.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
} 