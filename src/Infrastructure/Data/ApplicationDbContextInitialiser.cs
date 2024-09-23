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

            _context.Personas.Add(persona1);

            await _context.SaveChangesAsync();
        }

        if (!_context.Skills.Any())
        {
            var skill1 = new Skill
            {
                Name = "Emotional Mirroring",
                Description = "Emotional mirroring is the practice of reflecting the client’s emotional state back to them, often using similar words, tone, or body language to convey understanding. This technique helps clients feel seen and heard and provides a space for them to process their emotions."
            };


            _context.Skills.Add(skill1);

            await _context.SaveChangesAsync();
        }
    }
}
