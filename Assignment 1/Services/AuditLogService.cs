using Assignment_1.Models;
using Assignment_1.Repositories;

namespace Assignment_1.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditLogService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task<IEnumerable<AuditLog>> GetAllAuditLogsAsync()
        {
            return await _auditLogRepository.GetAllAsync();
        }

        public async Task<AuditLog?> GetAuditLogByIdAsync(int id)
        {
            return await _auditLogRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByStaffIdAsync(int staffId)
        {
            return await _auditLogRepository.GetByStaffIdAsync(staffId);
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByTableNameAsync(string tableName)
        {
            return await _auditLogRepository.GetByTableNameAsync(tableName);
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByOperationAsync(OperationType operation)
        {
            return await _auditLogRepository.GetByOperationAsync(operation);
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _auditLogRepository.GetByDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<AuditLog>> GetRecentAuditLogsAsync(int count = 100)
        {
            return await _auditLogRepository.GetRecentLogsAsync(count);
        }

        public async Task LogOperationAsync(string tableName, OperationType operation, int recordId, int staffId, string? details = null)
        {
            await _auditLogRepository.LogOperationAsync(tableName, operation, recordId, staffId, details);
        }
    }
}
