using Therasim.Application.Common.Interfaces;
using Therasim.Domain.Enums;

namespace Therasim.Application.Assessments.Queries.GetAssessment;

public record GetAssessmentQuery(Guid AssessmentId, Language Language) : IRequest<AssessmentDetailsDto>
{
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
            .Include(a => a.AssessmentLanguages.Where(l => l.Language == request.Language))
            .Where(a => a.Id == request.AssessmentId)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.Null(assessment, nameof(assessment));

        return _mapper.Map<AssessmentDetailsDto>(assessment);
    }
}
