using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();

    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {

        // Default data
        // Seed, if necessary
        if (!_context.Assessments.Any())
        {
            var assessmentLanguage = new AssessmentLanguage
            {
                Language = Language.English,
                Name = "TheraSim Facilitative Skills Assessment",
                Description = "The TheraSim Facilitative Skills Assessment is an AI-driven evaluation tool designed to " +
                              "assess psychotherapists' interpersonal effectiveness during challenging therapy moments. " +
                              "Based on the Facilitative Interpersonal Skills (FIS) method, the assessment evaluates " +
                              "therapists' core skills, focusing on how they handle emotionally charged scenarios such " +
                              "as alliance ruptures or client volatility. Using standardized client simulations, " +
                              "the assessment provides a controlled yet dynamic environment to measure the " +
                              "therapist's ability to respond to difficult situations across several key areas: " +
                              "empathy, verbal fluency, rupture-repair capability, and more. Performance is scored " +
                              "without real-time feedback, to allow for an authentic assessment of the therapist’s instinctive skills.",
                
            }

            var assessment = new Assessment
            {
            };

            _context.Personas.Add(persona1);

            await _context.SaveChangesAsync();
        }
    }
}
