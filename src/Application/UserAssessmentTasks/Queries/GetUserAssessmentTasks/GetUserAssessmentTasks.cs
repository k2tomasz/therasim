using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTasks;

public record GetUserAssessmentTasksQuery(Guid UserAssessmentId) : IRequest<IList<UserAssessmentTaskDto>>;

public class GetUserAssessmentTasksQueryValidator : AbstractValidator<GetUserAssessmentTasksQuery>
{
    public GetUserAssessmentTasksQueryValidator()
    {
    }
}

public class GetUserAssessmentTasksQueryHandler : IRequestHandler<GetUserAssessmentTasksQuery, IList<UserAssessmentTaskDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserAssessmentTasksQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<UserAssessmentTaskDto>> Handle(GetUserAssessmentTasksQuery request, CancellationToken cancellationToken)
    {
        var userAssessmentTasks = await _context.UserAssessmentTasks
            .Include(uat => uat.AssessmentTask.AssessmentTaskLanguages)
            .Where(uat => uat.UserAssessmentId == request.UserAssessmentId)
            .ToListAsync(cancellationToken);




        return _mapper.Map<List<UserAssessmentTaskDto>>(userAssessmentTasks);
    }
}
