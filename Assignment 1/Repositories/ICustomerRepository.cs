using Assignment_1.Models;

namespace Assignment_1.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<IEnumerable<Customer>> SearchByNameAsync(string name);
        Task<IEnumerable<Customer>> SearchByEmailAsync(string email);
        Task<IEnumerable<Customer>> SearchByPhoneAsync(string phone);
        Task<Customer?> GetByEmailAsync(string email);
        Task<bool> IsEmailUniqueAsync(string email, int excludeId = 0);
        Task<IEnumerable<Customer>> GetCustomersWithActiveBookingsAsync();
    }
}
