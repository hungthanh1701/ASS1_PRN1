using Microsoft.EntityFrameworkCore;
using Assignment_1.Models;
using Assignment_1.Data;

namespace Assignment_1.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Booking>> GetByCustomerIdAsync(int customerId)
        {
            return await _dbSet
                .Where(b => b.CustomerId == customerId)
                .Include(b => b.Room)
                .Include(b => b.Customer)
                .OrderByDescending(b => b.CheckInDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByRoomIdAsync(int roomId)
        {
            return await _dbSet
                .Where(b => b.RoomId == roomId)
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .OrderByDescending(b => b.CheckInDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByStatusAsync(BookingStatus status)
        {
            return await _dbSet
                .Where(b => b.Status == status)
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .OrderByDescending(b => b.CheckInDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(b => b.CheckInDate >= startDate && b.CheckOutDate <= endDate)
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .OrderBy(b => b.CheckInDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetActiveBookingsAsync()
        {
            return await _dbSet
                .Where(b => b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.CheckedIn)
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .OrderBy(b => b.CheckInDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsForRoomInDateRangeAsync(int roomId, DateTime checkIn, DateTime checkOut, int excludeBookingId = 0)
        {
            return await _dbSet
                .Where(b => b.RoomId == roomId && 
                           b.Id != excludeBookingId &&
                           b.Status != BookingStatus.Cancelled &&
                           (b.IsDeleted == null || b.IsDeleted == false) &&
                           ((b.CheckInDate < checkOut && b.CheckOutDate > checkIn)))
                .Include(b => b.Customer)
                .ToListAsync();
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut, int excludeBookingId = 0)
        {
            var conflictingBookings = await GetBookingsForRoomInDateRangeAsync(roomId, checkIn, checkOut, excludeBookingId);
            return !conflictingBookings.Any();
        }

        public async Task<float> CalculateTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _dbSet.Where(b => b.Status == BookingStatus.CheckedOut);

            if (startDate.HasValue)
                query = query.Where(b => b.CheckOutDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(b => b.CheckOutDate <= endDate.Value);

            return await query.Select(b => (float)b.TotalAmount).SumAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllDetailedAsync()
        {
            return await _dbSet
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .ToListAsync();
        }
    }
}
