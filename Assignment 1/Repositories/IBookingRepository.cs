using Assignment_1.Models;

namespace Assignment_1.Repositories
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<Booking>> GetByRoomIdAsync(int roomId);
        Task<IEnumerable<Booking>> GetByStatusAsync(BookingStatus status);
        Task<IEnumerable<Booking>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Booking>> GetActiveBookingsAsync();
        Task<IEnumerable<Booking>> GetBookingsForRoomInDateRangeAsync(int roomId, DateTime checkIn, DateTime checkOut, int excludeBookingId = 0);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut, int excludeBookingId = 0);
        Task<float> CalculateTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<Booking>> GetAllDetailedAsync();
    }
}
