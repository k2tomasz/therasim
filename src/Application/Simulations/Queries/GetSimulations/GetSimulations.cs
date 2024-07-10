using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.Simulations.Queries.GetSimulations;

public record GetSimulationsQuery : IRequest<IList<SimulationDto>>
{
    public string UserId { get; init; } = null!;
}

public class GetSimulationsQueryValidator : AbstractValidator<GetSimulationsQuery>
{
    public GetSimulationsQueryValidator()
    {
    }
}

public class GetSimulationsQueryHandler : IRequestHandler<GetSimulationsQuery, IList<SimulationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSimulationsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<IList<SimulationDto>> Handle(GetSimulationsQuery request, CancellationToken cancellationToken)
    {
        // var simulations = await _context.simulations
        //     .Where(x => x.UserId == request.UserId)
        //     .ProjectTo<SimulationDto>(_mapper.ConfigurationProvider)
        //     .ToListAsync(cancellationToken);

        var simulations = new List<SimulationDto>
        {
            new SimulationDto(),
            new SimulationDto
            {
                Persona = "Emily, 65, recently retired", Skill = "Empathy", Problem = "Anxiety", FeedbackType = "Limited"
            }
        };

        return Task.FromResult<IList<SimulationDto>>(simulations);
    }
}
