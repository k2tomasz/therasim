using Therasim.Domain.Entities;

namespace Therasim.Application.Personas.Queries.GetPersonas;

public class PersonaDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Background { get; set; } = null!;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Persona, PersonaDto>();
        }
    }
}
