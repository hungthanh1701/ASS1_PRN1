using Assignment_1.Models;

namespace Assignment_1.Services
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<Room?> GetRoomByIdAsync(int id);
        Task<Room> CreateRoomAsync(Room room);
        Task<Room> UpdateRoomAsync(Room room);
        Task DeleteRoomAsync(int id);
        Task<IEnumerable<Room>> SearchRoomsAsync(string searchTerm);
        Task<IEnumerable<Room>> GetRoomsByTypeAsync(RoomType roomType);
        Task<IEnumerable<Room>> GetRoomsByStatusAsync(RoomStatus status);
        Task<IEnumerable<Room>> GetAvailableRoomsAsync();
        Task<IEnumerable<Room>> GetAvailableRoomsForBookingAsync(DateTime checkIn, DateTime checkOut, int capacity);
        Task<IEnumerable<Room>> GetRoomsByPriceRangeAsync(double minPrice, double maxPrice);
        Task<bool> ValidateRoomAsync(Room room);
        Task<bool> IsRoomNumberUniqueAsync(string roomNumber, int excludeId = 0);
    }
}
