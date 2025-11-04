using Microsoft.EntityFrameworkCore;
using Assignment_1.Models;
using Assignment_1.Data;

namespace Assignment_1.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Customer>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Where(c => c.FullName.Contains(name))
                .OrderBy(c => c.FullName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> SearchByEmailAsync(string email)
        {
            return await _dbSet
                .Where(c => c.Email.Contains(email))
                .OrderBy(c => c.FullName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> SearchByPhoneAsync(string phone)
        {
            return await _dbSet
                .Where(c => c.PhoneNumber.Contains(phone))
                .OrderBy(c => c.FullName)
                .ToListAsync();
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int excludeId = 0)
        {
            return !await _dbSet.AnyAsync(c => c.Email == email && c.Id != excludeId);
        }

        public async Task<IEnumerable<Customer>> GetCustomersWithActiveBookingsAsync()
        {
            return await _dbSet
                .Where(c => c.Bookings.Any(b => b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.CheckedIn))
                .Include(c => c.Bookings.Where(b => b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.CheckedIn))
                .OrderBy(c => c.FullName)
                .ToListAsync();
        }
    }
}