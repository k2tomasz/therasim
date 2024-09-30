using Therasim.Application.Common.Interfaces;
using Therasim.Domain.Enums;

namespace Therasim.Application.AssessmentTasks.Queries.GetAssessmentTasks;

public record GetAssessmentTasksQuery(Guid AssessmentId, Language Language) : IRequest<IList<AssessmentTaskDto>>;

public class GetAssessmentTasksQueryValidator : AbstractValidator<GetAssessmentTasksQuery>
{
    public GetAssessmentTasksQueryValidator()
    {
    }
}

public class GetAssessmentTasksQueryHandler : IRequestHandler<GetAssessmentTasksQuery, IList<AssessmentTaskDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAssessmentTasksQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<AssessmentTaskDto>> Handle(GetAssessmentTasksQuery request, CancellationToken cancellationToken)
    {
        var assessmentTasks = await _context.AssessmentTasks
            .Include(at => at.AssessmentTaskLanguages.Where(l => l.Language == request.Language))
            .Where(at => at.AssessmentId == request.AssessmentId)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IList<AssessmentTaskDto>>(assessmentTasks);
    }
}
