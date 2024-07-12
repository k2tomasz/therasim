using System.Runtime.Serialization;

namespace Therasim.Domain.Enums;
public enum FeedbackType
{
    [EnumMember(Value = "Limited")]
    Limited = 1,
    [EnumMember(Value = "Extensive")]
    Extensive
}
