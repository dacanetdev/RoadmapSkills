namespace DeventSoft.RoadmapSkills.Shared.Domain.Common;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
} 