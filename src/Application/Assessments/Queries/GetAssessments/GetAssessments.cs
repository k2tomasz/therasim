using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.Assessments.Queries.GetAssessments;

public record GetAssessmentsQuery : IRequest<IList<AssessmentDto>>
{
}

public class GetAssessmentsQueryValidator : AbstractValidator<GetAssessmentsQuery>
{
    public GetAssessmentsQueryValidator()
    {
    }
}

public class GetAssessmentsQueryHandler : IRequestHandler<GetAssessmentsQuery, IList<AssessmentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAssessmentsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<AssessmentDto>> Handle(GetAssessmentsQuery request, CancellationToken cancellationToken)
    {
        var assessments = await _context.Assessments
            .Include(a => a.AssessmentLanguages)
            .ProjectTo<AssessmentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);


        return assessments;
    }
}
