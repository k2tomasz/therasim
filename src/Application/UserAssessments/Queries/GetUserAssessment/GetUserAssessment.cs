using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.UserAssessments.Queries.GetUserAssessment;

public record GetUserAssessmentQuery(Guid UserAssessmentId) : IRequest<UserAssessmentDetailsDto>;
public class GetAssessmentQueryValidator : AbstractValidator<GetUserAssessmentQuery>
{
    public GetAssessmentQueryValidator()
    {
    }
}

public class GetUserAssessmentQueryHandler : IRequestHandler<GetUserAssessmentQuery, UserAssessmentDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserAssessmentQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserAssessmentDetailsDto> Handle(GetUserAssessmentQuery request, CancellationToken cancellationToken)
    {
        var userAssessment = await _context.UserAssessments
            .Include(a => a.Assessment.AssessmentLanguages.Where(l=>l.Language == a.Language))
            .Include(ua => ua.UserAssessmentTasks.Where(uat => uat.EndDate == null).OrderBy(uat => uat.Order).Take(1))
            .Where(a => a.Id == request.UserAssessmentId)
            .ProjectTo<UserAssessmentDetailsDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);

        if (userAssessment == null) throw new NotFoundException(request.UserAssessmentId.ToString(), "UserAssessment");

        return userAssessment;
    }
}
