using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.UserAssessments.Queries.GetUserAssessments;

public record GetUserAssessmentsQuery(string UserId) : IRequest<IList<UserAssessmentDto>>;

public class GetAssessmentsQueryValidator : AbstractValidator<GetUserAssessmentsQuery>
{
    public GetAssessmentsQueryValidator()
    {
    }
}

public class GetAssessmentsQueryHandler : IRequestHandler<GetUserAssessmentsQuery, IList<UserAssessmentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAssessmentsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<UserAssessmentDto>> Handle(GetUserAssessmentsQuery request, CancellationToken cancellationToken)
    {
        var assessments = await _context.UserAssessments
            .Include(ua => ua.Assessment.AssessmentLanguages)
            .Include(ua=>ua.UserAssessmentTasks.Where(uat=>uat.EndDate == null).OrderBy(uat=>uat.Order).Take(1))
            .Where(a => a.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<UserAssessmentDto>>(assessments);
    }
}
