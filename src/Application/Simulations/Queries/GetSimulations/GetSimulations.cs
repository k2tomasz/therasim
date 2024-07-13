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

    public async Task<IList<SimulationDto>> Handle(GetSimulationsQuery request, CancellationToken cancellationToken)
    {
        var simulations = await _context.Simulations.Include(x=>x.Sessions.Where(x=>x.IsActive))
            .Where(x => x.UserId == request.UserId)
            .ProjectTo<SimulationDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return simulations;
    }
}
