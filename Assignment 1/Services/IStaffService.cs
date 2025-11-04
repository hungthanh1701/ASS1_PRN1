using Assignment_1.Models;

namespace Assignment_1.Services
{
    public interface IStaffService
    {
        Task<IEnumerable<Staff>> GetAllStaffAsync();
        Task<Staff?> GetStaffByIdAsync(int id);
        Task AddStaffAsync(Staff staff);
        Task<Staff> UpdateStaffAsync(Staff staff);
        Task DeleteStaffAsync(int id);
        Task<IEnumerable<Staff>> SearchStaffAsync(string searchTerm);
        Task<IEnumerable<Staff>> GetStaffByRoleAsync(StaffRole role);
        Task<IEnumerable<Staff>> GetActiveStaffAsync();
        Task<Staff?> GetStaffByEmailAsync(string email);
        Task<bool> ValidateStaffAsync(Staff staff);
        Task<bool> IsEmailUniqueAsync(string email, int excludeId = 0);
        Task<bool> IsStaffActiveAsync(int staffId);
    }
}
