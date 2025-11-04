using Microsoft.EntityFrameworkCore;
using Assignment_1.Models;
using Assignment_1.Data;

namespace Assignment_1.Repositories
{
    public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AuditLog>> GetByStaffIdAsync(int staffId)
        {
            return await _dbSet
                .Where(a => a.StaffId == staffId)
                .Include(a => a.Staff)
                .OrderByDescending(a => a.OperationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByTableNameAsync(string tableName)
        {
            return await _dbSet
                .Where(a => a.TableName == tableName)
                .Include(a => a.Staff)
                .OrderByDescending(a => a.OperationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByOperationAsync(OperationType operation)
        {
            return await _dbSet
                .Where(a => a.OperationType == operation)
                .Include(a => a.Staff)
                .OrderByDescending(a => a.OperationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(a => a.OperationDate >= startDate && a.OperationDate <= endDate)
                .Include(a => a.Staff)
                .OrderByDescending(a => a.OperationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetRecentLogsAsync(int count = 100)
        {
            return await _dbSet
                .Include(a => a.Staff)
                .OrderByDescending(a => a.OperationDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task LogOperationAsync(string tableName, OperationType operation, int recordId, int staffId, string? details = null)
        {
            var auditLog = new AuditLog
            {
                TableName = tableName,
                OperationType = operation,
                RecordId = recordId,
                StaffId = staffId,
                Details = details,
                OperationDate = DateTime.Now
            };

            await _dbSet.AddAsync(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}
