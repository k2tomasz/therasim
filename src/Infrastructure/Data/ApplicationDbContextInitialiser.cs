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
            var assessmentTask1LanguageEn = new AssessmentTaskLanguage
            {
                Language = Language.English,
                Name = "Alliance Rupture: Client Feels Unheard",
                Scenario =
                    "The client feels frustrated, believing that the therapist is not listening or grasping their main concerns.",
                Challenge =
                    "The therapist must validate the client’s experience, acknowledge the rupture, and demonstrate effective repair to restore trust",
                Skills = "Empathy, Alliance Rupture-Repair, Verbal Fluency",
                ClientPersona = "- **Name**: Alex\r\n- **Age**: 32\r\n- **Gender**: Female\r\n- **Occupation**: Marketing professional\r\n- **Ethnicity**: Caucasian\r\n- **Presenting Issues**: The client feels unfulfilled at work and frustrated with her lack of progress in therapy. She believes the therapist doesn’t understand her core issues and feels like her concerns are being dismissed.\r\n- **History**: She has a history of struggling with authority figures and often feels misunderstood by others in her personal and professional life. Past experiences of invalidation have made her sensitive to feeling unheard."
            };

            var assessmentTask1LanguagePl = new AssessmentTaskLanguage
            {
                Language = Language.Polish,
                Name = "Zerwanie Sojuszu Terapeutycznego: Klient Czuje się Niesłuchany",
                Scenario =
                    "Klient odczuwa frustrację, wierząc, że terapeuta nie słucha lub nie rozumie jego głównych problemów.",
                Challenge =
                    "Terapeuta musi potwierdzić doświadczenie klienta, uznać przerwanie sojuszu i pokazać skuteczną naprawę w celu przywrócenia zaufania.",
                Skills = "Empatia, Naprawa Przerwania Sojuszu, Płynność Werbalna",
                ClientPersona = @"
                            - **Imię**: Alex
                            - **Wiek**: 32 lata
                            - **Płeć**: Kobieta
                            - **Zawód**: Specjalistka ds. marketingu
                            - **Pochodzenie etniczne**: Kaukaska
                            - **Zgłaszane problemy**: Klientka czuje się niespełniona w pracy i sfrustrowana brakiem postępów w terapii. Uważa, że terapeuta nie rozumie jej kluczowych problemów i że jej obawy są lekceważone.
                            - **Historia**: Ma historię trudności w relacjach z autorytetami i często czuje się niezrozumiana zarówno w życiu osobistym, jak i zawodowym. Przeszłe doświadczenia z ignorowaniem jej uczuć sprawiły, że jest wrażliwa na brak słyszalności i zrozumienia.
                            "
            };

            var assessmentTask1 = new AssessmentTask();
            assessmentTask1.AssessmentTaskLanguages.Add(assessmentTask1LanguageEn);
            assessmentTask1.AssessmentTaskLanguages.Add(assessmentTask1LanguagePl);
            assessmentTask1.LengthInMinutes = 5;
            assessmentTask1.Order = 1;

            var assessmentTask2LanguageEn = new AssessmentTaskLanguage
            {
                Language = Language.English,
                Name = "Aggressive/Hostile Client Response",
                Scenario =
                    "The client reacts with hostility, accusing the therapist of being ineffective or contributing to their problems.",
                Challenge =
                    "The therapist must stay calm, regulate their own emotions, and de-escalate the situation while maintaining a productive dialogue",
                Skills = "Emotional Regulation, Warmth, Conflict Resolution",
                ClientPersona = "- **Name**: Alex\n- **Age**: 40\n- **Gender**: Male\n- **Occupation**: IT Manager\n- **Ethnicity**: Mixed-race\n- **Presenting Issues**: Frustration with therapy, dissatisfaction with progress, underlying anxiety, possible undiagnosed anger management issues.\n- **History**: Job stress and conflicts with coworkers; limited social support, feeling isolated."
            };

            var assessmentTask2LanguagePl = new AssessmentTaskLanguage
            {
                Language = Language.Polish,
                Name = "Agresywna/Wroga Reakcja Klienta",
                Scenario =
                    "Klient reaguje wrogo, oskarżając terapeutę o nieskuteczność lub przyczynianie się do jego problemów.",
                Challenge =
                    "Terapeuta musi zachować spokój, kontrolować własne emocje i złagodzić sytuację, jednocześnie utrzymując produktywny dialog.",
                Skills = "Regulacja Emocji, Ciepło, Rozwiązywanie Konfliktów",
                ClientPersona = @"
                        - **Imię**: Alex
                        - **Wiek**: 40 lat
                        - **Płeć**: Mężczyzna
                        - **Zawód**: Menedżer IT
                        - **Grupa etniczna**: Mieszana
                        - **Zgłaszane problemy**: Frustracja związana z terapią, niezadowolenie z postępów, ukryty lęk, możliwe niezdokumentowane problemy z kontrolą złości.
                        - **Historia**: Stres związany z pracą i konflikty ze współpracownikami; ograniczone wsparcie społeczne, poczucie izolacji.
                        "
            };

            var assessmentTask2 = new AssessmentTask();
            assessmentTask2.AssessmentTaskLanguages.Add(assessmentTask2LanguageEn);
            assessmentTask2.AssessmentTaskLanguages.Add(assessmentTask2LanguagePl);
            assessmentTask2.LengthInMinutes = 5;
            assessmentTask2.Order = 2;



            var assessmentLanguageEn = new AssessmentLanguage
            {
                Language = Language.English,
                Name = "TheraSim Interactive Psychotherapy Skills Assessment",
                Instructions = "### Before You Begin\n\n1. **Setup:**\n   - Ensure a quiet and comfortable environment for the assessment.\n   - Test your microphone and speakers to confirm they are working correctly.\n   - Have a reliable internet connection to avoid interruptions.\n\n2. **Familiarization:**\n   - Review the format of the client persona description, which includes demographics, presenting issues, and relevant history. Understanding this information is crucial for effectively engaging with the simulated client.\n   - Familiarize yourself with the interface, including how to start, pause, and stop the assessment tasks.",
                Description =
                    "The TheraSim Interactive Psychotherapy Skills Assessment is an AI-driven evaluation tool designed " +
                    "to assess psychotherapists' interpersonal effectiveness during challenging therapy moments. " +
                    "The assessment evaluates therapists' core skills, focusing on how they handle emotionally charged " +
                    "scenarios such as alliance ruptures or client volatility. Using standardized interactive client simulations, " +
                    "which replicate real-life conversations in a psychotherapy setting, the assessment provides a controlled " +
                    "yet dynamic environment to measure the therapist's ability to respond to difficult situations " +
                    "across several key areas: empathy, verbal fluency, rupture-repair capability, and more.\n\n" +
                    "The outcome of this assessment includes a quantitative score based on expert criteria and qualitative " +
                    "feedback highlighting the therapist's strengths and areas for improvement, providing comprehensive " +
                    "insights into their interpersonal effectiveness."

            };

            var assessmentLanguagePl = new AssessmentLanguage
            {
                Language = Language.Polish,
                Name = "Interaktywny Test Umiejętności Psychoterapeutycznych TheraSim",
                Instructions = @"
                        ### Zanim Zaczniesz

                        1. **Przygotowanie:**
                           - Upewnij się, że przebywasz w cichym i komfortowym miejscu, odpowiednim do przeprowadzenia oceny.
                           - Przetestuj mikrofon i głośniki, aby upewnić się, że działają prawidłowo.
                           - Zadbaj o stabilne połączenie internetowe, aby uniknąć przerw.

                        2. **Zapoznanie:**
                           - Zapoznaj się z formatem opisu postaci klienta, który zawiera dane demograficzne, problemy przedstawiane przez klienta oraz istotną historię. Zrozumienie tych informacji jest kluczowe dla efektywnej interakcji z symulowanym klientem.
                           - Zapoznaj się z interfejsem, w tym z funkcjami uruchamiania, wstrzymywania i zatrzymywania zadań oceny.
                        ",
                Description =
                    "Interaktywny Test Umiejętności Psychoterapeutycznych TheraSim to narzędzie oceny oparte na sztucznej inteligencji, " +
                    "zaprojektowane w celu sprawdzania efektywności interpersonalnej psychoterapeutów w trudnych momentach terapii. " +
                    "Narzędzie to bada kluczowe umiejętności terapeuty, koncentrując się na tym, jak radzi sobie z emocjonalnie napiętymi scenariuszami, " +
                    "takimi jak zerwanie przymierza terapeutycznego czy niestabilność emocjonalna klienta. Dzięki ustandaryzowanym, " +
                    "interaktywnym symulacjom klientów, które odzwierciedlają realne rozmowy prowadzone w warunkach psychoterapeutycznych, " +
                    "ocena zapewnia kontrolowane, a jednocześnie dynamiczne środowisko, aby mierzyć zdolność terapeuty do reagowania " +
                    "na trudne sytuacje w takich kluczowych obszarach, jak empatia, płynność werbalna, zdolność naprawy relacji i inne.\n\n" +
                    "Wynik tej oceny obejmuje ilościową ocenę opartą na kryteriach eksperckich oraz jakościową informację zwrotną wskazującą " +
                    "na mocne strony terapeuty i obszary wymagające poprawy, co daje kompleksowy wgląd w jego efektywność interpersonalną.",
            };

            var assessment = new Assessment();
            assessment.AssessmentLanguages.Add(assessmentLanguageEn);
            assessment.AssessmentLanguages.Add(assessmentLanguagePl);
            assessment.AssessmentTasks.Add(assessmentTask1);
            assessment.AssessmentTasks.Add(assessmentTask2);

            _context.Assessments.Add(assessment);

            await _context.SaveChangesAsync();
        }
    }
}
