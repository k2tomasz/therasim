using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Therasim.Domain.Entities;

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
        if (!_context.Personas.Any())
        {

            var persona1 = new Persona()
            {
                Name = "Alex, 25, recent graduated collage",
                Background = string.Empty

            };

            var persona2 = new Persona
            {
                Name = "Emily, 65, recently retired",
                Background = string.Empty
            };

            _context.Personas.Add(persona1);
            _context.Personas.Add(persona2);

            await _context.SaveChangesAsync();
        }

        if (!_context.Skills.Any())
        {
            var skill1 = new Skill
            {
                Name = "Empathy",
                Description = string.Empty
            };

            var skill2 = new Skill
            {
                Name = "Communication",
                Description = string.Empty
            };

            _context.Skills.Add(skill1);
            _context.Skills.Add(skill2);

            await _context.SaveChangesAsync();
        }

        if (!_context.PsychProblems.Any())
        {
            var problem1 = new Problem
            {
                Name = "Anxiety",
                Description = string.Empty
            };

            var problem2 = new Problem
            {
                Name = "Depression",
                Description = string.Empty
            };

            _context.PsychProblems.Add(problem1);
            _context.PsychProblems.Add(problem2);

            await _context.SaveChangesAsync();
        }


    }
}
