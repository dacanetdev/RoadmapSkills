using DeventSoft.RoadmapSkills.Users.Domain.Entities;
using DeventSoft.RoadmapSkills.Users.Domain.Repositories;

namespace DeventSoft.RoadmapSkills.Users.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync() => 
        await _userRepository.GetAllAsync();

    public async Task<User?> GetUserByIdAsync(Guid id) => 
        await _userRepository.GetByIdAsync(id);

    public async Task CreateUserAsync(User user) => 
        await _userRepository.AddAsync(user);

    public async Task<bool> UpdateUserAsync(User user)
    {
        var existingUser = await _userRepository.GetByIdAsync(user.Id);
        if (existingUser == null) return false;
        
        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;
        
        await _userRepository.DeleteAsync(user);
        return true;
    }
} 