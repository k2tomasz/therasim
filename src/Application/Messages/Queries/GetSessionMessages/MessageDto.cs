using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.Messages.Queries.GetSessionMessages;

public class MessageDto
{
    public string Content { get; set; } = null!;
    public AuthorRole Role { get; set; }
    public DateTime Created { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Message, MessageDto>();
        }
    }
}