using Assignment_1.Models;

namespace Assignment_1.Services
{
    public interface IAuditLogService
    {
        Task<IEnumerable<AuditLog>> GetAllAuditLogsAsync();
        Task<AuditLog?> GetAuditLogByIdAsync(int id);
        Task<IEnumerable<AuditLog>> GetAuditLogsByStaffIdAsync(int staffId);
        Task<IEnumerable<AuditLog>> GetAuditLogsByTableNameAsync(string tableName);
        Task<IEnumerable<AuditLog>> GetAuditLogsByOperationAsync(OperationType operation);
        Task<IEnumerable<AuditLog>> GetAuditLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<AuditLog>> GetRecentAuditLogsAsync(int count = 100);
        Task LogOperationAsync(string tableName, OperationType operation, int recordId, int staffId, string? details = null);
    }
}
