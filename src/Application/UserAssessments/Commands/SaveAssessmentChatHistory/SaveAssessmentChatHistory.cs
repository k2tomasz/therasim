using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.Assessments.Commands.SaveAssessmentChatHistory;

public record SaveAssessmentChatHistoryCommand : IRequest<bool>
{
    public Guid AssessmentId { get; init; }
    public string ChatHistory { get; init; } = string.Empty;
}

public class SaveAssessmentChatHistoryCommandValidator : AbstractValidator<SaveAssessmentChatHistoryCommand>
{
    public SaveAssessmentChatHistoryCommandValidator()
    {
    }
}

public class SaveAssessmentChatHistoryCommandHandler : IRequestHandler<SaveAssessmentChatHistoryCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public SaveAssessmentChatHistoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(SaveAssessmentChatHistoryCommand request, CancellationToken cancellationToken)
    {
        var assessment = await _context.Assessments.FindAsync(request.AssessmentId);

        if (assessment == null) throw new NotFoundException(request.AssessmentId.ToString(), "Assessment");

        assessment.ChatHistory = request.ChatHistory;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
