using Therasim.Application.Common.Interfaces;
namespace Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTask;

public record GetUserAssessmentTaskQuery(Guid UserAssessmentTaskId) : IRequest<UserAssessmentTaskDetailsDto>;

public class GetUserAssessmentTaskQueryValidator : AbstractValidator<GetUserAssessmentTaskQuery>
{
    public GetUserAssessmentTaskQueryValidator()
    {
    }
}

public class GetUserAssessmentTaskQueryHandler : IRequestHandler<GetUserAssessmentTaskQuery, UserAssessmentTaskDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserAssessmentTaskQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserAssessmentTaskDetailsDto> Handle(GetUserAssessmentTaskQuery request, CancellationToken cancellationToken)
    {
        var userAssessmentTask = await _context.UserAssessmentTasks
            .Include(uat => uat.AssessmentTask.AssessmentTaskLanguages.Where(atl=> atl.Language == uat.Language))
            .Where(uat => uat.Id == request.UserAssessmentTaskId)
            .ProjectTo<UserAssessmentTaskDetailsDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.Null(userAssessmentTask, nameof(userAssessmentTask));

        return userAssessmentTask;
    }

}
