using Assignment_1.Models;
using Assignment_1.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Assignment_1.Services
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;

        public StaffService(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        public async Task<IEnumerable<Staff>> GetAllStaffAsync()
        {
            return await _staffRepository.GetAllAsync();
        }

        public async Task<Staff?> GetStaffByIdAsync(int id)
        {
            return await _staffRepository.GetByIdAsync(id);
        }

        public async Task AddStaffAsync(Staff staff)
        {
            if (!await ValidateStaffAsync(staff))
            {
                throw new ValidationException("Staff validation failed");
            }

            if (!await IsEmailUniqueAsync(staff.Email))
            {
                throw new ValidationException("Email already exists");
            }

            // No CreatedDate/UpdatedDate in database

            await _staffRepository.AddAsync(staff);
        }

        public async Task<Staff> UpdateStaffAsync(Staff staff)
        {
            if (!await ValidateStaffAsync(staff))
            {
                throw new ValidationException("Staff validation failed");
            }

            if (!await IsEmailUniqueAsync(staff.Email, staff.Id))
            {
                throw new ValidationException("Email already exists");
            }

            // No UpdatedDate in database

            await _staffRepository.UpdateAsync(staff);
            return staff;
        }

        public async Task DeleteStaffAsync(int id)
        {
            // Soft delete - set IsActive to false instead of physical deletion
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff != null)
            {
                staff.IsActive = false;
                // No UpdatedDate in database
                await _staffRepository.UpdateAsync(staff);
            }
        }

        public async Task<IEnumerable<Staff>> SearchStaffAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllStaffAsync();
            }

            return await _staffRepository.SearchByNameAsync(searchTerm);
        }

        public async Task<IEnumerable<Staff>> GetStaffByRoleAsync(StaffRole role)
        {
            return await _staffRepository.GetByRoleAsync(role);
        }

        public async Task<IEnumerable<Staff>> GetActiveStaffAsync()
        {
            return await _staffRepository.GetActiveStaffAsync();
        }

        public async Task<Staff?> GetStaffByEmailAsync(string email)
        {
            return await _staffRepository.GetByEmailAsync(email);
        }

        public async Task<bool> ValidateStaffAsync(Staff staff)
        {
            if (staff == null)
                return false;

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(staff);

            bool isValid = Validator.TryValidateObject(staff, validationContext, validationResults, true);

            return await Task.FromResult(isValid);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int excludeId = 0)
        {
            return await _staffRepository.IsEmailUniqueAsync(email, excludeId);
        }

        public async Task<bool> IsStaffActiveAsync(int staffId)
        {
            return await _staffRepository.IsStaffActiveAsync(staffId);
        }
    }
}
