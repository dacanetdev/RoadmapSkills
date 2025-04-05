using DeventSoft.RoadmapSkills.Shared.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DeventSoft.RoadmapSkills.Shared.Infrastructure.Persistence;

public abstract class BaseDbContext : DbContext
{
    protected BaseDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is Entity && e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            var entity = (Entity)entry.Entity;
            if (entry.State == EntityState.Modified)
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
} 