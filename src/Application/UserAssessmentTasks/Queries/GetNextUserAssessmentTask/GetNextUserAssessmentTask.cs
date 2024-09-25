using Therasim.Application.Common.Interfaces;
using Therasim.Domain.Entities;

namespace Therasim.Application.UserAssessmentTasks.Queries.GetNextUserAssessmentTask;

public record GetNextUserAssessmentTaskQuery(Guid UserAssessmentId) : IRequest<Guid>
{
}

public class GetNextUserAssessmentTaskQueryValidator : AbstractValidator<GetNextUserAssessmentTaskQuery>
{
    public GetNextUserAssessmentTaskQueryValidator()
    {
    }
}

public class GetNextUserAssessmentTaskQueryHandler : IRequestHandler<GetNextUserAssessmentTaskQuery, Guid>
{
    private readonly IApplicationDbContext _context;

    public GetNextUserAssessmentTaskQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(GetNextUserAssessmentTaskQuery request, CancellationToken cancellationToken)
    {
        var userAssessmentTaskId = await _context.UserAssessmentTasks
            .Where(uat => uat.UserAssessmentId == request.UserAssessmentId && uat.EndDate == null)
            .OrderBy(uat => uat.Order)
            .Select(uat=>uat.Id)
            .FirstOrDefaultAsync(cancellationToken);

        Guard.Against.Null(userAssessmentTaskId, nameof(UserAssessmentTask));

        return userAssessmentTaskId;
    }
}
