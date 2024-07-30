using Therasim.Application.Problems.Queries.GetProblems;

namespace Therasim.Web.Services.Interfaces;

public interface IProblemService
{
    Task<IList<ProblemDto>> GetProblems();
}
