using FastEndpoints;
using DeventSoft.RoadmapSkills.Users.Domain.Repositories;
using DeventSoft.RoadmapSkills.Shared.Domain.Common;
using FluentValidation;
using Mapster;

namespace DeventSoft.RoadmapSkills.Users.Api.Endpoints.Users;

public class UpdateUserRequest
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? Email { get; init; }
}

public class UpdateUserResponse
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

public class UpdateUserValidator : Validator<UpdateUserRequest>
{
    public UpdateUserValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");

        When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format")
                .MustAsync(async (email, ct) => !await userRepository.ExistsByEmailAsync(email!))
                .WithMessage("Email already exists");
        });
    }
}

public class UpdateUserEndpoint : Endpoint<UpdateUserRequest, UpdateUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserEndpoint(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Put("/api/users/{id}");
        Summary(s => {
            s.Summary = "Updates a user";
            s.Description = "Updates a user's information";
        });
        Tags("Users");
    }

    public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        user.UpdateProfile(req.FirstName, req.LastName);
        
        if (!string.IsNullOrWhiteSpace(req.Email))
        {
            user.UpdateEmail(req.Email);
        }

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync(ct);

        var response = user.Adapt<UpdateUserResponse>();
        await SendOkAsync(response, ct);
    }
} 