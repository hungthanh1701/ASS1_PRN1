using Assignment_1.Models;

namespace Assignment_1.Repositories
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<IEnumerable<Room>> SearchByRoomNumberAsync(string roomNumber);
        Task<IEnumerable<Room>> GetByRoomTypeAsync(RoomType roomType);
        Task<IEnumerable<Room>> GetByStatusAsync(RoomStatus status);
        Task<IEnumerable<Room>> GetAvailableRoomsAsync();
        Task<IEnumerable<Room>> GetByPriceRangeAsync(double minPrice, double maxPrice);
        Task<IEnumerable<Room>> GetAvailableRoomsForBookingAsync(DateTime checkIn, DateTime checkOut, int capacity);
        Task<bool> IsRoomNumberUniqueAsync(string roomNumber, int excludeId = 0);
    }
}
