using Therasim.Application.Common.Interfaces;
using Therasim.Application.UserAssessments.Queries.GetCompletedAssessments;

namespace Therasim.Application.UserAssessments.Queries.GetUserAssessments;

public record GetCompletedAssessmentsQuery() : IRequest<IList<CompletedAssessmentDto>>;

public class GetCompletedAssessmentsQueryValidator : AbstractValidator<GetCompletedAssessmentsQuery>
{
    public GetCompletedAssessmentsQueryValidator()
    {
    }
}

public class GetCompletedAssessmentsQueryHandler : IRequestHandler<GetCompletedAssessmentsQuery, IList<CompletedAssessmentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCompletedAssessmentsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<CompletedAssessmentDto>> Handle(GetCompletedAssessmentsQuery request, CancellationToken cancellationToken)
    {
        var assessments = await _context.UserAssessments
            .Include(ua => ua.Assessment.AssessmentLanguages)
            .Include(ua=>ua.UserAssessmentTasks.Where(uat=>uat.Order > 0).OrderBy(uat=>uat.Order))
            .Where(a => a.UserAssessmentTasks.All(uat=>uat.EndDate != null))
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<CompletedAssessmentDto>>(assessments);
    }
}
