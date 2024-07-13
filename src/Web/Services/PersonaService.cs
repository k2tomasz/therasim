using MediatR;
using Therasim.Application.Personas.Queries.GetPersonas;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class PersonaService(IMediator mediator) : IPersonaService
{
    public async Task<IList<PersonaDto>> GetPersonas()
    {
        return await mediator.Send(new GetPersonasQuery());
    }
}
