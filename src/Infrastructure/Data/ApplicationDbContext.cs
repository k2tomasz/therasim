using System.Reflection;
using Therasim.Application.Common.Interfaces;
using Therasim.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Therasim.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Simulation> Simulations => Set<Simulation>();
    public DbSet<Persona> Personas => Set<Persona>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<Problem> PsychProblems => Set<Problem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
