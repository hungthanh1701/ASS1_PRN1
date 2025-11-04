using Assignment_1.Models;

namespace Assignment_1.Repositories
{
    public interface IStaffRepository : IRepository<Staff>
    {
        Task<IEnumerable<Staff>> SearchByNameAsync(string name);
        Task<IEnumerable<Staff>> GetByRoleAsync(StaffRole role);
        Task<IEnumerable<Staff>> GetActiveStaffAsync();
        Task<Staff?> GetByEmailAsync(string email);
        Task<bool> IsEmailUniqueAsync(string email, int excludeId = 0);
        Task<bool> IsStaffActiveAsync(int staffId);
    }
}
