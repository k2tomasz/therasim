using Therasim.Domain.Entities;

namespace Therasim.Application.Problems.Queries.GetProblems;

public class ProblemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Problem, ProblemDto>();
        }
    }
}
