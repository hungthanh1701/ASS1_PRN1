using Microsoft.EntityFrameworkCore;
using Assignment_1.Models;
using Assignment_1.Data;

namespace Assignment_1.Repositories
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Room>> SearchByRoomNumberAsync(string roomNumber)
        {
            // Since RoomNumber is computed from Id, we'll search by Id range
            if (int.TryParse(roomNumber.Replace("R", ""), out int roomId))
            {
                return await _dbSet
                    .Where(r => r.Id == roomId)
                    .ToListAsync();
            }
            return new List<Room>();
        }

        public async Task<IEnumerable<Room>> GetByRoomTypeAsync(RoomType roomType)
        {
            return await _dbSet
                .Where(r => r.Type == roomType)
                .OrderBy(r => r.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetByStatusAsync(RoomStatus status)
        {
            return await _dbSet
                .Where(r => r.Status == status)
                .OrderBy(r => r.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync()
        {
            return await _dbSet
                .Where(r => r.Status == RoomStatus.Available)
                .OrderBy(r => r.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetByPriceRangeAsync(double minPrice, double maxPrice)
        {
            return await _dbSet
                .Where(r => r.PricePerNight >= minPrice && r.PricePerNight <= maxPrice)
                .OrderBy(r => r.PricePerNight)
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsForBookingAsync(DateTime checkIn, DateTime checkOut, int capacity)
        {
            // Get rooms that are available (capacity check removed since it's not in DB)
            var availableRooms = await _dbSet
                .Where(r => r.Status == RoomStatus.Available)
                .ToListAsync();

            // Filter out rooms that have conflicting bookings
            var conflictingRoomIds = await _context.Set<Booking>()
                .Where(b => b.Status != BookingStatus.Cancelled && (b.IsDeleted == null || b.IsDeleted == false) &&
                           ((b.CheckInDate < checkOut && b.CheckOutDate > checkIn)))
                .Select(b => b.RoomId)
                .Distinct()
                .ToListAsync();

            return availableRooms.Where(r => !conflictingRoomIds.Contains(r.Id)).OrderBy(r => r.Id);
        }

        public async Task<bool> IsRoomNumberUniqueAsync(string roomNumber, int excludeId = 0)
        {
            // Since RoomNumber is computed from Id, we'll check if the Id exists
            if (int.TryParse(roomNumber.Replace("R", ""), out int roomId))
            {
                return !await _dbSet.AnyAsync(r => r.Id == roomId && r.Id != excludeId);
            }
            return true;
        }
    }
}