using Assignment_1.Models;

namespace Assignment_1.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int id);
        Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm);
        Task<Customer?> GetCustomerByEmailAsync(string email);
        Task<IEnumerable<Customer>> GetCustomersWithActiveBookingsAsync();
        Task<bool> ValidateCustomerAsync(Customer customer);
        Task<bool> IsEmailUniqueAsync(string email, int excludeId = 0);
    }
}
