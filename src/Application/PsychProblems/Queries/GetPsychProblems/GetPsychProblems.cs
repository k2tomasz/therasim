using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.PsychProblems.Queries.GetPsychProblems;

public record GetPsychProblemsQuery : IRequest<IList<PsychProblemDto>>
{
}

public class GetPsychProblemsQueryValidator : AbstractValidator<GetPsychProblemsQuery>
{
    public GetPsychProblemsQueryValidator()
    {
    }
}

public class GetPsychProblemsQueryHandler : IRequestHandler<GetPsychProblemsQuery, IList<PsychProblemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPsychProblemsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<IList<PsychProblemDto>> Handle(GetPsychProblemsQuery request, CancellationToken cancellationToken)
    {
        // var psychProblems = await _context.PsychProblems
        //     .ProjectTo<PsychProblemDto>(_mapper.ConfigurationProvider)
        //     .ToListAsync(cancellationToken);

        var psychProblems = new List<PsychProblemDto>
        {
            new PsychProblemDto { Name = "Depression", Id = Guid.NewGuid() }
        };

        return Task.FromResult<IList<PsychProblemDto>>(psychProblems);
    }
}
