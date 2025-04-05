namespace DeventSoft.RoadmapSkills.Shared.Domain.Common;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
} 