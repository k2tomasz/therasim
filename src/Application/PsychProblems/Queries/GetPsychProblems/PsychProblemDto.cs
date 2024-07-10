using Therasim.Domain.Entities;

namespace Therasim.Application.PsychProblems.Queries.GetPsychProblems;

public class PsychProblemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<PsychProblem, PsychProblemDto>();
        }
    }
}
