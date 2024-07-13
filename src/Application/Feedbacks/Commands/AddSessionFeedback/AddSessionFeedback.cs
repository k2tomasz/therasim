using Therasim.Application.Common.Interfaces;
using Therasim.Domain.Entities;

namespace Therasim.Application.Feedbacks.Commands.AddSessionFeedback;

public record AddSessionFeedbackCommand : IRequest<Guid>
{
    public Guid SessionId { get; set; }
    public string Content { get; set; } = null!;
    public string Message { get; set; } = null!;
}

public class AddSessionFeedbackCommandValidator : AbstractValidator<AddSessionFeedbackCommand>
{
    public AddSessionFeedbackCommandValidator()
    {
    }
}

public class AddSessionFeedbackCommandHandler : IRequestHandler<AddSessionFeedbackCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public AddSessionFeedbackCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(AddSessionFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = new Feedback
        {
            Content = request.Content,
            Message = request.Message,
            SessionId = request.SessionId
        };

        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync(cancellationToken);

        return feedback.Id;
    }
}
