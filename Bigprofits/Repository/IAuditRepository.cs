using Bigprofits.Models;

namespace Bigprofits.Repository
{
    public interface IAuditRepository
    {
        Task LogActionAsync(ActionLog log);
        Task LogActionAsync(string actionType, int entityId, string userType, string details, string ipAddress);
    }
}
