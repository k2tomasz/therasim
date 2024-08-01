using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.Messages.Queries.GetSessionMessages;

public class MessageDto
{
    public string Content { get; set; } = null!;
    public MessageAuthorRole Role { get; set; }
    public DateTimeOffset Created { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Message, MessageDto>();
        }
    }
}