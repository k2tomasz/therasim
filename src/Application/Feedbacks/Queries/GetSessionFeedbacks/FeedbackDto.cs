using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.Feedbacks.Queries.GetSessionFeedbacks;

public class FeedbackDto
{
    public string Content { get; set; } = null!;
    public string Message { get; set; } = null!;
    public AuthorRole Role { get; set; }
    public DateTimeOffset Created { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Feedback, FeedbackDto>();
        }
    }
}