using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.Problems.Queries.GetProblems;

public record GetProblemsQuery : IRequest<IList<ProblemDto>>
{
}

public class GetProblemsQueryValidator : AbstractValidator<GetProblemsQuery>
{
    public GetProblemsQueryValidator()
    {
    }
}

public class GetProblemsQueryHandler : IRequestHandler<GetProblemsQuery, IList<ProblemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProblemsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<ProblemDto>> Handle(GetProblemsQuery request, CancellationToken cancellationToken)
    {
        var psychProblems = await _context.Problems
            .ProjectTo<ProblemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return psychProblems;
    }
}
