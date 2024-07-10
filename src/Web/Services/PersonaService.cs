using System.Reflection.Metadata.Ecma335;
using MediatR;
using Therasim.Application.Personas.Queries.GetPersonas;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class PersonaService : IPersonaService
{
    private readonly IMediator _mediator;

    public PersonaService(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<IList<PersonaDto>> GetPersonas()
    {

        return await _mediator.Send(new GetPersonasQuery());
    }
}
