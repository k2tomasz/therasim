using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTasksFeedback;

public record GetUserAssessmentTasksFeedbackQuery(Guid UserAssessmentId) : IRequest<IList<UserAssessmentTaskFeedbackDto>>;

public class GetUserAssessmentTasksFeedbackQueryValidator : AbstractValidator<GetUserAssessmentTasksFeedbackQuery>
{
    public GetUserAssessmentTasksFeedbackQueryValidator()
    {
    }
}

public class GetUserAssessmentTasksFeedbackQueryHandler : IRequestHandler<GetUserAssessmentTasksFeedbackQuery, IList<UserAssessmentTaskFeedbackDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserAssessmentTasksFeedbackQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<UserAssessmentTaskFeedbackDto>> Handle(GetUserAssessmentTasksFeedbackQuery request, CancellationToken cancellationToken)
    {
        var userAssessmentTasks = await _context.UserAssessmentTasks
            .Include(uat => uat.AssessmentTask.AssessmentTaskLanguages)
            .Where(uat => uat.UserAssessmentId == request.UserAssessmentId && uat.Order > 0)
            .ToListAsync(cancellationToken);




        return _mapper.Map<List<UserAssessmentTaskFeedbackDto>>(userAssessmentTasks);
    }
}
