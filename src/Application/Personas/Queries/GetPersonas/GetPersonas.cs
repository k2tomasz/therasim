using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.Personas.Queries.GetPersonas;

public record GetPersonasQuery : IRequest<IList<PersonaDto>>
{
}

public class GetPersonasQueryValidator : AbstractValidator<GetPersonasQuery>
{
    public GetPersonasQueryValidator()
    {
    }
}

public class GetPersonasQueryHandler : IRequestHandler<GetPersonasQuery, IList<PersonaDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPersonasQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<PersonaDto>> Handle(GetPersonasQuery request, CancellationToken cancellationToken)
    {
        var personas = await _context.Personas
            .ProjectTo<PersonaDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return personas;
    }
}
