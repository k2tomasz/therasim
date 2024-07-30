using Therasim.Domain.Entities;

namespace Therasim.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Simulation> Simulations { get; }
    DbSet<Persona> Personas { get; }
    DbSet<Session> Sessions { get; }
    DbSet<Message> Messages { get; }
    DbSet<Feedback> Feedbacks { get; }
    DbSet<Skill> Skills { get; }
    DbSet<Problem> Problems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
