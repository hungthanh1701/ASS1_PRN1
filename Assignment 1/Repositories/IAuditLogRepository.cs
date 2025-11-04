using Assignment_1.Models;

namespace Assignment_1.Repositories
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
        Task<IEnumerable<AuditLog>> GetByStaffIdAsync(int staffId);
        Task<IEnumerable<AuditLog>> GetByTableNameAsync(string tableName);
        Task<IEnumerable<AuditLog>> GetByOperationAsync(OperationType operation);
        Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<AuditLog>> GetRecentLogsAsync(int count = 100);
        Task LogOperationAsync(string tableName, OperationType operation, int recordId, int staffId, string? details = null);
    }
}
