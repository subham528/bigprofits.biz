using Bigprofits.Data;
using Bigprofits.Models;
using Bigprofits.Repository;

namespace Bigprofits.Repository
{
    public class AuditRepository(ContextClass context) : IAuditRepository
    {
        private readonly ContextClass _context = context;

        public async Task LogActionAsync(ActionLog log)
        {
            await _context.ActionLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task LogActionAsync(string actionType, int entityId, string userType, string details, string ipAddress)
        {
            var log = new ActionLog
            {
                ActionType = actionType,
                EntityId = entityId,
                UserType = userType,
                Details = details,
                IpAddress = ipAddress,
                CreatedOn = DateTime.Now
            };

            await _context.ActionLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}