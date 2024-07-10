using Therasim.Application.PsychProblems.Queries.GetPsychProblems;

namespace Therasim.Web.Services.Interfaces;

public interface IPsychProblemsService
{
    Task<IList<PsychProblemDto>> GetPsychProblems();
}
