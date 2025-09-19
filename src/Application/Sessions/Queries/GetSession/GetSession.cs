using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.Sessions.Queries.GetSession;

public record GetSessionQuery(Guid SessionId) : IRequest<SessionDto>;

public class GetSessionQueryValidator : AbstractValidator<GetSessionQuery>
{
    public GetSessionQueryValidator()
    {
    }
}

public class GetSessionQueryHandler : IRequestHandler<GetSessionQuery, SessionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSessionQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SessionDto> Handle(GetSessionQuery request, CancellationToken cancellationToken)
    {
        var session = await _context.Sessions
            .Include(x => x.Simulation)
            .Where(x => x.Id == request.SessionId)
            .ProjectTo<SessionDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);

        if (session == null)
        {
            throw new NotFoundException(request.SessionId.ToString(), "Session");
        }

        return session;
    }
}
