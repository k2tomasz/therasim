using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.Assessments.Queries.GetAssessment;

public record GetAssessmentQuery : IRequest<AssessmentDetailsDto>
{
    public Guid AssessmentId { get; init; }
}

public class GetAssessmentQueryValidator : AbstractValidator<GetAssessmentQuery>
{
    public GetAssessmentQueryValidator()
    {
    }
}

public class GetAssessmentQueryHandler : IRequestHandler<GetAssessmentQuery, AssessmentDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAssessmentQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AssessmentDetailsDto> Handle(GetAssessmentQuery request, CancellationToken cancellationToken)
    {
        var assessment = await _context.Assessments
            .Include(a => a.Skill)
            .Where(a => a.Id == request.AssessmentId)
            .ProjectTo<AssessmentDetailsDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);

        if (assessment == null) throw new NotFoundException(request.AssessmentId.ToString(), "Assessment");

        return assessment;
    }
}
