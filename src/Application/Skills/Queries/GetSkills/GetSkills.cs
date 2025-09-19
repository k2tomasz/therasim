using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.Skills.Queries.GetSkills;

public record GetSkillsQuery : IRequest<IList<SkillDto>>
{
}

public class GetSkillsQueryValidator : AbstractValidator<GetSkillsQuery>
{
    public GetSkillsQueryValidator()
    {
    }
}

public class GetSkillsQueryHandler : IRequestHandler<GetSkillsQuery, IList<SkillDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSkillsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<SkillDto>> Handle(GetSkillsQuery request, CancellationToken cancellationToken)
    {
        var skills = await _context.Skills
            .ProjectTo<SkillDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return skills;
    }
}
