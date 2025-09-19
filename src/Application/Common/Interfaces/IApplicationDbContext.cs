using Therasim.Domain.Entities;

namespace Therasim.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Simulation> Simulations { get; }
    DbSet<Persona> Personas { get; }
    DbSet<Session> Sessions { get; }
    DbSet<Skill> Skills { get; }
    DbSet<Assessment> Assessments { get; }
    DbSet<AssessmentTask> AssessmentTasks { get; }
    DbSet<UserAssessment> UserAssessments { get; }
    DbSet<UserAssessmentTask> UserAssessmentTasks { get; }
    DbSet<UserProfile> UserProfiles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
