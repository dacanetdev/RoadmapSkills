namespace DeventSoft.RoadmapSkills.Shared.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; protected set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
        Update();
    }

    protected void Update()
    {
        UpdatedAt = DateTime.UtcNow;
    }
} 