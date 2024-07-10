using Therasim.Application.Personas.Queries.GetPersonas;

namespace Therasim.Web.Services.Interfaces;

public interface IPersonaService
{
    Task<IList<PersonaDto>> GetPersonas();
}
