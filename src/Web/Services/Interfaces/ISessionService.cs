namespace Therasim.Web.Services.Interfaces;

public interface ISessionService
{
    Task<Guid> CreateSession(Guid simulationId, bool isActive = true);
}