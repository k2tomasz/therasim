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
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<Assessment> Assessments => Set<Assessment>();
    public DbSet<AssessmentTask> AssessmentTasks => Set<AssessmentTask>();
    public DbSet<UserAssessment> UserAssessments => Set<UserAssessment>();
    public DbSet<UserAssessmentTask> UserAssessmentTasks => Set<UserAssessmentTask>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        // Configure relationships to avoid cycles or multiple cascade paths
        //builder.Entity<UserAssessmentTask>()
        //    .HasOne(uat => uat.UserAssessment)
        //    .WithMany(ua => ua.UserAssessmentTasks)
        //    .HasForeignKey(uat => uat.UserAssessmentId)
        //    .OnDelete(DeleteBehavior.NoAction);

        //builder.Entity<UserAssessmentTask>()
        //    .HasOne(uat => uat.AssessmentTask)
        //    .WithMany(at => at.UserAssessmentTasks)
        //    .HasForeignKey(uat => uat.AssessmentTaskId)
        //    .OnDelete(DeleteBehavior.NoAction);
    }
}
