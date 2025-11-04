using Assignment_1.Models;

namespace Assignment_1.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(int id);
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<Booking> UpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(int id);
        Task<Booking> CancelBookingAsync(int bookingId);
        Task<Booking> CheckInBookingAsync(int bookingId);
        Task<Booking> CheckOutBookingAsync(int bookingId);
        Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId);
        Task<IEnumerable<Booking>> GetBookingsByRoomIdAsync(int roomId);
        Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status);
        Task<IEnumerable<Booking>> GetActiveBookingsAsync();
        Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<float> CalculateBookingTotalAsync(int roomId, DateTime checkIn, DateTime checkOut, int numberOfGuests);
        Task<bool> ValidateBookingAsync(Booking booking);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut, int excludeBookingId = 0);
        Task<float> CalculateTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<Booking>> GetAllBookingsDetailedAsync();
    }
}
