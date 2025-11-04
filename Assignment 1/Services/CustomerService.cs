using Assignment_1.Models;
using Assignment_1.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Assignment_1.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            if (!await ValidateCustomerAsync(customer))
            {
                throw new ValidationException("Customer validation failed");
            }

            if (!await IsEmailUniqueAsync(customer.Email))
            {
                throw new ValidationException("Email already exists");
            }

            // No CreatedDate/UpdatedDate in existing database schema

            return await _customerRepository.AddAsync(customer);
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            if (!await ValidateCustomerAsync(customer))
            {
                throw new ValidationException("Customer validation failed");
            }

            if (!await IsEmailUniqueAsync(customer.Email, customer.Id))
            {
                throw new ValidationException("Email already exists");
            }

            // No UpdatedDate in existing database schema

            await _customerRepository.UpdateAsync(customer);
            return customer;
        }

        public async Task DeleteCustomerAsync(int id)
        {
            // Check if customer has active bookings
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer != null)
            {
                // You might want to add additional validation here
                // to prevent deletion of customers with active bookings
            }

            await _customerRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllCustomersAsync();
            }

            var nameResults = await _customerRepository.SearchByNameAsync(searchTerm);
            var emailResults = await _customerRepository.SearchByEmailAsync(searchTerm);
            var phoneResults = await _customerRepository.SearchByPhoneAsync(searchTerm);

            return nameResults.Concat(emailResults).Concat(phoneResults).Distinct().OrderBy(c => c.FullName);
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _customerRepository.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<Customer>> GetCustomersWithActiveBookingsAsync()
        {
            return await _customerRepository.GetCustomersWithActiveBookingsAsync();
        }

        public async Task<bool> ValidateCustomerAsync(Customer customer)
        {
            if (customer == null)
                return false;

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(customer);

            bool isValid = Validator.TryValidateObject(customer, validationContext, validationResults, true);

            // No DateOfBirth check because schema doesn't include it

            return await Task.FromResult(isValid);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int excludeId = 0)
        {
            return await _customerRepository.IsEmailUniqueAsync(email, excludeId);
        }
    }
}
