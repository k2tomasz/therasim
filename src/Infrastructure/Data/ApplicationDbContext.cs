using System.Reflection;
using Therasim.Application.Common.Interfaces;
using Therasim.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Therasim.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Console.WriteLine("ApplicationDbContext created!");
    }
    public DbSet<Simulation> Simulations => Set<Simulation>();
    public DbSet<Persona> Personas => Set<Persona>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<Problem> Problems => Set<Problem>();
    public DbSet<Assessment> Assessments => Set<Assessment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
