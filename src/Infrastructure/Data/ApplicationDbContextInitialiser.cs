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
            #region Task0

            var assessmentTask0LanguageEn = new AssessmentTaskLanguage
            {
                Language = Language.English,
                Name = "Intruduction to simulated psychotherapy session.",
                Scenario = "The client is a 35-year-old individual named Alex who has been experiencing moderate to severe depression for the past 6 months. The client feel hopeless, lack motivation, and struggle with daily tasks. Social anxiety compounds difficulties, leading to isolation. The client had negative past experiences with therapy.\n\n",
                Challenge = "Get yourself familiar with the UI, check if speach recognision works. Ask few questions to see how to interact with the 'Client'.\n\n",
                Skills = "Core Psychotherapy sklills",
                ClientInitialDialogue = "I feel tired, I guess. It’s just hard to get out of bed most days. What’s the point, you know?",
                AssessmentCriteria = "",

            };

            var assessmentTask0LanguagePl = new AssessmentTaskLanguage
            {
                Language = Language.Polish,
                Name = "Wprowadzenie do symulowanej sesji psychoterapeutycznej",
                Scenario = "Klientem jest 35-letni Alex, od 6 miesięcy cierpi na umiarkowaną lub ciężką depresję. Klient czuje się beznadziejny, brakuje mu motywacji i zmaga się z codziennymi zadaniami. Lęk społeczny pogłębia trudności, prowadząc do izolacji. Klient miał negatywne doświadczenia z terapią w przeszłości.\n\n",
                Challenge = "Zapoznaj się z interfejsem użytkownika, sprawdź, czy działa rozpoznawanie mowy. Zadaj kilka pytań, aby zobaczyć, jak wchodzić w interakcję z „Klientem”\n\n",
                Skills = "Podstawowe umiejętności psychoterapeutyczne",
                ClientInitialDialogue = "Czuję się zmęczona. Przez większość dni trudno jest mi wstać z łóżka.",
                AssessmentCriteria = "",
            };

            var assessmentTask0 = new AssessmentTask();
            assessmentTask0.AssessmentTaskLanguages.Add(assessmentTask0LanguageEn);
            assessmentTask0.AssessmentTaskLanguages.Add(assessmentTask0LanguagePl);
            assessmentTask0.LengthInMinutes = 4;
            assessmentTask0.Order = 0;

            #endregion

            #region Task1

            var assessmentTask1LanguageEn = new AssessmentTaskLanguage
            {
                Language = Language.English,
                Name = "Paraphrasing: Client Feels Overwhelmed by Life Circumstances",
                Scenario = "The client, a middle-aged individual, expresses feeling overwhelmed by several recent life events, including the death of a close family member, job insecurity, and health issues. The client begins to speak rapidly, sharing how everything is \"piling up\" and that they feel like they are \"drowning\" under the weight of it all. They mention feeling guilty for not \"holding it together\" and not being able to manage their emotions better, and they jump from one topic to another, describing their stress in a disorganized way.",
                Challenge = "The therapist must use paraphrasing to restate the client’s thoughts and feelings in a clear, concise way, demonstrating understanding and providing the client with a sense of being heard. The paraphrase should help the client feel more organized in their expression and open up space for deeper reflection or clarification.\n\n",
                Skills = "Active listening: Accurately picking up on the key points and emotional undertones of the client’s communication. Clarification: Providing a restatement that helps organize the client’s scattered thoughts and emotions. Empathy: Conveying understanding and nonjudgmental support through the paraphrase.",
                ClientInitialDialogue = "I just can't handle all of this anymore. First, my mom passed away, and I couldn't even be there because of my work schedule. And now I feel like I’m slipping at work, too, because I can't focus! And then last week, I got these tests back from my doctor, and it’s just... I don’t even know. It's just one thing after another, and I'm failing everywhere. I'm supposed to be stronger than this, but I'm not.",
                AssessmentCriteria = "The therapist’s paraphrase will be evaluated based on how well it captures the essence of the client’s concerns, organizes the scattered elements of the dialogue, and conveys empathy without distorting or minimizing the client’s feelings. The paraphrasing should help the client feel validated and understood, and ideally, create space for the client to expand on or clarify their thoughts.",

            };

            var assessmentTask1LanguagePl = new AssessmentTaskLanguage
            {
                Language = Language.Polish,
                Name = "Parafraza: Klient Czuje się Przytłoczony Okolicznościami Życiowymi",
                Scenario = "Klient, osoba w średnim wieku, wyraża uczucie przytłoczenia kilkoma niedawnymi wydarzeniami życiowymi, w tym śmiercią bliskiego członka rodziny, niepewnością zatrudnienia i problemami zdrowotnymi. Klient zaczyna mówić szybko, dzieląc się tym, jak wszystko się „nakłada” i że czuje, że „tonie” pod ciężarem wszystkiego. Mówi, że czuje się winny, że nie „trzyma się razem” i nie potrafi lepiej kontrolować swoich emocji, skacząc z jednego tematu na drugi, opisując swój stres w dezorganizowany sposób.",
                Challenge = "Terapeuta musi użyć parafrazy, aby ponownie przedstawić myśli i uczucia klienta w sposób jasny i zwięzły, demonstrując zrozumienie i dając klientowi poczucie bycia wysłuchanym. Parafraza powinna pomóc klientowi poczuć się bardziej zorganizowanym w swoim wyrażaniu i otworzyć przestrzeń do głębszej refleksji lub wyjaśnienia.\n\n",
                Skills = "Aktywne słuchanie: Dokładne zrozumienie kluczowych punktów i emocjonalnych podtekstów komunikacji klienta. Wyjaśnienie: Zapewnienie ponownego sformułowania, które pomaga uporządkować rozproszone myśli i emocje klienta. Empatia: Przekazywanie zrozumienia i wsparcia pozbawionego ocen poprzez parafrazę.",
                ClientInitialDialogue = "Po prostu nie mogę już tego znieść. Najpierw zmarła moja mama, a ja nawet nie mogłem być tam z powodu mojego grafiku pracy. A teraz mam wrażenie, że też tracę grunt pod nogami w pracy, bo nie mogę się skoncentrować! A potem w zeszłym tygodniu dostałem te wyniki od lekarza, i to po prostu... nie wiem nawet. To jedno za drugim, i wszędzie zawodzę. Powinienem być silniejszy niż to, ale nie jestem.",
                AssessmentCriteria = "Parafraza terapeuty zostanie oceniona na podstawie tego, jak dobrze uchwytuje istotę obaw klienta, uporządkowuje rozproszone elementy dialogu i przekazuje empatię bez zniekształcania ani minimalizowania uczuć klienta. Parafraza powinna pomóc klientowi poczuć się zrozumianym i zrozumianym, a idealnie, stworzyć przestrzeń dla klienta do rozwinięcia lub wyjaśnienia swoich myśli.",
            };

            var assessmentTask1 = new AssessmentTask();
            assessmentTask1.AssessmentTaskLanguages.Add(assessmentTask1LanguageEn);
            assessmentTask1.AssessmentTaskLanguages.Add(assessmentTask1LanguagePl);
            assessmentTask1.LengthInMinutes = 4;
            assessmentTask1.Order = 1;

            #endregion

            #region Task2

            var assessmentTask2LanguageEn = new AssessmentTaskLanguage
            {
                Language = Language.English,
                Name = "Open Questioning: Client Feels Stuck in Their Career",
                Scenario = "The client, a young professional, comes into the session feeling stuck in their career. They describe a sense of stagnation and dissatisfaction with their current job but are unsure about what direction to take or how to move forward. While they briefly mention feeling unfulfilled, they also express uncertainty about what’s really wrong or what they want for their future. The client feels anxious about making the wrong decision and seems hesitant to delve into their deeper feelings about the situation.",
                Challenge = "The therapist must use open questions to invite the client to explore their underlying feelings, motivations, and goals more fully. The open questions should help the client clarify what’s driving their dissatisfaction and hesitation, without leading them toward any particular conclusion.",
                Skills = "Open-ended inquiry: Asking questions that encourage deeper reflection and exploration without imposing the therapist’s assumptions. Exploration of emotions: Guiding the client to uncover underlying emotions and thoughts that may not be immediately obvious. Encouragement of client autonomy: Allowing the client to express themselves freely and decide for themselves what direction to take.",
                ClientInitialDialogue = "I just feel like I’m going nowhere in my career right now. I’ve been in the same role for a few years, and it’s not challenging me anymore. But I don’t really know what I want to do next, and the idea of changing jobs feels risky, especially since I don’t even know if I’d be happier anywhere else. I guess I should just stay where I am, but I’m not sure.",
                AssessmentCriteria = "The therapist’s ability to ask open questions will be assessed on how well the questions invite the client to explore their thoughts and emotions more deeply. Effective open questions should avoid suggesting a specific answer or direction, instead encouraging the client to reflect on their own experiences, clarify their feelings, and take ownership of their path forward.\r\n\r\nThe therapist’s success will be measured by their ability to:\r\n\r\nAvoid closed or leading questions.\r\nCreate opportunities for the client to elaborate on their feelings and uncertainties.\r\nPromote deeper insight into the client’s values, desires, and fears related to their career.",
            };

            var assessmentTask2LanguagePl = new AssessmentTaskLanguage
            {
                Language = Language.Polish,
                Name = "Pytania Otwarte: Klient Uwięziony w Karierze",
                Scenario = "Klient, młody profesjonalista, przychodzi na sesję, czując się uwięziony w swojej karierze. Opisuje poczucie stagnacji i niezadowolenia z obecnej pracy, ale nie jest pewien, w jakim kierunku pójść ani jak iść naprzód. Chociaż krótko wspomina o poczuciu niedosytu, wyraża również niepewność co do tego, co naprawdę jest nie tak lub czego chce w przyszłości. Klient czuje niepokój z powodu możliwości podjęcia złej decyzji i wydaje się niechętny do zagłębiania się w swoje głębsze uczucia na temat sytuacji.",
                Challenge = "Terapeuta musi użyć pytań otwartych, aby zachęcić klienta do pełniejszego zbadania swoich ukrytych uczuć, motywacji i celów. Pytania otwarte powinny pomóc klientowi wyjaśnić, co kieruje ich niezadowoleniem i wahaniem, bez prowadzenia ich do jakiegokolwiek określonego wniosku.",
                Skills = "Badanie otwartych pytań: Zadawanie pytań, które zachęcają do głębszej refleksji i eksploracji bez narzucania założeń terapeuty. Eksploracja emocji: Kierowanie klienta do odkrycia ukrytych emocji i myśli, które mogą nie być od razu oczywiste. Zachęcanie do autonomii klienta: Pozwolenie klientowi na swobodne wyrażanie siebie i decydowanie samemu, w jakim kierunku pójść.",
                ClientInitialDialogue = "",
                AssessmentCriteria = "",
            };

            var assessmentTask2 = new AssessmentTask();
            assessmentTask2.AssessmentTaskLanguages.Add(assessmentTask2LanguageEn);
            assessmentTask2.AssessmentTaskLanguages.Add(assessmentTask2LanguagePl);
            assessmentTask2.LengthInMinutes = 4;
            assessmentTask2.Order = 2;

            #endregion

            #region Task3

            var assessmentTask3LanguageEn = new AssessmentTaskLanguage
            {
                Language = Language.English,
                Name = "Reflection of Feelings: Client Navigating a Relationship Breakup",
                Scenario = "The client has recently ended a long-term romantic relationship and expresses a mix of sadness, anger, and confusion. They describe feeling betrayed by their partner’s decision to leave and are struggling to process the range of emotions they are experiencing. While the client talks about the practical aspects of the breakup, they avoid fully acknowledging or naming their emotions, often deflecting with details about what went wrong or what they could have done differently.",
                Challenge = "The therapist must reflect the client’s emotions, helping them become more aware of and connected to their feelings. The therapist’s reflection should capture the complexity of the client’s emotional state, validating both the sadness and the anger while encouraging the client to sit with and process those feelings.",
                Skills = "Accurate identification of emotions: Recognizing and verbalizing the feelings underlying the client’s words, even if the client hasn’t directly expressed them.\nValidation and normalization: Helping the client feel that their emotions are understood and accepted.\nEmotional exploration: Encouraging the client to further explore and reflect on their emotional experience.",
                ClientInitialDialogue = "It’s just been a mess lately. One minute, I’m angry that they didn’t even try to work things out, and the next, I’m sad and wondering if I could have done something to fix it. It’s like... I don’t even know how to feel about it anymore. I guess I should’ve seen it coming, but it still feels like a shock. Now, I just don’t know what to do or how to move on from here.",
                AssessmentCriteria = "The therapist’s ability to reflect the client’s feelings will be evaluated based on how accurately and empathetically they identify the emotions the client is experiencing. The reflection should acknowledge the full range of emotions and encourage the client to connect with those feelings more deeply.\n\nEffective reflection of feelings will be assessed on:\n\nWhether the therapist captures both primary and secondary emotions (e.g., anger and sadness).\nHow well the reflection helps the client feel understood and validated.\nWhether the reflection opens space for the client to explore their emotions further.",
            };

            var assessmentTask3LanguagePl = new AssessmentTaskLanguage
            {
                Language = Language.Polish,
                Name = "Odbicie Uczuć: Klient Przechodzący Rozstanie w Związku",
                Scenario = "Klient niedawno zakończył długotrwały związek romantyczny i wyraża mieszankę smutku, złości i dezorientacji. Opisuje uczucie zdrady z powodu decyzji partnera o odejściu i ma trudności z przetworzeniem zakresu emocji, które przeżywa. Podczas gdy klient mówi o praktycznych aspektach rozstania, unika pełnego uznania lub nazwania swoich emocji, często odwracając uwagę szczegółami dotyczącymi tego, co poszło nie tak lub co mogli zrobić inaczej.",
                Challenge = "Terape uta musi odzwierciedlić emocje klienta, pomagając mu stać się bardziej świadomym i połączonym z jego uczuciami. Odbicie emocji terapeuty powinno uchwycić złożoność stanu emocjonalnego klienta, potwierdzając zarówno smutek, jak i złość, jednocześnie zachęcając klienta do usiedzenia z tymi uczuciami i ich przetworzenia.",
                Skills = "Dokładna identyfikacja emocji: Rozpoznawanie i werbalizowanie uczuć leżących u podstaw słów klienta, nawet jeśli klient nie wyraził ich bezpośrednio.\nWalidacja i normalizacja: Pomaganie klientowi poczuć, że jego emocje są zrozumiane i akceptowane.\nEksploracja emocji: Zachęcanie klienta do dalszego eksplorowania i refleksji nad swoim doświadczeniem emocjonalnym.",
                ClientInitialDialogue = "",
                AssessmentCriteria = "",
            };

            var assessmentTask3 = new AssessmentTask();
            assessmentTask3.AssessmentTaskLanguages.Add(assessmentTask3LanguageEn);
            assessmentTask3.AssessmentTaskLanguages.Add(assessmentTask3LanguagePl);
            assessmentTask3.LengthInMinutes = 4;
            assessmentTask3.Order = 3;

            #endregion

            #region Task4

            var assessmentTask4LanguageEn = new AssessmentTaskLanguage
            {
                Language = Language.English,
                Name = "Alliance Rupture: Client Feels Unheard",
                Scenario = "The client feels that the therapist has not been listening or fully understanding their key concerns during the past few sessions. They express frustration and disappointment, indicating that they are unsure whether continuing therapy will be helpful. The client begins to withdraw emotionally, questioning the therapist's competence and doubting the therapeutic process, but their frustration is mixed with a desire for the therapist to understand them better.",
                Challenge = "The therapist must validate the client’s experience of feeling unheard, acknowledge the rupture in the therapeutic alliance, and demonstrate effective repair by responding with empathy and addressing the client’s core concerns. The therapist’s ability to navigate this emotionally charged moment is crucial for restoring trust and rebuilding the therapeutic relationship.",
                Skills = "Empathy: Accurately recognizing and responding to the client’s feelings of frustration and disappointment.\nRupture-repair responsiveness: Acknowledging and addressing the rupture in the relationship while rebuilding trust.\nVerbal fluency: Communicating clearly and effectively to repair the alliance.",
                ClientInitialDialogue = "I’ve been talking about this for weeks, and it just feels like you’re not getting it. I don’t think you’re really hearing what I’m saying. I’m starting to wonder if this is even helping at all. I come here to feel understood, and instead, I leave feeling more frustrated.",
                AssessmentCriteria = "The therapist’s ability to repair the rupture will be evaluated based on how well they acknowledge the client’s frustration and how effectively they work to restore the therapeutic alliance. Success will be measured by the therapist’s ability to:\n\nValidate the client’s experience of feeling unheard.\nAddress the rupture directly and with empathy.\nRebuild trust in the therapeutic relationship through effective communication and emotional responsiveness.",
            };

            var assessmentTask4LanguagePl = new AssessmentTaskLanguage
            {
                Language = Language.Polish,
                Name = "Zerwanie Sojuszu: Klient Czuje się Niesłyszany",
                Scenario = "Klient uważa, że terapeuta nie słuchał go ani nie rozumiał w pełni jego głównych obaw podczas ostatnich kilku sesji. Wyraża frustrację i rozczarowanie, wskazując, że nie jest pewien, czy kontynuowanie terapii będzie pomocne. Klient zaczyna się emocjonalnie wycofywać, kwestionując kompetencje terapeuty i wątpiąc w proces terapeutyczny, ale jego frustracja miesza się z pragnieniem, aby terapeuta lepiej go zrozumiał.",
                Challenge = "Terapeuta musi potwierdzić doświadczenie klienta, że czuje się niesłyszany, uznać zerwanie w sojuszu terapeutycznym i pokazać skuteczną naprawę, odpowiadając z empatią i zajmując się głównymi obawami klienta. Umiejętność terapeuty w nawigowaniu tym emocjonalnie naładowanym momentem jest kluczowa dla przywrócenia zaufania i odbudowy relacji terapeutycznej.",
                Skills = "Empatia: Dokładne rozpoznawanie i reagowanie na uczucia klienta związane z frustracją i rozczarowaniem.\nReagowanie na przerwę i naprawa: Uznanie i zajęcie się przerwą w relacji podczas odbudowy zaufania.\nPłynność werbalna: Komunikacja jasna i skuteczna w celu naprawy sojuszu.",
                ClientInitialDialogue = "",
                AssessmentCriteria = "",
            };

            var assessmentTask4 = new AssessmentTask();
            assessmentTask4.AssessmentTaskLanguages.Add(assessmentTask4LanguageEn);
            assessmentTask4.AssessmentTaskLanguages.Add(assessmentTask4LanguagePl);
            assessmentTask4.LengthInMinutes = 6;
            assessmentTask4.Order = 4;

            #endregion

            #region Task5

            var assessmentTask5LanguageEn = new AssessmentTaskLanguage
            {
                Language = Language.English,
                Name = "Aggressive/Hostile Client",
                Scenario = "The client reacts with hostility during the session, becoming defensive and accusatory. They blame the therapist for not providing any real help and suggest that the therapist might be part of the problem. The client’s frustration escalates, and their language becomes increasingly aggressive. Despite the hostility, there is an underlying sense of desperation and disappointment, indicating that the client is struggling to regain control of their emotions.",
                Challenge = "The therapist must remain calm and composed, regulating their own emotional response while addressing the client’s hostility. The therapist needs to de-escalate the situation by acknowledging the client’s frustration and guiding the conversation back to a constructive and collaborative space. Maintaining warmth and professionalism is critical while helping the client manage their intense emotional state.",
                Skills = "Emotional regulation: Maintaining calm and self-control while responding to client aggression.\nWarmth: Demonstrating non-defensive empathy and understanding in the face of hostility.\nConflict resolution: De-escalating the situation and refocusing the dialogue on productive problem-solving.",
                ClientInitialDialogue = "Honestly, this is getting ridiculous. Every week I come here, and nothing changes! I’m still stuck, and it feels like you’re not doing anything to help. Sometimes I wonder if you even understand what I’m going through. Maybe you’re just making it worse.",
                AssessmentCriteria = "The therapist’s ability to manage the client’s hostility and restore a productive dialogue will be evaluated based on:\n\nHow well they regulate their own emotional responses.\nTheir ability to remain warm and non-defensive despite the client’s aggression.\nTheir success in de-escalating the conflict and guiding the client toward a more constructive conversation.",
            };

            var assessmentTask5LanguagePl = new AssessmentTaskLanguage
            {
                Language = Language.Polish,
                Name = "Agresywna/Wroga Reakcja Klienta",
                Scenario = "Klient reaguje z wrogością podczas sesji, stając się obronny i oskarżycielski. Obwinia terapeutę o brak rzeczywistej pomocy i sugeruje, że terapeuta może być częścią problemu. Frustracja klienta się nasila, a jego język staje się coraz bardziej agresywny. Pomimo wrogości, istnieje ukryte poczucie desperacji i rozczarowania, wskazujące na to, że klient boryka się z próbą odzyskania kontroli nad swoimi emocjami.",
                Challenge = "Terapeuta musi zachować spokój i opanowanie, regulując własną reakcję emocjonalną w odpowiedzi na wrogość klienta. Terapeuta musi załagodzić sytuację, uznając frustrację klienta i kierując rozmowę z powrotem w konstruktywne i współpracujące miejsce. Zachowanie ciepła i profesjonalizmu jest kluczowe podczas pomagania klientowi w radzeniu sobie z jego intensywnym stanem emocjonalnym.",
                Skills = "Regulacja emocji: Zachowanie spokoju i samokontroli podczas reagowania na agresję klienta.\nCiepło: Wykazywanie empatii i zrozumienia bez defensywy w obliczu wrogości.\nRozwiązywanie konfliktów: Załagodzenie sytuacji i skupienie rozmowy na produktywnym rozwiązywaniu problemów.",
                ClientInitialDialogue = "",
                AssessmentCriteria = "",
            };

            var assessmentTask5 = new AssessmentTask();
            assessmentTask5.AssessmentTaskLanguages.Add(assessmentTask5LanguageEn);
            assessmentTask5.AssessmentTaskLanguages.Add(assessmentTask5LanguagePl);
            assessmentTask5.LengthInMinutes = 6;
            assessmentTask5.Order = 5;

            #endregion

            #region Task6

            var assessmentTask6LanguageEn = new AssessmentTaskLanguage
            {
                Language = Language.English,
                Name = "Client Dissatisfaction with Progress",
                Scenario = "The client expresses frustration and disappointment, feeling that they have not made enough progress in therapy. They begin to question whether therapy is even worthwhile, voicing concerns that it may be a waste of time. The client feels stuck and is unsure if continuing will lead to any meaningful improvement. Although their frustration is evident, there is also an underlying hope that things could still change if they see progress.",
                Challenge = "The therapist must tactfully acknowledge the client’s frustration while reframing their perspective on progress. Without invalidating the client’s feelings, the therapist should highlight any small victories the client may have overlooked, instill hope for future progress, and reinforce the therapeutic alliance.",
                Skills = "Hopefulness: Encouraging a sense of optimism for future growth and change.\nPersuasiveness: Convincing the client to see value in the therapeutic process without dismissing their concerns.\nTherapeutic alliance maintenance: Strengthening the bond with the client during a moment of doubt and frustration.\n",
                ClientInitialDialogue = "I don’t know if this is working anymore. I feel like I’m in the same place I was months ago, and nothing’s really changed. What’s the point if I’m not seeing results? Maybe therapy just isn’t for me, or maybe I’m doing something wrong.",
                AssessmentCriteria = "The therapist’s ability to reframe the client’s frustration and highlight progress will be evaluated based on:\n\nTheir ability to acknowledge and validate the client’s dissatisfaction without minimizing it.\nHow effectively they instill hope and optimism for future progress.\nWhether they strengthen the therapeutic alliance by maintaining rapport and trust in the face of the client’s doubts.",
            };

            var assessmentTask6LanguagePl = new AssessmentTaskLanguage
            {
                Language = Language.Polish,
                Name = "Niezadowolenie Klienta z Postępów",
                Scenario = "Klient wyraża frustrację i rozczarowanie, czując, że nie dokonał wystarczającego postępu w terapii. Zaczyna kwestionować, czy terapia ma sens, wyrażając obawy, że może to być strata czasu. Klient czuje się uwięziony i nie jest pewien, czy kontynuowanie terapii doprowadzi do jakiejkolwiek znaczącej poprawy. Chociaż jego frustracja jest oczywista, istnieje również ukryta nadzieja, że rzeczy mogą się zmienić, jeśli zobaczy postępy.",
                Challenge = "Terapeuta musi taktownie uznać frustrację klienta, jednocześnie zmieniając jego perspektywę na postęp. Bez negowania uczuć klienta, terapeuta powinien podkreślić jakiekolwiek małe zwycięstwa, które klient mógł przeoczyć, zaszczepić nadzieję na przyszły postęp i wzmocnić sojusz terapeutyczny.",
                Skills = "Optymizm: Zachęcanie do optymizmu wobec przyszłego wzrostu i zmian.\nPerswazyjność: Przekonanie klienta, aby dostrzegł wartość procesu terapeutycznego, nie bagatelizując jego obaw.\nUtrzymywanie sojuszu terapeutycznego: Wzmacnianie więzi z klientem w momencie wątpliwości i frustracji klienta.",
                ClientInitialDialogue = "",
                AssessmentCriteria = "",
            };

            var assessmentTask6 = new AssessmentTask();
            assessmentTask6.AssessmentTaskLanguages.Add(assessmentTask6LanguageEn);
            assessmentTask6.AssessmentTaskLanguages.Add(assessmentTask6LanguagePl);
            assessmentTask6.LengthInMinutes = 6;
            assessmentTask6.Order = 6;

            #endregion

            var assessmentLanguageEn = new AssessmentLanguage
            {
                Language = Language.English,
                Name = "TheraSim Interactive Psychotherapy Skills Assessment",
                Instructions = "### Before You Begin\n1. **Setup:**\n   - Ensure a quiet and comfortable environment for the assessment.\n   - Test your microphone and speakers to confirm they are working correctly.\n   - Have a reliable internet connection to avoid interruptions.\n2. **Familiarization:**\n   - Review the format of the client persona description, which includes demographics, presenting issues, and relevant history. Understanding this information is crucial for effectively engaging with the simulated client.\n   - Familiarize yourself with the interface, including how to start, pause, and stop the assessment tasks.",
                Description =
                    "The TheraSim Interactive Psychotherapy Skills Assessment is an AI-driven evaluation tool designed " +
                    "to assess psychotherapists' interpersonal effectiveness during challenging therapy moments. " +
                    "The assessment evaluates therapists' core skills, focusing on how they handle emotionally charged " +
                    "scenarios such as alliance ruptures or client volatility. Using standardized interactive client simulations, " +
                    "which replicate real-life conversations in a psychotherapy setting, the assessment provides a controlled " +
                    "yet dynamic environment to measure the therapist's ability to respond to difficult situations " +
                    "across several key areas: empathy, verbal fluency, rupture-repair capability, and more.\n" +
                    "The outcome of this assessment includes a quantitative score based on expert criteria and qualitative " +
                    "feedback highlighting the therapist's strengths and areas for improvement, providing comprehensive " +
                    "insights into their interpersonal effectiveness."

            };

            var assessmentLanguagePl = new AssessmentLanguage
            {
                Language = Language.Polish,
                Name = "Interaktywny Test Umiejętności Psychoterapeutycznych TheraSim – wersja beta",
                Instructions = "### Instrukcja wykonania testu:\r\n\r\n- Test trwa około 30 minut. Upewnij się, że przebywasz w cichym i komfortowym miejscu, w którym można swobodnie prowadzić konwersację.\r\n\r\n- Przetestuj mikrofon i głośniki, aby upewnić się, że działają prawidłowo. Jeśli wolisz, możesz korzystać ze słuchawek.\r\n\r\n- W trakcie całego zadania, Twój komputer powinien być podłączony do Internetu. Zadbaj o stabilne połączenie internetowe, aby uniknąć przerw.\r\n\r\n- Twoim zadaniem będzie wykonanie 6 modułów. Trzy z nich dotyczą podstawowych umiejętności (parafraza, pytania otwarte, odzwierciedlenie uczuć) i trwają po 4 minuty. Trzy pozostałe trwają po 6 minut i sprawdzają złożone umiejętności zareagowania na trudności w relacji terapeutycznej (klient czuje się niesłuchany, pacjent jest wrogi wobec terapeuty oraz pacjent jest niezadowolony z postępów terapii).\r\n\r\n- Na  początku każdego zadania otrzymasz szczegółową instrukcję.\r\n\r\n- Po zakończeniu jednego zadania, przejdź od razu do kolejnego zadania.\r\n\r\n- Aby przetestować formułę zadania, w pierwszej kolejności będzie zadanie testowe, które nie będzie uwzględniane w ocenie.\r\n\r\n- Jeśli jesteś gotowa/y, naciśnij przycisk START",
                Description = "Interaktywny Test Umiejętności Psychoterapeutycznych (ITUP) TheraSim to narzędzie oparte na sztucznej inteligencji. Narzędzie to bada podstawowe umiejętności psychoterapeutyczne (takie jak odzwierciedlenie emocji i parafraza) oraz złożone umiejętności (takie jak zdolność do budowania przymierza terapeutycznego). Dzięki ustandaryzowanym, interaktywnym symulacjom klientów, które odzwierciedlają realne rozmowy prowadzone w warunkach psychoterapeutycznych, ocena zapewnia kontrolowane, a jednocześnie dynamiczne środowisko.\r\n\r\nUwaga: to jest wersja beta programu, co oznacza, że mogą się pojawić pewne błędy w oprogramowaniu. W przypadku pojawiania się takich niedociągnięć, staraj się odpowiadać, jak uważasz za trafne, gdy błędy te uniemożliwiają przeprowadzenie testu, po prostu poczekaj do jego zakończenia i przejdź do kolejnego modułu. Jeśli program nie reaguje, spróbuj ponownie za jakiś czas. Na końcu testu będzie również możliwość zgłoszenia błędów."
            };

            var assessment = new Assessment();
            assessment.AssessmentLanguages.Add(assessmentLanguageEn);
            assessment.AssessmentLanguages.Add(assessmentLanguagePl);
            assessment.AssessmentTasks.Add(assessmentTask0);
            assessment.AssessmentTasks.Add(assessmentTask1);
            assessment.AssessmentTasks.Add(assessmentTask2);
            assessment.AssessmentTasks.Add(assessmentTask3);
            assessment.AssessmentTasks.Add(assessmentTask4);
            assessment.AssessmentTasks.Add(assessmentTask5);
            assessment.AssessmentTasks.Add(assessmentTask6);

            _context.Assessments.Add(assessment);

            await _context.SaveChangesAsync();
        }
    }
}
