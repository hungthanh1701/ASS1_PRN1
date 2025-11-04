using Microsoft.EntityFrameworkCore;
using Assignment_1.Models;
using Assignment_1.Data;

namespace Assignment_1.Repositories
{
    public class StaffRepository : Repository<Staff>, IStaffRepository
    {
        public StaffRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Staff>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Where(s => s.Name.Contains(name))
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Staff>> GetByRoleAsync(StaffRole role)
        {
            return await _dbSet
                .Where(s => s.Role == role)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Staff>> GetActiveStaffAsync()
        {
            return await _dbSet
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Staff?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int excludeId = 0)
        {
            return !await _dbSet.AnyAsync(s => s.Email == email && s.Id != excludeId);
        }

        public async Task<bool> IsStaffActiveAsync(int staffId)
        {
            var staff = await _dbSet.FindAsync(staffId);
            return staff?.IsActive ?? false;
        }
    }
}
