using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.Messages.Queries.GetSessionMessages;

public record GetSessionMessagesQuery(Guid SessionId) : IRequest<IList<MessageDto>>;

public class GetSessionMessagesQueryValidator : AbstractValidator<GetSessionMessagesQuery>
{
    public GetSessionMessagesQueryValidator()
    {
    }
}

public class GetSessionMessagesQueryHandler : IRequestHandler<GetSessionMessagesQuery, IList<MessageDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSessionMessagesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<MessageDto>> Handle(GetSessionMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await _context.Messages
            .Where(x=>x.SessionId == request.SessionId)
            .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return messages;
    }
}
