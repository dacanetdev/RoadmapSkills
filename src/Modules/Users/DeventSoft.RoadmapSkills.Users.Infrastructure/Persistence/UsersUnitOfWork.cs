using DeventSoft.RoadmapSkills.Shared.Infrastructure.Persistence;

namespace DeventSoft.RoadmapSkills.Users.Infrastructure.Persistence;

public class UsersUnitOfWork : BaseUnitOfWork
{
    public UsersUnitOfWork(UsersDbContext context) : base(context)
    {
    }
} 