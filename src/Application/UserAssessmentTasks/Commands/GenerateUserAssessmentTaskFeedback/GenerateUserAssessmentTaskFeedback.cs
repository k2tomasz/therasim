using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;
using System.Text.Json;
using Therasim.Application.Common.Interfaces;
using Therasim.Domain.Enums;

namespace Therasim.Application.UserAssessmentTasks.Commands.GenerateUserAssessmentTaskFeedback;

public record GenerateUserAssessmentTaskFeedbackCommand(Guid UserAssessmentTaskId) : IRequest<string>;

public class GenerateUserAssessmentTaskFeedbackCommandValidator : AbstractValidator<GenerateUserAssessmentTaskFeedbackCommand>
{
    public GenerateUserAssessmentTaskFeedbackCommandValidator()
    {
    }
}

public class GenerateUserAssessmentTaskFeedbackCommandHandler : IRequestHandler<GenerateUserAssessmentTaskFeedbackCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly ILanguageModelService _languageModelService;

    public GenerateUserAssessmentTaskFeedbackCommandHandler(IApplicationDbContext context, ILanguageModelService languageModelService)
    {
        _context = context;
        _languageModelService = languageModelService;
    }

    public async Task<string> Handle(GenerateUserAssessmentTaskFeedbackCommand request, CancellationToken cancellationToken)
    {
        var userAssessmentTask = await _context.UserAssessmentTasks
            .Include(x => x.AssessmentTask.AssessmentTaskLanguages)
            .Where(x => x.Id == request.UserAssessmentTaskId)
            .SingleOrDefaultAsync(cancellationToken);

        if (userAssessmentTask == null) throw new NotFoundException(request.UserAssessmentTaskId.ToString(), "UserAssessmentTask");

        if (userAssessmentTask.ChatHistory == null) return string.Empty;

        var transcriptStringBuilder = new StringBuilder();
       
        var deserializedHistory = JsonSerializer.Deserialize<ChatHistory>(userAssessmentTask.ChatHistory);
        if (deserializedHistory == null) return string.Empty;

        var therapistPrefix = userAssessmentTask.Language == Language.English ? "Therapist: " : "Terapeuta: ";
        var patientPrefix = userAssessmentTask.Language == Language.English ? "Client: " : "Klient: ";
        
        foreach (var chatMessage in deserializedHistory)
        {
            if (chatMessage.Role == AuthorRole.User)
            {
                transcriptStringBuilder.AppendLine($"{therapistPrefix}{chatMessage.Content}");
            }
            else if (chatMessage.Role == AuthorRole.Assistant)
            {
                transcriptStringBuilder.AppendLine($"{patientPrefix}{chatMessage.Content}");
            }
        }

        var taskDetails = userAssessmentTask.AssessmentTask.AssessmentTaskLanguages.Where(x=>x.Language == userAssessmentTask.Language).First();

        var feedback = await _languageModelService.GenerateAssessmentTaskFeedback(
            transcriptStringBuilder.ToString(), 
            taskDetails.Scenario, 
            taskDetails.Challenge, 
            taskDetails.Skills, 
            taskDetails.AssessmentCriteria, 
            taskDetails.FeedbackSystemPrompt,
            taskDetails.Language);

        userAssessmentTask.Feedback = feedback;
        await _context.SaveChangesAsync(cancellationToken);

        return feedback;
    }
}
