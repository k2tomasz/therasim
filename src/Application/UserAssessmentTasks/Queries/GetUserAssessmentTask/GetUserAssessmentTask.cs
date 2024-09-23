using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTask;

public record GetUserAssessmentTaskQuery(Guid UserAssessmentTaskId) : IRequest<UserAssessmentTaskDto>;

public class GetUserAssessmentTaskQueryValidator : AbstractValidator<GetUserAssessmentTaskQuery>
{
    public GetUserAssessmentTaskQueryValidator()
    {
    }
}

public class GetUserAssessmentTaskQueryHandler : IRequestHandler<GetUserAssessmentTaskQuery, UserAssessmentTaskDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserAssessmentTaskQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserAssessmentTaskDto> Handle(GetUserAssessmentTaskQuery request, CancellationToken cancellationToken)
    {
        var userAssessmentTask = await _context.UserAssessmentTasks
            .Include(a => a.AssessmentTask)
            .Where(a => a.Id == request.UserAssessmentTaskId)
            .ProjectTo<UserAssessmentTaskDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);

        if (userAssessmentTask == null) throw new NotFoundException(request.UserAssessmentTaskId.ToString(), "UserAssessmentTask");

        return userAssessmentTask;
    }
}
