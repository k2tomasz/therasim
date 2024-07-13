using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.Feedbacks.Queries.GetSessionFeedbacks;

public record GetSessionFeedbacksQuery : IRequest<IList<FeedbackDto>>
{
    public Guid SessionId { get; init; }
}

public class GetSessionFeedbacksQueryValidator : AbstractValidator<GetSessionFeedbacksQuery>
{
    public GetSessionFeedbacksQueryValidator()
    {
    }
}

public class GetSessionFeedbacksQueryHandler : IRequestHandler<GetSessionFeedbacksQuery, IList<FeedbackDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSessionFeedbacksQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<FeedbackDto>> Handle(GetSessionFeedbacksQuery request, CancellationToken cancellationToken)
    {
        var feedbacks = await _context.Feedbacks
            .Where(f => f.SessionId == request.SessionId)
            .ProjectTo<FeedbackDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return feedbacks;
    }
}
