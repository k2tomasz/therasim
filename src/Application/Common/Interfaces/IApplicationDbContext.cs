using Therasim.Domain.Entities;

namespace Therasim.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Simulation> Simulations { get; }
    DbSet<Persona> Personas { get; }
    DbSet<Conversation> Conversations { get; }
    DbSet<Skill> Skills { get; }
    DbSet<PsychProblem> PsychProblems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
