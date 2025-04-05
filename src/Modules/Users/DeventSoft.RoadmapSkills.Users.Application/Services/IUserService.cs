using DeventSoft.RoadmapSkills.Users.Domain.Entities;

namespace DeventSoft.RoadmapSkills.Users.Application.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(Guid id);
    Task CreateUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(Guid id);
} 