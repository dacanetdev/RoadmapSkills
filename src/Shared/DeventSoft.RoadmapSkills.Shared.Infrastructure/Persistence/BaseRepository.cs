using DeventSoft.RoadmapSkills.Shared.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DeventSoft.RoadmapSkills.Shared.Infrastructure.Persistence;

public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    protected readonly DbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    protected BaseRepository(DbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted, cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(e => !e.IsDeleted).ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.MarkAsDeleted();
        await UpdateAsync(entity, cancellationToken);
    }

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(e => e.Id == id && !e.IsDeleted, cancellationToken);
    }
} 